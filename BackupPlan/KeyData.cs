/*
Copyright (c) 2017 Paul Amonson

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
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
