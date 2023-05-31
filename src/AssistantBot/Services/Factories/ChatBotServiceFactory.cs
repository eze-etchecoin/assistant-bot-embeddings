﻿using AssistantBot.Configuration;
using AssistantBot.Common.Interfaces;
using AssistantBot.Common.Exceptions;
using AssistantBot.Services.ChatGpt;

namespace AssistantBot.Services.Factories
{
    public class ChatBotServiceFactory : IChatBotServiceFactory
    {
        public IChatBotService CreateService(ChatBotServiceOption chatBotServiceOption)
        {
            return chatBotServiceOption switch
            {
                ChatBotServiceOption.ChatGpt => new ChatGptService(Environment.GetEnvironmentVariable(StartupEnvironmentVariables.OpenAIApiKey) ?? ""),
                _ => throw new AssistantBotException("No ChatBotServiceOption specified.")
            };
        }
    }
}
