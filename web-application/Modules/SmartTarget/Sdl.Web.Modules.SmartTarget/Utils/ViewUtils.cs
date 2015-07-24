using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.Web.Common.Configuration;

namespace Sdl.Web.Modules.SmartTarget.Utils
{
    public static class ViewUtils
    {
       public static void ParseViewName(string value, out string moduleName, out string viewName)
       {
           moduleName = SiteConfiguration.GetDefaultModuleName();//Default module
           viewName = null;
           if (value != null)
           {
               var bits = value.Split(':');
               if (bits.Length > 1)
               {
                    moduleName = bits[0].Trim();
                    viewName = bits[1].Trim();
                }
                else
                {
                    viewName = bits[0].Trim();                
                }
            }
        }
    }
}
