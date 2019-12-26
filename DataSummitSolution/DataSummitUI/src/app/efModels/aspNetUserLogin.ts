import { AspNetUser } from "../account/models/asp-net-user";

export class AspNetUserLogin {

	LoginProvider: string;
	ProviderKey: string;
	ProviderDisplayName: string;
	UserId: string;
	User: AspNetUser;

	constructor(loginProvider?: string, providerKey?: string, providerDisplayName?: string,
		userId?: string, user?: AspNetUser)
	{
		this.LoginProvider = loginProvider || "";
		this.ProviderKey = providerKey || "";
		this.ProviderDisplayName = providerDisplayName || "";
		this.UserId = userId || "";
		this.User = user;
	}
}
