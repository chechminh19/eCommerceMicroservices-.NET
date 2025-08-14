using ProductApi.Infrastructure.DependencyInjection;
using ProductApi.Application.DependencyInject;
using ProductGrpc;
using ProductApi.Application.Service;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
var app = builder.Build();

app.UseInfrastructurePolicy();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<ProductGrpcServiceImpl>(); // map gRPC
app.MapGet("/", () => "Product gRPC Service running");
app.Run();
