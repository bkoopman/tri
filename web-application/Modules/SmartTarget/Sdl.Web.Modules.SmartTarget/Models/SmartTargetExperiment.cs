using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.SmartTarget.Analytics;
using Tridion.SmartTarget.Query;

namespace Sdl.Web.Modules.SmartTarget.Models
{
    public class SmartTargetExperiment : SmartTargetPromotion
    {
        public ExperimentDimensions ExperimentDimensions { get; set; }

        public ExperimentCookies NewExperimentCookies { get; set; }
    }
}
