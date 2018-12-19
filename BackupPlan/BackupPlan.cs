// Copyright (c) 2017-2018, Paul Amonson
// SPDX-License-Identifier: MIT

#region Using Section

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using Backup;
using Newtonsoft.Json;

#endregion

namespace BackupPlan
{
    public delegate void LogCallback(string text);

    [JsonObject]
    public class BackupPlan
    {
        #region Constants
        const int BufferSize = 4194304; // 4 MB Buffer.
        #endregion

        #region API Methods
        public BackupPlan() { }

        public BackupPlan(string backupName, string folderToBackup, string destinationFolder, string indexName=null, LogCallback callback = null)
        {
            Callback = callback;
            key_ = new KeyData();
            if (backupName == null || backupName.Trim() == "")
                throw new ArgumentNullException(nameof(backupName));
            Name = backupName;
            if (folderToBackup == null || folderToBackup.Trim() == "")
                throw new ArgumentNullException(nameof(folderToBackup));
            if (Directory.Exists(folderToBackup) == false)
                throw new DirectoryNotFoundException("BackupPlan.ctor: folderToBackup was not found!");
            FolderToBackup = folderToBackup;
            if(destinationFolder == null || destinationFolder.Trim() == "")
                throw new ArgumentNullException(nameof(destinationFolder));
            if (Directory.Exists(destinationFolder) == false)
                throw new DirectoryNotFoundException("BackupPlan.ctor: destinationFolder was not found!");
            DestinationFolder = destinationFolder;
            if (indexName != null && indexName.Trim() != "")
                IndexName = indexName;
            else
                IndexName = FindUniqueGuid(Directory.EnumerateFiles(DestinationFolder, "*.index"));
        }

        public bool RunBackup()
        {
            haltRun_ = false;
            if (File.Exists(IndexFilename))
                currentFileTree_ = LoadIndex(IndexFilename);
            else
                currentFileTree_ = new FileTree(Name, FolderToBackup);
            currentFileTree_.ScanTree();
            bool result = RunBackupUpdate();
            SaveIndex(IndexFilename, currentFileTree_);
            return result;
        }

        public bool RunRestore(string newDestination=null)
        {
            haltRun_ = false;
            if (newDestination == null)
                newDestination = FolderToBackup;
            if (File.Exists(IndexFilename))
            {
                currentFileTree_ = LoadIndex(IndexFilename);
                return RestoreFiles(currentFileTree_.AllFiles, newDestination);
            }
            return false;
        }

        static public BackupPlan Load(string filename, LogCallback callback = null)
        {
            using (var stream = new FileStream(filename, FileMode.Open))
                return Load(stream, callback);
        }

        static public BackupPlan Load(Stream stream, LogCallback callback = null)
        {
            using (var streamReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = JsonSerializer.Create();
                var plan = serializer.Deserialize<BackupPlan>(reader);
                plan.Callback = callback;
                return plan;
            }
        }

        static public BackupPlan ImportFromIndexFile(KeyData key, string indexFileName, LogCallback callback)
        {
            BackupPlan result = new BackupPlan()
            {
                Key = key,
                Callback = callback,
                DestinationFolder = Path.GetDirectoryName(indexFileName),
                IndexName = Path.GetFileNameWithoutExtension(indexFileName)
            };
            var tree = result.LoadIndex(indexFileName);
            result.FolderToBackup = tree.BaseFolder;
            result.Name = tree.Name;
            return result;
        }

        public void Save(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
                Save(stream);
        }

        public void Save(Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                writer.Indentation = 2;
                writer.IndentChar = ' ';
                writer.Formatting = Formatting.Indented;
                var serializer = JsonSerializer.Create();
                serializer.Serialize(writer, this);
            }
        }

        public void HaltRun()
        {
            haltRun_ = true;
            if (currentFileTree_ != null)
                currentFileTree_.StopScan();
        }
        #endregion

        #region Public Properties
        [JsonIgnore]
        public string IndexFilename { get { return Path.Combine(DestinationFolder, IndexName) + ".index"; } }

        [JsonProperty(Required = Required.Always, PropertyName = "name", NullValueHandling = NullValueHandling.Include)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "folder_to_backup", NullValueHandling = NullValueHandling.Include)]
        public string FolderToBackup { get; private set; }

        [JsonProperty(Required = Required.Always, PropertyName = "destination_folder", NullValueHandling = NullValueHandling.Include)]
        public string DestinationFolder { get; private set; }

        [JsonProperty(Required = Required.Always, PropertyName = "index_name", NullValueHandling = NullValueHandling.Include)]
        public string IndexName { get; private set; }

        [JsonIgnore]
        public KeyData Key { get { return key_; } set { key_ = value; } }

        [JsonIgnore]
        public object Tag { get; set; } = null;

        [JsonIgnore]
        public LogCallback Callback { get; set; }
        #endregion

        #region Private Implementation
        private FileTree LoadIndex(string filename)
        {
            var aes = Aes.Create();
            aes.Key = key_.Key;
            aes.IV = key_.IV;
            using (var inStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
            {
                var transform = aes.CreateDecryptor();
                using (var cryptStream = new CryptoStream(inStream, transform, CryptoStreamMode.Read))
                    return FileTree.LoadIndex(cryptStream);
            }
        }

        private void SaveIndex(string filename, FileTree tree)
        {
            var aes = Aes.Create();
            aes.Key = key_.Key;
            aes.IV = key_.IV;
            using (var outStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Write, BufferSize))
            {
                var transform = aes.CreateEncryptor();
                using (var cryptStream = new CryptoStream(outStream, transform, CryptoStreamMode.Write))
                    currentFileTree_.SaveIndex(cryptStream);
            }
        }

        private bool RunBackupUpdate()
        {
            var changedFiles = new List<FileObject>();
            int newCount = 0;
            int changedCount = 0;
            foreach (var file in currentFileTree_.AllFiles)
                if(file.Changed)
                {
                    changedFiles.Add(file);
                    if (file.IsNew)
                        newCount++;
                    else
                        changedCount++;
                }
            Log("*** INFO: Backing up {0} of {1} files...", changedFiles.Count, currentFileTree_.Count);
            Log("*** INFO: There are {0} new files and {1} updated files...", newCount, changedCount);
            return BackupFiles(changedFiles);
        }

        private bool BackupFiles(IEnumerable<FileObject> fileMetadata)
        {
            bool result = true;
            foreach(var file in fileMetadata)
            {
                if (haltRun_)
                    return false;
                try {
                    var folder = Path.Combine(DestinationFolder, IndexName);
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    BackupCopy(Path.Combine(FolderToBackup, file.RelativeFilename), Path.Combine(DestinationFolder, IndexName, file.Id), key_);
                    file.PromoteNewValues();
                } catch (Exception ex) {
                    Log("*** ERROR: failed to backup file {0}: {1}: {2}", Path.Combine(FolderToBackup, file.RelativeFilename), ex.GetType().Name, ex.Message);
                    result = false;
                }
            }
            return result;
        }

        private void BackupCopy(string source, string destination, KeyData key)
        {
            Log("*** BACKUP:  '{0}' ==> '{1}'", source, destination);
            Copy(source, destination, true, key);
        }

        private bool RestoreFiles(IEnumerable<FileObject> files, string newDestination)
        {
            bool result = true;
            foreach (var file in files)
            {
                if (haltRun_)
                    return false;
                try
                {
                    RestoreCopy(Path.Combine(DestinationFolder, IndexName, file.Id), Path.Combine(newDestination, file.RelativeFilename), key_);
                } catch (Exception ex) {
                    Log("*** ERROR: failed to restore file {0}: {1}", Path.Combine(newDestination, file.RelativeFilename), ex.Message);
                    result = false;
                }
            }
            return result;
        }

        private void RestoreCopy(string source, string destination, KeyData key, bool force=false)
        {
            if (Directory.Exists(Path.GetDirectoryName(destination)) == false)
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
            Log("*** RESTORE: '{0}' ==> '{1}'", source, destination);
            var backupTime = File.GetLastWriteTimeUtc(source);
            var originalTime = File.GetLastWriteTimeUtc(destination);
            if (originalTime >= backupTime && !force)
                Log("*** SKIPPING: '{0}': Destination is unchanged or newer than the backup!", destination);
            else
                Copy(source, destination, false, key);
        }

        private void Copy(string source, string destination, bool backup, KeyData key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key.Key;
                aes.IV = key.IV;
                if (backup)
                {
                    using (var fileStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
                    using (var outStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.Write, BufferSize))
                    {
                        var transform = aes.CreateEncryptor();
                        using (var cryptStream = new CryptoStream(outStream, transform, CryptoStreamMode.Write))
                        using (var compressStream = new DeflateStream(cryptStream, CompressionMode.Compress))
                            fileStream.CopyTo(compressStream);
                    }
                }
                else
                {
                    using (var fileStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
                    using (var outStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.Write, BufferSize))
                    {
                        var transform = aes.CreateDecryptor();
                        using (var cryptStream = new CryptoStream(fileStream, transform, CryptoStreamMode.Read))
                        using (var compressStream = new DeflateStream(cryptStream, CompressionMode.Decompress))
                            compressStream.CopyTo(outStream);
                    }
                }
            }
        }

        private void Log(string fmt, params object[] args)
        {
            if (Callback == null)
                Console.WriteLine(fmt, args);
            else
                Callback(string.Format(fmt, args));
        }

        private string FindUniqueGuid(IEnumerable<string> existingFiles)
        {
            var existingIndexNames = new List<string>();
            foreach (var file in existingFiles)
                existingIndexNames.Add(Path.GetFileNameWithoutExtension(file));
            string name;
            do name = Guid.NewGuid().ToString(""); while (existingIndexNames.Contains(name));
            return name;
        }
        #endregion

        #region Private Fields
        [JsonIgnore]
        private FileTree currentFileTree_;

        [JsonProperty(Required = Required.Always, PropertyName = "key", NullValueHandling = NullValueHandling.Include)]
        private KeyData key_;

        [JsonIgnore]
        private bool haltRun_ = false;
        #endregion
    }

    public class BackupPlanList : List<BackupPlan>
    {
        public static BackupPlanList Load(string filename, LogCallback callback = null)
        {
            using (var stream = new FileStream(filename, FileMode.Open))
                return Load(stream, callback);
        }

        public static BackupPlanList Load(Stream stream, LogCallback callback = null)
        {
            using (var streamReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = JsonSerializer.Create();
                var list = serializer.Deserialize<BackupPlanList>(reader);
                foreach (var plan in list)
                    plan.Callback = callback;
                return list;
            }
        }

        public void Save(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
                Save(stream);
        }

        public void Save(Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                writer.Indentation = 2;
                writer.IndentChar = ' ';
                writer.Formatting = Formatting.Indented;
                var serializer = JsonSerializer.Create();
                serializer.Serialize(writer, this);
            }
        }
    }
}
