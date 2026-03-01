using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var k3s = builder.AddKubernetesEnvironment("k3s");

var apiService = builder.AddProject<Projects.AspireApp2_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithComputeEnvironment(k3s);

builder.AddProject<Projects.AspireApp2_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithComputeEnvironment(k3s);


// var scalar = builder.AddScalarApiReference();
// scalar.WithApiReference(apiService);

builder.Build().Run();
