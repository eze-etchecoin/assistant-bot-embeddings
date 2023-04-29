using AssistantBot.Configuration;
using AssistantBot.Services.Factories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ChatGptService is instanciated.
var openAiApiKey = Environment.GetEnvironmentVariable(StartupEnvironmentVariables.OpenAIApiKey) ?? "NO_KEY";
builder.Services.AddSingleton(sp => new ChatBotServiceFactory().CreateService(ChatBotServiceOption.ChatGpt));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();