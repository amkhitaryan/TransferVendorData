using System.Threading.Tasks;
using TransferVendorData.Web.Models;
using TransferVendorData.Web.Repository.Base;

namespace TransferVendorData.Web.Repository
{
    public class VendorBankAccountRepository : Repository<VendorBankAccount>, IVendorBankAccountRepository
    {
        public VendorBankAccountRepository(ERPContext context)
           : base(context)
        {
        }

        public ERPContext ERPContext
        {
            get { return Context as ERPContext; }
        }

        public async Task AddOrUpdateAsync(VendorBankAccount bankAccount)
        {
            if (RecordExists(v => 
                v.VendorAccountNumber == bankAccount.VendorAccountNumber &&
                v.VendorBankAccountId == bankAccount.VendorBankAccountId))
            {
                await UpdateAsync(bankAccount);
            }
            else
            {
                await AddAsync(bankAccount);
            }
        }
    }
}
