using System.Threading.Tasks;
using TransferVendorData.Web.Models;

namespace TransferVendorData.Web.Repository.Base
{
    public interface IVendorBankAccountRepository : IRepository<VendorBankAccount>
    {
        Task AddOrUpdateAsync(VendorBankAccount vendorBankAccount);
    }
}
