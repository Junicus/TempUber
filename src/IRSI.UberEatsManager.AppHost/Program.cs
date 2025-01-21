var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");
var rabbitMq = builder.AddRabbitMQ("eventbus");
var postgres = builder.AddPostgres("postgres")
    .WithImage("ankane/pgvector")
    .WithImageTag("latest")
    .WithLifetime(ContainerLifetime.Persistent);

var menuDb = postgres.AddDatabase("menuDb");

var menuApi = builder.AddProject<Projects.IRSI_UberEatsManager_MenuApi>("menu-api")
    .WithReference(menuDb).WaitFor(menuDb);

var menuEditor = builder.AddProject<Projects.IRSI_UberEatsManager_MenuEditor>("menu-editor")
    .WithReference(menuApi).WaitFor(menuApi);

builder.Build().Run();