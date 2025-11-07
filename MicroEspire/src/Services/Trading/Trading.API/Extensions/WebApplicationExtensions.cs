namespace Trading.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseAplicationMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            return app;
        }

        public static WebApplication MapApplicationEndpoint(this WebApplication app)
        {
            app.MapControllers();
            return app;
        }
    }
}
