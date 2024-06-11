using TodoList.Core;
using TodoList.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastuctureModule();

builder.Services.AddCors(
    options => options.AddPolicy(AppConfiguration.PolicyName, 
    policy => policy.WithOrigins(AppConfiguration.BackEnd, AppConfiguration.FrontEnd)
    .AllowAnyHeader().AllowAnyMethod()));


builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AppConfiguration.PolicyName);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
