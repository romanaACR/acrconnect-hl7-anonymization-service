using AcrConnect.Anonymization.Service.Models;
using System;
using System.Threading.Tasks;

namespace AcrConnect.Anonymization.Service.Services
{
    public interface IHL7MessageHandlerService
    {
        Task<HL7MessageData> DoAnonymizatione(HL7MessageData hl7MessageData, Int32 apiId);
        Task<bool> BroadcastHL7Message(HL7MessageData hl7MessageData, Int32 apiId);

    }
}
