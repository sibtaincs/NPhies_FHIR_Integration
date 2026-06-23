using Microsoft.Extensions.Configuration;
using Nphies.Core.Models.Configurations;
using System;
using System.Configuration;
using System.Linq;

namespace Nphies.Core.Helper
{
    public static class EncounterTypeHelper
    {
        public static bool IsERClinic(int clinicId, IConfiguration configuration)
        {
            var erClinicConfig = configuration["ErClinics"].ToString();

            int [] clinics = Array.ConvertAll<string, int>(erClinicConfig.Split(','), Convert.ToInt32);

            return clinics.Contains(clinicId);
        }
    }
}
