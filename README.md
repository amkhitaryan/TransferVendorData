### About
This is an ASP.NET Core WebApi application which reads Vendors and Bank Accounts from D365FO via OData interface and writes them into an Azure SQL database.

### Parameters
Mandatory: filter -> Pass Odata filter criteria to specify which vendors should be transferred.

### Usage
- https://TransferVendorData/SyncVendorData?$filter=dataAreaId eq 'JPMF' -> Transfer all vendors from USMF company.
- https://TransferVendorData/SyncVendorData?$filter=dataAreaId eq 'JPMF' and VendorAccountNumber eq 'JPMF-000002' -> Transfer JPMF-000002 from JPMF company.

Expected result:

Vendors and Bank Accounts are inserted or updated in database depending on filter criteria.