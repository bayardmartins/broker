using Broker.Consumer.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Broker.Consumer.Infrastructure;

public class MongoRepository
{
    private readonly IMongoClient _client;
    public MongoRepository(IMongoClient client)
    {
        _client = client;
    }

    public void Insert(string database, string collectionName, MessageDb message)
    {
        IMongoCollection<BsonDocument> collection = _client.GetDatabase(database).GetCollection<BsonDocument>(collectionName);

        collection.InsertOne(message.ToBsonDocument());
    }
}
