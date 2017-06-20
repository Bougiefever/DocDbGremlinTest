using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocDbGremlinTest.Models;

namespace DocDbGremlinTest.Data
{
    public class ItemGraphRepository : GremlinRepository<Item>
    {

        public async Task AddItem(Item item)
        {
            string itemType = item.ItemType.ToString().ToLower();
            var queryString = $"g.addV('{itemType}').property('id', '{item.Id}').property('accountid', '1')";
            if (item is Asset)
            {
                queryString += ".property('isAsset', 'true')";
            }
            await base.CreateGraphItem(queryString);
        }

        public async Task<IEnumerable<string>> GetAssetsInBuilding(string id)
        {
            IList<string> ids = new List<string>() { id };
            //var floorsQuery = GetRelated(id, "has", "floor");
            var roomsQuery = "g.V('{id}').outE('has').inV().hasLabel('floor').outE('has').inV().hasLabel('room')";
            var roomIds = await base.GetRelated(roomsQuery);
            return ids;
        }

        public async Task AddRelationship(string id1, string id2, string relationship)
        {
            var queryString = $"g.V('{id1}').addE('{relationship}').to(g.V('{id2}'))";
            await base.CreateGraphItem(queryString);
        }

        public async Task RemoveRelationship(string id1, string id2, string relationship)
        {
            var queryString = $"g.V('{id1}').outE('{relationship}').where(inV().has('id', '{id2}')).drop()";
            await base.CreateGraphItem(queryString);
        }

        public async Task<IEnumerable<string>> GetRelated(string id, string relationship, string targetType)
        {
            var queryString = $"g.V('{id}').outE('{relationship}').inV().hasLabel('{targetType}')";
            return await base.GetRelated(queryString);
        }

        //public async Task<IEnumerable<string>> GetAssetsInTree(string id)
        //{
        //    //v1.out('childOf').loop(1){true}{true}.gather{it.add(v1);it._().out('knows')}.scatter().map()
        //    var v1 = 
        //    //var queryString = $"g.V('{id}').out('has').loop(1) {true}.gather(it.add(";
        //}

        public async Task Setup()
        {
            string endpoint = "https://paperstreetgraph.documents.azure.com:443/";
            string authKey = "Dj107GQ3rUdN40rAPZcjuIzCPBAg3RAfMcjxHqRVIZqvptBfTXa0J9Vzpaei04X2xm6pEYIyTLL2kS0E0AAkbg==";

            Microsoft.Azure.Documents.Client.DocumentClient client = new Microsoft.Azure.Documents.Client.DocumentClient(
                new Uri(endpoint),
                authKey,
                new Microsoft.Azure.Documents.Client.ConnectionPolicy { ConnectionMode = Microsoft.Azure.Documents.Client.ConnectionMode.Direct, ConnectionProtocol = Microsoft.Azure.Documents.Client.Protocol.Tcp });
            Microsoft.Azure.Documents.Database database = await client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database { Id = "graphdb" });

            Microsoft.Azure.Documents.DocumentCollection graph = await client.CreateDocumentCollectionIfNotExistsAsync(
                Microsoft.Azure.Documents.Client.UriFactory.CreateDatabaseUri("graphdb"),
                new Microsoft.Azure.Documents.DocumentCollection { Id = "graphcollz" },
                new Microsoft.Azure.Documents.Client.RequestOptions { OfferThroughput = 1000 });
            
        }
    }
}