using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BuildMonitor.Domain
{
    public class BuildRepository : IBuildRepository
    {
        private readonly JsonSerializer serializer;

        public BuildRepository(string pathToDb)
        {
            if(string.IsNullOrEmpty(pathToDb))
                throw new ArgumentNullException("pathToDb");

            Source = pathToDb;
            serializer = new JsonSerializer {Formatting = Formatting.Indented};
            serializer.Converters.Add(new IsoDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
        }

        public string Source { get; private set; }

        public void Save(IPersistable build)
        {
            using (var fileStream = new FileStream(Source, FileMode.Open, FileAccess.ReadWrite))
            {
                if (fileStream.Length > 0) //remove ] if we have content in file
                {
                    fileStream.Seek(-1, SeekOrigin.End);
                }
                using (var sw = new StreamWriter(fileStream)) //add item to array
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        if (sw.BaseStream.Position == 0) //begin array if this is first item
                        {
                            writer.WriteRawValue("[");
                        }
                        else //we are adding item to existing array
                        {
                            writer.WriteRawValue(",");
                        }

                        serializer.Serialize(writer, build.Data());

                        writer.WriteRawValue("]");
                    }
                }
            }
        }

        public void Save(string data)
        {
            var d = JsonConvert.DeserializeObject<IEnumerable<object>>(data);
            var jsonSerializerSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter());
            var s = JsonConvert.SerializeObject(d, Formatting.Indented, jsonSerializerSettings);

            File.WriteAllText(Source, s);
        }

        public string GetRawData()
        {
            return File.ReadAllText(Source);
        }
    }
}