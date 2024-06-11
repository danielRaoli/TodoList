using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Infrastructure.Persistence;
using MongoDB.Driver;
using MongoDB.Bson;
using TodoList.Core.Repositories;
using TodoList.Infrastructure.Persistence.Repositories;

namespace TodoList.Infrastructure
{
    public static class InfrastructureModules
    {
        public static IServiceCollection AddInfrastuctureModule(this IServiceCollection services) 
        {
            services.AddMongo().AddServices();
            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbOptions>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var options = new MongoDbOptions();

                configuration.GetSection("Mongo").Bind(options);

                return options;
            });

            services.AddSingleton<IMongoClient>(sp =>
            { 
                var configuration = sp.GetService<IConfiguration>();
                var options = sp.GetService<MongoDbOptions>();

                var client = new MongoClient(options.ConnectionString);
                var db = client.GetDatabase(options.Database);


                return client;
            });

            services.AddTransient(sp =>
            {
                BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
                var options = sp.GetService<MongoDbOptions>();

                var mongoClient = sp.GetService<IMongoClient>();

                var db = mongoClient.GetDatabase(options.Database);

                return db;
            });

            return services;
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITodoRepository, TodoRepository>();
        }

        
    }
}
