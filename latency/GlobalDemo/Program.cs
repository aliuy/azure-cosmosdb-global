using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
namespace GlobalDemo
{
    class Program
    {
        private const string EndpointUri = "https://andrl-dev2.documents.azure.com:443/";
        private const string PrimaryKey = "rtuhaRxBZ0GoJnQfTyBJuDBVWWKnt9mjyieD6VokKgQts4eVueyBUc0d0tkcdWrDB6fJFoCuW1PmDi7B1u3quA==";
        private static DocumentClient client;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var connectionPolicy = new ConnectionPolicy()
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };
            //Setting read region selection preference
            connectionPolicy.PreferredLocations.Add(LocationNames.WestUS2); // first preference
            connectionPolicy.PreferredLocations.Add(LocationNames.EastUS); // second preference
            connectionPolicy.PreferredLocations.Add(LocationNames.SoutheastAsia); // third preference

            client = new DocumentClient(new Uri(EndpointUri), PrimaryKey, connectionPolicy: connectionPolicy);

            client.OpenAsync().ConfigureAwait(false);

            while (true)
            {
                var sw = new Stopwatch();
                sw.Start();
                
                var document = client.CreateDocumentQuery<dynamic>(UriFactory.CreateDocumentCollectionUri("db", "data"), "SELECT TOP 1 * FROM c WHERE c.pk = 'test' ORDER BY c._ts desc").ToList().FirstOrDefault();

                Console.WriteLine(document);

                sw.Stop();

                Console.WriteLine($"Query document in {sw.ElapsedMilliseconds} ms");

                Thread.Sleep(500);
            }
        }
    }
}
