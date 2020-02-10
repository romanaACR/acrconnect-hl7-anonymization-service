using System;

namespace AcrConnect.Anonymization.Service.Models
{
    public class IdMapping
    {
        /// <summary>
        /// Auto generated Id in database table
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// HL7 segment element whose value is being randomize
        /// </summary>
        public string Element { get; set; }
        /// <summary>
        /// Original value of the element being randomize
        /// </summary>
        public string OriginalId { get; set; }
        /// <summary>
        /// Generated random value 
        /// </summary>
        public string MappedId { get; set; } // generated random string for the given map
        /// <summary>
        /// Datetime stamp when the mapping was created
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Datetime stamp when the mapping was last updated
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
