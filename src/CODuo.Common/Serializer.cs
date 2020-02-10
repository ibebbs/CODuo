using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CODuo.Common
{
    public static class Serializer
    {
        private static readonly JsonSerializerOptions Options = CreateSerializationOptions();

        private static JsonSerializerOptions CreateSerializationOptions()
        {
            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                DefaultBufferSize = 2097152
            };

            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.Converters.Add(JsonDoubleRoundingConverter.Instance);

            return options;
        }

        public static Task Serialize(Container container, Stream stream)
        {
            return JsonSerializer.SerializeAsync(stream, container, Options);
        }

        public static string Serialize(Container container)
        {
            return JsonSerializer.Serialize(container, Options);
        }

        public static ValueTask<Container> Deserialize(Stream stream)
        {
            return JsonSerializer.DeserializeAsync<Container>(stream, Options);
        }

        public static Container Deserialize(string json)
        {
            return JsonSerializer.Deserialize<Container>(json, Options);
        }
    }

    public class JsonDoubleRoundingConverter : JsonConverter<double>
    {
        public static readonly JsonDoubleRoundingConverter Instance = new JsonDoubleRoundingConverter();

        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return double.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("F2"));
        }
    }
}
