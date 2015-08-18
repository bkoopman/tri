using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD4T.ContentModel;

namespace Sdl.Web.Modules.SmartTarget.DD4T.Factories
{
    public interface IComponentPresentationFactory
    {
        IComponentPresentation GetComponentPresentation(string componentUri, string templateUri);

        bool TryGetComponentPresentation(string componentUri, string templateUri, out IComponentPresentation componentPresentation);
    }
}
