using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PortalFibrobeton.Models.Class
{
    public class TeknikResimler
    {
        public List<ARSIV_DOKUMAN> ArsivListe { get; set; }

        public List<DOKUMAN_YONETIM_LOG> ArsivListeNP { get; set; }

        public List<PROJE_KART_NP> ProjeListNP { get; set; }

        public List<PROJE_KART> ProjeList { get; set; }

        public List<string> AltKlasorlerList { get; set; }
    }
}