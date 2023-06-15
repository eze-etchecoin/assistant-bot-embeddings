using AssistantBot.Configuration;
using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Services.Factories;
using AssistantBot.Services.Integrations;
using AssistantBot.Services.Cache;
using AssistantBot.Configuration.Initializers;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.ConfigureCors();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AssistantBotConfiguration>();
builder.Services.Configure<AssistantBotConfigurationOptions>(
    builder.Configuration.GetSection("AssistantBotOptions"));

// InDiskCache is instanciated.
builder.Services.AddSingleton<InDiskCache<Dictionary<string, double[]>>>();

// ChatGptService is instanciated.
var openAiApiKey = Environment.GetEnvironmentVariable(StartupEnvironmentVariables.OpenAIApiKey) ?? "NO_KEY";
builder.Services.AddSingleton(sp => 
    new ChatBotServiceFactory(sp.GetService<InDiskCache<Dictionary<string, double[]>>>())
        .CreateService(ChatBotServiceOption.ChatGpt));

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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.UseCors();

await EmbeddingsDiskCacheInitializer.LoadEmbeddingsIntoCache(app.Services);

app.Run();