using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RestClientTemplate.RestClient
{
    internal static class DeserializationExtensions
    {
        public static string ToJson<T>(this T instance)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, instance);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        public static T DeserializeJson<T>(this string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}