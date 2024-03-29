using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DocumentVerification
    {
        public string? RequestNumber { get; set; }
        public string? RequestType { get; set; }
        public AadharInfo? AadharInfo { get; set; }
        public VoterInfo? VoterInfo { get; set; }
        public PanCardInfo? PanCardInfo { get; set; }
        public DriverLicenseInfo? DriverLicenseInfo { get; set; }
        public BusinessInCorpInfo? BusinessInCorpInfo { get; set; }
        public BusinessPanCardInfo? BusinessPanCardInfo { get; set; }
    }
}
