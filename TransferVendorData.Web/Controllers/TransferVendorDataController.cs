using Microsoft.AspNetCore.Mvc;
using TransferVendorData.Web.Services.Base;
using System.Threading.Tasks;
using TransferVendorData.Web.Models;
using Microsoft.AspNet.OData;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Query;
using TransferVendorData.Web.Repository.Base;
using System.Linq;
using Microsoft.OData;

namespace TransferVendorData.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferVendorDataController : ControllerBase
    {
        private readonly IDataRetrievalService _dataRetrievalService;
        private readonly IVendorRepository _vendorRepository;
        private readonly IVendorBankAccountRepository _vendorBankAccountRepository;

        public TransferVendorDataController(
            IVendorRepository vendorRepository,
            IVendorBankAccountRepository vendorBankAccountRepository,
            IDataRetrievalService dataRetrievalService)
        {
            _vendorRepository = vendorRepository;
            _vendorBankAccountRepository = vendorBankAccountRepository;
            _dataRetrievalService = dataRetrievalService;
        }

        [HttpGet]
        [EnableQuery]
        [Route("/[controller]/[action]")]
        public async Task<IEnumerable<Vendor>> SyncVendorData(ODataQueryOptions queryOptions)
        {
            ValidateQueryOptions(queryOptions);

            var syncedVendors = await SyncVendors(queryOptions);

            if (syncedVendors != null)
            {
                await SyncVendorBankAccounts(syncedVendors);
            }

            return syncedVendors;
        }

        private async Task<IEnumerable<Vendor>> SyncVendors(ODataQueryOptions options)
        {
            var vendors = await _dataRetrievalService.GetVendorsAsync(options);

            if (vendors != null)
            {
                foreach (Vendor vendor in vendors)
                {
                    await _vendorRepository.AddOrUpdateAsync(vendor);
                }

                return vendors;
            }

            return null;
        }

        private async Task SyncVendorBankAccounts(IEnumerable<Vendor> vendors)
        {
            // Looks like IN operator(https://docs.microsoft.com/en-us/odata/webapi/in-operator) is not supported, so use eq with or
            var vendorsFilter = string.Join(" or ", vendors.Select(v => string.Format("VendorAccountNumber eq '{0}'", v.VendorAccountNumber)));

            var vendorBankAccounts = await _dataRetrievalService.GetVendorsBankAccountsAsync(vendorsFilter);

            if (vendorBankAccounts != null)
            {
                foreach (VendorBankAccount bankAccount in vendorBankAccounts)
                {
                    await _vendorBankAccountRepository.AddOrUpdateAsync(bankAccount);
                }
            }
        }

        internal static void ValidateQueryOptions(ODataQueryOptions queryOptions)
        {
            var errorMessage = "The query specified in the URI is not valid.";

            if (queryOptions.Filter == null)
            {
                throw new ODataException(errorMessage + "The filter parameter is mandatory. " +
                    "Please pass OData filter criteria to specify which vendors should be transferred.");
            }
            if (queryOptions.SelectExpand != null)
            {
                throw new ODataException(errorMessage + "The select parameter is not supported.");
            }
        }
    }
}
