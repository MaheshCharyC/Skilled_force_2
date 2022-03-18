using System;

namespace Skilled_Force_VS_22.Models
{
    public class JobViewModel {     
        public string jobId { get; set; }
        public string title { get; set; }
        public string position { get; set; }
        public string description { get; set; }

        public bool ShowJobId => !string.IsNullOrEmpty(jobId);
    }
}
