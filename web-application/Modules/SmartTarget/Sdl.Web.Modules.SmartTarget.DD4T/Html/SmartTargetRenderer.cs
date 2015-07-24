using System;
using System.Text;
using System.Web.Mvc;
using Sdl.Web.Common.Models;
using Sdl.Web.DD4T.Html;
using Sdl.Web.Modules.SmartTarget.Interfaces;
using Sdl.Web.Modules.SmartTarget.Models;
using Sdl.Web.Modules.SmartTarget.Analytics;
using Sdl.Web.Mvc.Configuration;

namespace Sdl.Web.Modules.SmartTarget.DD4T.Html
{
    public class SmartTargetRenderer : DD4TRenderer, ISmartTargetRenderer
    {
        private const string _defaultSiteEditTag = "span";

        public MvcHtmlString RenderSmartTargetQuery(IRegion region, HtmlHelper helper, string siteEditTag = _defaultSiteEditTag)
        {
            var siteEditOpenTag = String.Format("<{0}>", siteEditTag);
            var siteEditCloseTag = String.Format("</{0}>", siteEditTag);

            if (region is SmartTargetRegion)
            {
                var smartTargetRegion = region as SmartTargetRegion;

                var xmpMarkup = new StringBuilder();
                var isPreview = WebRequestContext.IsPreview;
                if (isPreview)
                {
                    xmpMarkup.AppendLine(siteEditOpenTag);
                    xmpMarkup.AppendLine(smartTargetRegion.XpmMarkup);
                }
                foreach (var item in smartTargetRegion.Items)
                {
                    if (item is SmartTargetPromotion)
                    {
                        var promotion = item as SmartTargetPromotion;
                        if (promotion.IsVisible)
                        {
                            if (isPreview)
                            {
                                xmpMarkup.AppendLine(siteEditOpenTag);
                                xmpMarkup.AppendLine(promotion.XpmMarkup);
                            }
                            foreach (var promotionItem in promotion.Items)
                            {
                                if (promotionItem.IsVisible)
                                {
                                    string renderedContent = RenderEntity(promotionItem.Entity, helper).ToHtmlString();
                                    if (promotion is SmartTargetExperiment)
                                    {
                                        var experiment = promotion as SmartTargetExperiment;
                                        string renderedContentWithTrackedLinks = SmartTargetAnalytics.AddTrackingToLinks(renderedContent, experiment);
                                        xmpMarkup.AppendLine(renderedContentWithTrackedLinks);

                                        SmartTargetAnalytics.TrackView(experiment);
                                        SmartTargetAnalytics.SaveExperimentCookies(experiment);
                                    }
                                    else
                                    {
                                        xmpMarkup.AppendLine(renderedContent);
                                    }
                                }
                            }
                            if (isPreview)
                            {
                                xmpMarkup.AppendLine(siteEditCloseTag);
                            }
                        }
                    }
                    else
                    {
                        // Fallback content
                        xmpMarkup.AppendLine(RenderEntity(item, helper).ToHtmlString());
                    }
                }
                if (isPreview)
                {
                    xmpMarkup.AppendLine(siteEditCloseTag);
                }                

                return MvcHtmlString.Create(xmpMarkup.ToString());
            }
            return null;
        }
    }
}
