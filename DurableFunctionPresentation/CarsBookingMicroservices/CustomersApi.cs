using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureTableStorageRepository.Models;

namespace DurableFunctionPresentation.CarsApiDemo
{
    public static class CustomersApi
    {
        [FunctionName("PostCustomer")]
        public static async Task<IActionResult> PostCustomer(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Cutomer")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Process customer login");

            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(requestBody);

            var customerDataService = ServiceProvider.GetCustomerDataService();
            //vname = name ?? data?.name;
            await customerDataService.AddCustomer(customer);
            return (ActionResult)new CreatedResult(req.PathBase, customer.Username);
                
        }

        [FunctionName("GetCustomer")]
        public static async Task<IActionResult> GetCustomer(
              [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Cutomer/{username}")] HttpRequest req,string username,
              ILogger log)
        {
            log.LogInformation("Process customer login");
            var customerDataService = ServiceProvider.GetCustomerDataService();
            //vname = name ?? data?.name;
            var customer = await customerDataService.GetCustomer(username);
            return (ActionResult)new OkObjectResult(customer);

        }
    }
}
