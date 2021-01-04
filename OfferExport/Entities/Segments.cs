using System;
using System.Collections.Generic;
using System.Text;

namespace OfferExport.Entities
{
    /// <summary>
    /// Segment Class.
    /// </summary>
    class Segments
    {
        /// <summary>
        /// represnt the Segment
        /// </summary>
        /// <value>sets or gets the Segment</value>
        public string SegmentName { get; set; }

        /// <summary>
        /// represnt the OfferID
        /// </summary>
        /// <value>sets or gets the OfferID</value>
        [CsvHelper.Configuration.Attributes.Name("associatedoffers")] // change csv header from OfferId to associatedoffers
        public string OfferID { get; set; }
    }
}
