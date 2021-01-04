using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using OfferExport.Entities;

namespace OfferExport
{
    class WinOasisDAL
    {
        private Settings _settings;
        private string _connectionString = string.Empty;

        public WinOasisDAL()
        {
            _settings = new Settings();
            _connectionString = _settings.GetOasisConnectionString();
        }
   
        /// <summary>
        /// Gets the offer master data from tghe DB
        /// </summary>
        /// <returns>List of MasterOffers</returns>
        public List<MasterOffers> GetMasterOffers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = File.ReadAllText(@"Queries/GetMasterOfferData.txt");
                connection.Open();
                List<MasterOffers> master = new List<MasterOffers>();      
                master = connection.Query<MasterOffers>(query).ToList();
                connection.Close();
                return master;
            }
        }

        /// <summary>
        /// Takes a list of Offers and, for each promo offer, returns the associated players  
        /// </summary>
        /// <param name="offers"></param>
        /// <returns>Returns a PlayerOfferMap that contains players and promo offer association</returns>
        public List<PlayerOfferMap> GetPlayersPromoOffers(List<Offer> offers)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<PlayerOfferMap> promoPlayers = new List<PlayerOfferMap>();
                string promoIds = GetOfferIdAsString(offers, "Promo");
                String sqlQueryTemplate = File.ReadAllText(@"Queries/GetPlayersPromoOffers.txt");
                var query = sqlQueryTemplate.Replace("{promoIds}", promoIds);
                Console.WriteLine("Attempting to get players associated with these promo IDs: \n {0}", promoIds);

                if (promoIds.Length > 0)
                {
                    connection.Open();
                    promoPlayers = connection.Query<PlayerOfferMap>(query).ToList();
                    connection.Close();
                }
                return promoPlayers;
            }
        }

        /// <summary>
        /// Takes a list of MasterOffers and, for each campaign offer, returns the associated players  
        /// </summary>
        /// <param name="offers"></param>
        /// <returns>Returns a PlayerOfferMap that contains players and campaign offer association</returns>
        public List<PlayerOfferMap> GetPlayersCampaignOffers(List<MasterOffers> masterOffers)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<PlayerOfferMap> playerCampaignOffers = new List<PlayerOfferMap>();

                string offer_EventIds = GetOfferEventIdAsString(masterOffers);
                Console.WriteLine("Attempting to get campaign players for offer_EventIds: \n {0}", offer_EventIds);

                string coupon_RedemptionIds = GetCouponRedemptionIdAsString(masterOffers);
                Console.WriteLine("Attempting to get campaign players for coupon_RedemptionIds: \n {0}", coupon_RedemptionIds);
                
                string sqlQueryTemplate = File.ReadAllText(@"Queries/GetPlayersCampaignOffers.txt");
                var query = sqlQueryTemplate.Replace("{offer_EventIds}", offer_EventIds).Replace("{coupon_RedemptionIds}", coupon_RedemptionIds);

                if (offer_EventIds.Length > 0 && coupon_RedemptionIds.Length > 0)
                {
                    connection.Open();
                    playerCampaignOffers = connection.Query<PlayerOfferMap>(query).ToList();
                    connection.Close();
                }
                return playerCampaignOffers;
            }
        }

        /// <summary>
        /// Takes a list of MasterOffers and, for each event offer, returns the associated players  
        /// </summary>
        /// <param name="offers"></param>
        /// <returns>Returns a PlayerOfferMap that contains players and event offer association</returns>
        public List<PlayerOfferMap> GetPlayersEventOffers(List<MasterOffers> masterOffers)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<PlayerOfferMap> playerEventOffers = new List<PlayerOfferMap>();

                string offer_EventIds = GetOfferEventIdAsString(masterOffers);
                Console.WriteLine("Attempting to get event players for offer_EventIds: \n {0}", offer_EventIds);

                string coupon_RedemptionIds = GetCouponRedemptionIdAsString(masterOffers);
                Console.WriteLine("Attempting to get event players for coupon_RedemptionIds: \n {0}", coupon_RedemptionIds);

                string sqlQueryTemplate = File.ReadAllText(@"Queries/GetPlayersEventOffers.txt");
                var query = sqlQueryTemplate.Replace("{offer_EventIds}", offer_EventIds).Replace("{coupon_RedemptionIds}", coupon_RedemptionIds);

                if (offer_EventIds.Length > 0 && coupon_RedemptionIds.Length > 0)
                {
                    connection.Open();
                    playerEventOffers = connection.Query<PlayerOfferMap>(query).ToList();
                    connection.Close();
                }
                return playerEventOffers;
            }
        }

        /// <summary>
        /// helper method. Takes a list of Offer and a SubType
        /// </summary>
        /// <param name="offers"></param>
        /// <param name="subType"></param>
        /// <returns>comma-delimited string of unique OfferId</returns>
        private string GetOfferIdAsString(List<Offer> offers, string subType)
        {
            List<string> offerIds = new List<string>();

            foreach (Offer offer in offers)
            {
                Console.WriteLine(JsonConvert.SerializeObject(offer));
                if (offer.SubType.ToLower().Trim() == subType.ToLower().Trim() && offer.OfferId != null)
                {
                    offerIds.Add(offer.OfferId);
                }
            }
            List<string> offerIdsUnique = offerIds.Distinct().ToList();
            string result = string.Join(",", offerIdsUnique);
            return result;
        }

        /// <summary>
        /// helper method. Takes a list of Masteroffers
        /// </summary>
        /// <param name="masterOffers"></param>
        /// <returns>comma-delimited string of unique Offer_EventID</returns>
        private string GetOfferEventIdAsString(List<MasterOffers> masterOffers)
        {
            List<string> offerEventIds = new List<string>();

            foreach (MasterOffers masterOffer in masterOffers)
            {
                if (masterOffer.Offer_EventID != null)
                {
                    offerEventIds.Add(masterOffer.Offer_EventID);
                }                                 
            }

            List<string> offerEventIdsUnique = offerEventIds.Distinct().ToList();
            string result = string.Join(",", offerEventIdsUnique);
            return result;
        }

        /// <summary>
        /// helper method. Takes a list of Masteroffers
        /// </summary>
        /// <param name="masterOffers"></param>
        /// <returns>comma-delimited string of unique Coupon_RedemptionID</returns>
        private string GetCouponRedemptionIdAsString(List<MasterOffers> masterOffers)
        {
            List<string> couponRedemptionIds = new List<string>();
            foreach (MasterOffers masterOffer in masterOffers)
            {
                if (masterOffer.Coupon_RedemptionID != null)
                {
                    couponRedemptionIds.Add(masterOffer.Coupon_RedemptionID);
                }
            }
            List<string> offerEventIdsUnique = couponRedemptionIds.Distinct().ToList();
            string result = string.Join(",", offerEventIdsUnique);
            return result;
        }

    }
}
