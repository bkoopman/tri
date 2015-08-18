using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdl.Web.Modules.SmartTarget.Models
{
    public class SmartTargetRegionConfig
    {
        public string PageId { get; set; }

        public string RegionName { get; set; }

        public int MaxItems { get; set; }

        public int StartIndex { get; set; }

        public bool AllowDuplicates { get; set; }
    }
}
