// The following commands can be run in Package Manager Console

// This Creates a new migration, run this when you make any changes to the database model classes.
Add-Migration AddedUsersTable

// This applies all the pending migrations to the database and thus updates database to latest migration
Update-Database –Verbose

// This updates the database to the specified migration (use this to downgrade the database when needed)
Update-Database –TargetMigration 201610191246211_TwoFactorAuth -verbose

// This forces an update to an existing migration
Add-Migration AddedUsersTable -Force