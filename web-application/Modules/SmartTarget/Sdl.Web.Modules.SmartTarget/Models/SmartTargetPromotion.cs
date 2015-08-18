using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdl.Web.Modules.SmartTarget.Models
{
    public class SmartTargetPromotion
    {
        public const string _xpmMarkupFormat = "<!-- Start Promotion: {{ \"PromotionID\": \"{0}\", \"RegionID\" : \"{1}\"}} -->";

        public string RegionName { get; set; }

        public string PromotionId { get; set; }

        public string Title { get; set; }

        public string Slogan { get; set; }

        public List<SmartTargetItem> Items { get; set; }

        public bool IsVisible { get; set; }

        public string XpmMarkup
        {
            get { return String.Format(_xpmMarkupFormat, PromotionId, RegionName); }
        }
    }
}
