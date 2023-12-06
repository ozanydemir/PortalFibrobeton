using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.PeraRaporlar
{
    public class PeraV4ReportsViewModel
    {
        public List<PROJE_KART_NP> Projeler { get; set; }
        public List<UrunIzlemeModelItem> UrunIzleme { get; set; }
        public List<SiparisIzlemeModelItem> SiparisIzleme { get; set; }
        public List<UretimRaporuModelItem> UretimRaporu { get; set; }
        public List<KalipIzlemeModelItem> KalipIzleme { get; set; }
    }
}