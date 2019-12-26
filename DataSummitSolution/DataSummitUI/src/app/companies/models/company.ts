import { Address } from "src/app/account/models/address";
import { Project } from "src/app/projects/models/project";
import { Order } from "src/app/archive/models/order";
import { AzureCompanyResourceUrl } from "src/app/efModels/azureCompanyResourceUrl";
import { AspNetUser } from "src/app/account/models/asp-net-user";
import { ProfileVersion } from "src/app/profileVersion/models/profileVersion";

export class Company {

	CompanyId: number;
	Name: string;
	CompanyNumber: string;
	Vatnumber?: string;
	CreatedDate?: Date | string;
	UserId?: number;
	Website?: string;
	Addresses: Address[];
	AspNetUsers: AspNetUser[];
	AzureCompanyResourceUrls: AzureCompanyResourceUrl[];
	Orders: Order[];
	ProfileVersions: ProfileVersion[];
	Projects: Project[];

	constructor(companyId?: number, name?: string, companyNumber?: string,
		vatnumber?: string, createdDate?: Date | string, userId?: number,
		website?: string, addresses?: Address[], aspNetUsers?: AspNetUser[],
		azureCompanyResourceUrls?: AzureCompanyResourceUrl[], orders?: Order[], profileVersions?: ProfileVersion[],
		projects?: Project[])
	{
		this.CompanyId = companyId || 0;
		this.Name = name || "";
		this.CompanyNumber = companyNumber || "";
		this.Vatnumber = vatnumber || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
		this.Website = website || "";
		this.Addresses = addresses;
		this.AspNetUsers = aspNetUsers;
		this.AzureCompanyResourceUrls = azureCompanyResourceUrls;
		this.Orders = orders;
		this.ProfileVersions = profileVersions;
		this.Projects = projects;
	}
}