using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.UretimSevkiyatRapor
{
    public class UretimSevkiyatRaporViewModelItem
    {
        //Döküm

        public int ID { get; set; }
        public string Poz_No { get; set; }
        public DateTime DOKUM_TARIH { get; set; }

        public string DOKUM_BARKOD { get; set; }

        public string CEPHE_ADI { get; set; }

        public string BLOK_ADI { get; set; }

        public DateTime SEVK_TARIH { get; set; }

        public int Sevk_Miktar { get; set; }

        public int Siparis_Miktar { get; set; }

        public int HOLD { get; set; }
    }
}