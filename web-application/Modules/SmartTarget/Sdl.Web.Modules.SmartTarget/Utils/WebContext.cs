using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Sdl.Web.Mvc.Configuration;

namespace Sdl.Web.Modules.SmartTarget.Utils
{
    public static class WebContext
    {
        public static HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        public static HttpResponse Response
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }
    }
}
