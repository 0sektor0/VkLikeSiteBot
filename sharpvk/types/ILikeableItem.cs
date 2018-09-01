using System;



namespace SharpVK.Types
{
    public interface ILikeableItem
    {
        int Id { get; set; }

        int OwnerId { get; set; }

        string Url { get; set; }

        int Date { get; set; }

        string AccessKey { get; set; }
    }
}