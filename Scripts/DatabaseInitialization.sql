USE ERP;
GO

--ALTER DATABASE ERP
--COLLATE Latin1_General_100_CI_AS_SC_UTF8;
--GO


DROP TABLE [VendorBankAccounts], [Vendors]
GO

CREATE TABLE [Vendors](
DataAreaId varchar(4) NULL,
VendorAccountNumber varchar(32) NOT NULL,
VendorOrganizationName varchar(64) NULL,
AddressCountryRegionId varchar(3) NULL,
AddressZipCode varchar(16) NULL,
FormattedPrimaryAddress varchar(128) NULL,
AddressCity varchar(32) NULL,
AddressValidFrom datetime default('1900-01-01T00:00:00Z') NULL,
AddressValidTo datetime default('2154-12-31T23:59:59Z') NULL,
PRIMARY KEY (VendorAccountNumber)
);
GO

CREATE TABLE [VendorBankAccounts](
DataAreaId varchar(4) NULL,
VendorAccountNumber varchar(32) NOT NULL,
VendorBankAccountId varchar(64) NOT NULL,
BankName varchar(64) NULL,
IBAN varchar(32) NULL,
SWIFTCode varchar(32) NULL,
PRIMARY KEY (VendorAccountNumber, VendorBankAccountId),
CONSTRAINT VendorBankAccounts_VendorAccountNumber_FK FOREIGN KEY (VendorAccountNumber) REFERENCES [Vendors](VendorAccountNumber)
);
GO