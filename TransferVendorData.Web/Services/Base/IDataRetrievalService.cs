using Microsoft.AspNet.OData.Query;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransferVendorData.Web.Models;

namespace TransferVendorData.Web.Services.Base
{
    public interface IDataRetrievalService
    {
        Task<IEnumerable<Vendor>> GetVendorsAsync(ODataQueryOptions options);

        Task<IEnumerable<VendorBankAccount>> GetVendorsBankAccountsAsync(string vendorsFilter);
    }
}
