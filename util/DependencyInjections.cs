using JavaHateBE.Repository;
using JavaHateBE.Service;

namespace JavaHateBE.Util
{
    public static class DependencyInjections
    {
        /// <summary>
        /// Adds custom services, repositories, and other dependencies to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param> 
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<UserRepository>();
            services.AddScoped<QuestionRepository>();
            services.AddScoped<GameRepository>();
            services.AddScoped<UserService>();
            services.AddScoped<QuestionService>();
            services.AddScoped<GameService>();

            return services;
        }
    }
}