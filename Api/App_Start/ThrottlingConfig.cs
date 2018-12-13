using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiThrottle;

namespace Api.App_Start
{
    /// <summary>
    /// Refer: https://github.com/stefanprodan/WebApiThrottle
    /// </summary>
    public class ThrottlingConfig
    {
        public static long? CallsPerSecond => 2;
        public static long? CallsPerMinute => 30;
        public static long? CallsPerHour => 1200;
        public static long? CallsPerDay => 16000;
        public static long? CallsPerWeek => null;

        public static ThrottlePolicy GetDefaultPolicy()
        {
            return new ThrottlePolicy(perSecond: CallsPerSecond, perMinute: CallsPerMinute, perHour: CallsPerHour, perDay: CallsPerDay, perWeek: CallsPerWeek)
            {
                IpThrottling = true,
                EndpointThrottling = true
            };
        }
    }
}