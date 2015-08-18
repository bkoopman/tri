using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.Web.Common.Models;

namespace Sdl.Web.Modules.SmartTarget.Models
{
    public class SmartTargetRegion : Region
    {
        public bool HasSmartTargetContent { get; set; }

        public string XpmMarkup { get; set; }
    }
}
