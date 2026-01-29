using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<StudentApp_ApiMinimal>("Api");

builder.Build().Run();
