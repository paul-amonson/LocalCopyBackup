// Copyright (c) 2017-2018, Paul Amonson
// SPDX-License-Identifier: MIT

#region Using Section

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

#endregion

namespace BackupPlan
{
    [JsonObject]
    public class FileObject
    {
        [JsonIgnore]
        public const int BufferSize = 4194304; // 4MB

        #region Public API
        public FileObject() { }

        public FileObject(string id, string relativeFilename)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            RelativeFilename = relativeFilename ?? throw new ArgumentNullException(nameof(relativeFilename));
            Changed = true;
            IsNew = true;
        }

        public void GetNewValues(string filename)
        {
            _newTimeStampUtc = File.GetLastWriteTimeUtc(filename);
            if (_newTimeStampUtc > TimeStampUtc)
            {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
                using (var sha = new SHA256Managed())
                    _newHash = Encoding.UTF8.GetString(sha.ComputeHash(stream));
                Changed = Hash != _newHash;
            }
        }

        public void PromoteNewValues()
        {
            if (_newTimeStampUtc != DateTime.MinValue && _newTimeStampUtc != TimeStampUtc)
                TimeStampUtc = _newTimeStampUtc;
            if (_newHash != null && _newHash != Hash)
                Hash = _newHash;
        }

        [JsonProperty(Required = Required.Always, PropertyName = "hash", NullValueHandling = NullValueHandling.Include)]
        public string Hash { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "id", NullValueHandling = NullValueHandling.Include)]
        public string Id { get; private set; }

        [JsonProperty(Required = Required.Always, PropertyName = "file", NullValueHandling = NullValueHandling.Include)]
        public string RelativeFilename { get; private set; }

        [JsonProperty(Required = Required.Always, PropertyName = "time_stamp", NullValueHandling = NullValueHandling.Include)]
        public DateTime TimeStampUtc { get; private set; }

        [JsonIgnore]
        public bool Changed { get; private set; }
        #endregion

        #region Private Fields
        [JsonIgnore]
        private string _newHash;

        [JsonIgnore]
        private DateTime _newTimeStampUtc = DateTime.MinValue;

        [JsonIgnore]
        public bool IsNew { get; private set; }
        #endregion
    }

    [JsonObject]
    public class FileTree
    {
        #region Public API Methods
        public FileTree() { } // Old FileTree Constructor from Serialization...

        public FileTree(string name, string baseFolder) // New FileTree Constuctor...
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            BaseFolder = baseFolder ?? throw new ArgumentNullException(nameof(baseFolder));
        }

        public void ScanTree()
        {
            _stopScanning = false;
            _scanning = true;
            WalkFolder(BaseFolder);
            _scanning = false;
            _stopScanning = false;
        }

        public void StopScan()
        {
            _stopScanning = true;
        }

        public bool HasFilename(string filename)
        {
            return FileMap.ContainsKey(filename);
        }

        public string GetHashForFile(string filename)
        {
            return FileMap[filename].Hash;
        }

        public int Count => FileMap.Count;

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
            if (!Directory.Exists(folder))
                return;

            var fileList = Directory.GetFiles(folder);
            if (!IndexFiles(fileList)) return;
            var folders = Directory.GetDirectories(folder);
            foreach (var subFolder in folders)
            {
                WalkFolder(subFolder);
                if (_stopScanning)
                    return;
            }
        }

        private bool IndexFiles(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var relative = file.Substring(BaseFolder.Length + 1);
                if(HasFilename(relative))
                {
                    FileMap[relative].GetNewValues(file);
                }
                else
                {
                    string guid = null;
                    for (var count = 0; count <= 5000; count++)
                    {
                        if (count == 5000)
                        {
                            return false;
                        }
                        guid = Guid.NewGuid().ToString("");
                        if (!_uniqueGuids.Contains(guid))
                        {
                            break;
                        }
                        guid = null;
                        System.Threading.Thread.Sleep(10);
                    }
                    FileMap[relative] = new FileObject(guid, relative);
                    FileMap[relative].GetNewValues(file);
                    FileMap[relative].PromoteNewValues();
                    _uniqueGuids.Add(FileMap[relative].Id);
                }
                if (_stopScanning)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Private Properties and Fields
        [JsonProperty(Required = Required.Always, PropertyName = "file_map", NullValueHandling = NullValueHandling.Include)]
        private Dictionary<string, FileObject> FileMap { get; set; } = new Dictionary<string, FileObject>();

        [JsonIgnore]
        private readonly List<string> _uniqueGuids = new List<string>();

        [JsonIgnore]
        private bool _scanning;

        [JsonIgnore]
        private bool _stopScanning;
        #endregion
    }
}
