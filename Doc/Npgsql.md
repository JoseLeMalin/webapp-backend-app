


Create migration file with CLI:
dotnet ef migrations add <name-of-the-migration> -o .\Data\Migrations --project <project-associated(e.g. User.API.csproj)>

Run Migration:
dotnet ef database update

Rollback migration:
dotnet ef migrations remove

Rollback migration:
IF only 1 migration file => dotnet ef update 0
IF Multiple migration files => dotnet ef update <name-migration-file-before-the-one-we-want-down>
