using Sdl.Web.Tridion.Common;
using System;
using System.Linq;
using System.Xml;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.Templating;
using Tridion.ContentManager.Templating.Assembly;
using Tridion.ExternalContentLibrary.Templating.V2;

namespace Sdl.Web.Tridion.Templates.MediaManager
{
    /// <summary>
    /// Resolves Media Manager references from the package output.
    /// Update Url tags with the Media Manager Url so these can 
    /// be resolved correctly by the web application. 
    /// </summary>
    /// <remarks>
    /// Should be placed after the "Publish binaries for component" TBB
    /// </remarks>
    [TcmTemplateTitle("Resolve Media Manager References")]
    public class ResolveMediaManagerReferences : TemplateBase
    {
        private ExternalContentLibraryFunctionSource _eclFunctions;

        protected void Init(Engine engine, Package package)
        {
            Initialize(engine, package);
            _eclFunctions = new ExternalContentLibraryFunctionSource();
            _eclFunctions.Initialize(engine, package);
        }

        public override void Transform(Engine engine, Package package)
        {
            try
            {
                Init(engine, package);

                Component comp = GetComponent();
                if (IsPageTemplate() || comp == null)
                {
                    Logger.Error("No Component found (is this a Page Template?)");
                    return;
                }
                Item outputItem = package.GetByName(Package.OutputName);
                if (outputItem == null)
                {
                    Logger.Error("No Output package item found (is this TBB placed at the end?)");
                    return;
                }

                XmlDocument doc = new XmlDocument();
                string output = outputItem.GetAsString();
                doc.LoadXml(output);
                var mediaManagerComponents = doc.SelectNodes("//Field[@FieldType='MultiMediaLink']/LinkedComponentValues/Component/Multimedia/MimeType['application/externalcontentlibrary']/../..");
                foreach (XmlElement mediaManagerComponent in mediaManagerComponents)
                {
                    mediaManagerComponent.InnerXml = ResolveMediaManagerReference(mediaManagerComponent.InnerXml);
                }
                package.Remove(outputItem);
                package.PushItem(Package.OutputName, package.CreateXmlDocumentItem(ContentType.Xml, doc));
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
            }
        }

        private string ResolveMediaManagerReference(string input)
        {
            XmlDocument doc = new XmlDocument();
            var nsmgr = new XmlNamespaceManager(doc.NameTable);            
            doc.LoadXml(String.Format("<Component>{0}</Component>", input));

            string id = doc.SelectSingleNode("/Component/Id", nsmgr).IfNotNull(i => i.InnerText);
            if (!String.IsNullOrEmpty(id))
            {
                XmlNode urlNode = doc.SelectSingleNode("/Component/Multimedia/Url", nsmgr);
                if (urlNode != null)
                {                    
                    if (_eclFunctions.IsExternalContentLibraryComponent(id))
                    {
                        urlNode.InnerText = _eclFunctions.GetExternalContentLibraryDirectLink(id);
                    }
                }
            }

            return doc.DocumentElement.InnerXml;
        }
    }
}
