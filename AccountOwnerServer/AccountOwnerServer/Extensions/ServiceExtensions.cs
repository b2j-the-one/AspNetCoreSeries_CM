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
    }
}
