export class Project {

	ProjectId: number;
	Name: string;
	CompanyId: number;
	CreatedDate: Date | string;

	constructor(projectId?: number, 
		name?: string, 
		companyId?: number,
		createdDate?: Date | string)
	{
		this.ProjectId = projectId || 0;
		this.Name = name || "";
		this.CompanyId = companyId || 0;
		this.CreatedDate = createdDate || new Date(Date.now());
	}
}
