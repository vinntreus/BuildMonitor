using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
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
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
        }

        public void Save(IPersistable build)
        {
            using (var sw = new StreamWriter(pathToDb, true))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, build.Data());
            }   
        }
    }
}