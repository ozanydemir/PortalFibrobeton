using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Entity
{
    public class AnketSonucModel
    {
        public int SelectedAnketID { get; set; }
        public List<Anketler> AnketList { get; set; }

        public List<AnketProjeler> ProjeList { get; set; }
        public List<IGrouping<string, AnketSonuclar>> AnketSonuclari { get; internal set; }

        //public List<AnketProjeler>
    }
}