### About
This is an ASP.NET Core WebApi application which reads Vendors and Bank Accounts from D365FO via OData interface and writes them into an Azure SQL database. 
It uses the provided client id and client secret to obtain oauth 2.0 token which is required to access the D365 resource and performs the data migration to the db with the help of Entity Framework Core.

### Parameters
Mandatory: filter -> Pass Odata filter criteria to specify which vendors should be transferred.

### Usage
- https://TransferVendorData/SyncVendorData?$filter=dataAreaId eq 'JPMF' -> Transfer all vendors from USMF company.
- https://TransferVendorData/SyncVendorData?$filter=dataAreaId eq 'JPMF' and VendorAccountNumber eq 'JPMF-000002' -> Transfer JPMF-000002 from JPMF company.

Expected result:

Vendors and Bank Accounts are inserted or updated in database depending on filter criteria.