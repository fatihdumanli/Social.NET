using SocialMediaSharing.BLL.FacebookGraph.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.TwitterAPI
{
    public class TwitterAPIResult<T>
    {
        public bool IsSuccessful { get; private set; }
        public string Message { get; private set; }
        public T Entity { get; private set; }

        private TwitterAPIResult() {}
        public static TwitterAPIResult<T> Success(T entity)
        {
            return new TwitterAPIResult<T>() { IsSuccessful = true, Message = "OK", Entity = entity };
        }

        public static TwitterAPIResult<T> Fail(string message)
        {
            return new TwitterAPIResult<T>() { IsSuccessful = false, Message = message };
        }

    }
}
