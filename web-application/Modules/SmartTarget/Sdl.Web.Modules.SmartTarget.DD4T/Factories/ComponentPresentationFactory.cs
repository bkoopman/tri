using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD4T.ContentModel;
using DD4T.ContentModel.Factories;
using Tridion.ContentDelivery.DynamicContent.Query;
using Tridion.ContentDelivery.Meta;

namespace Sdl.Web.Modules.SmartTarget.DD4T.Factories
{
    public class ComponentPresentationFactory : IComponentPresentationFactory
    {
        private IComponentFactory _componentFactory;

        public ComponentPresentationFactory(IComponentFactory componentFactory)            
        {
            _componentFactory = componentFactory;
        }

        public IComponentPresentation GetComponentPresentation(string componentUri, string templateUri)
        {
            IComponent component = null;
            if (_componentFactory.TryGetComponent(componentUri, out component, templateUri))
            {
                var componentTcmUri = new TcmUri(componentUri);
                var templateTcmUri = new TcmUri(templateUri);              
                                
                var publicationCriteria = new PublicationCriteria(templateTcmUri.PublicationId);
                var itemReferenceCriteria = new ItemReferenceCriteria(templateTcmUri.ItemId);
                var itemTypeTypeCriteria = new ItemTypeCriteria(32);                

                var query = new Tridion.ContentDelivery.DynamicContent.Query.Query(
                    CriteriaFactory.And(new Criteria[] { publicationCriteria, itemReferenceCriteria, itemTypeTypeCriteria }));

                var results = query.ExecuteEntityQuery();
                if (results != null)
                {
                    var templateMeta = (ITemplateMeta)results.FirstOrDefault();

                    var componentPresentation = new ComponentPresentation();
                    componentPresentation.Component = component as Component;
                    componentPresentation.IsDynamic = true;

                    var template = new ComponentTemplate()
                    {
                        Id = templateUri,
                        Title = templateMeta.Title,
                        OutputFormat = templateMeta.OutputFormat
                    };
                    componentPresentation.ComponentTemplate = template;
                    return componentPresentation;
                }                
            }
            return null;
        }

        public bool TryGetComponentPresentation(string componentUri, string templateUri, out IComponentPresentation componentPresentation)
        {
            componentPresentation = GetComponentPresentation(componentUri, templateUri);
            return componentPresentation != null;
        }

    }
}
