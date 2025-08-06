using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.Framework.Outbox;

public sealed class OutboxMessageProcess<TDbContext> : IOutboxMessageProcess 
    where TDbContext : DbContext, IOutboxDbContext
{
    private readonly ConcurrentDictionary<string, Type?> _messagesTypeDictionary = new();
    
    private readonly TDbContext _dbContext;
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<OutboxMessageProcess<TDbContext>> _logger;

    public OutboxMessageProcess(
        TDbContext dbContext,
        IPublishEndpoint publisher,
        ILogger<OutboxMessageProcess<TDbContext>> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task ExecuteAsync(Assembly messagesAssembly, CancellationToken cancellationToken = default)
    {
        var messages = await _dbContext
            .OutboxMessages
            .OrderBy(m => m.CreatedAt)
            .Where(m => m.ProcessedAt == null)
            .Take(100)
            .ToListAsync(cancellationToken);

        if (messages.Count == 0)
            return;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(2),
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                OnRetry = retryArgs =>
                {
                    _logger.LogCritical(
                        retryArgs.Outcome.Exception,
                        "Current attempt: {attemptNumber}",
                        retryArgs.AttemptNumber);

                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        var processingTasks = messages.Select(m => ProcessMessageAsync(m, messagesAssembly, pipeline, cancellationToken));
        
        await Task.WhenAll(processingTasks);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save changes to the database");
        }
    }

    private async Task ProcessMessageAsync(
        OutboxMessage message, 
        Assembly messagesAssembly,
        ResiliencePipeline pipeline,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var messageType = _messagesTypeDictionary.GetOrAdd(message.Type, messagesAssembly.GetType) 
                              ?? throw new NullReferenceException("Message type not found");

            var deserializedMessage = JsonSerializer.Deserialize(message.Payload, messageType)
                                      ?? throw new NullReferenceException("Message payload not found");

            await pipeline.ExecuteAsync(async token =>
            {
                await _publisher.Publish(deserializedMessage, messageType, token);

                message.ProcessedAt = DateTime.UtcNow;
                message.Error = string.Empty;
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            message.Error = ex.Message;
            message.ProcessedAt = DateTime.UtcNow;
            _logger.LogError(ex, "Failed to process message id: {messageId}", message.Id);
        }
    }
    
}