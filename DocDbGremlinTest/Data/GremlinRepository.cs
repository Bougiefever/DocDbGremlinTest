using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Graphs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DocDbGremlinTest.Data
{
    public class GremlinRepository<T>
    {

        private string _endpoint = "https://paperstreetgraph.documents.azure.com:443/";
        private string _authKey = "Dj107GQ3rUdN40rAPZcjuIzCPBAg3RAfMcjxHqRVIZqvptBfTXa0J9Vzpaei04X2xm6pEYIyTLL2kS0E0AAkbg==";

        protected DocumentClient Client { get; set; }

        protected Database Database { get; set; }

        protected DocumentCollection Graph { get; set; }
        
        protected async Task CreateGraphItem(string graphQuery)
        {
            DocumentClient client = new DocumentClient(
                new Uri(_endpoint),
                _authKey,
                new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
            Database database = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "DuffandPhelps" });

            DocumentCollection graph = await client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri("DuffandPhelps"),
                new DocumentCollection { Id = "AssetGraph" },
                new RequestOptions { OfferThroughput = 1000 });

            IDocumentQuery<dynamic> query = client.CreateGremlinQuery<dynamic>(graph, graphQuery);
            dynamic result = await query.ExecuteNextAsync<T>();
        }

        protected async Task<object> GetGraphItem(string id)
        {
            DocumentClient client = new DocumentClient(
                new Uri(_endpoint),
                _authKey,
                new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
            Database database = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "DuffandPhelps" });

            DocumentCollection graph = await client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri("DuffandPhelps"),
                new DocumentCollection { Id = "AssetGraph" },
                new RequestOptions { OfferThroughput = 1000 });

            IDocumentQuery<dynamic> query = client.CreateGremlinQuery<dynamic>(graph, $"g.V('{id}')");
            var result = query.ExecuteNextAsync<dynamic>();
            return result;
        }

        public async Task Test(string id)
        {
            DocumentClient client = new DocumentClient(
                new Uri(_endpoint),
                _authKey,
                new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
            Database database = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "DuffandPhelps" });

            DocumentCollection graph = await client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri("DuffandPhelps"),
                new DocumentCollection { Id = "AssetGraph" },
                new RequestOptions { OfferThroughput = 1000 });
            var ids = new List<string>();
            var q = $"g.V('{id}').repeat(out()).until({true}).path()";
            IDocumentQuery<dynamic> query = client.CreateGremlinQuery<dynamic>(graph, q);
            while (query.HasMoreResults)
            {
                foreach (dynamic result in await query.ExecuteNextAsync())
                {
                    var json = JsonConvert.SerializeObject(result, Formatting.None);
                    dynamic relatedItem = JObject.Parse(json);
                    string i = relatedItem.id;
                    ids.Add(i);
                    //Console.WriteLine($"\t {it}");
                }
            }
        }

        protected async Task<IEnumerable<string>> GetRelated(string graphQuery)
        {
            DocumentClient client = new DocumentClient(
                new Uri(_endpoint),
                _authKey,
                new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
            Database database = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "DuffandPhelps" });

            DocumentCollection graph = await client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri("DuffandPhelps"),
                new DocumentCollection { Id = "AssetGraph" },
                new RequestOptions { OfferThroughput = 1000 });
            IDocumentQuery<dynamic> query = client.CreateGremlinQuery<dynamic>(graph, graphQuery);

            var ids = new List<string>();
            while (query.HasMoreResults)
            {
                foreach (dynamic result in await query.ExecuteNextAsync())
                {
                    var json = JsonConvert.SerializeObject(result, Formatting.None);
                    dynamic relatedItem = JObject.Parse(json);
                    string id = relatedItem.id;
                    ids.Add(id);
                    //Console.WriteLine($"\t {it}");
                }
            }

            return ids;
        }

        public async Task Setup()
        {
            string endpoint = "https://paperstreetgraph.documents.azure.com:443/";
            string authKey = "Dj107GQ3rUdN40rAPZcjuIzCPBAg3RAfMcjxHqRVIZqvptBfTXa0J9Vzpaei04X2xm6pEYIyTLL2kS0E0AAkbg==";

            DocumentClient client = new DocumentClient(
                new Uri(endpoint),
                authKey,
                new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
            Database database = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "graphdb" });

            DocumentCollection graph = await client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri("graphdb"),
                new DocumentCollection { Id = "graphcollz" },
                new RequestOptions { OfferThroughput = 1000 });

            Client = client;
            Database = database;
            Graph = graph;
        }
    }
}