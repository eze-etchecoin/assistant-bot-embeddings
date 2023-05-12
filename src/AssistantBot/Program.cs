using AssistantBot.Configuration;
using AssistantBot.DataTypes;
using AssistantBot.Services.Factories;
using AssistantBot.Common.Interfaces;
using AssistantBot.Services.RedisStorage;
using AssistantBot.Services.Integrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AssistantBotConfiguration>();
builder.Services.Configure<AssistantBotConfigurationOptions>(
    builder.Configuration.GetSection("AssistantBotOptions"));

// ChatGptService is instanciated.
var openAiApiKey = Environment.GetEnvironmentVariable(StartupEnvironmentVariables.OpenAIApiKey) ?? "NO_KEY";
builder.Services.AddSingleton(sp => new ChatBotServiceFactory().CreateService(ChatBotServiceOption.ChatGpt));

//var redisUrl = Environment.GetEnvironmentVariable(StartupEnvironmentVariables.RedisServerUrl)
//    ?? "localhost:6379";

builder.Services
    .AddSingleton<IIndexedVectorStorage<EmbeddedTextVector>, CustomMemoryStorageService<EmbeddedTextVector>>();

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