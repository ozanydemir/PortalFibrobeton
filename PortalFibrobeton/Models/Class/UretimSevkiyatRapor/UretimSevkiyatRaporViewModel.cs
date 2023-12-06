using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.UretimSevkiyatRapor
{
    public class UretimSevkiyatRaporViewModel
    {
        public List<Hedefler> Hedefler { get; set; }
        public List<PROJE_BLOK_CEPHE> Cepheler { get; set; }

        public List<UretimSevkiyatRaporViewModelItem> Siparisler { get; set; }

        public List<UretimSevkiyatRaporViewModelItem> Dokum { get; set; }

        public List<UretimSevkiyatRaporViewModelItem> DokumTotalTarihsiz { get; set; }

        public List<UretimSevkiyatRaporViewModelItem> Sevkiyat { get; set; }

        public List<UretimSevkiyatRaporViewModelItem> SevkiyatTotalTarihsiz { get; set; }

        public List<UretimSevkiyatRaporViewModelItem> SiparisHafta { get; set; }

    }
}