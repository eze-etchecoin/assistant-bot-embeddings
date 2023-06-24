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
                async(HttpContext context, CustomMemoryStorage<EmbeddedTextVector> storage) =>
                {
                    var requestObject = await context.Request.ReadFromJsonAsync<AddVectorRequest>();
                    var storedHash = storage.AddVector(requestObject.Vector, requestObject.KeyComplementStr);
                    return Results.Ok(storedHash);
                })
            .WithName("AddVector");

            app.MapPost(
                "/SearchDataBySimilarVector",
                async (HttpContext context, CustomMemoryStorage<EmbeddedTextVector> storage) =>
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
                (HttpContext context, CustomMemoryStorage<EmbeddedTextVector> storage) => 
                {
                    var key = context.Request.Query["key"].ToString();
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
                (CustomMemoryStorage<EmbeddedTextVector> storage) => storage.GetKeys())
            .WithName("GetKeys");

            app.MapGet(
                "/Check",
                (CustomMemoryStorage<EmbeddedTextVector> storage) => Results.Ok(storage.TestConnection()))
            .WithName("Check");

            app.MapDelete(
                "/DeleteAllKeys",
                (CustomMemoryStorage<EmbeddedTextVector> storage) => storage.DeleteAllKeys())
            .WithName("DeleteAllKeys");
        }
    }
}
