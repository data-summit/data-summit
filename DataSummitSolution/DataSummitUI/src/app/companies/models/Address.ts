import { Company } from "./company";
import { Project } from "src/app/projects/models/project";
import { Country } from "src/app/efModels/country";

export class Address {

	AddressId: number;
	NumberName: string;
	Street: string;
	Street2: string;
	Street3: string;
	TownCity: string;
	County: string;
	CountryId: number;
	PostCode: string;
	CreatedDate?: Date | string;
	CompanyId?: number;
	ProjectId?: number;
	Company: Company;
	Country: Country;
	Project: Project;

	constructor(addressId?: number, numberName?: string, street?: string,
		street2?: string, street3?: string, townCity?: string,
		county?: string, countryId?: number, postCode?: string,
		createdDate?: Date | string, companyId?: number, projectId?: number,
		company?: Company, country?: Country, project?: Project)
	{
		this.AddressId = addressId || 0;
		this.NumberName = numberName || "";
		this.Street = street || "";
		this.Street2 = street2 || "";
		this.Street3 = street3 || "";
		this.TownCity = townCity || "";
		this.County = county || "";
		this.CountryId = countryId || 0;
		this.PostCode = postCode || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.CompanyId = companyId || 0 || null;
		this.ProjectId = projectId || 0 || null;
		this.Company = company;
		this.Country = country;
		this.Project = project;
	}
}
