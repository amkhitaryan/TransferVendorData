using System.Threading.Tasks;
using TransferVendorData.Web.Models;

namespace TransferVendorData.Web.Repository.Base
{
    public interface IVendorRepository : IRepository<Vendor>
    {
        Task AddOrUpdateAsync(Vendor vendor);
    }
}
