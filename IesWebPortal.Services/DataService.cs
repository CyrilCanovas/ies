using IesWebPortal.Model;
using IesWebPortal.Services.Interfaces;
using SageClassLibrary;
using SageClassLibrary.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace IesWebPortal.Services
{
    public class DataService : IDataService
    {
        private readonly SageDataContext _sageDataContext;
        private readonly IMemoManager _memoManager;
        private readonly byte[] _emptyPicture;
        //private readonly Func<string, string> _mapPicturePath;
        public DataService(SageDataContext sageDataContext, IMemoManager memoManager, byte[] emptyPicture)
        {
            _sageDataContext = sageDataContext;
            _memoManager = memoManager;
            _emptyPicture = emptyPicture;
            //_mapPicturePath = mapPicturePath;
        }

        private static Regex flashpoint = new Regex(@"(?<number>\d+)", RegexOptions.Compiled);
        private static double GetFlashPoint(string value)
        {
            var result = double.NaN;
            if (string.IsNullOrEmpty(value)) return result;
            var match = flashpoint.Match(value);
            if (match.Success) result = double.Parse(match.Groups["number"].Value);
            return result;
        }
        public IMLItemInventory[] GetInventories(Func<string, string> mapPicturePath)

        {
            var itemsinventories = (from i in _sageDataContext.ItemInventories
                                    where i.QteSto > 0
                                    select new MLItemInventory() { DeNo = i.DeNo, ItemNo = i.RefArticle, Qty = i.QteSto, NoPrincipal = i.NoPrincipal }
            ).ToArray();

            var locations = (from i in _sageDataContext.Locations
                             select i).ToDictionary(x => x.DpNo);

            var warehouses = (from i in _sageDataContext.Warehouses
                              select i).ToDictionary(x => x.DeNo);

            Array.ForEach(itemsinventories, x => x.DeIntitule = warehouses[x.DeNo].Intitule);

            var dicoiteminfo = GetSubItemsInfos(itemsinventories.Select(x => x.ItemNo).Distinct().ToArray(), string.Empty,mapPicturePath).ToDictionary(x => x.ItemNo);

            Array.ForEach(itemsinventories,
                x =>
                {
                    x.Item = dicoiteminfo[x.ItemNo];
                    SageClassLibrary.DataModel.Location location;
                    if (locations.TryGetValue(x.NoPrincipal, out location)) x.LocationCode = location.Code;
                }
                    );
            return itemsinventories;

        }

        private static string[] _propertyNames = new string[] { nameof(MLItemInfo.Picture1), nameof(MLItemInfo.Picture2), nameof(MLItemInfo.Picture3), nameof(MLItemInfo.Picture4), nameof(MLItemInfo.Picture5) };
        private MLItemInfo[] GetSubItemsInfos(
            string[] itemnos,
            string codepays,
            Func<string, string> mapPicturePath
        )
        {

            var items =
                DbPager<MLItemInfo>.Get<string>(itemnos,
                x =>
                (from i in _sageDataContext.Items
                 where x.Contains(i.RefArticle)
                 select new MLItemInfo(_emptyPicture)
                 {
                     ItemNo = i.RefArticle,
                     Description = i.Design,
                     FlashPoint = GetFlashPoint(i.FlashPoint),
                     RID = i.RID,
                     ICADIATA = i.ICADIATA,
                     IMDG = i.IMDG,
                     Dangerous = i.DangerousSection,
                     Family = i.CodeFamille
                 }).ToArray()).ToArray();


            var itemsmemos =
                DbPager<SageClassLibrary.DataModel.ItemMemo>.Get<string>(itemnos,
                x => (from i in _sageDataContext.ItemMemos
                      where x.Contains(i.RefArticle)
                      select i).ToArray()
                ).ToArray();


            var glnos = itemsmemos.Select(x => x.GlNo).Distinct().ToArray();

            var memosintitules =
                DbPager<SageClassLibrary.DataModel.Memo>.Get<int>(glnos,
                x =>

                (from i in _sageDataContext.Memos
                 where x.Contains(i.GlNo)
                 select i).ToArray()).ToDictionary(x => x.Intitule);

            var itemsmedias =
                (from i in
                     (
                         DbPager<MediaInfo>.Get<string>(itemnos,
                         x =>
                             (from i in _sageDataContext.ItemsMedias
                              where x.Contains(i.RefArticle)
                              select new MediaInfo { ItemNo = i.RefArticle, FileName = Path.GetFileName(i.Fichier) }
                         ).ToArray().Where(y => _memoManager.IsPicture(y.FileName))
                    )
                    )
                 group i by i.ItemNo into a
                 select a).ToDictionary(z => z.Key, z => z.OrderBy(y => y.FileName).ToArray());

            var itemsdico = items.ToDictionary(x => x.ItemNo);
            var memosglnos = memosintitules.Values.ToDictionary(x => x.GlNo);



            var eniecs = _memoManager.GetEniecs(memosintitules.Keys).ToDictionary(x => x.Description);
            var risks = _memoManager.GetRisks(memosintitules.Keys).ToDictionary(x => x.Description);
            var un = _memoManager.GetUN(memosintitules.Keys).ToDictionary(x => x.Description);

            //var codepays = "SA";

            foreach (var i in eniecs.Values)
            {
                i.FullDescription = memosintitules[i.Description].Text;
            }
            foreach (var i in risks.Values)
            {
                string FullDescriptionOverride = null;
                if (!string.IsNullOrEmpty(codepays) && i.Language == ELanguages.French)
                {
                    FullDescriptionOverride = _sageDataContext.IesRisques.Where(x => x.CodePays == codepays && x.Intitule == i.ShortName).Select(x => x.Text).SingleOrDefault();
                    //FullDescriptionOverride = sagedatacontext.IesRisques.Where(x => x.CodePays == codepays && x.Intitule == "H361").Select(x => x.Text).SingleOrDefault();
                }
                i.FullDescription = string.IsNullOrEmpty(FullDescriptionOverride) ? memosintitules[i.Description].Text : FullDescriptionOverride;
            }
            foreach (var i in un.Values) i.FullDescription = memosintitules[i.Description].Text;

            var files = new Dictionary<string, byte[]>();


            var properties = (from i in typeof(MLItemInfo).GetProperties()
                              where _propertyNames.Contains(i.Name)
                              orderby i.Name
                              select i).ToArray();


            foreach (var item in items)
            {
                MediaInfo[] mediainfos;

                if (!itemsmedias.TryGetValue(item.ItemNo, out mediainfos)) continue;

                var sb = new StringBuilder();
                int idx = 0;
                foreach (var value in mediainfos)
                {
                    byte[] buff;


                    if (!files.TryGetValue(value.FileName, out buff))
                    {

                        var filename = mapPicturePath(value.FileName);

                        try
                        {
                            buff = File.ReadAllBytes(filename);
                        }
                        catch
                        {
                        }
                        files.Add(value.FileName, buff);
                    }
                    if (sb.Length > 0) sb.Append(",");
                    sb.Append(value.FileName);
                    if (idx >= properties.Length) break;

                    properties[idx].SetValue(item, buff, null);
                    idx++;

                }

                item.Files = sb.ToString();

            }

            foreach (var itemmemo in itemsmemos.GroupBy(x => x.RefArticle))
            {
                var item = itemsdico[itemmemo.Key];

                var subkeys = from i in itemmemo
                              where memosglnos.ContainsKey(i.GlNo)
                              select memosglnos[i.GlNo].Intitule;

                item.SetRisks(
                            (from i in
                                 (from i in subkeys
                                  where risks.ContainsKey(i)
                                  select risks[i])
                             group i by i.ShortName into a
                             select a).ToDictionary(x => x.Key, x => x.ToArray())
                );

                item.SetEiniecs(
                                (from i in subkeys
                                 where eniecs.ContainsKey(i)
                                 select eniecs[i]).ToDictionary(x => x.ShortName)
                );

                item.SetUn(
                    (from i in subkeys
                     where un.ContainsKey(i)
                     select un[i]).SingleOrDefault()
                );

            }
            return items;
        }

        public IMLSalePurchaseHeader[] GetDocHeaders(int docType)
        {
            var q = (from i in _sageDataContext.DocHeaders
                     join a in _sageDataContext.CustomersVendors on i.Tiers equals a.Tiers
                     join address in _sageDataContext.ShipToAddresses on i.LiNo equals address.LiNo into tmpadresses
                     from address in tmpadresses.DefaultIfEmpty()
                     where i.DocType == docType
                     select new MLSalePurchaseHeader()
                     {
                         DocumentType = i.DocType,
                         DocumentNo = i.Piece,
                         CustomerVendorNo = i.Tiers,
                         DeleveryDate = i.DateLivr <= SageClassLibrary.DataModel.SageObject.SageMinDate ? null : new DateTime?(i.DateLivr),
                         DocumentDate = i.DatePiece,
                         Reference = i.Reference,
                         Intitule = a.Intitule,
                         ToId = i.LiNo,
                         To = new MLShipTo()
                         {
                             LiNo = i.LiNo,
                             Description = address.Intitule,
                             Address = address.Adresse,
                             Address1 = address.Complement,
                             ZipCode = address.CodePostal,
                             City = address.Ville,
                             Country = address.Pays,
                             Mail = address.Email,
                             Phone = address.Telephone
                         }
                     }).ToArray();

            return q;
        }

        public IMLSalePurchaseHeader GetDocHeader(int doctype,string docpiece)
        {
            var q = (from i in _sageDataContext.DocHeaders
                     join a in _sageDataContext.CustomersVendors on i.Tiers equals a.Tiers
                     where i.DocType == doctype && i.Piece == docpiece
                     select new MLSalePurchaseHeader()
                     {
                         DocumentType = i.DocType,
                         DocumentNo = i.Piece,
                         CustomerVendorNo = i.Tiers,
                         DeleveryDate = i.DateLivr <= SageClassLibrary.DataModel.SageObject.SageMinDate ? null : new DateTime?(i.DateLivr),
                         DocumentDate = i.DatePiece,
                         Reference = i.Reference,
                         ToId = i.LiNo,
                         Intitule = a.Intitule
                     }).SingleOrDefault();
            if (q != null)
            {
                q.Lines = GetDocLines( q.DocumentType, q.DocumentNo);
                if (q.Lines != null) Array.ForEach(q.Lines, x => x.Header = q);
            }

            return q;
        }

        private static double GetAsDouble(string value)
        {
            double result = 0;
            if (!string.IsNullOrEmpty(value))
                try
                {
                    result = XmlConvert.ToDouble(value.Replace(",", ".").Trim());
                }
                catch
                {
                }
            return result;

        }
        public  IMLSalePurchaseLine[] GetDocLines(int doctype, string docpiece)
        {
            var q = (from i in _sageDataContext.DocLines
                     where i.DocType == doctype && i.Piece == docpiece
                     && i.RefArticle != string.Empty
                     select new MLSalePurchaseLine()
                     {
                         DlNo = i.DlNo,
                         LineNo = i.NoLigne,
                         ItemNo = i.RefArticle,
                         Description = i.Designation,
                         GrossWeight = i.PoidsBrut,
                         Tare = GetAsDouble(i.Tare),
                         DocumentNo = i.PieceBC,
                         //                        BestBeforeDate=
                         //                        SerialNo
                         ExtItemNo = i.RefClient,
                         Qty = i.Qte
                     }).OrderBy(x => x.LineNo).ToArray();

            Dictionary<int, SageClassLibrary.DataModel.LotSerial[]> dicolotserial = null;
            var dlnos = q.Select(x => x.DlNo).ToArray();
            switch ((SageClassLibrary.DataModel.EnumDocType)doctype)
            {
                case SageClassLibrary.DataModel.EnumDocType.SalesDeliveryNote:
                    dicolotserial = (from i in
                                         ((from i in _sageDataContext.LotSerials
                                           where dlnos.Contains(i.DlNoOut)
                                           select i).ToArray())
                                     group i by i.DlNoOut into a
                                     select a).ToDictionary(x => x.Key, x => x.ToArray());

                    break;
                case SageClassLibrary.DataModel.EnumDocType.PurchaseDeliveryNote:
                    dicolotserial = (from i in
                                         ((from i in _sageDataContext.LotSerials
                                           where dlnos.Contains(i.DlNoIn)
                                           select i).ToArray())
                                     group i by i.DlNoIn into a
                                     select a).ToDictionary(x => x.Key, x => x.ToArray());
                    break;
            }

            if (dicolotserial != null)
                foreach (var i in dicolotserial)
                {
                    var o = q.Where(x => x.DlNo == i.Key).Single();
                    o.ManufacturedDate =
                        i.Value.First().Fabrication <= SageClassLibrary.DataModel.SageObject.SageMinDate ? null : new DateTime?(i.Value.First().Fabrication);
                    o.SerialNo = i.Value.First().NoSerie;
                    o.BestBeforeDate =
                        i.Value.First().Peremption <= SageClassLibrary.DataModel.SageObject.SageMinDate ? null : new DateTime?(i.Value.First().Peremption);

                }
            var result = q.ToArray();
            //            var itemnos = result.Select(x => x.ItemNo).Distinct().ToArray();




            return result;
        }

        public IMLItemLotSerial[] GetItemsLotSerial()
        {
            var result = (from i in _sageDataContext.LotSerials
                          where i.QteRestant > 0
                          group i by new { i.DeNo, i.RefArticle, i.NoSerie, i.Fabrication, i.Peremption } into a
                          select new MLItemLotSerial()
                          {
                              DeNo = a.Key.DeNo,
                              ItemNo = a.Key.RefArticle,
                              SerialNo = a.Key.NoSerie,
                              ExpirationDate = a.Key.Peremption,
                              ManufacturingDate = a.Key.Fabrication,
                              Qty = a.Sum(x => x.QteRestant)
                          }).ToArray();

            var warehouses = (from i in _sageDataContext.Warehouses
                              select i).ToDictionary(x => x.DeNo);

            Array.ForEach(result, x => x.DeIntitule = warehouses[x.DeNo].Intitule);

            var itemnos = result.Select(x => x.ItemNo).Distinct().ToArray();
            var items =
                DbPager<MLItemInfo>.Get<string>(itemnos,
                x =>
                (from i in _sageDataContext.Items
                 where x.Contains(i.RefArticle)
                 select new MLItemInfo(null)
                 {
                     ItemNo = i.RefArticle,
                     Description = i.Design,
                     FlashPoint = GetFlashPoint(i.FlashPoint),
                     RID = i.RID,
                     ICADIATA = i.ICADIATA,
                     IMDG = i.IMDG,
                     Dangerous = i.DangerousSection,
                     Family = i.CodeFamille
                 }).ToArray()).ToDictionary(x => x.ItemNo);

            Array.ForEach(result, x => x.Item = items[x.ItemNo]);
            return result;
        }
    }
}
