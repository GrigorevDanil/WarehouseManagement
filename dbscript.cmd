dotnet-ef database drop -f -c WriteDbContext -p ./src/ResourceManagement/Infrastructure/WarehouseManagement.ResourceManagement.Infrastructure/ -s ./src/WarehouseManagement.API/

dotnet-ef migrations remove -c WriteDbContext -p ./src/ResourceManagement/Infrastructure/WarehouseManagement.ResourceManagement.Infrastructure/ -s ./src/WarehouseManagement.API/
dotnet-ef migrations remove -c WriteDbContext -p ./src/ClientManagement/Infrastructure/WarehouseManagement.ClientManagement.Infrastructure/ -s ./src/WarehouseManagement.API/
dotnet-ef migrations remove -c WriteDbContext -p ./src/UnitManagement/Infrastructure/WarehouseManagement.UnitManagement.Infrastructure/ -s ./src/WarehouseManagement.API/
dotnet-ef migrations remove -c WriteDbContext -p ./src/IncomeProcessing/Infrastructure/WarehouseManagement.IncomeProcessing.Infrastructure/ -s ./src/WarehouseManagement.API/

dotnet-ef migrations add ResourceManagement_Initial -c WriteDbContext -p ./src/ResourceManagement/Infrastructure/WarehouseManagement.ResourceManagement.Infrastructure/ -s ./src/WarehouseManagement.API/
dotnet-ef migrations add ClientManagement_Initial -c WriteDbContext -p ./src/ClientManagement/Infrastructure/WarehouseManagement.ClientManagement.Infrastructure/ -s ./src/WarehouseManagement.API/
dotnet-ef migrations add UnitManagement_Initial -c WriteDbContext -p ./src/UnitManagement/Infrastructure/WarehouseManagement.UnitManagement.Infrastructure/ -s ./src/WarehouseManagement.API/
dotnet-ef migrations add IncomeProcessing_Initial -c WriteDbContext -p ./src/IncomeProcessing/Infrastructure/WarehouseManagement.IncomeProcessing.Infrastructure/ -s ./src/WarehouseManagement.API/

dotnet-ef database update -c WriteDbContext -p ./src/ResourceManagement/Infrastructure/WarehouseManagement.ResourceManagement.Infrastructure/ -s ./src/WarehouseManagement.API/
dotnet-ef database update -c WriteDbContext -p ./src/ClientManagement/Infrastructure/WarehouseManagement.ClientManagement.Infrastructure/ -s ./src/WarehouseManagement.API/
dotnet-ef database update -c WriteDbContext -p ./src/UnitManagement/Infrastructure/WarehouseManagement.UnitManagement.Infrastructure/ -s ./src/WarehouseManagement.API/
dotnet-ef database update -c WriteDbContext -p ./src/IncomeProcessing/Infrastructure/WarehouseManagement.IncomeProcessing.Infrastructure/ -s ./src/WarehouseManagement.API/

pause