using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.IO;
using System.Reflection;

namespace APIAutomationTestSuite
{
    public class Utilities
    {
        /// <summary>
        /// Parse a schema file and return it as JSchema
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static JSchema ParseSchema(string resourcePath)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            StreamReader file = new StreamReader(
                assembly.GetManifestResourceStream(resourcePath)
            );
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                return JSchema.Load(reader);
            }
        }

        /// <summary>
        /// Get JSON data from embedded resource file about the service under test
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static dynamic GetService(string service)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var serviceJson = new JObject(); ;

            using (Stream stream = assembly.GetManifestResourceStream($"APIAutomationTestSuite.env.json"))
            using (StreamReader reader = new StreamReader(stream))
            {
                serviceJson = JObject.Parse(reader.ReadToEnd());
            }

            return serviceJson[service];
        }
    }
}
