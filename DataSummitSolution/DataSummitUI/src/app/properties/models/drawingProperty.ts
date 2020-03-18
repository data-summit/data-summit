import { ProfileAttribute } from "../../profileAttributes/models/profileAttribute";
import { Sentence } from "../../drawings/models/sentence";
import { Drawing } from "../../properties/models/drawing";
import { ProfileVersion } from "../../profileVersion/models/profileVersion";
import { Property } from "./property";

export class DrawingProperty {

	Drawing: Drawing;
	Template: ProfileVersion;
	Attributes: ProfileAttribute[];
	Properties: Property[];
	Sentences: Sentence[];

	constructor(drawing?: Drawing, profileVersion?: ProfileVersion, profileAttributes?: ProfileAttribute[],
		properties?: Property[], sentences?: Sentence[])
	{
		this.Drawing = drawing || new Drawing();
		this.Template = profileVersion || new ProfileVersion();
		this.Attributes = profileAttributes || [];
		this.Properties = properties || [];
		this.Sentences = sentences || [];
	}
}