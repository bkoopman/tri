using System;
using System.Linq;
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

        public MvcHtmlString RenderSmartTargetQuery(SmartTargetRegion region, HtmlHelper helper, string siteEditTag = _defaultSiteEditTag)
        {
            var siteEditOpenTag = String.Format("<{0}>", siteEditTag);
            var siteEditCloseTag = String.Format("</{0}>", siteEditTag);

            var xmpMarkup = new StringBuilder();
            var isPreview = WebRequestContext.IsPreview;
            if (isPreview)
            {
                xmpMarkup.AppendLine(siteEditOpenTag);
                xmpMarkup.AppendLine(region.XpmMarkup);
            }
            if (region.HasSmartTargetContent)
            {
                foreach (var promotion in region.Items.Cast<SmartTargetPromotion>().Where(p => p.IsVisible))
                {
                    if (isPreview)
                    {
                        xmpMarkup.AppendLine(siteEditOpenTag);
                        xmpMarkup.AppendLine(promotion.XpmMarkup);
                    }
                    foreach (var item in promotion.Items.Where(i => i.IsVisible))
                    {
                        xmpMarkup.AppendLine(RenderPromotionContent(promotion, helper));
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
                foreach (var item in region.Items)
                {
                    xmpMarkup.AppendLine(RenderEntity(item, helper).ToHtmlString());
                }
            }
            if (isPreview)
            {
                xmpMarkup.AppendLine(siteEditCloseTag);
            }

            return MvcHtmlString.Create(xmpMarkup.ToString());
        }

        public MvcHtmlString RenderSmartTargetPromotion(SmartTargetPromotion promotion, HtmlHelper helper)
        {
            return new MvcHtmlString(RenderPromotionContent(promotion, helper));
        }

        private string RenderPromotionContent(SmartTargetPromotion promotion, HtmlHelper helper)
        {
            var renderedContent = new StringBuilder();
            foreach (var item in promotion.Items.Where(i => i.IsVisible))
            {
                renderedContent.AppendLine(RenderEntity(item.Entity, helper).ToHtmlString());
            }

            if (promotion is SmartTargetExperiment)
            {
                var experiment = promotion as SmartTargetExperiment;
                string renderedContentWithTrackedLinks = SmartTargetAnalytics.AddTrackingToLinks(renderedContent.ToString(), experiment);

                SmartTargetAnalytics.TrackView(experiment);
                SmartTargetAnalytics.SaveExperimentCookies(experiment);

                return renderedContentWithTrackedLinks;
            }
            else
            {
                return renderedContent.ToString();
            }
        }

    }
}
