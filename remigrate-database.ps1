Write-Host "Removing existing migrations..."
dotnet ef migrations remove

Write-Host "Dropping existing database..."
dotnet ef database drop

# Generate a random migration number between 1 and 1000 (you can adjust the range if needed)
$randomMigrationNumber = Get-Random -Minimum 1 -Maximum 1000000
$randomMigrationName = "NewMigration$randomMigrationNumber"

Write-Host "Adding new migration: $randomMigrationName"
dotnet ef migrations add $randomMigrationName

Write-Host "Updating the database..."
dotnet ef database update