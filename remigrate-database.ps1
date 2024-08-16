Write-Host "Removing existing migrations..."
dotnet ef migrations remove

Write-Host "Dropping existing database..."
dotnet ef database drop

Write-Host "Adding new migration..."
dotnet ef migrations add NewMigration

Write-Host "Updating the database..."
dotnet ef database update