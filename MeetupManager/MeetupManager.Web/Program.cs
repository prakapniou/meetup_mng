var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services =builder.Services;
var logging = builder.Logging;
var logger=ApiConfiguration.SetConfiguration(configuration,services,logging);
var app = builder.Build();

ApiConfiguration.SetMiddleware(app);

try
{    
    logger.Information("Application launch successfully");
    app.Run();
}

catch (Exception ex)
{
    logger.Error("Application launch is failed with error: \"{Exception}\"",ex);
}

