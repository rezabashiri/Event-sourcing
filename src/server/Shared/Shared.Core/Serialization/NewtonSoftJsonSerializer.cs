using System;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Core.Interfaces.Serialization;

namespace Shared.Core.Serialization
{
    public class NewtonSoftJsonSerializer(IOptions<JsonSerializerSettingsOptions> settings) : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings = settings.Value.JsonSerializerSettings;

        public T Deserialize<T>(string text, IJsonSerializerSettingsOptions settings = null)
            => JsonConvert.DeserializeObject<T>(text, settings?.JsonSerializerSettings ?? _settings);

        public string Serialize<T>(T obj, IJsonSerializerSettingsOptions settings = null)
            => JsonConvert.SerializeObject(obj, settings?.JsonSerializerSettings ?? _settings);

        public string Serialize<T>(T obj, Type type, IJsonSerializerSettingsOptions settings = null)
            => JsonConvert.SerializeObject(obj, type, settings?.JsonSerializerSettings ?? _settings);
    }
}