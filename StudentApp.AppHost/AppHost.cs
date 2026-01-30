using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sql", port: 1433)
    .WithBindMount("c:\\temp\\docker\\mssql\\log", "/var/opt/mssql/log")
    .WithBindMount("c:\\temp\\docker\\mssql\\data", "/var/opt/mssql/data")
    .WithBindMount("c:\\temp\\docker\\mssql\\secrets", "/var/opt/mssql/secrets")
    .WithLifetime(ContainerLifetime.Persistent);

var sqlDatabase = sqlServer.AddDatabase("students");

var api = builder.AddProject<StudentApp_ApiMinimal>("Api")
    .WithReference(sqlDatabase)
    .WaitFor(sqlDatabase);

var frontend = builder.AddPnpmApp("frontend", "../../ProjetAngular")
    .WaitFor(api);

await builder.Build().RunAsync();