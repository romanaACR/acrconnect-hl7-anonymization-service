using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AcrConnect.Anonymization.Service
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MessageStatus
    {
        Unknown = 0,
        Received = 1,
        ValidationFailed = 2,
        ValidationSuccess = 3,
        AnonymizationFailed = 4,
        Anonymiized = 5,
        SentSuccess = 6,
        SentFailed = 7
    }
}
