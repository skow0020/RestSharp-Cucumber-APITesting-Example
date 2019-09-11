using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace APIAutomationTestSuite
{
    public class RestApiHelper
    {
        private static RestClient client;
        private static RestRequest restRequest;
        private static IRestResponse restResponse;

        /// <summary>
        /// Initialize the rest client with a 10 second timeout
        /// </summary>
        /// <returns></returns>
        public static void CreateRestClient()
        {
            client = new RestClient();
            client.Timeout = 20000;
        }

        /// <summary>
        /// Apply the rest client with a base url
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static void SetBaseUrl(string baseUrl)
        {
            client.BaseUrl = new Uri(baseUrl);
        }

        /// <summary>
        /// Set the rest client basic authenticator with credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static void SetAuthenticator(string username, string password)
        {
            client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        /// <summary>
        /// Create the initial request object with a specified type and return it
        /// </summary>
        /// <param name="type"></param>
        /// /// <param name="endpoint"></param>
        /// <returns></returns>
        public static void CreateRequest(Method type, string endpoint)
        {
            restRequest = new RestRequest(endpoint, type);
        }

        /// <summary>
        /// Add a SOAP request envelope to the request
        /// </summary>
        /// <param name="envelope"></param>
        /// <returns></returns>
        public static void SetSOAPRequestEnvelope(string envelope)
        {
            restRequest.RequestFormat = DataFormat.Xml;
            restRequest.AddParameter("text/xml", envelope, ParameterType.RequestBody);
        }

        /// <summary>
        /// Add JSON request object as a parameter
        /// </summary>
        /// <param name="jsonRequestObject"></param>
        /// <returns></returns>
        public static void AddJSONRequestObject(string jsonRequestObject)
        {
            restRequest.AddParameter("text/json", jsonRequestObject, ParameterType.RequestBody);
        }

        /// <summary>
        /// Add a header to the rest client
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void AddDefaultHeader(string key, string value)
        {
            client.AddDefaultHeader(key, value);
        }

        /// <summary>
        /// Add a parameter to the request object by name and value and return the request object
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void AddRequestQueryParameter(string name, string value)
        {
            restRequest.AddParameter(name, value, ParameterType.QueryString);
        }

        /// <summary>
        /// Execute the request and return the response 
        /// </summary>
        /// <returns></returns>
        public static void ExecuteRequest()
        {
            restResponse = client.Execute(restRequest);
        }

        /// <summary>
        /// Get rest response from the helper
        /// </summary>
        /// <returns></returns>
        public static IRestResponse GetResponse()
        {
            return restResponse;
        }

        /// <summary>
        /// Set Token for Two factor Authntication
        /// </summary>
        /// <returns></returns>
        public static void SetTwoFactorAuthentication()
        {
           AddDefaultHeader("Authorization", $"Bearer {GetBearerTokenForTwoFactorAuthentication()}");
        }

        /// <summary>
        /// Get Bearer Token for two factor authentication
        /// </summary>
        /// <returns></returns>
        private static string GetBearerTokenForTwoFactorAuthentication()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var client = new RestClient("");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", ConfigurationManager.AppSettings["client_id"]);
            request.AddParameter("client_secret", ConfigurationManager.AppSettings["client_secret"]);
            IRestResponse response = client.Execute(request);

            var responseJson = response.Content;
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["access_token"].ToString();           
        }       
    }
}