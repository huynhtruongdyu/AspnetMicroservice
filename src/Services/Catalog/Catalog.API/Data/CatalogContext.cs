using Catalog.API.Entities;
using Catalog.API.Settings;

using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.API.Data;

public interface ICatalogContext
{
    IMongoCollection<Product> Products { get; }
}

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        // Check if the database exists, create it if not
        //{
        //    var databaseNames = mongoClient.ListDatabaseNames().ToList();
        //    if (!databaseNames.Contains(databaseSettings.Value.DatabaseName))
        //    {
        //        string createJson = "{create: " + databaseSettings.Value.DatabaseName + "}";
        //        var createDatabseCommand = new JsonCommand<BsonDocument>(createJson);
        //        mongoClient.GetDatabase("admin").RunCommand(createDatabseCommand);
        //    }
        //}
        Products = database.GetCollection<Product>(databaseSettings.Value.CollectionName);
        // Check if the collection exists, create it if not
        //{
        //    var collectionNames = database.ListCollectionNames().ToList();
        //    if (!collectionNames.Contains(databaseSettings.Value.CollectionName))
        //    {
        //        database.CreateCollection(databaseSettings.Value.CollectionName);
        //    }
        //}
        CatalogContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}