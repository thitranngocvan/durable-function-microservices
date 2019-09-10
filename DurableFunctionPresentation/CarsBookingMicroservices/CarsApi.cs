using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AzureTableStorageRepository;
using AzureTableStorageRepository.Models;
using DurableFunctionPresentation.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurableFunctionPresentation.FanOut_FanIn
{

    public class CarResultWrapper
    {
        public List<CarPrice> CarPrices { get; set; }
       public CarSearchRequest BookingDetail { get; set; }
    }
    public static class CarsApi
    {
        [FunctionName("SearchCarsOrchestration")]
        public static async Task<CarResultWrapper> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var searchRequest = context.GetInput<CarSearchRequest>();
            var suppliers = await context.CallActivityWithRetryAsync<List<Supplier>>("GetActiveSuppliers", RetryPolicy.DbReadRetryOption, null);

            var searchParallelTasks = new List<Task<List<CarPrice>>>();
            foreach (var sup in suppliers)
            {
                var task = context.CallActivityAsync<List<CarPrice>>("SearchCarSupplier", new
                {
                    CarSearchRequest = searchRequest,
                    Supplier = sup
                });
                searchParallelTasks.Add(task);
            }

            await Task.WhenAll(searchParallelTasks);
            var carPriceResults = new CarResultWrapper { BookingDetail = searchRequest};
            var carResults = new List<CarPrice>();
            foreach (var t in searchParallelTasks)
            {
                carResults.AddRange(t.Result);
            }
            carPriceResults.CarPrices = carResults;
            // TODO: fan-in, aggregate & de-duplicate data, sort/grouping etc.

            return carPriceResults;
        }

        [FunctionName("GetActiveSuppliers")]
        public static async Task<List<Supplier>> GetActiveSuppliers([ActivityTrigger]DurableActivityContext inputs, ILogger log)
        {
            log.LogInformation("GetActiveSuppliers");
            var carService = ServiceProvider.GetCarService();
            return await carService.GetActiveSuppliers();
        }

        [FunctionName("SearchCarSupplier")]
        public static async Task<List<CarPrice>> SearchCarSupplier([ActivityTrigger] DurableActivityContext inputs)
        {
            var r = inputs.GetInput<SupplierSearchRequest>();
            var carSearchService = new CarSearchService();
            return await carSearchService.SearchCars(r.CarSearchRequest, r.Supplier);
        }


        //[FunctionName("AggregateCarResultsFromSuppliers")]
        //public static async Task<List<CarPrice>> AggregateCarResultsFromSuppliers([ActivityTrigger] List<CarPrice> carPrices, ILogger log)
        //{
        //    log.LogInformation($"AggregateCarResultsFromSuppliers...");
        //    //var carSearchService = new CarSearchService();
        //    //return await carSearchService.SearchCars(searchRequest, supplier);
        //}


        [FunctionName("HttpStart_SearchCar")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            var carSearchRequest = ParseSearchRequest(req);
            string instanceId = await starter.StartNewAsync("SearchCarsOrchestration", carSearchRequest);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        private static async Task<CarSearchRequest> ParseSearchRequest(HttpRequestMessage req)
        {
            var searchRequest = await req.Content.ReadAsAsync<CarSearchRequest>();

            return searchRequest;
        }
        private class SupplierSearchRequest
        {
            public CarSearchRequest CarSearchRequest { get; set; }
            public Supplier Supplier { get; set; }
        }
    
}

}  