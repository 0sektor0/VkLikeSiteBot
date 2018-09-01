using System;



namespace SharpVK
{
    public class VkApiClientException : Exception
    {
        public VkApiClientException(string message) : base(message)
        {
            
        }
    }
}