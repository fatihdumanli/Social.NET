using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.OAuthv2
{
    public class OAuthRequestResult<T>
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public T Entity { get; set; }

        public static OAuthRequestResult<T> Success(T entity)
        {
            return new OAuthRequestResult<T>() { IsSuccessful = true, Message = "OK", Entity = entity };
        }

        public static OAuthRequestResult<T> Fail(string message)
        {
            return new OAuthRequestResult<T>() { IsSuccessful = true, Message = message };
        }
    }
}
