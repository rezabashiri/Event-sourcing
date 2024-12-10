using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Shared.Core.Interfaces.Serialization;

namespace Shared.Core.Serialization
{
    public class SystemTextJsonSerializer(IOptions<JsonSerializerSettingsOptions> options) : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options = options.Value.JsonSerializerOptions;

        public T Deserialize<T>(string data, IJsonSerializerSettingsOptions options = null)
            => JsonSerializer.Deserialize<T>(data, options?.JsonSerializerOptions ?? _options);

        public string Serialize<T>(T data, IJsonSerializerSettingsOptions options = null)
            => JsonSerializer.Serialize(data, options?.JsonSerializerOptions ?? _options);

        public string Serialize<T>(T data, Type type, IJsonSerializerSettingsOptions options = null)
            => JsonSerializer.Serialize(data, type, options?.JsonSerializerOptions ?? _options);
    }
}