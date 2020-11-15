using System.Net.Http;
using System.Threading.Tasks;

namespace TransferVendorData.Web.Services.Base
{
    public interface IAuthorizationService
    {
        Task AddTokenAsync(HttpClient httpClient);
    }
}
