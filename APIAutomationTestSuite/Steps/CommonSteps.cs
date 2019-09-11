using APIAutomationTestSuite.ResponseObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Xml;
using TechTalk.SpecFlow;

namespace APIAutomationTestSuite.Steps
{
    [Binding]
    public sealed class CommonSteps
    {
        public IList<string> messages;

        [Given(@"I want to (.*) resource (.*)")]
        public void GivenIWantToAccessResource(string verb, string resource)
        {
            RestSharp.Method method;

            switch (verb.ToUpper())
            {
                case "GET":
                    method = RestSharp.Method.GET;
                    break;
                case "PUT":
                    method = RestSharp.Method.PUT;
                    break;
                case "ACCESS SOAP":
                case "POST":
                    method = RestSharp.Method.POST;
                    break;
                case "DELTE":
                    method = RestSharp.Method.DELETE;
                    break;                
                default:
                    throw new System.Exception("Step method was not understood");
            }
            RestApiHelper.CreateRequest(method, resource);
        }

        [When(@"I add parameters")]
        public void WhenIAddParameters(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                string name = row[0], value = row[1];
                RestApiHelper.AddRequestQueryParameter(name, value);
            }
        }

        [When(@"I add a SOAP envelope to the request")]
        public void WhenIAddASOAPEnvelopeToTheRequest(string soapEnvelope)
        {
            RestApiHelper.SetSOAPRequestEnvelope(soapEnvelope);
        }

        [When(@"I provide the JSON request object")]
        public void WhenIProvideTheJSONRequestObject(string jsonRequestObject)
        {
            RestApiHelper.AddJSONRequestObject(jsonRequestObject);
        }

        [When(@"I execute the rest request")]
        public void WhenIExecuteTheRestRequest()
        {
            RestApiHelper.ExecuteRequest();
        }

        [Then(@"the API response contains '(.*)'")]
        public void ThenTheAPIResponseContains(string responseText)
        {
            var apiResponse = RestApiHelper.GetResponse();

            apiResponse.Content.ShouldContain(responseText);
        }

        [Then(@"the API response code is OK")]
        public void ThenTheAPIResponseCodeIsOk()
        {
            var apiResponse = RestApiHelper.GetResponse();

            apiResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK, $"Error message: {apiResponse.ErrorMessage}");
        }

        [Then(@"the API response code is NOTFOUND")]
        public void ThenTheAPIResponseCodeIsNotFound()
        {
            var apiResponse = RestApiHelper.GetResponse();

            apiResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound, $"Error message: {apiResponse.ErrorMessage}");
        }

        [Then(@"the API response code is InternalServerError")]
        public void ThenTheAPIResponseCodeIsInternalServerError()
        {
            var apiResponse = RestApiHelper.GetResponse();

            apiResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError, $"Error message: {apiResponse.ErrorMessage}");
        }

        [Then(@"the API response times out")]
        public void ThenTheAPIResponseTimesOut()
        {
            var apiResponse = RestApiHelper.GetResponse();

            apiResponse.StatusCode.ShouldNotBe(System.Net.HttpStatusCode.OK);
            apiResponse.ResponseStatus.ToString().ShouldBe("TimedOut");
        }

        [Then(@"the response schema matches '(.*)'")]
        public void ThenTheResponseSchemaMatches(string schemaFile)
        {
            JSchema schema = Utilities.ParseSchema($"APIAutomationTestSuite.data.schema.{schemaFile}");

            var response = RestApiHelper.GetResponse();

            JObject responseJsonObject = JObject.Parse(response.Content);

            responseJsonObject.IsValid(schema, out messages).ShouldBe(true, string.Join(",", messages));
        }

        [Then(@"the SOAP response schema matches '(.*)'")]
        public void ThenTheSOAPResponseSchemaMatches(string schemaFile)
        {
            JSchema schema = Utilities.ParseSchema($"APIAutomationTestSuite.data.schema.{schemaFile}");

            var response = RestApiHelper.GetResponse();

            XmlDocument responseXMLDoc = new XmlDocument();
            responseXMLDoc.LoadXml(response.Content);

            string jsonResponse = JsonConvert.SerializeXmlNode(responseXMLDoc);
            JObject jsonObject = JObject.Parse(jsonResponse);
            jsonObject.IsValid(schema, out messages).ShouldBe(true, string.Join(",", messages));
        }

        [Then(@"the response contains json error code '(.*)'")]
        public void ThenTheResponseContainsJsonErrorCode(string errorCode)
        {
            var response = RestApiHelper.GetResponse();
            JsonErrorsResponse resp = JsonConvert.DeserializeObject<JsonErrorsResponse>(response.Content);

            var firstError = resp.Errors[0];

            Assert.That(firstError.Code, Is.EqualTo(errorCode));
        }

        [Then(@"the response contains json warning code '(.*)'")]
        public void ThenTheResponseContainsJsonWarningCode(string warningCode)
        {
            var response = RestApiHelper.GetResponse();
            JsonWarningsResponse resp = JsonConvert.DeserializeObject<JsonWarningsResponse>(response.Content);

            var firstError = resp.Warning[0];

            Assert.That(firstError.Code, Is.EqualTo(warningCode));
        }
    }
}
