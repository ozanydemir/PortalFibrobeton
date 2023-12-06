using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalFibrobeton.Models.Entity;
using System.Data.Entity;
using PortalFibrobeton.Models.Class.PeraRaporlar;
using OfficeOpenXml.Style;
using System.Reflection;
using System.Data.SqlClient;
using System.Text;

namespace PortalFibrobeton.Controllers.PeraFibro
{
    public class PeraV4RaporlarController : Controller
    {

        PERA_FIBRO_ULTIMATEEntities db2 = new PERA_FIBRO_ULTIMATEEntities();
        string connectionString = "data source=10.20.41.101;initial catalog=PERA_FIBRO_ULTIMATE;user id=oz;password=Oz200an;MultipleActiveResultSets=True;App=EntityFramework";
        // GET: PeraV4Raporlar
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UretimRaporu()
        {
            var projeler = db2.PROJE_KART.OrderBy(a => a.PROJE_ADI).ToList();
            PeraV4ReportsViewModel viewModel = new PeraV4ReportsViewModel()
            {
                Projeler = projeler,
                UretimRaporu = new List<UretimRaporuModelItem>()

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UretimRaporu(DateTime bas, DateTime bit, string proje, string hat, string ekip, string poz)
        {
            var projeler = db2.PROJE_KART.OrderBy(a => a.PROJE_ADI).ToList();

            bit = bit.AddDays(1);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                StringBuilder SQL = new StringBuilder();

                SQL.Append("select KIE.IS_EMRI_BARKOD,KIE.IS_EMRI_TARIH,PK.PROJE_ADI,PROJE_TURU,SF.SIPARIS_TARIH,SF.SIPARIS_NO,P.MALZEME_ADI,P.BOY,P.EN,P.MALZEME_CINSI,Convert(DateTime, Convert(VarChar, D.DOKULDU_TARIH, 103)) DOKUM_TARIHX,D.HAT,D.DOKUM_EKIP,P.POZ_NO,SP.SON_SIPARIS_MIKTAR,sum(D.BIR_DOKUM_MIKTAR) as DOK_ADET,round(P.BIRIM_M2,3) as BIRIM_M2,round(P.BIRIM_M2 * sum(BIR_DOKUM_MIKTAR),3) as SIP_M2,P.DOKUM_M2,round(P.DOKUM_M2 * sum(BIR_DOKUM_MIKTAR),3) as DOK_M2,sum(D.BIRIM_KG*D.BIR_DOKUM_MIKTAR) as URUN_KGX,sum(F.FRAME_BIRIM_KG*D.BIR_DOKUM_MIKTAR) as FRAME_KGX,SP.SON_SIPARIS_MIKTAR-SP.URETIM_MIKTAR as KAL_DOK,SP.BLOK_ADI,SP.CEPHE_ADI,P.POZ_TURU from DOKUM_IS_EMRI_DOKUM D inner join POZ_KART P on P.ID=D.POZ_ID inner join POZ_SIPARIS SP on SP.POZ_ID=D.POZ_ID and SP.ID = D.SIPARIS_POZ_ID inner join POZ_SIPARIS_FORM SF on SF.ID = SP.SIPARIS_ID inner join PROJE_KART PK on SP.PROJE_ID = PK.ID left join KALIP_IS_EMRI KIE on KIE.ID=D.KALIP_IS_ID left join FRAME_TEDARIK F on SP.POZ_ID=F.POZ_ID and F.FRAME_BARKOD = D.FRAME_BARKOD where D.DOKULDU = 1 and D.DOKULDU_TARIH between @p1 and @p2 ");

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@p1",bas),
                    new SqlParameter("@p2",bit)
                };

                if (!string.IsNullOrEmpty(proje))
                {
                    SQL.Append(" AND PK.PROJE_ADI = @p3");
                    parameters.Add(new SqlParameter("@p3", proje));
                }

                if (!string.IsNullOrEmpty(ekip))
                {
                    SQL.Append(" AND d.DOKUM_EKIP = @p4");
                    parameters.Add(new SqlParameter("@p4", ekip));
                }
                if (!string.IsNullOrEmpty(hat))
                {
                    SQL.Append(" AND d.HAT = @p5");
                    parameters.Add(new SqlParameter("@p5", hat));
                }
                if (!string.IsNullOrEmpty(poz))
                {
                    SQL.Append(" AND p.POZ_NO = @p6");
                    parameters.Add(new SqlParameter("@p6", poz));
                }

                SQL.Append(" group by KIE.IS_EMRI_BARKOD,KIE.IS_EMRI_TARIH,Convert(DateTime, Convert(VarChar, D.DOKULDU_TARIH, 103)),PK.PROJE_ADI,PROJE_TURU,P.MALZEME_ADI,P.BOY,P.EN,P.MALZEME_CINSI,SP.SON_SIPARIS_MIKTAR,D.HAT,D.DOKUM_EKIP,P.POZ_NO,P.BIRIM_M2,P.DOKUM_M2,SP.URETIM_MIKTAR,SF.SIPARIS_TARIH,SF.SIPARIS_NO,SP.BLOK_ADI,SP.CEPHE_ADI,P.POZ_TURU order by DOKUM_TARIHX");
                
                SqlCommand command = new SqlCommand(SQL.ToString(), connection);
                command.Parameters.AddRange(parameters.ToArray());

                using(SqlDataReader reader = command.ExecuteReader())
                {
                    List<UretimRaporuModelItem> sonuclar = new List<UretimRaporuModelItem>();
                    while (reader.Read())
                    {
                        UretimRaporuModelItem uretim = new UretimRaporuModelItem
                        {
                            PROJE_ADI = reader.IsDBNull(reader.GetOrdinal("PROJE_ADI")) ? null : reader.GetString(reader.GetOrdinal("PROJE_ADI")),
                            PROJE_TURU = reader.IsDBNull(reader.GetOrdinal("PROJE_TURU")) ? null : reader.GetString(reader.GetOrdinal("PROJE_TURU")),
                            SIPARIS_TARIH = reader.IsDBNull(reader.GetOrdinal("SIPARIS_TARIH")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("SIPARIS_TARIH")),
                            SIPARIS_NO = reader.IsDBNull(reader.GetOrdinal("SIPARIS_NO")) ? null : reader.GetString(reader.GetOrdinal("SIPARIS_NO")),
                            POZ_NO = reader.IsDBNull(reader.GetOrdinal("POZ_NO")) ? null : reader.GetString(reader.GetOrdinal("POZ_NO")),
                            MALZEME_ADI = reader.IsDBNull(reader.GetOrdinal("MALZEME_ADI")) ? null : reader.GetString(reader.GetOrdinal("MALZEME_ADI")),
                            MALZEME_CINSI = reader.IsDBNull(reader.GetOrdinal("MALZEME_CINSI")) ? null : reader.GetString(reader.GetOrdinal("MALZEME_CINSI")),
                            EN = reader.IsDBNull(reader.GetOrdinal("EN")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("EN")),
                            BOY = reader.IsDBNull(reader.GetOrdinal("BOY")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("BOY")),
                            SON_SIPARIS_MIKTARI = reader.IsDBNull(reader.GetOrdinal("SON_SIPARIS_MIKTAR")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("SON_SIPARIS_MIKTAR")),
                            KALAN_DOKUM = reader.IsDBNull(reader.GetOrdinal("KAL_DOK")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("KAL_DOK")),
                            DOKUM_TARIH = reader.IsDBNull(reader.GetOrdinal("DOKUM_TARIHX")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DOKUM_TARIHX")),
                            HAT = reader.IsDBNull(reader.GetOrdinal("HAT")) ? null : reader.GetString(reader.GetOrdinal("HAT")),
                            DOKULEN_ADET = reader.IsDBNull(reader.GetOrdinal("DOK_ADET")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("DOK_ADET")),
                            SIPARIS_METREKARE = reader.IsDBNull(reader.GetOrdinal("BIRIM_M2")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("BIRIM_M2")),
                            DOKUM_SIPARIS_METREKARE = reader.IsDBNull(reader.GetOrdinal("SIP_M2")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("SIP_M2")),
                            DOKUM_METREKARE = reader.IsDBNull(reader.GetOrdinal("DOKUM_M2")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("DOKUM_M2")),
                            DOK_DOKUM_M2 = reader.IsDBNull(reader.GetOrdinal("DOK_M2")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("DOK_M2")),
                            DOKUM_URUN_AGIRLIK = reader.IsDBNull(reader.GetOrdinal("URUN_KGX")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("URUN_KGX")),
                            DOKUM_FRAME_AGIRLIK = reader.IsDBNull(reader.GetOrdinal("FRAME_KGX")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("FRAME_KGX")),
                            IS_EMRI_BARKODU = reader.IsDBNull(reader.GetOrdinal("IS_EMRI_BARKOD")) ? null : reader.GetString(reader.GetOrdinal("IS_EMRI_BARKOD")),
                            IS_EMRI_TARIH = reader.IsDBNull(reader.GetOrdinal("IS_EMRI_TARIH")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("IS_EMRI_TARIH")),
                            BLOK_ADI = reader.IsDBNull(reader.GetOrdinal("BLOK_ADI")) ? null : reader.GetString(reader.GetOrdinal("BLOK_ADI")),
                            CEPHE_ADI = reader.IsDBNull(reader.GetOrdinal("CEPHE_ADI")) ? null : reader.GetString(reader.GetOrdinal("CEPHE_ADI")),
                            POZ_TURU = reader.IsDBNull(reader.GetOrdinal("POZ_TURU")) ? null : reader.GetString(reader.GetOrdinal("POZ_TURU")),
                            DOKUM_EKIP = reader.IsDBNull(reader.GetOrdinal("DOKUM_EKIP")) ? null : reader.GetString(reader.GetOrdinal("DOKUM_EKIP"))

                        };

                        sonuclar.Add(uretim);
                    }

                    var viewModel = new PeraV4ReportsViewModel
                    {
                        Projeler = projeler,
                        UretimRaporu = sonuclar
                    };

                    return View(viewModel);
                }
            }

            
        }

        [HttpGet]
        public ActionResult UrunIzlemeRaporu()
        {
            var projeler = db2.PROJE_KART.OrderBy(a => a.PROJE_ADI).ToList();

            PeraV4ReportsViewModel viewModel = new PeraV4ReportsViewModel()
            {
                Projeler = projeler,
                UrunIzleme = new List<UrunIzlemeModelItem>()

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UrunIzlemeRaporu(DateTime bas, DateTime bit, string proje, string poz, string barkod)
        {
            var projeler = db2.PROJE_KART.OrderBy(a => a.PROJE_ADI).ToList();

            var results = (from d in db2.DOKUM_IS_EMRI_DOKUM
                           join PK in db2.POZ_KART on d.POZ_ID equals PK.ID
                           join DIE in db2.DOKUM_IS_EMRI on d.DOKUM_IS_ID equals DIE.ID
                           join IE in db2.KALIP_IS_EMRI on d.KALIP_IS_ID equals IE.ID
                           join PR in db2.PROJE_KART on d.PROJE_ID equals PR.ID
                           join PS in db2.POZ_SIPARIS on new { d.POZ_ID, SIPARIS_POZ_ID = (long)d.SIPARIS_POZ_ID } equals new { PS.POZ_ID, SIPARIS_POZ_ID = PS.ID } into psGroup
                           //join PS in db2.POZ_SIPARIS on new { d.POZ_ID, SIPARIS_POZ_ID = (int?)d.ID } equals new { PS.POZ_ID, SIPARIS_POZ_ID = (int?)PS.ID } into psGroup
                           from PS in psGroup.DefaultIfEmpty()
                           join SF in db2.POZ_SIPARIS_FORM on PS.SIPARIS_ID equals SF.ID into sfGroup
                           from SF in sfGroup.DefaultIfEmpty()
                           join SSF in db2.SEVK_SANDIKLAMA_FIS on d.SANDIK_FIS_ID equals SSF.ID into ssfGroup
                           from SSF in ssfGroup.DefaultIfEmpty()
                           join SKF in db2.SEVK_FIS on d.SEVK_ID equals SKF.ID into skfGroup
                           from SKF in skfGroup.DefaultIfEmpty()
                           where 
                           d.ID > 0 
                           && d.PLAN_DOKUM_TARIH >= bas && d.PLAN_DOKUM_TARIH <= bit
                           && (string.IsNullOrEmpty(proje) || PR.PROJE_ADI == proje)
                           && (string.IsNullOrEmpty(poz) || PK.POZ_NO == poz)
                           && (string.IsNullOrEmpty(barkod) || d.DOKUM_BARKOD == barkod)
                           orderby d.PLAN_DOKUM_TARIH
                           select new UrunIzlemeModelItem 
                           {
                               DOKUM_BARKOD = d.DOKUM_BARKOD,
                               PLAN_DOKUM_TARIH = (DateTime)d.PLAN_DOKUM_TARIH,
                               URUN_DURUMU = d.URUN_DURUMU,
                               BIRIM_KG = (double)d.BIRIM_KG,
                               HAT = d.HAT,
                               DOKUM_EKIP = d.DOKUM_EKIP,
                               V_URUN_M2_KULLAN = (bool)PR.V_URUN_M2_KULLAN,
                               FRAME_BARKOD = d.FRAME_BARKOD,
                               KUR_SURE_SONU = (DateTime)d.KUR_SURE_SONU,
                               PS_KAT_ADI = PS.KAT_ADI,
                               DOKUME_BASLANDI = (bool)d.DOKUME_BASLANDI,
                               GEVSETME_YAPILDI = (bool)d.GEVSETME_YAPILDI,
                               GEVSETME_ZAMANI = (DateTime)d.GEVSETME_ZAMANI,
                               GEVSETME_YAPAN = d.GEVSETME_YAPAN,
                               DOKULDU = (bool)d.DOKULDU,
                               SOKULDU = (bool)d.SOKULDU,
                               CATLAK_KONTROL_ONAY = (bool)d.CATLAK_KONTROL_ONAY,
                               YUZEY_ISLEM_1_BASLA = (bool)d.YUZEY_ISLEM_1_BASLA,
                               YUZEY_ISLEM_1_BASLA_ZAMAN = (DateTime)d.YUZEY_ISLEM_1_BASLA_ZAMAN,
                               YUZEY_ISLEM_1_BASLA_YAPAN = d.YUZEY_ISLEM_1_BASLA_YAPAN,
                               YUZEY_ISLEM_1_TAMAM = (bool)d.YUZEY_ISLEM_1_TAMAM,
                               YUZEY_ISLEM_1_ZAMAN = (DateTime)d.YUZEY_ISLEM_1_ZAMAN,
                               YUZEY_ISLEM_1_YAPAN = d.YUZEY_ISLEM_1_YAPAN,
                               YUZEY_ISLEM_2_BASLA = (bool)d.YUZEY_ISLEM_2_BASLA,
                               YUZEY_ISLEM_2_BASLA_ZAMAN = (DateTime)d.YUZEY_ISLEM_2_BASLA_ZAMAN,
                               YUZEY_ISLEM_2_BASLA_YAPAN = d.YUZEY_ISLEM_2_BASLA_YAPAN,
                               YUZEY_ISLEM_2_TAMAM = (bool)d.YUZEY_ISLEM_2_TAMAM,
                               YUZEY_ISLEM_2_ZAMAN = (DateTime)d.YUZEY_ISLEM_2_ZAMAN,
                               YUZEY_ISLEM_2_YAPAN = d.YUZEY_ISLEM_2_YAPAN,
                               YUZEY_ISLEM_3_BASLA = (bool)d.YUZEY_ISLEM_3_BASLA,
                               YUZEY_ISLEM_3_BASLA_ZAMAN = (DateTime)d.YUZEY_ISLEM_3_BASLA_ZAMAN,
                               YUZEY_ISLEM_3_BASLA_YAPAN =d.YUZEY_ISLEM_3_BASLA_YAPAN,
                               YUZEY_ISLEM_3_TAMAM = (bool)d.YUZEY_ISLEM_3_TAMAM,
                               YUZEY_ISLEM_3_ZAMAN = (DateTime)d.YUZEY_ISLEM_3_ZAMAN,
                               YUZEY_ISLEM_3_YAPAN = d.YUZEY_ISLEM_3_YAPAN,
                               YUZEY_ISLEM_4_YAPAN = d.YUZEY_ISLEM_4_YAPAN,
                               YUZEY_ISLEM_4_TAMAM = (bool)d.YUZEY_ISLEM_4_TAMAM,
                               YUZEY_ISLEM_5_YAPAN = d.YUZEY_ISLEM_5_YAPAN,
                               YUZEY_ISLEM_5_TAMAM = (bool)d.YUZEY_ISLEM_5_TAMAM,
                               YUZEY_ISLEM_4_ZAMAN = (DateTime)d.YUZEY_ISLEM_4_ZAMAN,
                               YUZEY_ISLEM_5_ZAMAN = (DateTime)d.YUZEY_ISLEM_5_ZAMAN,
                               EK_ISLEM_1_ADI = d.EK_ISLEM_1_ADI,
                               EK_ISLEM_1_TAMAM = (bool)d.EK_ISLEM_1_TAMAM,
                               EK_ISLEM_1_YAPAN = d.EK_ISLEM_1_YAPAN,
                               EK_ISLEM_1_ZAMAN = (DateTime)d.EK_ISLEM_1_ZAMAN,
                               EK_ISLEM_2_ADI = d.EK_ISLEM_2_ADI,
                               EK_ISLEM_2_TAMAM = (bool)d.EK_ISLEM_2_TAMAM,
                               EK_ISLEM_2_YAPAN = d.EK_ISLEM_2_YAPAN,
                               EK_ISLEM_2_ZAMAN = (DateTime)d.EK_ISLEM_2_ZAMAN,
                               EK_ISLEM_3_ADI = d.EK_ISLEM_3_ADI,
                               EK_ISLEM_3_TAMAM = (bool)d.EK_ISLEM_3_TAMAM,
                               EK_ISLEM_3_YAPAN = d.EK_ISLEM_3_YAPAN,
                               EK_ISLEM_3_ZAMAN = (DateTime)d.EK_ISLEM_3_ZAMAN,
                               BLOK_ADI = d.BLOK_ADI,
                               CEPHE_ADI = d.CEPHE_ADI,
                               K_S_D_YUZEY_ISLEMI_YAPILDI = d.K_S_D_DUZEY_ISL_YAPILDI,
                               CEPHE_KAT_ADI = d.CEPHE_KAT_ADI,
                               URETIM_KONTROL_ONAY = d.URETIM_KONTROL_ONAY,
                               SEVK_SAHA_ALINDI = d.SEVK_SAHA_ALINDI,
                               Y_I_T_SEVK_SAHA_ALINDI = d.Y_I_T_SEVK_SAHA_ALINDI,
                               K_S_D_SEVK_SAHA_ALINDI = d.K_S_D_SEVK_SAHA_ALINDI,
                               K_S_D_SEVK_ONAY = d.K_S_D_SEVK_ONAY,
                               POZ_HOLD = PK.HOLD,
                               MONTAJ_YAPILDI = d.MONTAJ_YAPILDI,
                               MUSTERI_SEVK_PLAN_NO = d.MUSTERI_SEVK_PLAN_NO,
                               C3D_TARAMA_YAPILDI = (bool)d.C3D_TARAMA_YAPILDI,
                               C3D_TARAMA_YAPAN = d.C3D_TARAMA_YAPAN,
                               C3D_TARAMA_ZAMAN = (DateTime)d.C3D_TARAMA_ZAMAN,
                               YUZEY_ISLEM_4_BASLA = (bool)d.YUZEY_ISLEM_4_BASLA,
                               YUZEY_ISLEM_4_BASLA_ZAMAN = (DateTime)d.YUZEY_ISLEM_4_BASLA_ZAMAN,
                               YUZEY_ISLEM_4_BASLA_YAPAN = d.YUZEY_ISLEM_4_BASLA_YAPAN,
                               YUZEY_ISLEM_5_BASLA = (bool)d.YUZEY_ISLEM_5_BASLA,
                               YUZEY_ISLEM_5_BASLA_YAPAN = d.YUZEY_ISLEM_5_BASLA_YAPAN,
                               YUZEY_ISLEM_5_BASLA_ZAMAN = (DateTime)d.YUZEY_ISLEM_5_BASLA_ZAMAN,
                               EK_ISLEM_4_ADI = d.EK_ISLEM_4_ADI,
                               EK_ISLEM_4_TAMAM = (bool)d.EK_ISLEM_4_TAMAM,
                               EK_ISLEM_4_YAPAN =d.EK_ISLEM_4_YAPAN,
                               SANDIK_BARKODU = d.SANDIK_BARKODU,
                               SEVK_NO = d.SEVK_NO,
                               AGIRLIK_SORUNLU = (bool)d.AGIRLIK_SORUNLU,
                               SIPARIS_ID = (int)d.SIPARIS_ID,
                               SIPARIS_POZ_ID = (int)d.SIPARIS_POZ_ID,
                               URUN_HOLD = (bool)d.URUN_HOLD,
                               URUN_HOLD_YAPAN = d.URUN_HOLD_YAPAN,
                               URUN_HOLD_ZAMAN = (DateTime)d.URUN_HOLD_ZAMAN,
                               URUN_KARANTINA = (bool)d.URUN_KARANTINA,
                               URUN_KARANTINA_YAPAN = d.URUN_KARANTINA_YAPAN,
                               URUN_KARANTINA_ZAMAN = (DateTime)d.URUN_KARANTINA_ZAMAN,
                               URUN_HURDA = (bool)d.URUN_HURDA,
                               URUN_HURDA_YAPAN = d.URUN_HURDA_YAPAN,
                               URUN_HURDA_ZAMAN = (DateTime)d.URUN_HURDA_ZAMAN,
                               IMHA_BEKLIYOR = (bool)d.IMHA_BEKLIYOR,
                               IMHA_BEKLIYOR_YAPAN = d.IMHA_BEKLIYOR_YAPAN,
                               IMHA_BEKLIYOR_ZAMAN = (DateTime)d.IMHA_BEKLIYOR_ZAMAN,
                               FAB_KAB_BEK_YAPAN = d.FAB_KAB_BEK_YAPAN,
                               TAMIR_BAKIM_BEKLIYOR = (bool)d.TAMIR_BAKIM_BEKLIYOR,
                               TAMIR_BAKIM_BEK_YAPAN = d.TAMIR_BAKIM_BEK_YAPAN,
                               TAMIR_BAKIM_BEK_ZAMAN = (DateTime)d.TAMIR_BAKIM_BEK_ZAMAN,
                               FABRIKAYA_KABUL_BEKLIYOR = (bool)d.FABRIKAYA_KABUL_BEKLIYOR,
                               FAB_KAB_BEK_ZAMAN = (DateTime)d.FAB_KAB_BEK_ZAMAN,
                               POZ_TURU = PK.POZ_TURU,
                               POZ_NO = PK.POZ_NO,
                               POZ_ADI = PK.POZ_ADI,
                               BOY = (double)PK.BOY,
                               EN = (double)PK.EN,
                               RENK =PK.RENK,
                               MALZEME_YUZEYI = PK.MALZEME_YUZEYI,
                               MALZEME_GRUP = PK.MALZEME_GRUP,
                               MALZEME_CINSI = PK.MALZEME_CINSI,
                               MALZEME_ADI = PK.MALZEME_ADI,
                               POZ_ACIKLAMASI = PK.POZ_ACIKLAMASI,
                               BIRIM = PK.BIRIM,
                               BIRIM_M2 = (double)PK.BIRIM_M2,
                               POZ_TOP_SIP_M2 = (double)PK.POZ_TOP_SIP_M2,
                               SIPARIS_M2 = PK.BIRIM_M2,
                               DOKUM_M2 = (double)PK.DOKUM_M2,
                               POZ_TOP_DOKUM_M2 = (double)PK.POZ_TOP_DOKUM_M2,
                               MT = (double)PK.MT,
                               POZ_TOPLAM_MT = (double)PK.POZ_TOPLAM_MT,
                               ONGORULEN_URUN_AGIRLIK = (double)PK.ONGORULEN_URUN_AGIRLIK,
                               KESIT_ACILIMI = PK.KESIT_ACILIMI,
                               KESIT_ACILIM_ARTI = PK.KESIT_ACILIM_ARTI,
                               KESIT_ACILIM_EKSI = PK.KESIT_ACILIM_EKSI,
                               ONCELIK_SIRASI = PK.ONCELIK_SIRASI,
                               FRAME_DURUMU = PK.FRAME_DURUMU,
                               FRAME_KG = PK.FRAME_KG,
                               YUZ_ISL_VAR = PK.YUZ_ISL_VAR,
                               GALVENIZE_GITMELI = PK.GALVENIZE_GITMELI,
                               MONTAJSIZ = PK.MONTAJSIZ,
                               POZ_IPTAL = PK.POZ_IPTAL,
                               HOLD = PK.HOLD,
                               POZ_SIPARIS_MIKTAR = PK.POZ_SIPARIS_MIKTAR,
                               POZ_REV_EKSI = PK.POZ_REV_EKSI,
                               POZ_SON_SIPARIS_MIKTAR = PK.POZ_SON_SIPARIS_MIKTAR,
                               POZ_URETIM_MIKTAR = PK.POZ_URETIM_MIKTAR,
                               POZ_SEVK_MIKTAR = PK.POZ_SEVK_MIKTAR,
                               POZ_MONTAJ_MIKTAR = PK.POZ_MONTAJ_MIKTAR,
                               SON_REV_NO = PK.SON_REV_NO,
                               REPORT_CODE = PK.REPORT_CODE,
                               ILK_EN = PK.ILK_EN,
                               ILK_BOY = PK.ILK_BOY,
                               ILK_BIRIM_M2 = PK.ILK_BIRIM_M2,
                               ILK_DOKUM_RENGI = PK.ILK_DOKUM_RENGI,
                               ILK_MALZEME_YUZEYI = PK.ILK_MALZEME_YUZEYI,
                               ILK_MALZEME_GRUP = PK.ILK_MALZEME_GRUP,
                               ILK_MALZEME_CINSI = PK.ILK_MALZEME_CINSI,
                               ILK_POZ_NO = PK.ILK_POZ_NO,
                               ILK_MALZEME_ADI = PK.ILK_MALZEME_ADI,
                               ILK_POZ_TURU = PK.ILK_POZ_TURU,
                               ILK_HACIM = PK.ILK_HACIM,
                               BIRIM_HACIM = PK.BIRIM_HACIM,
                               POZ_TOPLAM_HACIM = PK.POZ_TOPLAM_HACIM,
                               YUZEY_ISLEM_1 = PK.YUZEY_ISLEM_1,
                               YUZEY_ISLEM_1_ACIKLAMA = PK.YUZEY_ISLEM_1_ACIKLAMA,
                               YUZEY_ISLEM_2 = PK.YUZEY_ISLEM_2,
                               YUZEY_ISLEM_2_ACIKLAMA = PK.YUZEY_ISLEM_2_ACIKLAMA,
                               YUZEY_ISLEM_3 = PK.YUZEY_ISLEM_3,
                               YUZEY_ISLEM_3_ACIKLAMA = PK.YUZEY_ISLEM_3_ACIKLAMA,
                               YUZEY_ISLEM_4_ACIKLAMA = PK.YUZEY_ISLEM_4_ACIKLAMA,
                               YUZEY_ISLEM_4 = PK.YUZEY_ISLEM_4,
                               YUZEY_ISLEM_5 = PK.YUZEY_ISLEM_5,
                               YUZEY_ISLEM_5_ACIKLAMA = PK.YUZEY_ISLEM_5_ACIKLAMA,
                               MODEL_UYGUNLUK = PK.MODEL_UYGUNLUK,
                               MODEL_UYG_ONAY_VEREN = PK.MODEL_UYG_ONAY_VEREN,
                               MODEL_UYG_ONAY_TARIH = (DateTime)PK.MODEL_UYG_ONAY_TARIH,
                               POZ_BARKOD = PK.POZ_BARKOD,
                               POZ_REV_ARTI = PK.POZ_REV_ARTI,
                               FRAME_SIPARIS_MIKTAR = PK.FRAME_SIPARIS_MIKTAR,
                               FRAME_KABUL_BEKLE_MIKTAR = PK.FRAME_KABUL_BEKLE_MIKTAR,
                               FRAME_GELEN_MIKTAR = PK.FRAME_GELEN_MIKTAR,
                               FRAME_GALVENIZLI_MIKTAR = PK.FRAME_GALVENIZLI_MIKTAR,
                               FRAME_GALVENIZ_BEKLE_MIKTAR = PK.FRAME_GALVENIZ_BEKLE_MIKTAR,
                               FRAME_KULLAN_MIKTAR = PK.FRAME_KULLAN_MIKTAR,
                               FRAME_MUSAIT_MIKTAR = PK.FRAME_MUSAIT_MIKTAR,
                               FRAME_EKSIK_TEMIN = PK.FRAME_EKSIK_TEMIN,
                               FRAME_TURU = PK.FRAME_TURU,
                               KALIP_TASARIM_SAYISI = PK.KALIP_TASARIM_SAYISI,
                               IS_EMRI_SAYISI = PK.IS_EMRI_SAYISI,
                               POZ_PLAN_MIKTAR = PK.POZ_PLAN_MIKTAR,
                               FRAME_DOSYA_YUKLENDI = PK.FRAME_DOSYA_YUKLENDI,
                               FRAME_DOSYA_YUKLENDI_YAPAN = PK.FRAME_DOSYA_YUKLENDI_YAPAN,
                               FRAME_DOSYA_YUKLENDI_TARIH = (DateTime)PK.FRAME_DOSYA_YUKLENDI_TARIH,
                               KALIP_IS_ID = (int)DIE.KALIP_IS_ID,
                               AKTIF = DIE.AKTIF,
                               DOKUM_IS_EMRI_NO = DIE.DOKUM_IS_EMRI_NO,
                               PLAN_BASLANGIC_TARIH = (DateTime)DIE.PLAN_BASLANGIC_TARIH,
                               PLAN_BITIS_TARIH = (DateTime)DIE.PLAN_BITIS_TARIH,
                               TOPLAM_DOKULECEK_ADET = DIE.TOPLAM_DOKULECEK_ADET,
                               TOPLAM_DOKULEN_ADET = DIE.TOPLAM_DOKULEN_ADET,
                               TOPLAM_KALAN_DOKUM_ADET = DIE.TOPLAM_KALAN_DOKUM_ADET,
                               TOPLAM_PLAN_DOK_ADET = DIE.TOPLAM_PLAN_DOK_ADET,
                               TOPLAM_KALAN_PLAN_ADET = DIE.TOPLAM_KALAN_PLAN_ADET,
                               DOKUM_IS_EMRI_TARIH = (DateTime)DIE.DOKUM_IS_EMRI_TARIH,
                               DOKUM_IS_EMRINI_VEREN = DIE.DOKUM_IS_EMRINI_VEREN,
                               KALIP_TASARIM_ID = (int)IE.KALIP_TASARIM_ID,
                               IS_EMRI_BARKOD = IE.IS_EMRI_BARKOD,
                               IS_EMRI_TARIH = (DateTime)IE.IS_EMRI_TARIH,
                               IS_EMRI_DURUMU = IE.IS_EMRI_DURUMU,
                               KALIP_DURUMU = IE.KALIP_DURUMU,
                               KALIP_HAZIR_OL_TARIH = (DateTime)IE.KALIP_HAZIR_OL_TARIH,
                               ONG_DOKUM_BASLAMA_TARIH = (DateTime)IE.ONG_DOKUM_BASLAMA_TARIH,
                               DOKUM_HOLU = IE.DOKUM_HOLU,
                               DOKUM_ORMU_ADET = IE.DOKUM_ORMU_ADET,
                               TOPLAM_DOKUM_ADET = IE.TOPLAM_DOKUM_ADET,
                               KALAN_DOKUM_ADET = IE.KALAN_DOKUM_ADET,
                               DEPO_ID = IE.DEPO_ID,
                               DEPO_ADI = d.DEPO_ADI,
                               DEPO_KONUM = IE.DEPO_KONUM,
                               IS_EMRINI_VEREN = IE.IS_EMRINI_VEREN,
                               OLUSTURULMA_TARIH = (DateTime)IE.OLUSTURULMA_TARIH,
                               PLAN_OLUSTUR_TARIH =(DateTime) d.OLUSTUR_TARIH,
                               KALIP_3D_TARAMA_YAP = IE.KALIP_3D_TARAMA_YAP,
                               KALIP_3D_TARAMA_ONAY_BILGI = IE.KALIP_3D_TARAMA_ONAY_BILGI,
                               KALIPHANELER = IE.KALIPHANELER,
                               KALIPHANE_IMALAT_DURUM = IE.KALIPHANE_IMALAT_DURUM,
                               TADILAT_SAY = (int)IE.TADILAT_SAY,
                               KALIPHANE_ISLEM_TURU = IE.KALIPHANE_ISLEM_TURU,
                               KALIPHANE_ISLEM_ADLARI = IE.KALIPHANE_ISLEM_ADLARI,
                               KALIPHANE_ROTA = IE.KALIPHANE_ROTA,
                               TOPLAM_DOKULECEK_AD = IE.TOPLAM_DOKULECEK_AD,
                               PLAN_DOKUM_AD = IE.PLAN_DOKUM_AD,
                               KALAN_DOKUM_PLAN_AD = IE.KALAN_DOKUM_PLAN_AD,
                               TAR_MODEL = IE.TAR_MODEL,
                               TAR_BETON = IE.TAR_BETON,
                               TAR_KALIP = IE.TAR_KALIP,
                               KALIP_TIPI = IE.KALIP_TIPI,
                               PROJE_DURUMU = PR.PROJE_DURUMU,
                               PROJE_KODU = PR.PROJE_KODU,
                               PROJE_ADI = PR.PROJE_ADI,
                               PROJE_MUDURU = PR.PROJE_MUDURU,
                               PROJE_YONETICISI = PR.PROJE_YONETICISI,
                               TEKLIF_NO = PR.TEKLIF_NO,
                               SOZLESME_TARIH = (DateTime)PR.SOZLESME_TARIH,
                               BASLAMA_TARIH = (DateTime)PR.BASLAMA_TARIH,
                               BITIS_TARIH = (DateTime)PR.BITIS_TARIH,
                               PROJE_TURU = PR.PROJE_TURU,
                               PROJE_YERI = PR.PROJE_YERI,
                               PROJE_ADRES = PR.PROJE_ADRES,
                               PROJE_ACIKLAMASI = PR.PROJE_ACIKLAMASI,
                               IL = PR.IL,
                               ILCE = PR.ILCE,
                               PROJE_GUN = PR.PROJE_GUN,
                               PROJE_GERCEK_GUN = PR.PROJE_GERCEK_GUN,
                               PROJE_GERCEK_GUN_LIST = PR.PROJE_GERCEK_GUN_LIST,
                               PROJE_SATIS_M2 = PR.PROJE_SATIS_M2,
                               PROJE_SATIS_MIKTAR = PR.PROJE_SATIS_MIKTAR,
                               SANTIYE_SEFI = PR.SANTIYE_SEFI,
                               SOZLESME_M2 = PR.SOZLESME_M2,
                               SOZLESME_M2_KONTROL_YUZDESI = PR.SOZLESME_M2_KONTROL_YUZDESI,
                               WEB_PANEL_YANLIS_SIFRE = PR.WEB_PANEL_YANLIS_SIFRE,
                               WEB_PANEL_KULLANICI_ADI = PR.WEB_PANEL_KULLANICI_ADI,
                               WEB_PANEL_KULLANICI_SIFRE = PR.WEB_PANEL_KULLANICI_SIFRE,
                               ULKE = PR.ULKE,
                               IS_DEVIR_ACIKLAMALARI = PR.IS_DEVIR_ACIKLAMALARI,
                               SOZLESME_NO = PR.SOZLESME_NO,
                               BIZDEN_ILGILI = PR.BIZDEN_ILGILI,
                               SANDIK_PALET = PR.SANDIK_PALET,
                               V_URUN_M2_SEVK_TARIH = (DateTime)PR.V_URUN_M2_SEVK_TARIH,
                               V_URUN_M2_TARIH_ONCE = PR.V_URUN_M2_TARIH_ONCE,
                               V_URUN_M2_TARIH_SONRA = PR.V_URUN_M2_TARIH_SONRA,
                               MONTAJ_DETAY_PLAN_ACIKLAMA1 = PR.MONTAJ_DETAY_PLAN_ACIKLAMA1,
                               MONTAJ_DETAY_PLAN_ACIKLAMA2 = PR.MONTAJ_DETAY_PLAN_ACIKLAMA2,
                               SANTIYE_SORUMLUSU = PR.SANTIYE_SORUMLUSU,
                               MONTAJ_PLAN_BASLANGIC = PR.MONTAJ_PLAN_BASLANGIC,
                               MONTAJ_BASLANMA_TARIH = (DateTime)PR.MONTAJ_BASLANMA_TARIH,
                               MONTAJ_BITIS_TARIH = (DateTime)PR.MONTAJ_BITIS_TARIH,
                               MONTAJ_GUN = PR.MONTAJ_GUN,
                               MASTIK = PR.MASTIK,
                               BOYA = PR.BOYA,
                               KUR_SURE_GUN = PR.KUR_SURE_GUN,
                               URUN_3D_TARAMA_ZORUNLU = PR.URUN_3D_TARAMA_ZORUNLU,
                               KALIP_3D_TARAMA_ZORUNLU = PR.KALIP_3D_TARAMA_ZORUNLU,
                               MODEL_UYGUNLUK_KONTROL_ZORUNLU = PR.MODEL_UYGUNLUK_KONTROL_ZORUNLU,
                               KALITE_SON_ONAY= d.KALITE_SON_ONAY,
                               OTO_REV_NO = PR.OTO_REV_NO,
                               SIPARIS_BASLANMA_TARIH = (DateTime)PR.SIPARIS_BASLANMA_TARIH,
                               SIPARIS_BITIS_TARIH = (DateTime)PR.SIPARIS_BITIS_TARIH,
                               SIPARIS_GUN = PR.SIPARIS_GUN,
                               DOKUM_BASLANMA_TARIH = (DateTime)PR.DOKUM_BASLANMA_TARIH,
                               DOKUM_BITIS_TARIH = (DateTime)PR.DOKUM_BITIS_TARIH,
                               DOKUM_GUN = PR.DOKUM_GUN,
                               POZ_ID = (int)PS.POZ_ID,
                               SIPARIS_TURU = PS.SIPARIS_TURU,
                               SIPARIS_MIKTAR = PS.SIPARIS_MIKTAR,
                               REV_EKSI = PS.REV_EKSI,
                               SON_SIPARIS_MIKTAR = PS.SON_SIPARIS_MIKTAR,
                               URETIM_MIKTAR = PS.URETIM_MIKTAR,
                               SEVK_MIKTAR = PS.SEVK_MIKTAR,
                               MONTAJ_MIKTAR = PS.MONTAJ_MIKTAR,
                               TOP_SIP_M2 = PS.TOP_SIP_M2,
                               TOPLAM_HACIM = PS.TOPLAM_HACIM,
                               TOP_DOKUM_M2 = PS.TOP_DOKUM_M2,
                               TOPLAM_MT = PS.TOPLAM_MT,
                               REV_ARTI = PS.REV_ARTI,
                               PLANLANAN_MIKTAR = PS.PLANLANAN_MIKTAR,
                               KALAN_PLAN_MIKTAR = PS.KALAN_PLAN_MIKTAR,
                               SIPARIS_NO = SF.SIPARIS_NO,
                               SIPARIS_TARIH = (DateTime)SF.SIPARIS_TARIH,
                               SANDIKLAMA_TARIH = (DateTime)SSF.SANDIKLAMA_TARIH,
                               SANDIKLAMA_ACIKLAMASI = SSF.SANDIKLAMA_ACIKLAMASI,
                               PERSONEL_ADI = SSF.PERSONEL_ADI,
                               SANDIK_TOPLAM_M2 = SSF.SANDIK_TOPLAM_M2,
                               SANDIK_TOPLAM_ADET = SSF.SANDIK_TOPLAM_ADET,
                               SANDIK_TOPLAM_KG = SSF.SANDIK_TOPLAM_KG,
                               RAF_KONUM = d.RAF_KONUM,
                               OLUSTURAN = d.OLUSTURAN,
                               OLUSTURMA_TARIH = (DateTime)SSF.OLUSTURMA_TARIH,
                               FIS_DURUMU = SSF.FIS_DURUMU,
                               YUKSEKLIK = SSF.YUKSEKLIK,
                               M3 = SSF.M3,
                               BOS_SANDIK_KG = SSF.BOS_SANDIK_KG,
                               PLAN_SEVK_TARIH = (DateTime)SKF.PLAN_SEVK_TARIH,
                               SEVK_EDILDI = d.SEVK_EDILDI,
                               TALEP_FIS_ID = (int)SKF.TALEP_FIS_ID,
                               IRSALIYE_NO = SKF.IRSALIYE_NO,
                               ARAC_PLAKA = SKF.ARAC_PLAKA,
                               SOFOR_ADI = SKF.SOFOR_ADI,
                               SOFOR_TEL = SKF.SOFOR_TEL,
                               SEVK_POZ_MIKTAR = SKF.SEVK_POZ_MIKTAR,
                               SEVK_TON = SKF.SEVK_TON,
                               ARAC_TIPI = SKF.ARAC_TIPI,
                               ARAC_EN = SKF.ARAC_EN,
                               ARAC_BOY = SKF.ARAC_BOY,
                               ARAC_MAK_TON = SKF.ARAC_MAK_TON,
                               YUKLEYEN = SKF.YUKLEYEN,
                               YUKLEME_ZAMANI = (DateTime)SKF.YUKLEME_ZAMANI,
                               SEVKEDEN_FIRMA = SKF.SEVKEDEN_FIRMA,
                               SEVKEDEN_FIRMA_ID = (int)SKF.SEVKEDEN_FIRMA_ID,
                               SEVK_TUTARI = SKF.SEVK_TUTARI,
                               SEVK_FIS_ACIKLAMA = SKF.SEVK_FIS_ACIKLAMA,
                               ARAC_GIRIS_ZAMAN = (DateTime)SKF.ARAC_GIRIS_ZAMAN,
                               ARAC_CIKIS_ZAMAN = (DateTime)SKF.ARAC_CIKIS_ZAMAN,
                               ILK_TARTIM = SKF.ILK_TARTIM,
                               SON_TARTIM = SKF.SON_TARTIM,
                               YUKLEME_BASLAMA_ZAMANI = (DateTime)SKF.YUKLEME_BASLAMA_ZAMANI,
                               YUKLEME_SEFI = SKF.YUKLEME_SEFI,
                               SANTIYE_KODU = SKF.SANTIYE_KODU,
                               SANTIYE_ID = (int)SKF.SANTIYE_ID,
                               SANTIYE_SEVK_KABUL = SKF.SANTIYE_SEVK_KABUL,
                               SANTIYE_SEVK_KABUL_EDEN = SKF.SANTIYE_SEVK_KABUL_EDEN,
                               SANTIYE_SEVK_KABUL_TARIH = (DateTime)SKF.SANTIYE_SEVK_KABUL_TARIH,
                               SEVK_KABUL_ACIKLAMA_FIS = SKF.SEVK_KABUL_ACIKLAMA_FIS,
                               SEVK_PLAN_POZ_MIKTAR = SKF.SEVK_PLAN_POZ_MIKTAR,
                               SEVK_KALAN_POZ_MIKTAR = SKF.SEVK_KALAN_POZ_MIKTAR,
                               DORSE_PLAKA_NO = SKF.DORSE_PLAKA_NO,
                               KONTEYNER_NO = SKF.KONTEYNER_NO,
                               ARAC_YUKLEME_SURE = SKF.ARAC_YUKLEME_SURE,
                               ARAC_FABRIKADA_KALMA_SURE = SKF.ARAC_FABRIKADA_KALMA_SURE,
                               INDIR_POZ_MIKTAR = SKF.INDIR_POZ_MIKTAR,
                               KALAN_INDIR_POZ_MIKTAR = SKF.KALAN_INDIR_POZ_MIKTAR,
                               SEVK_M2 = SKF.SEVK_M2,
                               MONTAJ_POZ_MIKTAR = SKF.MONTAJ_POZ_MIKTAR,
                               KALAN_MONTAJ_POZ_MIKTAR = SKF.KALAN_MONTAJ_POZ_MIKTAR,

                           }).ToList();

            ViewBag.deneme = results.Select(a => a.SEVK_MIKTAR).Sum();
            ViewBag.deneme2 = results.Select(a => a.MONTAJ_MIKTAR).Sum();
            var viewModel = new PeraV4ReportsViewModel
            {
                Projeler = projeler,
                UrunIzleme = results
            };

            return View(viewModel);
        }

        [HttpGet] 
        public ActionResult SiparisIzleme()
        {
            var projeler = db2.PROJE_KART.OrderBy(a => a.PROJE_ADI).ToList();

            var viewModel = new PeraV4ReportsViewModel
            {
                Projeler = projeler,
                SiparisIzleme = new List<SiparisIzlemeModelItem>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SiparisIzleme(DateTime bas, DateTime bit, string proje, string siparisTur, string poz, bool dokumEksigi = false, bool montajKalan = false)
        {
            var projeler = db2.PROJE_KART.OrderBy(a => a.PROJE_ADI).ToList();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                StringBuilder SQL = new StringBuilder();
                SQL.Append("select SP.ID,P.POZ_TURU,SF.SIPARIS_TARIH,P.ONGORULEN_URUN_AGIRLIK,P.YUZEY_ISLEM_1, P.YUZEY_ISLEM_1_ACIKLAMA, P.YUZEY_ISLEM_2, P.YUZEY_ISLEM_2_ACIKLAMA, P.YUZEY_ISLEM_3, P.YUZEY_ISLEM_3_ACIKLAMA, P.YUZEY_ISLEM_4_ACIKLAMA, P.YUZEY_ISLEM_4, P.YUZEY_ISLEM_5, P.YUZEY_ISLEM_5_ACIKLAMA,(select TOP 1 REVIZYON_TARIH  from POZ_REVIZYON R  where R.POZ_ID = SP.POZ_ID order by REVIZYON_TARIH desc) as  REVIZE_TARIH,PK.PROJE_ADI,(select TOP 1 IE.DOKUM_SEKLI  from KALIP_TASARIM_FORM IE inner join KALIP_TASARIM_POZ KP on KP.KALIP_TASARIM_ID = IE.ID and KP.POZ_ID = SP.POZ_ID order by IE.ID desc) as  DOKUM_SEKLI,(select TOP 1 D.HAT from  DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID order by PLAN_DOKUM_TARIH desc) as HAT,P.REPORT_CODE,SP.SIPARIS_TURU,P.POZ_NO,P.MALZEME_ADI,P.MONTAJSIZ,P.BOY,P.EN,IS_EMRI.IS_EMRI_NO,IS_EMRI.IS_EMRINI_VEREN,IS_EMRI.IS_EMRITARIH,KALIP_TASARIM_NO,P.RENK,P.MALZEME_YUZEYI,P.BIRIM_M2,P.DOKUM_M2,P.BIRIM_HACIM,P.FRAME_DURUMU,SP.BLOK_ADI, SP.CEPHE_ADI,(select TOP 1 ISLEM_ADI from  KALIP_OLUSTUR_FORM U  inner join KALIP_TASARIM_FORM KT on KT.ID = U.KALIP_TASARIM_ID inner join KALIP_TASARIM_POZ KP on KP.KALIP_TASARIM_ID = KT.ID where KP.POZ_ID = SP.POZ_ID and U.ISLEM_TURU = 'Kalıp' order by M2  desc) as KALIP_SEKLI,(select TOP 1 UF.UYGUNSUZLUK_NO from UYGUNSUZLUK_FORM UF inner join DOKUM_IS_EMRI_DOKUM D on D.ID = UF.DOKUM_ID where UF.UYGUNSUZLUK_TURU in ('Montaj','Sevkiyat','Üretim') and D.SIPARIS_POZ_ID = SP.ID and UF.POZ_ID = SP.POZ_ID order by UF.TESPIT_TARIH desc) as UYGUNSUZLUK_NO,isnull((select TOP 1 D.BIRIM_KG  from DOKUM_IS_EMRI_DOKUM D  where D.POZ_ID = SP.POZ_ID order by PLAN_DOKUM_TARIH desc),0) as  DOK_BIRIM_KG,isnull((select TOP 1 T.FRAME_BIRIM_KG from DOKUM_IS_EMRI_DOKUM D inner join FRAME_TEDARIK T on T.FRAME_BARKOD = D.FRAME_BARKOD and D.POZ_ID = SP.POZ_ID order by PLAN_DOKUM_TARIH desc),0) as FRAME_BIRIM_KG,isnull((select sum(D.BIR_DOKUM_MIKTAR)  from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.DOKULDU = 1 and D.SEVK_EDILDI = 0 and D.SIPARIS_POZ_ID = SP.ID),0) as STOK,SP.SON_SIPARIS_MIKTAR as TOPLAM,isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.DOKULDU = 1 and D.SIPARIS_POZ_ID = SP.ID ),0) as DOKULEN,SP.SON_SIPARIS_MIKTAR - isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.DOKULDU = 1 and D.SIPARIS_POZ_ID = SP.ID),0) as KALAN,P.BIRIM_M2 * SP.SON_SIPARIS_MIKTAR as TOP_SAT_M2,P.BIRIM_M2 * (SON_SIPARIS_MIKTAR - isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.DOKULDU = 1 and D.SIPARIS_POZ_ID = SP.ID),0)) as KALAN_SAT_M2,P.BIRIM_HACIM * SP.SON_SIPARIS_MIKTAR as TOP_SIP_HACIM,P.BIRIM_HACIM * (SON_SIPARIS_MIKTAR - isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.DOKULDU = 1 and D.SIPARIS_POZ_ID = SP.ID),0)) as KALAN_SIP_HACIM,P.BIRIM_M2 * (isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.DOKULDU = 1 and D.SIPARIS_POZ_ID = SP.ID),0)) as DOKULEN_SAT_M2,P.BIRIM_M2 * isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.DOKULDU = 1 and D.SIPARIS_POZ_ID = SP.ID),0) as TOP_DOK_M2,isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where SP.POZ_ID = D.POZ_ID and D.SIPARIS_POZ_ID = SP.ID and DEPO_ID in (select ID from DEPO D where (D.DEPO_TURU='Hurda' or D.DEPO_TURU='Karantina'))),0) as HURDA_KARANTINA_ADET,isnull((select P.BIRIM_M2*sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where SP.POZ_ID = D.POZ_ID and D.SIPARIS_POZ_ID = SP.ID and DEPO_ID in (select ID from DEPO D where (D.DEPO_TURU='Hurda' or D.DEPO_TURU='Karantina'))),0) as HURDA_KARANTINA_M2,(isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.MONTAJ_YAPILDI = 1 and D.SIPARIS_POZ_ID = SP.ID),0)) as MONTAJLANAN_ADET,P.BIRIM_M2 * (isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.MONTAJ_YAPILDI = 1 and D.SIPARIS_POZ_ID = SP.ID),0)) as MONTAJLANAN_M2,(isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.SEVK_EDILDI = 1 and D.SIPARIS_POZ_ID = SP.ID ),0)) as NAKLEDILEN_ADET,P.BIRIM_M2 * (isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.SEVK_EDILDI = 1  and D.SIPARIS_POZ_ID = SP.ID),0)) as NAKLEDILEN_M2,(isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.INDIRILDI = 1 and D.SIPARIS_POZ_ID = SP.ID),0)) as INDIRILEN_ADET,P.BIRIM_M2 * (isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.INDIRILDI = 1 and D.SIPARIS_POZ_ID = SP.ID),0)) as INDIRILEN_M2,SP.SON_SIPARIS_MIKTAR - (isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.MONTAJ_YAPILDI = 1 and D.SIPARIS_POZ_ID = SP.ID),0))-isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where SP.POZ_ID = D.POZ_ID and D.SIPARIS_POZ_ID = SP.ID and DEPO_ID in (select ID from DEPO D where (D.DEPO_TURU='Hurda' or D.DEPO_TURU='Karantina'))),0) as KALAN_MONTAJ_ADET,P.BIRIM_M2 * (SP.SON_SIPARIS_MIKTAR - isnull((select sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where D.POZ_ID = SP.POZ_ID and D.MONTAJ_YAPILDI = 1 and D.SIPARIS_POZ_ID = SP.ID),0)-isnull((select P.BIRIM_M2*sum(D.BIR_DOKUM_MIKTAR) from DOKUM_IS_EMRI_DOKUM D where SP.POZ_ID = D.POZ_ID and D.SIPARIS_POZ_ID = SP.ID and DEPO_ID in (select ID from DEPO D where (D.DEPO_TURU='Hurda' or D.DEPO_TURU='Karantina'))),0)) as KALAN_MONTAJ_M2 from POZ_SIPARIS SP inner join POZ_SIPARIS_FORM SF on SF.ID = SP.SIPARIS_ID inner join POZ_KART P on P.ID = SP.POZ_ID inner join PROJE_KART PK on PK.ID = SP.PROJE_ID outer apply (select TOP 1 IE.IS_EMRI_TARIH as IS_EMRITARIH,IE.IS_EMRINI_VEREN as IS_EMRINI_VEREN,IE.IS_EMRI_BARKOD as IS_EMRI_NO,KT.TASARIM_NO as KALIP_TASARIM_NO from KALIP_IS_EMRI IE inner join DOKUM_IS_EMRI_POZ KP on KP.KALIP_IS_ID = IE.ID and KP.POZ_ID = SP.POZ_ID  inner join KALIP_TASARIM_FORM KT on IE.KALIP_TASARIM_ID = KT.ID order by IS_EMRI_TARIH desc)  as IS_EMRI where SP.ID > 0 and SF.SIPARIS_TARIH between @p1 and @p2");

                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@p1", bas),
                    new SqlParameter("@p2", bit)
                };

                if (!string.IsNullOrEmpty(proje))
                {
                    SQL.Append(" and PROJE_ADI = @p3");
                    parameters.Add(new SqlParameter("@p3", proje));
                }
                if (!string.IsNullOrEmpty(siparisTur))
                {
                    SQL.Append(" and SIPARIS_TURU = @p4");
                    parameters.Add(new SqlParameter("@p4", siparisTur));
                }
                if(!string.IsNullOrEmpty(poz)) 
                {
                    SQL.Append(" and POZ_NO = @p5");
                    parameters.Add(new SqlParameter("@p5", poz));
                }

                SQL.Append(" order by  SF.SIPARIS_TARIH");

                using (SqlCommand command = new SqlCommand(SQL.ToString(),connection))
                {

                    command.Parameters.AddWithValue("@p1", bas);
                    command.Parameters.AddWithValue("@p2", bit);
                    command.Parameters.AddWithValue("@p3", proje);
                    command.Parameters.AddWithValue("@p4", siparisTur);
                    command.Parameters.AddWithValue("@p5", poz);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<SiparisIzlemeModelItem> sonuclar = new List<SiparisIzlemeModelItem>();
                        while (reader.Read())
                        {
                            SiparisIzlemeModelItem siparis = new SiparisIzlemeModelItem
                            {
                                SIPARIS_TARIH = (DateTime)(reader.IsDBNull(reader.GetOrdinal("SIPARIS_TARIH")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SIPARIS_TARIH"))),
                                SIPARIS_TURU = reader.IsDBNull(reader.GetOrdinal("SIPARIS_TURU")) ? null : reader.GetString(reader.GetOrdinal("SIPARIS_TURU")),
                                PROJE_ADI = reader.IsDBNull(reader.GetOrdinal("PROJE_ADI")) ? null : reader.GetString(reader.GetOrdinal("PROJE_ADI")),
                                ONGORULEN_URUN_AGIRLIK = reader.IsDBNull(reader.GetOrdinal("ONGORULEN_URUN_AGIRLIK")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("ONGORULEN_URUN_AGIRLIK")),
                                REVIZE_TARIH = reader.IsDBNull(reader.GetOrdinal("REVIZE_TARIH")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REVIZE_TARIH")),
                                DOKUM_SEKLI = reader.IsDBNull(reader.GetOrdinal("DOKUM_SEKLI")) ? null : reader.GetString(reader.GetOrdinal("DOKUM_SEKLI")),
                                HAT = reader.IsDBNull(reader.GetOrdinal("HAT")) ? null : reader.GetString(reader.GetOrdinal("HAT")),
                                POZ_NO = reader.IsDBNull(reader.GetOrdinal("POZ_NO")) ? null : reader.GetString(reader.GetOrdinal("POZ_NO")),
                                MALZEME_ADI = reader.IsDBNull(reader.GetOrdinal("MALZEME_ADI")) ? null : reader.GetString(reader.GetOrdinal("MALZEME_ADI")),
                                EN = reader.IsDBNull(reader.GetOrdinal("EN")) ? null : reader.GetDouble(reader.GetOrdinal("EN")),
                                BOY = reader.IsDBNull(reader.GetOrdinal("BOY")) ? null : reader.GetDouble(reader.GetOrdinal("BOY")),
                                KALIP_SEKLI = reader.IsDBNull(reader.GetOrdinal("KALIP_SEKLI")) ? null : reader.GetString(reader.GetOrdinal("KALIP_SEKLI")),
                                IS_EMRI_NO = reader.IsDBNull(reader.GetOrdinal("IS_EMRI_NO")) ? null : reader.GetString(reader.GetOrdinal("IS_EMRI_NO")),
                                IS_EMRINI_VEREN = reader.IsDBNull(reader.GetOrdinal("IS_EMRINI_VEREN")) ? null : reader.GetString(reader.GetOrdinal("IS_EMRINI_VEREN")),
                                RENK = reader.IsDBNull(reader.GetOrdinal("RENK")) ? null : reader.GetString(reader.GetOrdinal("RENK")),
                                MALZEME_YUZEYI = reader.IsDBNull(reader.GetOrdinal("MALZEME_YUZEYI")) ? null : reader.GetString(reader.GetOrdinal("MALZEME_YUZEYI")),
                                MONTAJSIZ = reader.IsDBNull(reader.GetOrdinal("MONTAJSIZ")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("MONTAJSIZ")),
                                IS_EMRI_TARIH = reader.IsDBNull(reader.GetOrdinal("IS_EMRITARIH")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("IS_EMRITARIH")),
                                SATIS_BIRIM_M2 = reader.IsDBNull(reader.GetOrdinal("BIRIM_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("BIRIM_M2")),
                                DOKUM_BIRIM_M2 = reader.IsDBNull(reader.GetOrdinal("DOKUM_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("DOKUM_M2")),
                                MALZEME_BIRIM_KG = reader.IsDBNull(reader.GetOrdinal("DOK_BIRIM_KG")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("DOK_BIRIM_KG")),
                                BIRIM_HACIM_M3 = reader.IsDBNull(reader.GetOrdinal("BIRIM_HACIM")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("BIRIM_HACIM")),
                                FRAME_BIRIM_KG = reader.IsDBNull(reader.GetOrdinal("FRAME_BIRIM_KG")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("FRAME_BIRIM_KG")),
                                STOK = reader.IsDBNull(reader.GetOrdinal("STOK")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("STOK")),
                                TOPLAM = reader.IsDBNull(reader.GetOrdinal("TOPLAM")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("TOPLAM")),
                                DOKULEN = reader.IsDBNull(reader.GetOrdinal("DOKULEN")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("DOKULEN")),
                                KALAN = reader.IsDBNull(reader.GetOrdinal("KALAN")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("KALAN")),
                                KALAN_SATIS_M2 = reader.IsDBNull(reader.GetOrdinal("KALAN_SAT_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("KALAN_SAT_M2")),
                                DOKULEN_SATIS_M2 = reader.IsDBNull(reader.GetOrdinal("DOKULEN_SAT_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("DOKULEN_SAT_M2")),
                                TOPLAM_SIPARIS_HACIM_M2 = reader.IsDBNull(reader.GetOrdinal("TOP_SIP_HACIM")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("TOP_SIP_HACIM")),
                                KALAN_SIPARIS_HACIM_M2 = reader.IsDBNull(reader.GetOrdinal("KALAN_SIP_HACIM")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("KALAN_SIP_HACIM")),
                                NAKLEDILEN_M2 = reader.IsDBNull(reader.GetOrdinal("NAKLEDILEN_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("NAKLEDILEN_M2")),
                                NAKLEDILEN_ADET = reader.IsDBNull(reader.GetOrdinal("NAKLEDILEN_ADET")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("NAKLEDILEN_ADET")),
                                INDIRILEN_AD = reader.IsDBNull(reader.GetOrdinal("INDIRILEN_ADET")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("INDIRILEN_ADET")),
                                INDIRILEN_M2 = reader.IsDBNull(reader.GetOrdinal("INDIRILEN_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("INDIRILEN_M2")),
                                MONTAJLANAN_AD = reader.IsDBNull(reader.GetOrdinal("MONTAJLANAN_ADET")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("MONTAJLANAN_ADET")),
                                MONTAJLANAN_M2 = reader.IsDBNull(reader.GetOrdinal("MONTAJLANAN_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("MONTAJLANAN_M2")),
                                HURDA_KARANTINA_AD = reader.IsDBNull(reader.GetOrdinal("HURDA_KARANTINA_ADET")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("HURDA_KARANTINA_ADET")),
                                HURDA_KARANTINA_M2 = reader.IsDBNull(reader.GetOrdinal("HURDA_KARANTINA_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("HURDA_KARANTINA_M2")),
                                KALAN_MONTAJ_AD = reader.IsDBNull(reader.GetOrdinal("KALAN_MONTAJ_ADET")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("KALAN_MONTAJ_ADET")),
                                KALAN_MONTAJ_M2 = reader.IsDBNull(reader.GetOrdinal("KALAN_MONTAJ_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("KALAN_MONTAJ_M2")),
                                TOPLAM_SATIS_M2 = reader.IsDBNull(reader.GetOrdinal("TOP_SAT_M2")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("TOP_SAT_M2")),
                                YUZEY_ISLEM_1 = reader.IsDBNull(reader.GetOrdinal("YUZEY_ISLEM_1")) ? null : reader.GetString(reader.GetOrdinal("YUZEY_ISLEM_1")),
                                YUZEY_ISLEM_2 = reader.IsDBNull(reader.GetOrdinal("YUZEY_ISLEM_2")) ? null : reader.GetString(reader.GetOrdinal("YUZEY_ISLEM_2")),
                                YUZEY_ISLEM_3 = reader.IsDBNull(reader.GetOrdinal("YUZEY_ISLEM_3")) ? null : reader.GetString(reader.GetOrdinal("YUZEY_ISLEM_3")),
                                YUZEY_ISLEM_4 = reader.IsDBNull(reader.GetOrdinal("YUZEY_ISLEM_4")) ? null : reader.GetString(reader.GetOrdinal("YUZEY_ISLEM_4")),
                                YUZEY_ISLEM_5 = reader.IsDBNull(reader.GetOrdinal("YUZEY_ISLEM_5")) ? null : reader.GetString(reader.GetOrdinal("YUZEY_ISLEM_5")),
                                REPORT_CODE = reader.IsDBNull(reader.GetOrdinal("REPORT_CODE")) ? null : reader.GetString(reader.GetOrdinal("REPORT_CODE")),
                                BLOK_ADI = reader.IsDBNull(reader.GetOrdinal("BLOK_ADI")) ? null : reader.GetString(reader.GetOrdinal("BLOK_ADI")),
                                CEPHE_ADI = reader.IsDBNull(reader.GetOrdinal("CEPHE_ADI")) ? null : reader.GetString(reader.GetOrdinal("CEPHE_ADI")),
                                POZ_TURU = reader.IsDBNull(reader.GetOrdinal("POZ_TURU")) ? null : reader.GetString(reader.GetOrdinal("POZ_TURU")),
                                KALIP_TASARIM_NO = reader.IsDBNull(reader.GetOrdinal("KALIP_TASARIM_NO")) ? null : reader.GetString(reader.GetOrdinal("KALIP_TASARIM_NO")),
                                FRAME_DURUMU = reader.IsDBNull(reader.GetOrdinal("FRAME_DURUMU")) ? null : reader.GetString(reader.GetOrdinal("FRAME_DURUMU"))

                            };
                            sonuclar.Add(siparis);
                        }
                        
                        //Döküm Eksiği - Kalan Montaj Kontrol

                        if(dokumEksigi == true)
                        {
                            sonuclar = sonuclar.Where(s => s.KALAN >= 1).ToList();
                        }
                        if(montajKalan == true)
                        {
                            sonuclar = sonuclar.Where(s => s.KALAN_MONTAJ_AD > 0).ToList();
                        }

                        var viewModel = new PeraV4ReportsViewModel
                        {
                            Projeler = projeler,
                            SiparisIzleme = sonuclar
                        };

                        return View(viewModel);
                    }
                }
            }
           
        }

        [HttpGet]
        public ActionResult KalipIzlemeRaporu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KalipIzlemeRaporu(DateTime bas, DateTime bit)
        {
            return View();
        }
    }
}