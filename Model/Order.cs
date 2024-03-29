using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Order
    {
        public RequestOrigin? RequestOrigin { get; set; }
        public PersonalInfo? PersonalInfo { get; set; }
        public BusinessInfo? BusinessInfo { get; set; }
        public List<Domain>? Domains { get; set; }
        public Payment? Payment { get; set; }
        public List<Payment>? Payments { get; set; }
        public AadharInfo? AadharInfo { get; set; }
        public PanCardInfo? PanCardInfo { get; set; }
        public BusinessInCorpInfo? BusinessInCorpInfo { get; set; }
        public BusinessPanCardInfo? BusinessPanCardInfo { get; set; }
        public VoterInfo? VoterInfo { get; set; }
        public DriverLicenseInfo? DriverLicenseInfo { get; set; }
    }
}
