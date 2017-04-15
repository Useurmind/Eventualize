using System;
using System.Linq;
using System.Text;

using Eventualize.Interfaces.Infrastructure;

using Newtonsoft.Json;

namespace Eventualize.Infrastructure
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize(object instance)
        {
            var json = JsonConvert.SerializeObject(instance);
            return Encoding.UTF8.GetBytes(json);
        }

        public object Deserialize(Type objectType, byte[] data)
        {
            var json = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject(json, objectType);
        }
    }
}