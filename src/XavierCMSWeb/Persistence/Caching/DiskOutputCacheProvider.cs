﻿// GNU Version 3 License Copyright (c) 2020 Javier Cañon | https://javiercanon.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.IO;

namespace XavierCMSWeb.Persistence.Caching
{
    public class DiskOutputCacheProvider : OutputCacheProvider
    {
        static readonly object _lockObject = new object();

        private Dictionary<string, DiskOutputCacheItem> _cacheItems = new Dictionary<string, DiskOutputCacheItem>();
        private Dictionary<string, DiskOutputCacheItem> CacheItems
        {
            get
            {
                return _cacheItems;
            }
        }

        private string _cacheFolder = HostingEnvironment.ApplicationPhysicalPath + @"App_Data\Cache\";
        public string CacheFolder
        {
            get
            {
                return _cacheFolder;
            }
        }

        public virtual string LogPath
        {
            get
            {
                return Path.Combine(this.CacheFolder, "log.txt");
            }
        }


        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (!string.IsNullOrEmpty(config["cacheFolder"]))
            {
                _cacheFolder = config["cacheFolder"];
                if (!_cacheFolder.EndsWith(@"\"))
                    _cacheFolder += @"\";

                config.Remove("cacheFolder");
            }

            base.Initialize(name, config);
        }

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            LogAction("Add", string.Format("Key: {0} | UtcExpiry: {1}", key, utcExpiry.ToString()));

            // See if this key already exists in the cache. If so, we need to return it and NOT overwrite it!
            var results = this.Get(key);
            if (results != null)
                return results;
            else
            {
                // If the item is NOT in the cache, then save it!
                this.Set(key, entry, utcExpiry);

                return entry;
            }
        }

        public override object Get(string key)
        {
            LogAction("Get", string.Format("Key: {0}", key));

            DiskOutputCacheItem item = null;
            CacheItems.TryGetValue(key, out item);

            // Was the item found?
            if (item == null)
                return null;

            // Has the item expired?
            if (item.UtcExpiry < DateTime.UtcNow)
            {
                // Item has expired
                this.Remove(key);

                return null;
            }

            return GetCacheData(item);
        }

        public override void Remove(string key)
        {
            LogAction("Remove", string.Format("Key: {0}", key));

            DiskOutputCacheItem item = null;
            this.CacheItems.TryGetValue(key, out item);

            if (item != null)
            {
                // Attempt to delete the cached content on disk and then remove the item from CacheItems... 
                // If there is a problem, fail silently
                //try
                //{
                RemoveCacheData(item);
                CacheItems.Remove(key);
                //}
                //catch { }
            }
        }

        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            LogAction("Set", string.Format("Key: {0} | UtcExpiry: {1}", key, utcExpiry.ToString()));

            // Create a DiskOutputCacheItem object
            var item = new DiskOutputCacheItem(this.GetSafeFileName(key), utcExpiry);

            WriteCacheData(item, entry);

            // Add item to CacheItems, if needed, or update the existing key, if it already exists
            lock (_lockObject)
            {
                if (this.CacheItems.ContainsKey(key))
                    this.CacheItems[key] = item;
                else
                    this.CacheItems.Add(key, item);
            }
        }

        protected virtual object GetCacheData(DiskOutputCacheItem item)
        {
            var fileToRetrieve = Path.Combine(this.CacheFolder, item.FileName);

            if (File.Exists(fileToRetrieve))
            {
                var formatter = new BinaryFormatter();
                using (var stream = new FileStream(fileToRetrieve, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return formatter.Deserialize(stream);
                }
            }
            else
                // Eep, could not find the file!
                return null;
        }

        protected virtual void RemoveCacheData(DiskOutputCacheItem item)
        {
            var fileToRetrieve = Path.Combine(this.CacheFolder, item.FileName);
            if (File.Exists(fileToRetrieve))
                File.Delete(fileToRetrieve);
        }

        protected virtual void WriteCacheData(DiskOutputCacheItem item, object entry)
        {
            var fileToWrite = Path.Combine(this.CacheFolder, item.FileName);

            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(fileToWrite, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, entry);
            }
        }

        protected virtual string GetSafeFileName(string unsafeFileName)
        {
            var safeFileName = unsafeFileName;

            foreach (char c in Path.GetInvalidFileNameChars())
                safeFileName = safeFileName.Replace(c.ToString(), "_");

            return safeFileName;
        }


        protected virtual void LogAction(string actionName, string details)
        {
            var context = HttpContext.Current;

            // Only log when debugging is enabled...
            if (context != null && context.IsDebuggingEnabled)
            {
                if (!Directory.Exists(this.CacheFolder))
                    Directory.CreateDirectory(this.CacheFolder);

                using (FileStream fs = new FileStream(this.LogPath,
                                          FileMode.Append,
                                          FileAccess.Write,
                                          FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {

                        sw.WriteLine(string.Format("[{0} - {1}] {2}",
                                                                DateTime.Now.ToString(),
                                                                actionName,
                                                                details));
                    }
                }


            }

        }
    }
}