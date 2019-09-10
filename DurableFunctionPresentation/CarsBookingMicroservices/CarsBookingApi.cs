using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AzureTableStorageRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunctionPresentation.CarsBookingMicroservices
{
    public static class CarsBookingApi
    {
        const string ConfirmBookingEvent = "ConfirmBooking";
        [FunctionName("CarsBookingApi")]
        public static async Task<BookingResponseModel> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var bookingDetail = context.GetInput<BookingResponseModel>();
            // wait for confirm action

            // timer check on timeout and cancel order


           

            // return final result

            using (var timeoutCts = new CancellationTokenSource())
            {
                // The user has 10 minutes to confirm the booking
                DateTime expiration = context.CurrentUtcDateTime.AddMinutes(5);
                Task timeoutTask = context.CreateTimer(expiration, timeoutCts.Token);

                Task<bool> confirmedResponseTask =
                        context.WaitForExternalEvent<bool>(ConfirmBookingEvent);

                Task result = await Task.WhenAny(confirmedResponseTask, timeoutTask);
                if (result == confirmedResponseTask)
                {
                    bookingDetail.BookingStatus = BookingStatusEnum.Confirmed.ToString();
                }
                else
                {
                    bookingDetail.BookingStatus = BookingStatusEnum.Cancelled.ToString();
                    bookingDetail.Message = "Your booking request has been timed out and cancelled. Please contact customer service to activate your booking!";
                }
               

                if (!timeoutTask.IsCompleted)
                {
                    // All pending timers must be complete or canceled before the function exits.
                    timeoutCts.Cancel();
                }

                return bookingDetail;
            }

         
        }

        [FunctionName("ConfirmBooking")]
        public static async Task<HttpResponseMessage> ConfirmBooking(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
           [OrchestrationClient]DurableOrchestrationClient client,
           ILogger log)
        {
            // Function input comes from the request content.
            var confirmRequest = await req.Content.ReadAsAsync<ConfirmBookingRequest>();

            await client.RaiseEventAsync(confirmRequest.InstanceId, ConfirmBookingEvent,true);

            log.LogInformation($"Raise event for instance ID = '{confirmRequest.InstanceId}'.");

            return  req.CreateResponse( $"Thank you! Booking transaction id {confirmRequest.InstanceId} has been confirmed!");
        }


        [FunctionName("CarsBookingApi_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            var bookingRequest = await ParseBookingRequest(req);

            // persist booking info to DB, set status = pending, the workflow will update the status accordingly
            // e.g Cancel if the customer doesn't confirm the booking within 1h
            // e.g Confirmed if user has clicked the confirmed booking link from their email
            var bookingService = ServiceProvider.GetCarBookingService();
            var bookingResponse = await bookingService.AddBooking(bookingRequest);

            string instanceId = await starter.StartNewAsync("CarsBookingApi", bookingResponse);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
        private static async Task<BookingRequestModel> ParseBookingRequest(HttpRequestMessage req)
        {
            var searchRequest = await req.Content.ReadAsAsync<BookingRequestModel>();

            return searchRequest;
        }

        public class ConfirmBookingRequest
        {
            public string InstanceId { get; set; }
        }
    }

    
}