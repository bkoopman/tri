using System;
using System.Web.Mvc;
using Sdl.Web.Common.Interfaces;
using Sdl.Web.Common.Models;
using Sdl.Web.Modules.SmartTarget.Models;

namespace Sdl.Web.Modules.SmartTarget.Interfaces
{
    public interface ISmartTargetRenderer : IRenderer
    {
        MvcHtmlString RenderSmartTargetQuery(SmartTargetRegion region, HtmlHelper helper, string siteEditTag);

        MvcHtmlString RenderSmartTargetPromotion(SmartTargetPromotion promotion, HtmlHelper helper);
    }
}
