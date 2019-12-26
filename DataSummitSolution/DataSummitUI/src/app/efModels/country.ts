import { Address } from "../companies/models/address";

export class Country {

	CountryId: number;
	Iso: string;
	Name: string;
	SentenceCaseName: string;
	Iso3: string;
	Numcode?: number;
	Phonecode: number;
	CreatedDate?: Date | string;
	UserId?: number;
	Addresses: Address[];

	constructor(countryId?: number, iso?: string, name?: string,
		sentenceCaseName?: string, iso3?: string, numcode?: number,
		phonecode?: number, createdDate?: Date | string, userId?: number,
		addresses?: Address[])
	{
		this.CountryId = countryId || 0;
		this.Iso = iso || "";
		this.Name = name || "";
		this.SentenceCaseName = sentenceCaseName || "";
		this.Iso3 = iso3 || "";
		this.Numcode = numcode || 0 || null;
		this.Phonecode = phonecode || 0;
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
		this.Addresses = addresses;
	}
}
