using System;
using System.Collections.Generic;

namespace OfferExport.Entities
{
	/// <summary>
	/// Class MasterOffers
	/// </summary>
    class MasterOffers : Offer
    {
		/// <summary>
		/// represents the MasterOffers Offer_EventID
		/// </summary>
		/// <value>sets of gets the Offer_EventID</value>
		public string Offer_EventID { get; set; }

		/// <summary>
		/// represents the MasterOffers Coupon_RedemptionID
		/// </summary>
		/// <value>sets of gets the Coupon_RedemptionID</value>
		public string Coupon_RedemptionID { get; set; }

		/// <summary>
		/// represents the number of players in the specified MasterOffer
		/// </summary>
		/// <value>sets of gets the CountOfPlayersInOffer</value>
		public int CountOfPlayersInOffer { get; set; }		
	}
}
