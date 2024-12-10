using Newtonsoft.Json;

namespace Shared.Core.Interfaces.Serialization
{
    public interface IJsonSerializerSettings
    {
        /// <summary>
        /// Settings for <see cref="Newtonsoft.Json"/>.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; }
    }
}