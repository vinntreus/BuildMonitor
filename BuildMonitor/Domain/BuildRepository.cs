using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BuildMonitor.Domain
{
    public class BuildRepository : IBuildRepository
    {
        private readonly string pathToDb;
        private readonly JsonSerializer serializer;

        public BuildRepository(string pathToDb)
        {
            if(string.IsNullOrEmpty(pathToDb))
                throw new ArgumentNullException("pathToDb");

            this.pathToDb = pathToDb;
            serializer = new JsonSerializer();
            serializer.Converters.Add(new IsoDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
        }

        public void Save(IPersistable build)
        {
            using(var fileStream = new FileStream(pathToDb, FileMode.Open, FileAccess.ReadWrite))
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
    }
}