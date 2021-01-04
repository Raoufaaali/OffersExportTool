using System;
using System.Collections.Generic;
using System.Text;

namespace OfferExport.Entities
{
	/// <summary>
	/// Class Offer
	/// </summary>
	class Offer
    {

		/// <summary>
		/// represents the offer id
		/// </summary>
		/// <value>sets or gets the offer id</value>
		[CsvHelper.Configuration.Attributes.Name("offer#")] // change csv header from OfferId to offer#
		public string OfferId { get; set; }

		/// <summary>
		/// represents the offer name
		/// </summary>
		/// <value>sets or gets the offer name</value>
		public string OfferName { get; set; }

		/// <summary>
		/// represents the offer type
		/// </summary>
		/// <value>sets or gets the offer type</value>
		public string OfferType { get; set; }

		/// <summary>
		/// represents the offer subtype
		/// </summary>
		/// <value>sets or gets the offer subtype</value>
		public string SubType { get; set; }

		/// <summary>
		/// represents the offer start date
		/// </summary>
		/// <value>sets or gets the offer ValidFromDate</value>
		public DateTime ValidFromDate { get; set; }

		/// <summary>
		/// represents the offer start time
		/// </summary>
		/// <value>sets or gets the offer ValidFromTime</value>
		public TimeSpan ValidFromTime { get; set; }

		/// <summary>
		/// represents the offer end date
		/// <value>sets or gets the offer ValidToDate</value>
		public DateTime ValidToDate { get; set; }

		/// <summary>
		/// represents the offer end time
		/// </summary>
		/// <value>sets or gets the offer ValidToTime</value>
		public TimeSpan ValidToTime { get; set; }

		/// <summary>
		/// represents the days on which the offer is valid
		/// </summary>
		/// <value>sets or gets the offer ValidDays</value>
		public string ValidDays { get; set; }

		/// <summary>
		/// represents the offer description
		/// </summary>
		/// <value>sets or gets the offer description</value>
		public string Description { get; set; }

		/// <summary>
		/// represents the offer alternate description
		/// </summary>
		/// <value>sets or gets the offer alternate description</value>
		public string AlternateDescription { get; set; }

		/// <summary>
		/// represents the offer value
		/// </summary>
		/// <value>sets or gets the offer value</value>
		public string Value { get; set; }
	}
}
