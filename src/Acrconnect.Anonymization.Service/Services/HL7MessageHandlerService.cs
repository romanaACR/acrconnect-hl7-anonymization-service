using AcrConnect.Anonymization.Service.Models;
using AcrConnect.HL7.Anonymization;
using HL7.Dotnetcore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AcrConnect.Anonymization.Service.Services
{
    public class HL7MessageHandlerService : IHL7MessageHandlerService
    {

        private AnonymizationServiceDBContext _context;
        private ILogger<HL7MessageData> _logger;
        IHttpClientFactory _httpClientFactory = null;


        public HL7MessageHandlerService(AnonymizationServiceDBContext context, ILogger<HL7MessageData> logger, IHttpClientFactory clientFactory)
        {
            _context = context;
            _logger = logger;
            _httpClientFactory = clientFactory;
        }

        public Task<bool> BroadcastHL7Message(HL7MessageData hl7MessageData, int apiId)
        {
            throw new NotImplementedException();
        }

        public async Task<HL7MessageData> DoAnonymizatione(HL7MessageData hl7MessageData, Int32 apiId)
        {
            try
            {
                var tmpProfile = await _context.AcrProfileDetails.FirstOrDefaultAsync(r => r.Id == apiId);

                if (tmpProfile == null)
                {
                    throw new Exception("Profile is missing");
                }

                IdMapping idMapping = null;


                var hl7MessageString = System.Text.Encoding.Default.GetString(hl7MessageData.Content);

                //TODO: PROFILE SHOULD TAKE FROM DB, THE BELOW CODE IS TEMP SOLUTION.
                string strProfile = GetRuleData();

                var profile = Profile.Load(strProfile);

                Message message = new Message(hl7MessageString);

                if (!message.ParseMessage())
                {
                    throw new InvalidOperationException("The given input is not a valid HL7 Message");
                }


                var idMappingRules = profile.Rules.Where(x => x.Operation == Operation.IDMAPPING);

                if (idMappingRules == null || idMappingRules.Count() < 1)
                {
                    throw new InvalidOperationException("Invalid rules.");
                }

                foreach (var pidRule in idMappingRules)
                {
                    int multisegmentId = 0;
                    var hasMultisegment = Int32.TryParse(pidRule.MultiSegmentValue, out multisegmentId);
                    int elementIndex = pidRule.ElementIndex != null ? (int)pidRule.ElementIndex : 0;
                    int elementPart = pidRule.ElementPart != null ? (int)pidRule.ElementPart : 0;
                    string fullSegment = pidRule.SegmentName + ((elementIndex > 0) ? "-" +
                        elementIndex.ToString() : string.Empty) + ((elementPart > 0) ? "." + elementPart.ToString() : string.Empty);


                    var segments = message.Segments(pidRule.SegmentName);

                    if (segments == null)
                    {
                        throw new InvalidOperationException("Invalid segments.");
                    }

                    using (var client = _httpClientFactory.CreateClient("IdMappingService"))
                    {
                        foreach (var segment in segments)
                        {
                            if (elementPart == 0)
                            {
                                var field = segment.GetAllFields()[elementIndex];
                                idMapping = new IdMapping() { Element = segment.Name, OriginalId = field.Value };
                                idMapping = await GetIdMapValue(idMapping);
                                field.Value = idMapping.MappedId;
                            }
                            else if (elementPart > 0)
                            {
                                var element = segment.Fields(elementIndex).Value;
                                var elementPartList = element.Split('^');
                                if (elementPartList.Count() >= elementPart)
                                {
                                    var elementPartValue = elementPartList[(int)elementPart - 1];
                                    idMapping = new IdMapping() { Element = segment.Name, OriginalId = elementPartValue };
                                    idMapping = await GetIdMapValue(idMapping);
                                    elementPartList[(int)elementPart - 1] = idMapping.MappedId;
                                    segment.Fields(elementIndex).Value = string.Join("^", elementPartList);
                                }
                            }
                        }
                    }
                }


                var mappedHl7Message = message.SerializeMessage(true);
                var result = profile.Anonymize(mappedHl7Message);
                if (result.IsAnonymized)
                {
                    var resultMessage = Encoding.ASCII.GetBytes(result.AnonymizedMessage);
                    hl7MessageData.Content = resultMessage;
                    hl7MessageData.Status = MessageStatus.Anonymiized;
                    return hl7MessageData;
                }
                else
                {
                    _logger.LogInformation("Failed to Anonymize the hl7 Message. MessageName: " + hl7MessageData.Name + ", ContentId:" + hl7MessageData.Id + ", appId=" + apiId + ", exception:");
                    throw new InvalidOperationException("Failed to load profile for anonymization. MessageName: " + hl7MessageData.Name + ", ContentId:" + hl7MessageData.Id + ", appId=" + apiId);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //   _logger.LogInformation("Failed to Anonymize the hl7 Message. MessageName: " + hl7MessageData.Name + ", ContentId:" + hl7MessageData.Id + ", appId=" + apiId + ", exception:" + ex.Message);
                //   throw new InvalidOperationException("Failed to Anonymize the hl7 Message. MessageName: " + hl7MessageData.Name + ", ContentId:" + hl7MessageData.Id + ", appId=" + apiId + ", exception:" + ex.Message);
            }

        }

        async Task<IdMapping> GetIdMapValue(IdMapping record)
        {
            IdMapping result = null;

            using (var client = _httpClientFactory.CreateClient("IdMappingService"))
            {
                var serializedObj = JsonConvert.SerializeObject(record, Formatting.Indented);
                var buffer = System.Text.Encoding.UTF8.GetBytes(serializedObj);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("/api/IdMapping/get/originalId", byteContent);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<IdMapping>();
                }
            }
            return result;
        }


        public string GetRuleData()
        {
            var fileName = "HL7AnonymizationRules.xml";
            var filePath = Path.Combine("Rules", fileName);

            return File.ReadAllText(filePath, Encoding.UTF8);
        }
    }

}
