using System.Collections.Generic;

namespace TransferVendorData.Web.Models
{
    internal class ODataResponse<T>
    {
        public List<T> Value { get; set; }
    }
}
