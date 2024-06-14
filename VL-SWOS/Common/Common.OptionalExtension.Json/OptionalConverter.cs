using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.OptionalExtension.Json;

public class OptionalConverter<T> : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Optional<T>);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var token = JValue.Load(reader);

        if (((JValue)token).Value == null)
        {
            return Optional<T>.Empty;
        }

        return new Optional<T>(token.ToObject<T>()!);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null || !((Optional<T>)value).HasValue)
        {
            serializer.Serialize(writer, null);
        }
        else
        {
            serializer.Serialize(writer, ((Optional<T>)value).Value);
        }
    }
}