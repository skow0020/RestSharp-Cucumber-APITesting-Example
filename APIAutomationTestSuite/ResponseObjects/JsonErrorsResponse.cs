using Newtonsoft.Json;
using System.Collections.Generic;

namespace APIAutomationTestSuite.ResponseObjects
{
    class JsonErrorsResponse
    {
        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }   
    }

    class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("context")]
        public string Context { get; set; }
    }
}
