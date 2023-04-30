using AssistantBot.Configuration;
using AssistantBot.DataTypes;
using AssistantBot.Services.Factories;
using AssistantBot.Services.Interfaces;
using AssistantBot.Services.RedisStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ChatGptService is instanciated.
var openAiApiKey = Environment.GetEnvironmentVariable(StartupEnvironmentVariables.OpenAIApiKey) ?? "NO_KEY";
builder.Services.AddSingleton(sp => new ChatBotServiceFactory().CreateService(ChatBotServiceOption.ChatGpt));

builder.Services.AddSingleton<IIndexedVectorStorage<EmbeddedTextVector>>(sp =>
    new RedisVectorStorageService<EmbeddedTextVector>("localhost:6379"));

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