
export class Currency {

	CurrencyId: number;
	Entity: string;
	Name: string;
	AlphabeticCode: string;
	NumericCode: string;
	MinorUnit: string;
	IsFundYesNo: boolean;
	CreatedDate?: Date | string;
	UserId?: number;

	constructor(currencyId?: number, entity?: string, name?: string,
		alphabeticCode?: string, numericCode?: string, minorUnit?: string,
		isFundYesNo?: boolean, createdDate?: Date | string, userId?: number)
	{
		this.CurrencyId = currencyId || 0;
		this.Entity = entity || "";
		this.Name = name || "";
		this.AlphabeticCode = alphabeticCode || "";
		this.NumericCode = numericCode || "";
		this.MinorUnit = minorUnit || "";
		this.IsFundYesNo = isFundYesNo || false;
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
	}
}
