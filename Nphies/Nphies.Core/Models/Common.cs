namespace Nphies.Core.Models
{
    public static class Common
    {
        public static string GetStatus(string status)
        {
            switch (status.ToLower().Trim())
            {
                case "queued":
                    return "67";
                case "rejected":
                    return "71";
                case "complete":
                    return "9";
                case "completed":
                    return "9";
                case "approved":
                    return "9";
                case "partial approved":
                case "partial":
                    return "8";
                case "internalerror":
                    return "20";
                case "error":
                    return "52";
                case "furtherdetails":
                    return "59";
                case "perror":
                    return "51";
                case "outcome":
                    return "99";
                case "pended":
                    return "93";
                case "nfailed":
                    return "107";
                case "nsuccess":
                    return "109";
                case "updateerror":
                    return "113";
                default:
                    return "113";
            }

        }
        public static class AdapterCode
        {
            public static string Nphies = "nphies-malath-1";
            public static string DHPO = "dhpo";
            public static string ZATCA = "zatca1";
            public static string AIURL = "aiurl";
            public static string Regular = "regular-company";
        }
    }
}
