using AcrConnect.Anonymization.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcrConnect.Anonymization.Service.Services
{
    public interface IAcrProfileDetailsService
    {
        Task<IList<AcrProfileDetail>> GetAcrProfileDetails();

        Task<bool> SaveAcrProfileDetails(AcrProfileDetail record);
    }
}
