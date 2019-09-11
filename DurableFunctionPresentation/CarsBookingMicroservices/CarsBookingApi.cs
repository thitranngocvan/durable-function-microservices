using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureTableStorageRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace DurableFunctionPresentation.CarsBookingMicroservices
{
    public static class CarsBookingApi
    {
        const string ConfirmBookingEvent = "ConfirmBooking";
        const string CancelBookingEvent = "CancelBookingByUser";
        [FunctionName("CarsBookingApi")]
        public static async Task<BookingResponseModel> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context
            )

        {
            var bookingDetail = context.GetInput<BookingResponseModel>();
        
            using (var timeoutCts = new CancellationTokenSource())
            {
                // The user has 10 minutes to confirm the booking
                DateTime expiration = context.CurrentUtcDateTime.AddMinutes(5);
                Task timeoutTask = context.CreateTimer(expiration, timeoutCts.Token);

                Task<bool> confirmedResponseTask =
                        context.WaitForExternalEvent<bool>(ConfirmBookingEvent);
                //Task<bool> userCancelledResponseTask = context.WaitForExternalEvent<bool>(CancelBookingEvent);
                Task result = await Task.WhenAny(confirmedResponseTask, timeoutTask);
                var bookingService = ServiceProvider.GetCarBookingService();
                var bookingUpdateRequest = new UpdateBookingActivityInput() {
                    BookingGuid = bookingDetail.BookingGuid,
                    InstanceId = context.InstanceId
                };
                if (result == confirmedResponseTask)
                {
                    bookingDetail.BookingStatus = BookingStatusEnum.Confirmed.ToString();
                    bookingDetail.Message = "Your booking has been confirmed! Thank you and enjoy your trip!";
                    bookingUpdateRequest.Status = BookingStatusEnum.Confirmed.ToString();
                    


                }
                //else if(result == userCancelledResponseTask)
                //{
                //    bookingDetail.BookingStatus = BookingStatusEnum.Cancelled.ToString();
                //    bookingDetail.Message = "You have cancelled the booking from our booking confirmation email. Thank you!";
                //    await bookingService.CancelBooking(bookingDetail.BookingGuid, $"Cancelled By User at {context.CurrentUtcDateTime.ToString("dd MMM yyyy hh:mm:ss")} UTC Time", context.InstanceId);
                //}
                else
                {
                    bookingDetail.BookingStatus = BookingStatusEnum.Cancelled.ToString();
                    bookingDetail.Message = "Your booking request has been timed out and cancelled. Please contact customer service to activate your booking!";
                    bookingUpdateRequest.Status = BookingStatusEnum.Cancelled.ToString();
                    bookingUpdateRequest.CancelReason = bookingDetail.Message;
                }
               

                if (!timeoutTask.IsCompleted)
                {
                    // All pending timers must be complete or canceled before the function exits.
                    timeoutCts.Cancel();
                }
                await context.CallActivityAsync<bool>("UpdateBookingResult", bookingUpdateRequest);
                //if (isConfirmed)
                //{
                //    await bookingService.ConfirmBooking(bookingDetail.BookingGuid, context.InstanceId);
                //}
                //else
                //{
                //    await bookingService.CancelBooking(bookingDetail.BookingGuid, $"Booking request timed out at {context.CurrentUtcDateTime.ToString("dd MMM yyyy hh:mm:ss")} UTC Time", context.InstanceId);
                //}
                
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
            var instanceId = "";
            if(req.Method == HttpMethod.Post)
            {
                var confirmRequest = await req.Content.ReadAsAsync<ConfirmBookingRequest>();
                instanceId = confirmRequest.InstanceId;
            }
            else
            {
                instanceId = req.RequestUri.ParseQueryString().Get(0);
            }

            await client.RaiseEventAsync(instanceId, ConfirmBookingEvent,true);

            log.LogInformation($"Raise event for instance ID = '{instanceId}'.");

            return  req.CreateResponse( $"Thank you! Booking transaction id {instanceId} has been confirmed!");
        }

        [FunctionName("CancelBooking")]
        public static async Task<HttpResponseMessage> CancelBooking(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
          [OrchestrationClient]DurableOrchestrationClient client,
          ILogger log)
        {
            // Function input comes from the request content.
            var confirmRequest = await req.Content.ReadAsAsync<ConfirmBookingRequest>();

            await client.RaiseEventAsync(confirmRequest.InstanceId, CancelBookingEvent, true);

            log.LogInformation($"Raise event for instance ID = '{confirmRequest.InstanceId}'.");

            return req.CreateResponse($"Your booking with transaction id {confirmRequest.InstanceId} has been cancelled. If you wish to make a new booking please contact us at booking-support@gmail.com");
        }

       
        [FunctionName("UpdateBookingResult")]
        public static async Task<bool> UpdateBookingResult([ActivityTrigger] DurableActivityContext inputs)
        {
            var input = inputs.GetInput<UpdateBookingActivityInput>();
            var bookingService = ServiceProvider.GetCarBookingService();
            if(input.Status == BookingStatusEnum.Confirmed.ToString())
            {
                await bookingService.ConfirmBooking(input.BookingGuid, input.InstanceId);
            }
            else if(input.Status == BookingStatusEnum.Cancelled.ToString())
            {
                await bookingService.CancelBooking(input.BookingGuid, input.CancelReason, input.InstanceId);
            }
           
            return true;
        }

        [FunctionName("CarsBookingApi_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log, [SendGrid(ApiKey = "SendGridKey")] IAsyncCollector<SendGridMessage> messageCollector)
        {
            // Function input comes from the request content.
            var bookingRequest = await ParseBookingRequest(req);
            if(string.IsNullOrEmpty(bookingRequest.CustomerEmail))
            {
                return req.CreateResponse(System.Net.HttpStatusCode.BadRequest, "CustomerEmail is required!");
            }

            // persist booking info to DB, set status = pending, the workflow will update the status accordingly
            // e.g Cancel if the customer doesn't confirm the booking within 1h
            // e.g Confirmed if user has clicked the confirmed booking link from their email
            var bookingService = ServiceProvider.GetCarBookingService();
            var bookingResponse = await bookingService.AddBooking(bookingRequest);
            var message = new SendGridMessage();
           
            

            string instanceId = await starter.StartNewAsync("CarsBookingApi", bookingResponse);

            var mailContent = BuildEmailContent(bookingResponse,instanceId);
            var from = "thitranngocvan@gmail.com";
            var subject = $"OnlineBookingService - Please Confirm Your Booking For {bookingResponse.CarName} ";
            message.AddTo(bookingResponse.CustomerEmail);
            message.AddContent("text/html", mailContent);
            message.SetFrom(new EmailAddress(from));
            message.SetSubject(subject);


            await messageCollector.AddAsync(message);
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            var response = starter.CreateCheckStatusResponse(req, instanceId);
            response.Headers.Add("BookingGuid", bookingResponse.BookingGuid.ToString());
            response.Headers.Add("BookingStatus", bookingResponse.BookingStatus);
            return response;
        }

        static string BuildEmailContent(BookingResponseModel bookingInfo, string instanceId)
        {
            var strBuilder = new StringBuilder($"Please confirm your booking: {bookingInfo.BookingGuid} - {bookingInfo.CarName}");
            strBuilder.Append($"<p>Thank you for your booking.</p>");
            var confirmLink = $"<a href='{Settings.BaseUrl}/api/ConfirmBooking?bookingId={instanceId}'>Confirm booking for {bookingInfo.CarName}</a>";
            //var cancelLink = $"";
            strBuilder.Append($"<p>Please click on this link to confirm your booking: {confirmLink}</p>");
            //strBuilder.Append($"If you would like to cancel the booking, please click on this link: {cancelLink}{newLine}");
            strBuilder.Append($"<p>For customer support please call us at +84 32843843849 or email us at support@online-booking.com<p>");
            return strBuilder.ToString();
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

        private class UpdateBookingActivityInput
        {
            public string Status { get; set; }

            public Guid BookingGuid { get; set; }

            public string CancelReason { get; set; }

            public string InstanceId { get; set; }
        }
    }

    
}