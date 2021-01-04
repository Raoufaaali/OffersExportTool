using System;
using System.Collections.Generic;
using System.Text;

namespace OfferExport.Entities
{
    /// <summary>
    /// Class PlayerOfferMap.
    /// Maps Offers to Players
    /// </summary>
    struct PlayerOfferMap
    {
        /// <summary>
        /// represent the SegmentName
        /// </summary>
        /// <value>sets or gets the SegmentName</value>
        public string SegmentName { get; set; }

        /// <summary>
        /// represent the SegmentName
        /// </summary>
        /// <value>sets or gets the SegmentName</value>
        public string PlayerID { get; set; }

        /// <summary>
        /// represent the OfferID
        /// </summary>
        /// <value>sets or gets the OfferID</value>
        public string OfferID { get; set; }

        /// <summary>
        /// represent the SubType
        /// </summary>
        /// <value>sets or gets the SubType</value>
        public string SubType { get; set; }
    }
}
