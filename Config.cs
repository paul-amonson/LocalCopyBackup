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
