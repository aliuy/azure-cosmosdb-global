using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace GlobalDemo
{
    class Program
    {
        private const string EndpointUri = "https://andrl-global.documents.azure.com:443/";
        private const string PrimaryKey = "jQc9nfPjB2B8WuoJWtNzLV2FPvsTRnXyMnPAkPxxLZ0s19sAblwNixApq2qQUYCY35rSJKe8c94Fflk0XBjXZw==";
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
            connectionPolicy.PreferredLocations.Add(LocationNames.CentralUS); // first preference
            connectionPolicy.PreferredLocations.Add(LocationNames.SoutheastAsia); // second preference
            connectionPolicy.PreferredLocations.Add(LocationNames.NorthEurope); // third preference

            client = new DocumentClient(new Uri(EndpointUri), PrimaryKey, connectionPolicy: connectionPolicy);

            client.OpenAsync().ConfigureAwait(false);

            while (true)
            {
                var sw = new Stopwatch();
                sw.Start();
                
                RequestOptions requestOptions = new RequestOptions
                {
                    PartitionKey = new PartitionKey("323558af-6957-4d0c-ac7d-adcd40ccdf4b")
                };

                var document = client.ReadDocumentAsync(UriFactory.CreateDocumentUri("db", "data", "9b01a96e-6d75-4129-bd04-cce4b9f783ef"), requestOptions).Result;

                sw.Stop();

                Console.WriteLine($"Read document in {sw.ElapsedMilliseconds} ms");

                Thread.Sleep(1000);
            }
        }
    }
}
