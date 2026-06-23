namespace Nphies.Core.Models.Configurations
{
    public class DocumentConfiguration
    {
        public string ExternalURL { get; set; }
        public string DocumentUsername { get; set; }
        public string DocumentPassword { get; set; }
        public string FileDBConnection { get; set; }
        public string DocumentUser { get; set; }
        public string Domain { get; set; }
        public string DomainUserPassword { get; set; }
        public string documentQuery { get; set; }
    }

    public class ErClinicConfiguration
    {
        public string ErClinics { get; set; }
    }
}
