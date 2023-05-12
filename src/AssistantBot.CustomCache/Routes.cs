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
                    var requestBody = await context.Request.ReadFromJsonAsync<IVectorWithObject>();
                    storage.AddVector(requestBody);
                })
            .WithName("AddVector");

            app.MapPost(
                "/SearchDataBySimilarVector",
                async (HttpContext context, CustomMemoryStorage<IVectorWithObject> storage) =>
                {
                    var requestBody = await context.Request.ReadFromJsonAsync<IVectorWithObject>();
                    if(!int.TryParse(context.Request.Query["numResults"], out var num))
                    {
                        num = 1;
                    }

                    return Results.Ok(storage.SearchDataBySimilarVector<object>(requestBody, num));
                })
            .WithName("AddVector");

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
