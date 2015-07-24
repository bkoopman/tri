using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.SmartTarget.Utils;

namespace Sdl.Web.Modules.SmartTarget.Utils
{
    public static class AllowDuplicatesValueHelper
    {
        public static bool DefaultAllowDuplicates
        {
            get
            {
                return ConfigurationUtility.DefaultAllowDuplicates;
            }
        }

        public static bool Parse(string value)
        {
            if ("No".Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if ("Yes".Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return DefaultAllowDuplicates;
        }
    }
}
