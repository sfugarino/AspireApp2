using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithRedisInsight();

var apiService = builder.AddProject<Projects.AspireApp2_ApiService>("apiservice")
    .WithReference(cache)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.AspireApp2_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WithReference(apiService)
    .WaitFor(apiService);


// var scalar = builder.AddScalarApiReference();
// scalar.WithApiReference(apiService);

builder.Build().Run();
