using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeterServer;

namespace GrpcGreeterClient;

class Program {
    static async Task Main(string[] args) {
        // The port number(5001) must match the port of the gRPC server.
        using var channel = 
            // GrpcChannel.ForAddress("https://localhost:15001");
            GrpcChannel.ForAddress("http://localhost:15000");

        var greeterClient = new Greeter.GreeterClient(channel);
        var reply = await greeterClient.SayHelloAsync(new HelloRequest { 
            Name = "GreeterClient"
        });
        Console.WriteLine("Greeting: " + reply.Message);

        var customersClient = new Customers.CustomersClient(channel);
        var userId = 1;
        var customerInfo = await customersClient.GetCustomerInfoAsync(
            new CustomerLookupModel {
                UserId = userId
            });
        Console.WriteLine($"========= Customer {userId} =========");
        Console.WriteLine($"First Name: {customerInfo.FirstName}");
        Console.WriteLine($"Last Name: {customerInfo.LastName}");
        Console.WriteLine($"Email: {customerInfo.EmailAddress}");
        Console.WriteLine($"Age: {customerInfo.Age}");

        using (var call = customersClient.GetNewCustomers(new NewCustomerRequest())) {
            var token = new CancellationToken();
            while (await call.ResponseStream.MoveNext(token)) {
                var c = call.ResponseStream.Current;
                Console.WriteLine($"========= Customer =========");
                Console.WriteLine($"First Name: {c.FirstName}");
                Console.WriteLine($"Last Name: {c.LastName}");
                Console.WriteLine($"Email: {c.EmailAddress}");
                Console.WriteLine($"Age: {c.Age}");
            }
        }


        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}