-- Migration Command

add-migration FirstMigrate -OutputDir  Data/Migrations/ApplicationDb  -StartupProject Authen -Context ApplicationDbContext

add-migration GenerateApplicationTenantDB1 -OutputDir  Data/Migrations/TenantStoreDb  -StartupProject Authen -Context TenantStoreDbContext


-- Update Database

update-database -StartupProject Authen  -Context ApplicationDbContext

update-database -StartupProject Authen  -Context TenantStoreDbContext



-- Remove Migration
remove-migration  -StartupProject BlazorIdentity -Context ApplicationDbContext
remove-migration  -StartupProject BlazorIdentity -Context TenantStoreDbContext

--Revert Migration
Update-Database <tenmigration> -StartupProject BlazorIdentity  -Context ApplicationDbContext

Update-Database 20240826093257_GenerateApplicationForDuende -StartupProject BlazorIdentity  -Context ApplicationDbContext