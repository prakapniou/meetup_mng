var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services =builder.Services;

ApiConfiguration.SetConfiguration(configuration,services);

var app = builder.Build();

ApiConfiguration.SetMiddleware(app);

app.Run();
