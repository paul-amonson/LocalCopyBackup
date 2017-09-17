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
using System.IO;
using Newtonsoft.Json;

namespace LocalCopyBackup
{
    [JsonObject]
    public class Config
    {
        public Config(string filename)
        {
            if (filename == null)
                return;
            filename_ = filename;
            if (!File.Exists(filename))
            {
                TimedEnabled = true;
                Hour = 4;
                Save();
            }
            using (var stream = new FileStream(filename, FileMode.Open))
            using (var streamReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(streamReader))
            {
                var tmp = JsonSerializer.Create().Deserialize<Config>(reader);
                TimedEnabled = tmp.TimedEnabled;
                Hour = tmp.Hour;
            }
        }

        [JsonProperty(Required=Required.Always, PropertyName = "timed_on")]
        public bool TimedEnabled;

        [JsonProperty(Required = Required.Always, PropertyName = "hours")]
        public int Hour;

        public void Save()
        {
            using (var stream = new FileStream(filename_, FileMode.Create))
            using (var streamWriter = new StreamWriter(stream))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                writer.Indentation = 2;
                writer.IndentChar = ' ';
                writer.Formatting = Formatting.Indented;
                JsonSerializer.Create().Serialize(writer, this);
            }
        }

        [JsonIgnore]
        private string filename_;
    }
}
