using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcGreeterServer;

public class CustomersService : Customers.CustomersBase {
    private readonly ILogger<CustomersService> _logger;
    public CustomersService(ILogger<CustomersService> logger) {
        _logger = logger;
    }

    public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, 
                                            ServerCallContext context) {

        var output = new CustomerModel();
        switch (request.UserId) {
        case 1:
            output.FirstName = "John";
            output.LastName = "Smith";
            output.EmailAddress = "john.smith@contoso.com";
            output.Age = 24;
            output.IsAlive = true;
            break;
        case 2:
            output.FirstName = "Jane";
            output.LastName = "Doe";
            output.EmailAddress = "jane.doe@contoso.com";
            output.Age = 33;
            output.IsAlive = true;
            break;
        default:
            output.FirstName = "Greg";
            output.LastName = "Thomas";
            output.EmailAddress = "greg.thomas@contoso.com";
            output.Age = 55;
            output.IsAlive = true;
            break;
        }
        return Task.FromResult(output);
    }
    
    public override async Task GetNewCustomers(NewCustomerRequest request, 
                                        IServerStreamWriter<CustomerModel> responseStream, 
                                        ServerCallContext context) {
        var customers = new List<CustomerModel> {
            new CustomerModel {
                FirstName = "John",
                LastName = "Smith",
                EmailAddress = "john.smith@contoso.com",
                Age = 24,
                IsAlive = true
            },
            new CustomerModel {
                FirstName = "Jane",
                LastName = "Doe",
                EmailAddress = "jane.doe@contoso.com",
                Age = 33,
                IsAlive = true
            },
            new CustomerModel {
                FirstName = "Greg",
                LastName = "Thomas",
                EmailAddress = "greg.thomas@contoso.com",
                Age = 55,
                IsAlive = true
            }
        };
    
        foreach (var c in customers) {
            await responseStream.WriteAsync(c);
        }
    }
 }
