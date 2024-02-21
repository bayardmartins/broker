using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class MessageDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}

public class Message
{
    public string Title { get; set; }
    public string Body { get; set; }
}

public static class MessageMapper
{
    public static Message Map(this MessageDb message)
    {
        return new Message
        {
            Title = message.Title,
            Body = message.Body,
        };
    }
}