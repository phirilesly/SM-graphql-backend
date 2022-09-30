﻿using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using StockManager.GraphQL.Models;

namespace StockManager.GraphQL.Data
{
    public class MongoContext
    {
        public IMongoDatabase Database { get; }
        private readonly ConventionPack camelConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        private readonly ConventionPack ignoreExtraElementsPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        private readonly ConventionPack ignoreNullsPack = new ConventionPack { new IgnoreIfNullConvention(true) };
        private readonly MongoClient client;

        public MongoContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("StockmangerDatabase");

            ConventionPack pack = new ConventionPack
            {
                new IgnoreIfNullConvention(true),
                new IgnoreExtraElementsConvention(true),
                new CamelCaseElementNameConvention()
            };
            ConventionRegistry.Register("defaults", pack, t => true);
            client = new MongoClient(connectionString);
            Database = client.GetDatabase(MongoUrl.Create(connectionString).DatabaseName);
        }

        public MongoContext(string connectionString)
        {
            ConventionRegistry.Register("CamelCaseConvensions", camelConventionPack, t => true);
            ConventionRegistry.Register("IgnoreExtraElements", ignoreExtraElementsPack, t => true);
            ConventionRegistry.Register("Ignore null values", ignoreNullsPack, t => true);
            client = new MongoClient(connectionString);
            Database = client.GetDatabase(MongoUrl.Create(connectionString).DatabaseName);
        }


        public IMongoCollection<User> Users => Database.GetCollection<User>("Users");

        public IMongoCollection<Branch> Branches => Database.GetCollection<Branch>("Branchs");

        public IMongoCollection<Sale> Purchases => Database.GetCollection<Sale>("Sales");

        public IMongoCollection<Product> Products => Database.GetCollection<Product>("Products");

        public IMongoCollection<Inventory> Inventories => Database.GetCollection<Inventory>("Inventories");

        public IMongoCollection<Category> Categories => Database.GetCollection<Category>("Categories");

        public IMongoCollection<Brand> Brands => Database.GetCollection<Brand>("Brands");


    }
}
