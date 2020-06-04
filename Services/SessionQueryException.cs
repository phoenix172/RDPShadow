using System;

namespace RDPShadow.Services
{
    public class SessionQueryException : Exception
    {
        public SessionQueryException(Exception innerException)
            :base("An error occured while connecting to the computer")
        {
            
        }
    }
}