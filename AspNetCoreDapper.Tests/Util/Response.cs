using System.Collections.Generic;
using Flunt.Notifications;

namespace AspNetCoreDapper.Tests.Util
{
    public class Response
    {
        public bool success { get; set; }
        public List<Notification> errors { get; set; }
    }
}
