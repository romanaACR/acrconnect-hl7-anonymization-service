using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcrConnect.Anonymization.Service.Models
{
    public class HL7MessageData
    {
        [JsonProperty("Id")]
        public Int32 Id { get; set; }

        [JsonProperty("contentId")]
        public Int32 ContentId { get; set; }

        [JsonProperty("dataPrimaryId")]
        public Int32 DataPrimaryId { get; set; }

        [JsonProperty("dataId")]
        public Guid DataId { get; set; }

        [JsonProperty("content")]
        public byte[] Content { get; set; }

        [JsonProperty("messageName")]
        public string Name { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("contentLength")]
        public int ContentLength { get; set; }

        public MessageStatus Status { get; set; }
    }
}
