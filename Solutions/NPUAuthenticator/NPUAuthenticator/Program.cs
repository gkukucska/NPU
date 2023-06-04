using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NPU.Interfaces;
using NPU.Services;
using NPU.Services.AuthenticationService;
using NPU.Services.RegistrationService;
using NPU.Utils.CredentialManager;
using NPU.Utils.SessionTokenManager;

var builder = WebApplication.CreateBuilder();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<ICredentialManager, CredentialManager>();
builder.Services.AddSingleton<ISessionTokenManager, SessionTokenManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<AuthenticationService>();
app.MapGrpcService<RegistrationService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();