using Broker.Api.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Broker.Api.Infra;

public class MongoRepository
{
    private readonly IMongoClient _client;
    public MongoRepository(IMongoClient client)
    {
        _client = client;
    }

    public async Task<List<MessageDb>> FindMany(string database, string collectionName)
    {
        IMongoCollection<BsonDocument> collection = _client.GetDatabase(database).GetCollection<BsonDocument>(collectionName);

        var messages = await collection.Find(FilterDefinition<BsonDocument>.Empty)
            .ToListAsync()
            .ConfigureAwait(false);

        return BsonSerializer.Deserialize<List<MessageDb>>(messages.ToJson());
    }
}
