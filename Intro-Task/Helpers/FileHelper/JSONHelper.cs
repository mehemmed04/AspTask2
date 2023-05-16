using Intro_Task.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Intro_Task.Helpers.FileHelper
{
    public class JSONHelper
    {
        public static void WriteListToFile<T>(List<T> list, string fileName)
        {
            var serializer = new JsonSerializer();

            using (var sw = new StreamWriter(fileName))
            {
                using (var jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    serializer.Serialize(jw, list);
                }
            }
        }

        public static List<T> ReadListFromFile<T>(string fileName)
        {
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(fileName))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    var obj = serializer.Deserialize<List<T>>(jr);
                    return obj;
                }
            }
        }
    }
}
