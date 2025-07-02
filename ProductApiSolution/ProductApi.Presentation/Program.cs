using ProductApi.Infrastructure.DependencyInjection;
using ProductApi.Application.DependencyInject;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

app.Run();
