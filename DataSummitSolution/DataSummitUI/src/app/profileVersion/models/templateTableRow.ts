export class TemplateTableRow {

	ProfileVersionId: number;
	TemplateName: string;
	CompanyId: number;
	Width?: number;
	Height?: number;
	CreatedDate?: Date | string;

	constructor(profileVersionId?: number, 
		templateName?: string, 
		companyId?: number,
		width?: number, 
		height?: number,
		createdDate?: Date | string)
	{
		this.ProfileVersionId = profileVersionId || 0;
		this.TemplateName = templateName || "";
		this.CompanyId = companyId || 0;
		this.Width = width || 0 || null;
		this.Height = height || 0 || null;
		this.CreatedDate = createdDate || null;
	}
}
