import { Company } from "../companies/models/company";

export class AzureCompanyResourceUrl {

	AzureCompanyResourceUrlId: number;
	Name: string;
	Url: string;
	Key: string;
	ResourceType: string;
	CompanyId: number;
	Company: Company;

	constructor(azureCompanyResourceUrlId?: number, name?: string, url?: string,
		key?: string, resourceType?: string, companyId?: number,
		company?: Company)
	{
		this.AzureCompanyResourceUrlId = azureCompanyResourceUrlId || 0;
		this.Name = name || "";
		this.Url = url || "";
		this.Key = key || "";
		this.ResourceType = resourceType || "";
		this.CompanyId = companyId || 0;
		this.Company = company;
	}
}
