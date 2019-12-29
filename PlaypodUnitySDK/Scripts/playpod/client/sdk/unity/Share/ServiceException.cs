using System;

namespace playpod.client.sdk.unity.Share
{
    public class ServiceException : Exception
    {
        public ServiceException()
        { }

        public ServiceException(string message) : base(message)
        { }

        public ServiceException(string message, Exception originalException) : base(message, originalException)
        { }

        public ServiceException(Exception originalException) : base("unhandled exception", originalException)
        { }
    }
}
