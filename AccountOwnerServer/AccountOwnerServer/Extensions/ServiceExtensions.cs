using Contracts;
using Entities;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Ripository;

namespace AccountOwnerServer.Extensions
{
    public static class ServiceExtensions
    {
        /**
         * CORS (Cross-Origin Resource Sharing) : est un mécanisme qui donne des droits à l'utilisateur pour accéder 
         * aux ressources du serveur sur un domaine différent.
         * Comme nous allons utiliser Angular côté client sur un domaine différent de celui du serveur, 
         * la configuration de CORS est obligatoire.
         */
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorspPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
        }

        /**
         * Ajouter le service logger dans le conteneur IOC de .NET Core. Il y a trois façons de le faire :
         * 
         * En appelant builder.Services.AddSingleton, le service est créé la première fois que vous le demandez et 
         * chaque demande ultérieure appelle la même instance du service. Cela signifie que tous les composants 
         * partagent le même service chaque fois qu'ils en ont besoin. Vous utilisez toujours la même instance
         * 
         * En appelant builder.Services.AddScoped, le service sera créé une fois par requête. Cela signifie qu'à 
         * chaque fois que nous envoyons une requête HTTP vers l'application, une nouvelle instance du service est créée.
         * 
         * En appelant builder.Services.AddTransient, le service sera créé chaque fois que l'application le demandera. 
         * Cela signifie que si, au cours d'une requête vers notre application, plusieurs composants ont besoin du 
         * service, ce service sera créé à nouveau pour chaque composant qui en a besoin.
         * */
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        /**
         * Configuration du contexte MySQL
         */
        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["mysqlconnection:connectionString"];

            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion));
        }


        /**
         * Mise en place d'un wrapper de référentiel
         */
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
