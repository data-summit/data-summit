export class CompanyTableRow {

	CompanyId: number;
	CompanyName: string;
	CompanyNumber: string;
	Website?: string;
	VatNumber?: string;
	CreatedDate?: Date | string;

	constructor(companyId?: number, 
		companyName?: string, 
		companyNumber?: string,
		website?: string,
		vatnumber?: string, 
		createdDate?: Date | string)
	{
		this.CompanyId = companyId || 0;
		this.CompanyName = companyName || "";
		this.CompanyNumber = companyNumber || "";
		this.Website = website || "";
		this.VatNumber = vatnumber || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
	}
}