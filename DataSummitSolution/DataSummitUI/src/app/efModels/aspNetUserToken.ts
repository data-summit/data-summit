
export class AspNetUserToken {

	UserId: string;
	LoginProvider: string;
	Name: string;
	Value: string;

	constructor(userId?: string, loginProvider?: string, name?: string,
		value?: string)
	{
		this.UserId = userId || "";
		this.LoginProvider = loginProvider || "";
		this.Name = name || "";
		this.Value = value || "";
	}
}
