using Social.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();
var app = builder.Build();
app.AddMiddlewares();
await app.RunAsync();