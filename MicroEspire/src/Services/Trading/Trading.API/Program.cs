using Trading.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();
app.MapApplicationEndpoint();
app.UseAplicationMiddleware();
app.Run();
