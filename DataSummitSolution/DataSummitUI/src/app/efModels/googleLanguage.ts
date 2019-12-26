
export class GoogleLanguage {

	GoogleLanguageId: number;
	Name: string;
	Code: string;
	Notes: string;

	constructor(googleLanguageId?: number, name?: string, code?: string,
		notes?: string)
	{
		this.GoogleLanguageId = googleLanguageId || 0;
		this.Name = name || "";
		this.Code = code || "";
		this.Notes = notes || "";
	}
}
