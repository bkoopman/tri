using System;
using System.Linq;
using System.Collections.Generic;
using DD4T.ContentModel;
using DD4T.ContentModel.Factories;
using Sdl.Web.Common.Interfaces;
using Sdl.Web.DD4T.Mapping;
using Sdl.Web.Modules.SmartTarget.DD4T.Factories;
using Sdl.Web.Modules.SmartTarget.Models;
using Sdl.Web.Modules.SmartTarget.Query;
using Sdl.Web.Modules.SmartTarget.Utils;

namespace Sdl.Web.Modules.SmartTarget.DD4T.Mapping
{
    public class SmartTargetModelBuilder : DD4TModelBuilder
    {
        private IComponentPresentationFactory _componentPresentationFactory;

        public SmartTargetModelBuilder(ILinkFactory linkFactory, IContentResolver contentResolver, IComponentPresentationFactory componentPresentationFactory)
            : base(linkFactory, contentResolver) 
        {
            _componentPresentationFactory = componentPresentationFactory;
        }

        protected override object CreatePage(object sourceEntity, Type type, List<object> includes)
        {
            var page = base.CreatePage(sourceEntity, type, includes) as Sdl.Web.Common.Models.IPage;
            if (page != null)
            {
                Dictionary<string, string> moduleMap;
                List<SmartTargetRegionConfig> regionConfigList;                
                if (TryGetSmartTargetRegionConfiguration(sourceEntity, out moduleMap, out regionConfigList))
                {
                    var queryResults = SmartTargetQuery.GetPagePromotions(regionConfigList);
                    foreach (var queryResult in queryResults)
                    {
                        var region = new SmartTargetRegion
                        {
                            Module = moduleMap[queryResult.RegionName],
                            Name = queryResult.RegionName,
                            XpmMarkup = queryResult.XpmMarkup,
                            HasSmartTargetContent = queryResult.HasSmartTargetContent
                        };

                        if (region.HasSmartTargetContent)
                        {
                            foreach (var promotion in queryResult.Promotions)
                            {
                                RetrievePromotionEntities(promotion);
                                region.Items.Add(promotion);
                            }
                        }
                        else
                        {
                            var fallbackContent = page.Regions.ContainsKey(region.Name) ? page.Regions[region.Name] : null;
                            if (fallbackContent != null)
                            {
                                region.Items = fallbackContent.Items;
                            }
                        }
                        page.Regions[region.Name] = region;
                    }
                }         
            }
            return page;
        }

        private bool TryGetSmartTargetRegionConfiguration(object sourceEntity, out Dictionary<string, string> moduleMap, out List<SmartTargetRegionConfig> regionConfigList)
        {
            moduleMap = new Dictionary<string, string>();
            regionConfigList = new List<SmartTargetRegionConfig>(); 

            IPage page = sourceEntity as IPage;
            if (page != null && page.PageTemplate.MetadataFields.ContainsKey("smartTargetRegions"))
            {
                foreach (var smartTargetRegion in page.PageTemplate.MetadataFields["smartTargetRegions"].EmbeddedValues)
                {
                    string module, regionName;
                    ViewUtils.ParseViewName(smartTargetRegion["regionName"].Value, out module, out regionName); //Mandatory field; regionName
                    moduleMap[regionName] = module;

                    int maxItems = 0;
                    Int32.TryParse(smartTargetRegion["maxItems"].Value, out maxItems); //Mandatory field; maxItems

                    bool allowDuplicates = AllowDuplicatesValueHelper.DefaultAllowDuplicates;
                    if (smartTargetRegion.ContainsKey("allowDuplicates"))
                    {
                        allowDuplicates = AllowDuplicatesValueHelper.Parse(smartTargetRegion["allowDuplicates"].Value); //Optional field; allowDuplicates
                    }

                    var regionConfig = new SmartTargetRegionConfig
                    {
                        PageId = page.Id,
                        RegionName = regionName,
                        MaxItems = maxItems,
                        AllowDuplicates = allowDuplicates
                    };
                    regionConfigList.Add(regionConfig);
                }
                return true;
            }
            return false;
        }

        private void RetrievePromotionEntities(SmartTargetPromotion promotion)
        {
            foreach (var promotionItem in promotion.Items.Where(p => p.IsVisible))
            {
                IComponentPresentation componentPresentation;
                if (_componentPresentationFactory.TryGetComponentPresentation(promotionItem.ComponentUri, promotionItem.TemplateUri, out componentPresentation))
                {
                    promotionItem.Entity = componentPresentation;
                }
            }
        }

    }
}
