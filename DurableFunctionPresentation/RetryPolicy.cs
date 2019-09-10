using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DurableFunctionPresentation
{
    public static class RetryPolicy
    {
        public static RetryOptions DbReadRetryOption = new RetryOptions(firstRetryInterval: TimeSpan.FromSeconds(3), maxNumberOfAttempts: 3);

        public static RetryOptions ExternalServiceRetryOption = new RetryOptions(firstRetryInterval: TimeSpan.FromSeconds(3), maxNumberOfAttempts: 3);

    }
}
