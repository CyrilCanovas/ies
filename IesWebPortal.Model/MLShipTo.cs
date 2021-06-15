using IesWebPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IesWebPortal.Model
{
    public class MLShipTo : IMLShipTo
    {
        [DisplayName("Identifiant")]
        [ScaffoldColumn(false)]
        public int LiNo { get; set; }

        [DisplayName("Intitulé adr.")]
        public string Description { get; set; }

        [DisplayName("Adresse")]
        public string Address { get; set; }

        [DisplayName("Complément")]
        public string Address1 { get; set; }

        [DisplayName("Ville")]
        public string City { get; set; }

        [DisplayName("Code postal")]
        public string ZipCode { get; set; }

        [DisplayName("Pays")]
        public string Country { get; set; }

        [DisplayName("Téléphone")]
        public string Phone { get; set; }

        [DisplayName("Fax")]
        public string Fax { get; set; }

        [DisplayName("Mail")]
        public string Mail { get; set; }

        [DisplayName("Adresse complète")]
        public string FullAddress
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine(Description ?? string.Empty);
                sb.AppendLine(Address ?? string.Empty);
                sb.AppendLine(Address1 ?? string.Empty);
                sb.AppendLine(string.Format("{0} {1}", ZipCode ?? string.Empty, City ?? string.Empty));
                return sb.ToString();
            }
        }

    }
}
