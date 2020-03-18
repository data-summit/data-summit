import { ProfileAttribute } from "../../profileAttributes/models/profileAttribute";
import { Sentence } from "../../drawings/models/sentence";

export class Property {

	PropertyId: number;
	SentenceId: number;
	ProfileAttributeId: number;
	ProfileAttribute: ProfileAttribute;
	Sentence: Sentence;
	// TODO fix this property
	profileVersionId: any;

	constructor(propertyId?: number, sentenceId?: number, profileAttributeId?: number,
		profileAttribute?: ProfileAttribute, sentence?: Sentence)
	{
		this.PropertyId = propertyId || 0;
		this.SentenceId = sentenceId || 0;
		this.ProfileAttributeId = profileAttributeId || 0;
		this.ProfileAttribute = profileAttribute;
		this.Sentence = sentence;
	}
}
