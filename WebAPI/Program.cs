using Web.UI.DataAccess.Repository.Interfaces;
using Web.UI.Services;
using Web.UI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITodoRepository, TodosRepository>();
builder.Services.AddSingleton<ITodosQueueService, TodosQueueService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();