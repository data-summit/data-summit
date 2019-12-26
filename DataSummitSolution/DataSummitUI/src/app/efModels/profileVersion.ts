import { Company } from "../companies/models/company";
import { ProfileAttribute } from "../projects/models/profile-attributes";

export class ProfileVersion {

	ProfileVersionId: number;
	Name: string;
	CompanyId: number;
	Image: string;
	Width?: number;
	Height?: number;
	CreatedDate?: Date | string;
	UserId?: number;
	WidthOriginal?: number;
	HeightOriginal?: number;
	X?: number;
	Y?: number;
	Company: Company;
	ProfileAttributes: ProfileAttribute[];

	constructor(profileVersionId?: number, name?: string, companyId?: number,
		image?: string, width?: number, height?: number,
		createdDate?: Date | string, userId?: number, widthOriginal?: number,
		heightOriginal?: number, x?: number, y?: number,
		company?: Company, profileAttributes?: ProfileAttribute[])
	{
		this.ProfileVersionId = profileVersionId || 0;
		this.Name = name || "";
		this.CompanyId = companyId || 0;
		this.Image = image || "";
		this.Width = width || 0 || null;
		this.Height = height || 0 || null;
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
		this.WidthOriginal = widthOriginal || 0 || null;
		this.HeightOriginal = heightOriginal || 0 || null;
		this.X = x || 0 || null;
		this.Y = y || 0 || null;
		this.Company = company;
		this.ProfileAttributes = profileAttributes;
	}
}
