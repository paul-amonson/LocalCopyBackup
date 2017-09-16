#region Using Section
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
#endregion

namespace Backup
{
    [JsonObject]
    public class FileObject
    {
        [JsonIgnore]
        public const int BUFFER_SIZE = 4194304; // 4MB

        #region Public API
        public FileObject() { }

        public FileObject(string id, string relativeFilename)
        {
            ID = id ?? throw new ArgumentNullException("FileMetadata.ctor: id must not be null!");
            RelativeFilename = relativeFilename ?? throw new ArgumentNullException("FileMetadata.ctor: relativeFilename must not be null!");
            Changed = true;
            IsNew = true;
        }

        public void GetNewValues(string filename)
        {
            NewTimeStampUTC = File.GetLastWriteTimeUtc(filename);
            if (NewTimeStampUTC > TimeStampUTC)
            {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, BUFFER_SIZE))
                using (var sha = new SHA256Managed())
                    NewHash = Encoding.UTF8.GetString(sha.ComputeHash(stream));
                Changed = Hash != NewHash;
            }
        }

        public void PromoteNewValues()
        {
            if (NewTimeStampUTC != DateTime.MinValue && NewTimeStampUTC != TimeStampUTC)
                TimeStampUTC = NewTimeStampUTC;
            if (NewHash != null && NewHash != Hash)
                Hash = NewHash;
        }

        [JsonProperty(Required = Required.Always, PropertyName = "hash", NullValueHandling = NullValueHandling.Include)]
        public string Hash { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "id", NullValueHandling = NullValueHandling.Include)]
        public string ID { get; private set; }

        [JsonProperty(Required = Required.Always, PropertyName = "file", NullValueHandling = NullValueHandling.Include)]
        public string RelativeFilename { get; private set; }

        [JsonProperty(Required = Required.Always, PropertyName = "time_stamp", NullValueHandling = NullValueHandling.Include)]
        public DateTime TimeStampUTC { get; private set; }

        [JsonIgnore]
        public bool Changed { get; private set; } = false;
        #endregion

        #region Private Fields
        [JsonIgnore]
        private string NewHash = null;

        [JsonIgnore]
        private DateTime NewTimeStampUTC = DateTime.MinValue;

        [JsonIgnore]
        public bool IsNew { get; private set; } = false;
        #endregion
    }

    [JsonObject]
    public class FileTree
    {
        #region Public API Methods
        public FileTree() { } // Old FileTree Constructor from Serialization...

        public FileTree(string name, string baseFolder) // New FileTree Constuctor...
        {
            Name = name ?? throw new ArgumentNullException("FileTree.ctor: name must not be null!");
            BaseFolder = baseFolder ?? throw new ArgumentNullException("FileTree.ctor: baseFolder must not be null!");
        }

        public void ScanTree()
        {
            stopScanning_ = false;
            scanning_ = true;
            WalkFolder(BaseFolder);
            scanning_ = false;
            stopScanning_ = false;
        }

        public void StopScan()
        {
            stopScanning_ = true;
        }

        public bool HasFilename(string filename)
        {
            return FileMap.ContainsKey(filename);
        }

        public string GetHashForFile(string filename)
        {
            return FileMap[filename].Hash;
        }

        public int Count { get { return FileMap.Count;  } }

        public void SaveIndex(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
                SaveIndex(stream);
        }

        public void SaveIndex(Stream outStream)
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

        public static FileTree LoadIndex(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open))
                return LoadIndex(stream);
        }

        public static FileTree LoadIndex(Stream inStream)
        {
            FileTree result;
            using (var streamReader = new StreamReader(inStream))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = JsonSerializer.Create();
                var fileTree = serializer.Deserialize<FileTree>(reader);
                result = fileTree;
            }
            return result;
        }
        #endregion

        #region Public Properties
        [JsonIgnore]
        public IEnumerable<FileObject> AllFiles { get { return FileMap.Values; } }

        [JsonProperty(Required = Required.Always, PropertyName = "name", NullValueHandling = NullValueHandling.Include)]
        public string Name { get; private set; }

        [JsonProperty(Required = Required.Always, PropertyName = "folder", NullValueHandling = NullValueHandling.Include)]
        public string BaseFolder { get; private set; }
        #endregion

        #region Private Implementation
        private void WalkFolder(string folder)
        {
            if (Directory.Exists(folder))
            {
                var fileList = Directory.GetFiles(folder);
                IndexFiles(fileList);
                var folders = Directory.GetDirectories(folder);
                foreach (var subFolder in folders)
                {
                    WalkFolder(subFolder);
                    if (stopScanning_)
                        return;
                }
            }
        }

        private void IndexFiles(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                string relative = file.Substring(BaseFolder.Length + 1);
                if(HasFilename(relative))
                {
                    FileMap[relative].GetNewValues(file);
                }
                else
                {
                    string guid;
                    do guid = Guid.NewGuid().ToString(""); while (uniqueGuids_.Contains(guid));
                    FileMap[relative] = new FileObject(guid, relative);
                    FileMap[relative].GetNewValues(file);
                    FileMap[relative].PromoteNewValues();
                    uniqueGuids_.Add(FileMap[relative].ID);
                }
                if (stopScanning_)
                    return;
            }
        }
        #endregion

        #region Private Properties and Fields
        [JsonProperty(Required = Required.Always, PropertyName = "file_map", NullValueHandling = NullValueHandling.Include)]
        private Dictionary<string, FileObject> FileMap { get; set; } = new Dictionary<string, FileObject>();

        [JsonIgnore]
        private List<string> uniqueGuids_ = new List<string>();

        [JsonIgnore]
        private bool scanning_ = false;

        [JsonIgnore]
        private bool stopScanning_ = false;
        #endregion
    }
}
