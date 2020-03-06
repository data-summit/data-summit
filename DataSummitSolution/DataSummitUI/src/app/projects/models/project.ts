import { Company } from "../../companies/models/company";
import { Drawing } from "../../drawings/models/drawing";
import { Address } from "../../companies/models/address";

export class Project {

	ProjectId: number;
	Name: string;
	BlobStorageName: string;
	BlobStorageKey: string;
	CompanyId: number;
	CreatedDate: Date | string;
	UserId: number;
	Company: Company;
	Addresses: Address[];
	Drawings: Drawing[];

	constructor(projectId?: number, name?: string, blobStorageName?: string,
		blobStorageKey?: string, companyId?: number, createdDate?: Date | string,
		userId?: number, company?: Company, addresses?: Address[],
		drawings?: Drawing[])
	{
		this.ProjectId = projectId || 0;
		this.Name = name || "";
		this.BlobStorageName = blobStorageName || "";
		this.BlobStorageKey = blobStorageKey || "";
		this.CompanyId = companyId || 0;
		this.CreatedDate = createdDate || new Date(Date.now());
		this.UserId = userId || 0;
		this.Company = company;
		this.Addresses = addresses;
		this.Drawings = drawings;
	}
}
