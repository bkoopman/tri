using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdl.Web.Modules.SmartTarget.Models
{
    public class SmartTargetItem
    {
        public string RegionName { get; set; }

        public string PromotionId { get; set; }

        public string ComponentUri { get; set; }

        public string TemplateUri  { get; set; }

        public bool IsVisible { get; set; }

        // The raw entity that make up the promotion (eg Component Presentation)
        public object Entity { get; set; }
    }
}
