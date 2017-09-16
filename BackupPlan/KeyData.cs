#region Using Section
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
#endregion

namespace Backup
{
    [JsonObject]
    public class KeyData
    {
        #region Public API
        public KeyData()
        {
            var aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
            Key = aes.Key;
            IV = aes.IV;
        }

        public KeyData(KeyData other)
        {
            Key = other.Key;
            IV = other.IV;
        }

        static public KeyData Load(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open))
                return KeyData.Load(stream);
        }

        static public KeyData Load(Stream inStream)
        {
            using (var streamReader = new StreamReader(inStream))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = JsonSerializer.Create();
                var data = serializer.Deserialize<KeyData>(reader);
                return data;
            }
        }

        public void Save(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
                Save(stream);
        }

        public void Save(Stream outStream)
        {
            using (var streamWriter = new StreamWriter(outStream))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                writer.Indentation = 2;
                writer.IndentChar = ' ';
                writer.Formatting = Formatting.Indented;
                var serializer = JsonSerializer.Create();
                serializer.Serialize(writer, this);
            }
        }

        [JsonProperty(Required = Required.Always, PropertyName = "A")]
        public byte[] IV { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "B")]
        public byte[] Key { get; set; }
        #endregion
    }
}
