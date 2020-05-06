export class StandardAttribute {

	StandardAttributeId: number;
	Name: string;

	constructor(standardAttributeId?: number, name?: string)
	{
		this.StandardAttributeId = standardAttributeId || 0;
		this.Name = name || "";
	}
}