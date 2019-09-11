using System;
using System.Configuration;
using TechTalk.SpecFlow;

namespace APIAutomationTestSuite.Steps
{
    [Binding]
    public class Hooks
    {
        private dynamic _env;

        [BeforeScenario]
        public void BeforeScenario()
        {
            _env = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("environment")) 
                ? ConfigurationManager.AppSettings["environment"] 
                : Environment.GetEnvironmentVariable("environment");

            RestApiHelper.CreateRestClient();
        }

        [BeforeScenario("ExampleServices")]
        public void BeforeScenarioDataServices()
        {
            dynamic exampleServices = Utilities.GetService("exampleServices")[_env];

            SetupClient(exampleServices);
        }

        /// <summary>
        /// Setup RestClient with credentials and base url
        /// </summary>
        /// <param name="service"></param>
        private void SetupClient(dynamic service)
        {
            if (service == null) throw new NullReferenceException("Unable to set up client - make sure you set the correct environment for this service");

            string username = service.credentials.username;
            string password = service.credentials.password;

            RestApiHelper.SetBaseUrl((string)service.uri);

            RestApiHelper.SetAuthenticator(username, password);
        }
    }
}
