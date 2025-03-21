﻿using Newtonsoft.Json;

namespace JsonRpcWebServer.Contract;

public class InterfaceConverter<TInterface,TImplementation> : JsonConverter where TImplementation : TInterface
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return serializer.Deserialize<TImplementation>(reader);
    }

    public override  bool CanConvert(Type objectType) => objectType == typeof(TInterface);
}
