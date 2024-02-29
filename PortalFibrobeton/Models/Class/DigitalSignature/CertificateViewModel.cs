using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.DigitalSignature
{
    public class CertificateViewModel
    {
        public string Subject { get; set; }
        public string Thumbprint { get; set; }
        public string CertPassword { get; set; }
    }
}