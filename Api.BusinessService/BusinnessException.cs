using System;
using System.Runtime.Serialization;

namespace Api.BusinessService
{
    [Serializable]
    public class BusinnessException : Exception
    {
        public BusinnessException()
        {
        }

        public BusinnessException(string message) : base(message)
        {
        }

        public BusinnessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BusinnessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}