using AccountOwnerServer.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Configuration du service Logger pour l'enregistrement des messages
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHttpsRedirection();

/**
 * Permet d'utiliser des fichiers statiques pour la requête. Si nous ne définissons pas de chemin pour les 
 * fichiers statiques, le dossier wwwroot de notre explorateur de solutions sera utilisé par défaut.
 */
app.UseStaticFiles();

// Transmettra les en-têtes du proxy à la requête en cours. Cela nous aidera lors du déploiement de Linux.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorspPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
