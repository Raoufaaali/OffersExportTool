using OfferExport.Common;
using OfferExport.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferExport
{
    /// <summary>
    /// This class performs all required operations to export offers, segments and player from WinOasis DB into csv files
    /// </summary>
    class OasisOfferProcessor
    {
        private List<MasterOffers> masterOffers;
        private List<Offer> offers;
        private List<Segments> segments;
        private List<PlayerSegments> playerSegments;
        private Util _util;

        public OasisOfferProcessor(Util util)
        {
            _util = util;
        }

        /// <summary>
        /// This method does the main offer export processing
        /// </summary>
        public void ProcessOasisOffers()
        {      
            // get the masterOffers. This is the base data from which Offers, Segments and Player files are extracted. 
            masterOffers = GetWinOasisMasterOffers();
            
            // generate offers from master offers and write to csv
            offers = GetOffersFromMasterOffers(masterOffers);
            bool isOfferFileWritten = _util.WriteListToCsv<Offer>(offers, "Offers.csv");

            // generate segment details and write to csv
            segments = GetSegmentDetails(masterOffers);
            bool isSegmentDetailsWritten = _util.WriteListToCsv<Segments>(segments, "Segments.csv");

            // get player segment details and write to csv
            playerSegments = BuildPlayerSegmentFile(offers, masterOffers);
            bool isPlayerFileWritten = _util.WriteListToCsv<PlayerSegments>(playerSegments, "Players.csv");

            // display the result to the user
            NotifyResult(isOfferFileWritten, isSegmentDetailsWritten, isPlayerFileWritten);
        }

        /// <summary>
        /// Gets the current master offer data from WinOasis
        /// </summary>
        /// <returns></returns>
        private List<MasterOffers> GetWinOasisMasterOffers()
        {
            WinOasisDAL winOasisDAL = new WinOasisDAL();         
            List<MasterOffers> masterOffers = winOasisDAL.GetMasterOffers();
            return masterOffers;
        }

        /// <summary>
        /// Helper Method. Takes the master offer data and convert it to Offer entity
        /// </summary>
        /// <param name="masterOffers"></param>
        /// <returns>A list of Offer</returns>
        public List<Offer> GetOffersFromMasterOffers(List<MasterOffers> masterOffers)
        {
            List<Offer> offerList = masterOffers.Select(x => new Offer
            {
                OfferId = x.OfferId,
                OfferType = x.OfferType,
                SubType = x.SubType,
                OfferName = x.OfferName,
                ValidFromDate = x.ValidFromDate,
                ValidFromTime = x.ValidFromTime,
                ValidToDate = x.ValidToDate,
                ValidToTime = x.ValidToTime,
                ValidDays = x.ValidDays,
                Description = x.Description,
                AlternateDescription = x.AlternateDescription
            }).ToList();

            offerList = SetOfferValidDays(offerList);
            return offerList;
        }
        /// <summary>
        /// Helper method. Takes the master offer data and convert it to Segments
        /// </summary>
        /// <param name="masterOffers"></param>
        /// <returns>A list of Segments</returns>
        private List<Segments> GetSegmentDetails(List<MasterOffers> masterOffers )
        {
            List<Segments> segmentsList = masterOffers.Select(x => new Segments
            {
                SegmentName = x.OfferName,
                OfferID = x.OfferId
            }).ToList();
            return segmentsList;
        }

        /// <summary>
        /// Helper method. Takes 2 lists; MasterOffer and Offer and builds the PlayerSegment entity
        /// </summary>
        /// <param name="offers"></param>
        /// <param name="masterOffers"></param>
        /// <returns>A list of PlayerSegments</returns>
        private List<PlayerSegments> BuildPlayerSegmentFile(List<Offer> offers, List<MasterOffers> masterOffers)
        {
            // initialize 3 PlayerOfferMap. Each map will contain players associated with specific type of offers
            List<PlayerOfferMap> playerPromoOffers = new List<PlayerOfferMap>();
            List<PlayerOfferMap> playerCampaignOffers = new List<PlayerOfferMap>();
            List<PlayerOfferMap> playerEventOffers = new List<PlayerOfferMap>();
            List<PlayerOfferMap> AllOffersPlayers = new List<PlayerOfferMap>();

            // populate the 3 lists from the DB.
            WinOasisDAL dal = new WinOasisDAL();
            playerPromoOffers = dal.GetPlayersPromoOffers(offers);
            playerCampaignOffers = dal.GetPlayersCampaignOffers(masterOffers);
            playerEventOffers = dal.GetPlayersEventOffers(masterOffers);

            // Combine the 3 lists into 1 for easier processing
            AllOffersPlayers.AddRange(playerPromoOffers);
            AllOffersPlayers.AddRange(playerCampaignOffers);
            AllOffersPlayers.AddRange(playerEventOffers);

            // update the segment name in AllOffersPlayers from the original offers list
            List<PlayerOfferMap> result = AllOffersPlayers.Join(offers, d => d.OfferID, s => s.OfferId, (d, s) =>
            {
                d.SegmentName = s.OfferName;
               return d;
            }).ToList();

            // create a PlayerSegment list from the playerOfferMap list and return it
            List<PlayerSegments> playerSegmentList = result.Select(x => new PlayerSegments
            {
                SegmentName = x.SegmentName,
                PlayerID = x.PlayerID,
            }).ToList();
            return playerSegmentList;
        }

        /// <summary>
        /// Helper method. Takes a list of offers and for each offer, sets the unique valid days
        /// </summary>
        /// <param name="offerList"></param>
        /// <returns>A list of Offer that has ValidDays populated</returns>
        private List<Offer> SetOfferValidDays(List<Offer> offerList)
        {
            foreach (Offer offer in offerList)
            {
                // clear the valid days column before populating it
                offer.ValidDays = "";
                string uniqueValidDays = "";
                List<string> listOfOfferAllValidDays = new List<string>();

                for (DateTime dt = offer.ValidFromDate.Date ; dt.Date <= offer.ValidToDate.Date; dt = dt.AddDays(1) )
                {
                    listOfOfferAllValidDays.Add(dt.DayOfWeek.ToString());
                }
                
                List<string> listOfOfferUniqueValidDays = listOfOfferAllValidDays.Distinct().ToList();
                uniqueValidDays = string.Join("; ", listOfOfferUniqueValidDays);
                offer.ValidDays = uniqueValidDays;
            }
            return offerList;
        }

        private void NotifyResult(bool offerFile, bool segmentFile, bool playersFile)
        {
            if (offerFile && segmentFile && playersFile)
            {
                string successMessage = $"Completed successfully. Offer files were exported to Desktop/Offers";
                _util.LogColoredMessage(successMessage, Util.SuccessStatus.SUCCESS);
            }
            else
            {
                string failureMesage = "Something went wrong! See the logs for details. ";
                _util.LogColoredMessage(failureMesage, Util.SuccessStatus.FAILURE);
            }        
        }
    }
}
