dotnet-ef database drop -f -c WriteDbContext -p ./src/ResourceManagement/Infrastructure/WarehouseManagement.ResourceManagement.Infrastructure/ -s ./src/WarehouseManagement.API/

dotnet-ef migrations remove -c WriteDbContext -p ./src/ResourceManagement/Infrastructure/WarehouseManagement.ResourceManagement.Infrastructure/ -s ./src/WarehouseManagement.API/

dotnet-ef migrations add ResourceManagement_Initial -c WriteDbContext -p ./src/ResourceManagement/Infrastructure/WarehouseManagement.ResourceManagement.Infrastructure/ -s ./src/WarehouseManagement.API/

dotnet-ef database update -c WriteDbContext -p ./src/ResourceManagement/Infrastructure/WarehouseManagement.ResourceManagement.Infrastructure/ -s ./src/WarehouseManagement.API/

pause