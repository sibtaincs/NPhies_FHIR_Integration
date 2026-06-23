using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nphies.Core.Data.Common
{
    public class DHPO_Observation
    {
        /// <summary>
        /// Text 
        /// </summary>
        public short Type { get; set; }
        /// <summary>
        /// Procedure Description
        /// </summary>
        public short Code { get; set; }
        /// <summary>
        /// CPT 4 Codes
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// ICD 10
        /// </summary>
        public string ValueType { get; set; }

    }
    public class Activity
    {
        public string Code { get; set; }
        // Constructor that accepts a code parameter
        public Activity(string code)
        {
            Code = code;
        }
    }

}
