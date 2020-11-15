using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TransferVendorData.Web.Models
{
    public partial class Vendor
    {
        public Vendor()
        {
            VendorBankAccounts = new HashSet<VendorBankAccount>();
        }

        [StringLength(4)]
        public string DataAreaId { get; set; }
        [Key]
        [StringLength(32)]
        public string VendorAccountNumber { get; set; }
        [StringLength(64)]
        public string VendorOrganizationName { get; set; }
        [StringLength(3)]
        public string AddressCountryRegionId { get; set; }
        [StringLength(16)]
        public string AddressZipCode { get; set; }
        [StringLength(128)]
        public string FormattedPrimaryAddress { get; set; }
        [StringLength(32)]
        public string AddressCity { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AddressValidFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AddressValidTo { get; set; }

        [InverseProperty(nameof(VendorBankAccount.VendorAccountNumberNavigation))]
        public virtual ICollection<VendorBankAccount> VendorBankAccounts { get; set; }
    }
}
