using System;
using System.Web.Mvc;
using Sdl.Web.Common.Interfaces;
using Sdl.Web.Common.Models;

namespace Sdl.Web.Modules.SmartTarget.Interfaces
{
    public interface ISmartTargetRenderer : IRenderer
    {
        MvcHtmlString RenderSmartTargetQuery(IRegion region, HtmlHelper helper, string siteEditTag);
    }
}
