export class ProfileAttribute {

	ProfileAttributeId: number;
	StandardAttributeName: string;
	Name: string;
	ValueX?: number;
	ValueY?: number;
	Width: number;
	Height: number;
	CreatedDate?: Date | string;
	
	constructor(profileAttributeId?: number, 
		standardAttributeName?: string, 
		name?: string,
		valueX?: number, 
		valueY?: number, 
		width?: number, 
		height?: number,
		createdDate?: Date | string)
	{
		this.ProfileAttributeId = profileAttributeId || 0;
		this.StandardAttributeName = standardAttributeName || "";
		this.Name = name || "";
		this.ValueX = valueX || 0 || null;
		this.ValueY = valueY || 0 || null;
		this.Width = width || 0;
		this.Height = height || 0;
		this.CreatedDate = createdDate || null;
	}
}
