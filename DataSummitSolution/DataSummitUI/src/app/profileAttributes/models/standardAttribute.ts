import { ProfileAttribute } from "./profileAttribute";

export class StandardAttribute {

	StandardAttributeId: number;
	Name: string;
	ProfileAttributes: ProfileAttribute[];

	constructor(standardAttributeId?: number, name?: string, profileAttributes?: ProfileAttribute[])
	{
		this.StandardAttributeId = standardAttributeId || 0;
		this.Name = name || "";
		this.ProfileAttributes = profileAttributes;
	}
}