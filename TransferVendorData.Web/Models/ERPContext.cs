using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TransferVendorData.Web.Models
{
    public partial class ERPContext : DbContext
    {
        public ERPContext(DbContextOptions<ERPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<VendorBankAccount> VendorBankAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.VendorAccountNumber)
                    .HasName("PK__Vendors__08935260EED07F89");

                entity.Property(e => e.VendorAccountNumber).IsUnicode(false);

                entity.Property(e => e.AddressCity).IsUnicode(false);

                entity.Property(e => e.AddressCountryRegionId).IsUnicode(false);

                entity.Property(e => e.AddressValidFrom).HasDefaultValueSql("('1900-01-01T00:00:00Z')");

                entity.Property(e => e.AddressValidTo).HasDefaultValueSql("('2154-12-31T23:59:59Z')");

                entity.Property(e => e.AddressZipCode).IsUnicode(false);

                entity.Property(e => e.DataAreaId).IsUnicode(false);

                entity.Property(e => e.FormattedPrimaryAddress).IsUnicode(false);

                entity.Property(e => e.VendorOrganizationName).IsUnicode(false);
            });

            modelBuilder.Entity<VendorBankAccount>(entity =>
            {
                entity.HasKey(e => new { e.VendorAccountNumber, e.VendorBankAccountId })
                    .HasName("PK__VendorBa__547C17EAA394AC83");

                entity.Property(e => e.VendorAccountNumber).IsUnicode(false);

                entity.Property(e => e.VendorBankAccountId).IsUnicode(false);

                entity.Property(e => e.BankName).IsUnicode(false);

                entity.Property(e => e.DataAreaId).IsUnicode(false);

                entity.Property(e => e.Iban).IsUnicode(false);

                entity.Property(e => e.Swiftcode).IsUnicode(false);

                entity.HasOne(d => d.VendorAccountNumberNavigation)
                    .WithMany(p => p.VendorBankAccounts)
                    .HasForeignKey(d => d.VendorAccountNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VendorBankAccounts_VendorAccountNumber_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
