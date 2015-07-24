using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.Web.Modules.SmartTarget.Models;

namespace Sdl.Web.Modules.SmartTarget.Query
{
    public class SmartTargetQueryResult    
    {
        public string RegionName { get; set; }

        public List<SmartTargetPromotion> Promotions { get; set; }

        public bool HasSmartTargetContent { get; set; }

        public string XpmMarkup { get; set; }         
    }
}
