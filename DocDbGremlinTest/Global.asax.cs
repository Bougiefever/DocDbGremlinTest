using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DocDbGremlinTest.Data;

namespace DocDbGremlinTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string endpoint = "https://paperstreetgraph.documents.azure.com:443/";
            string authKey = "Dj107GQ3rUdN40rAPZcjuIzCPBAg3RAfMcjxHqRVIZqvptBfTXa0J9Vzpaei04X2xm6pEYIyTLL2kS0E0AAkbg==";

            //Microsoft.Azure.Documents.Client.DocumentClient client = new Microsoft.Azure.Documents.Client.DocumentClient(
            //    new Uri(endpoint),
            //    authKey,
            //    new Microsoft.Azure.Documents.Client.ConnectionPolicy { ConnectionMode = Microsoft.Azure.Documents.Client.ConnectionMode.Direct, ConnectionProtocol = Microsoft.Azure.Documents.Client.Protocol.Tcp});
            //Setup(client).Wait();
        }

        private async Task Setup(Microsoft.Azure.Documents.Client.DocumentClient client)
        {
            Microsoft.Azure.Documents.Database database = await client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database { Id = "graphdb" });

            Microsoft.Azure.Documents.DocumentCollection graph = await client.CreateDocumentCollectionIfNotExistsAsync(
                Microsoft.Azure.Documents.Client.UriFactory.CreateDatabaseUri("graphdb"),
                new Microsoft.Azure.Documents.DocumentCollection { Id = "graphcollz" },
                new Microsoft.Azure.Documents.Client.RequestOptions { OfferThroughput = 1000 });
        }
    }
}
