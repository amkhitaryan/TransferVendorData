using System.Threading.Tasks;
using TransferVendorData.Web.Models;
using TransferVendorData.Web.Repository.Base;

namespace TransferVendorData.Web.Repository
{
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(ERPContext context)
           : base(context)
        {
        }

        public ERPContext ERPContext
        {
            get { return Context as ERPContext; }
        }

        public async Task AddOrUpdateAsync(Vendor vendor)
        {
            if (RecordExists(v => v.VendorAccountNumber == vendor.VendorAccountNumber))
            {
                await UpdateAsync(vendor);
            }
            else
            {
                await AddAsync(vendor);
            }
            
        }
    }
}
