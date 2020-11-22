using SocialMediaSharing.BLL.FacebookGraph.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph
{
    public class FacebookGraphResult<T>
    {
        public bool IsSuccessful { get; private set; }
        public string Message { get; private set; }
        public T Entity { get; private set; }

        private FacebookGraphResult() {}
        public static FacebookGraphResult<T> Success(T entity)
        {
            return new FacebookGraphResult<T>() { IsSuccessful = true, Message = "OK", Entity = entity };
        }

        public static FacebookGraphResult<T> Fail(string message)
        {
            return new FacebookGraphResult<T>() { IsSuccessful = false, Message = message };
        }

    }
}
