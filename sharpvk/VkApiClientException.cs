using System;



namespace sharpvk
{
    public class VkApiClientException : Exception
    {
        public VkApiClientException(string message) : base(message)
        {
            
        }
    }
}