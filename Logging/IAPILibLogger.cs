using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.Logging
{
    public interface IAPILibLogger
    {
        void LogInfo(string message);
        void LogError(string error);
    }
}
