using Microsoft.AspNet.OData.Query;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TransferVendorData.Exceptions;
using TransferVendorData.Web.Models;
using TransferVendorData.Web.Services.Base;

namespace TransferVendorData.Web.Services
{
    public class DynamicsDataRetrievalService : IDataRetrievalService
    {
        private readonly HttpClient _httpClient;

        private readonly IAuthorizationService _authorizationService;
        private readonly IConfiguration _configuration;

        private const string _filter = "?$filter=";
        private const string _crossCompany = "cross-company=true";
        private const string _vendorsSelect = "$select=dataAreaId,VendorAccountNumber,VendorOrganizationName,AddressCountryRegionId,AddressZipCode,FormattedPrimaryAddress,AddressCity,AddressValidFrom,AddressValidTo";
        private const string _vendorBankAccountsSelect = "$select=dataAreaId,VendorAccountNumber,VendorBankAccountId,BankName,IBAN,SWIFTCode";

        public DynamicsDataRetrievalService(HttpClient httpclient, 
            IAuthorizationService authorizationService, 
            IConfiguration configuration)
        {
            _httpClient = httpclient;
            _authorizationService = authorizationService;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Vendor>> GetVendorsAsync(ODataQueryOptions queryOptions)
        {
            await this.InitializeHttpClientAsync();

            HttpResponseMessage response = await _httpClient.GetAsync(ConstructVendorsUri(queryOptions));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var vendors = JsonConvert.DeserializeObject<ODataResponse<Vendor>>(content);

                return vendors.Value;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.StatusCode == HttpStatusCode.Forbidden ||
                response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ServiceAuthenticationException(response.ReasonPhrase);
            }

            throw new HttpRequestExceptionEx(response.StatusCode, response.ReasonPhrase);
        }

        public async Task<IEnumerable<VendorBankAccount>> GetVendorsBankAccountsAsync(string vendorsFilter)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(ConstructVendorBankAccountsUri(vendorsFilter));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var vendorBankAccounts = JsonConvert.DeserializeObject<ODataResponse<VendorBankAccount>>(content);

                return vendorBankAccounts.Value;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.StatusCode == HttpStatusCode.Forbidden ||
                response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ServiceAuthenticationException(response.ReasonPhrase);
            }

            throw new HttpRequestExceptionEx(response.StatusCode, response.ReasonPhrase);
        }

        private async Task InitializeHttpClientAsync()
        {
            await _authorizationService.AddTokenAsync(_httpClient);
        }

        private Uri ConstructVendorsUri(ODataQueryOptions queryOptions)
        {
            var baseQuery = Uri.UnescapeDataString(queryOptions.Request.QueryString.ToString());

            return new Uri(string.Format("{0}/{1}{2}&{3}&{4}",
                _httpClient.BaseAddress, _configuration["Endpoints:vendors"], baseQuery, _vendorsSelect, _crossCompany));
        }

        private Uri ConstructVendorBankAccountsUri(string vendorsFilter)
        {
            return new Uri(string.Format("{0}/{1}{2}{3}&{4}&{5}",
                _httpClient.BaseAddress, _configuration["Endpoints:vendorBankAccounts"], _filter, vendorsFilter, _vendorBankAccountsSelect, _crossCompany));
        }
    }
}
