using Newtonsoft.Json;
using System.Collections.Generic;

namespace APIAutomationTestSuite.ResponseObjects
{
    class JsonWarningsResponse
    {
        [JsonProperty("warnings")]
        public List<Warning> Warning { get; set; }   
    }

    class Warning
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("context")]
        public string Context { get; set; }
    }
}
