using AccountOwnerServer.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHttpsRedirection();

/**
 * ermet d'utiliser des fichiers statiques pour la requ�te. Si nous ne d�finissons pas de chemin pour les 
 * fichiers statiques, le dossier wwwroot de notre explorateur de solutions sera utilis� par d�faut.
 */
app.UseStaticFiles();

// Transmettra les en-t�tes du proxy � la requ�te en cours. Cela nous aidera lors du d�ploiement de Linux.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorspPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
