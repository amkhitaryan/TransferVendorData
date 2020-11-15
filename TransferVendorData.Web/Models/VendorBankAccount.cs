using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TransferVendorData.Web.Models
{
    public partial class VendorBankAccount
    {
        [StringLength(4)]
        public string DataAreaId { get; set; }
        [Key]
        [StringLength(32)]
        public string VendorAccountNumber { get; set; }
        [Key]
        [StringLength(64)]
        public string VendorBankAccountId { get; set; }
        [StringLength(64)]
        public string BankName { get; set; }
        [Column("IBAN")]
        [StringLength(32)]
        public string Iban { get; set; }
        [Column("SWIFTCode")]
        [StringLength(32)]
        public string Swiftcode { get; set; }

        [ForeignKey(nameof(VendorAccountNumber))]
        [InverseProperty(nameof(Vendor.VendorBankAccounts))]
        public virtual Vendor VendorAccountNumberNavigation { get; set; }
    }
}
