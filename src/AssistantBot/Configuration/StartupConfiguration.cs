namespace AssistantBot.Configuration
{
    public static class StartupConfiguration
    {
        public static IServiceCollection ConfigureCors(this WebApplicationBuilder builder)
        {
            //var frontendUrl = builder.Configuration.GetSection("DemoUrl").Value;
            //if(string.IsNullOrEmpty(frontendUrl))
            //    return builder.Services;
            
            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(bld =>
                    bld.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            return builder.Services;
        }
    }
}
