using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.Web.Modules.SmartTarget.Models;
using Sdl.Web.Modules.SmartTarget.Utils;
using Tridion.SmartTarget.Analytics;
using Tridion.SmartTarget.Utils;

namespace Sdl.Web.Modules.SmartTarget.Analytics
{
    public static class SmartTargetAnalytics
    {
        private static AnalyticsManager _analyticsManager = new AnalyticsManager();

        public static void TrackView(SmartTargetExperiment experiment)
        {
            _analyticsManager.TrackView(experiment.ExperimentDimensions, new AnalyticsMetaData());
        }

        public static string AddTrackingToLinks(string content, SmartTargetExperiment experiment)
        {
            return _analyticsManager.AddTrackingToLinks(content, experiment.ExperimentDimensions, new AnalyticsMetaData());
        }

        public static void SaveExperimentCookies(SmartTargetExperiment experiment)
        {
            CookieProcessor.SaveExperimentCookies(WebContext.Response, null, experiment.NewExperimentCookies);
        }
    }
}
