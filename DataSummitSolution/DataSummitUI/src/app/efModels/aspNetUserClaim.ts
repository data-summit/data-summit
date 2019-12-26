import { AspNetUser } from "../account/models/asp-net-user";

export class AspNetUserClaim {

	Id: number;
	ClaimType: string;
	ClaimValue: string;
	UserId: string;
	User: AspNetUser;

	constructor(id?: number, claimType?: string, claimValue?: string,
		userId?: string, user?: AspNetUser)
	{
		this.Id = id || 0;
		this.ClaimType = claimType || "";
		this.ClaimValue = claimValue || "";
		this.UserId = userId || "";
		this.User = user;
	}
}
