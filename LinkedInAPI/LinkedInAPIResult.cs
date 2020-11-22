using SocialMediaSharing.BLL.LinkedInAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI
{
    public class LinkedInAPIResult<T> 
    {
        public bool IsSuccessful { get; private set; }
        public string Message { get; private set; }
        public T Entity { get; private set; }

        private LinkedInAPIResult() { }
        public static LinkedInAPIResult<T> Success(T entity)
        {
            return new LinkedInAPIResult<T>() { IsSuccessful = true, Message = "OK", Entity = entity };
        }

        public static LinkedInAPIResult<T> Fail(string message)
        {
            return new LinkedInAPIResult<T>() { IsSuccessful = false, Message = message };
        }

    }
}
