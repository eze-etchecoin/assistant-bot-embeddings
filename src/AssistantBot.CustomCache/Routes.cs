using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;

namespace AssistantBot.CustomCache
{
    public static class Routes
    {
        public static void RegisterRoutes(this WebApplication app)
        {
            app.MapPost(
                "/AddVector", 
                async(HttpContext context, CustomMemoryStorage<IVectorWithObject> storage) =>
                {
                    var requestBody = await context.Request.ReadFromJsonAsync<EmbeddedTextVector>();
                    storage.AddVector(requestBody);
                })
            .WithName("AddVector");

            app.MapPost(
                "/SearchDataBySimilarVector",
                async (HttpContext context, CustomMemoryStorage<IVectorWithObject> storage) =>
                {
                    var requestBody = await context.Request.ReadFromJsonAsync<EmbeddedTextVector>();
                    if(!int.TryParse(context.Request.Query["numResults"], out var num))
                    {
                        num = 1;
                    }

                    return Results.Ok(storage.SearchDataBySimilarVector<object>(requestBody, num));
                })
            .WithName("SearchDataBySimilarVector");

            app.MapGet(
                "/GetDataByKey",
                (HttpContext context, CustomMemoryStorage<IVectorWithObject> storage) => 
                {
                    var key = context.Request.Query["key"];
                    if (string.IsNullOrEmpty(key))
                        return Results.BadRequest("A valid key must be specified.");

                    var result = storage.GetDataByKey(key);

                    if (result == null)
                        return Results.BadRequest("Invalid key");

                    return Results.Ok(result);
                })
            .WithName("GetDataByKey");

            app.MapGet(
                "/GetKeys",
                (CustomMemoryStorage<IVectorWithObject> storage) => storage.GetKeys())
            .WithName("GetKeys");

            app.MapGet(
                "/Check",
                (CustomMemoryStorage<IVectorWithObject> storage) => Results.Ok(storage.TestConnection()))
            .WithName("Check");
        }
    }
}
