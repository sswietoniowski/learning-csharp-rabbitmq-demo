using RabbitMqLibrary.Services;
using RabbitMqLibrary.Services.Interfaces;
using Web.UI.DataAccess.Repository;
using Web.UI.DataAccess.Repository.Interfaces;
using Web.UI.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITodoRepository, TodosRepository>();
builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddSingleton<IMessagePublisher<TodoDto>, MessagePublisher<TodoDto>>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();