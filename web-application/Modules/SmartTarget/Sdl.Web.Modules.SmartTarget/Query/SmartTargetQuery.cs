using System.Collections.Generic;
using Sdl.Web.Modules.SmartTarget.Models;
using Sdl.Web.Modules.SmartTarget.Utils;
using Tridion.ContentDelivery.AmbientData;
using Tridion.SmartTarget.Analytics;
using Tridion.SmartTarget.Query;
using Tridion.SmartTarget.Query.Builder;
using Tridion.SmartTarget.Utils;

namespace Sdl.Web.Modules.SmartTarget.Query
{
    public class SmartTargetQuery
    {
        public static List<SmartTargetQueryResult> GetPagePromotions(List<SmartTargetRegionConfig> regionConfigList)
        {
            var queryResults = new List<SmartTargetQueryResult>();
            
            var itemsAlreadyOnPage = new List<string>();
            foreach(var regionConfig in regionConfigList)
            {
                var queryResult = GetRegionPromotions(regionConfig, itemsAlreadyOnPage);
                queryResults.Add(queryResult);
            }

            return queryResults;
        }

        public static SmartTargetQueryResult GetRegionPromotions(SmartTargetRegionConfig regionConfig, List<string> itemsAlreadyOnPage)
        {
            var queryResult = new SmartTargetQueryResult();
            queryResult.RegionName = regionConfig.RegionName;

            var pageUri = new TcmUri(regionConfig.PageId);
            var publicationUri = new TcmUri(0, pageUri.PublicationId, 1);
            //TODO; via TRI??
            ClaimStore claimStore = AmbientDataContext.CurrentClaimStore;
            string triggers = AmbientDataHelper.GetTriggers(claimStore);

            var queryBuilder = new QueryBuilder();
            queryBuilder.Parse(triggers);
            queryBuilder.AddCriteria(new PublicationCriteria(publicationUri));
            queryBuilder.AddCriteria(new PageCriteria(pageUri));
            queryBuilder.AddCriteria(new RegionCriteria(regionConfig.RegionName));
            if (regionConfig.MaxItems > 0)
            {
                queryBuilder.MaxItems = regionConfig.MaxItems;
            }
            if (regionConfig.StartIndex > 0)
            {
                queryBuilder.StartIndex = regionConfig.StartIndex;
            }
           
            ResultSet fredHopperResultset = queryBuilder.Execute();
            if (fredHopperResultset.Promotions.Count > 0)
            {
                queryResult.Promotions = ProcessRegionPromotions(fredHopperResultset.Promotions, publicationUri, pageUri, regionConfig.RegionName, regionConfig.MaxItems, regionConfig.AllowDuplicates, itemsAlreadyOnPage);
                queryResult.HasSmartTargetContent = true;
            }

            var xmpQueryMarkup = ResultSet.GetExperienceManagerMarkup(regionConfig.RegionName, regionConfig.MaxItems, fredHopperResultset.Promotions);
            queryResult.XpmMarkup = xmpQueryMarkup;            

            return queryResult;
        }

        private static List<SmartTargetPromotion> ProcessRegionPromotions(List<Promotion> promotions, TcmUri publicationUri, TcmUri pageUri, string region, int maxItems, bool allowDuplicates, List<string> itemsAlreadyOnPage)
        {
            var itemsOutputInRegion = new List<string>();
            var existingExperimentCookies = CookieProcessor.GetExperimentCookies(WebContext.Request);
            var newExperimentCookies = new ExperimentCookies();
            ExperimentDimensions experimentDimensions = null;

            //Method will populate experimentDimensions variable when an experiment is present. Also it will populate the newExperimentCookies variable when no existing experiment cookies are found
            ResultSet.FilterPromotions(promotions, region, maxItems, allowDuplicates, itemsOutputInRegion, itemsAlreadyOnPage, ref existingExperimentCookies, ref newExperimentCookies, out experimentDimensions);

            var smartTargetPromotions = new List<SmartTargetPromotion>();
            foreach (var promotion in promotions)
            {
                var smartTargetPromotion = new SmartTargetPromotion();
                if (experimentDimensions != null && experimentDimensions.ExperimentId.Equals(promotion.PromotionId))
                {
                    //Populate the remaining emtpy properties
                    experimentDimensions.PublicationId = publicationUri.ToString();
                    experimentDimensions.PageId = pageUri.ToString();
                    experimentDimensions.Region = region;

                    smartTargetPromotion = new SmartTargetExperiment()
                    {
                        ExperimentDimensions = experimentDimensions,
                        NewExperimentCookies = newExperimentCookies
                    };
                }
                smartTargetPromotion.PromotionId = promotion.PromotionId;
                smartTargetPromotion.RegionName = promotion.Region;
                smartTargetPromotion.Title = promotion.Title;
                smartTargetPromotion.Slogan = promotion.Slogan;
                smartTargetPromotion.IsVisible = promotion.Visible;

                var smartTargetPromotionItems = new List<SmartTargetItem>();
                foreach (var item in promotion.Items)
                {
                    var smartTargetItem = new SmartTargetItem
                    {
                        PromotionId = item.PromotionId,
                        RegionName = item.Region,
                        ComponentUri = item.ComponentUriAsString,
                        TemplateUri = item.TemplateUriAsString,
                        IsVisible = item.Visible
                    };
                    smartTargetPromotionItems.Add(smartTargetItem);
                }
                smartTargetPromotion.Items = smartTargetPromotionItems;
                smartTargetPromotions.Add(smartTargetPromotion);
            }
            return smartTargetPromotions;
        }
    }
}
