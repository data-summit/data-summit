import { AspNetRole } from "./aspNetRole";

export class AspNetRoleClaim {

	Id: number;
	ClaimType: string;
	ClaimValue: string;
	RoleId: string;
	Role: AspNetRole;

	constructor(id?: number, claimType?: string, claimValue?: string,
		roleId?: string, role?: AspNetRole)
	{
		this.Id = id || 0;
		this.ClaimType = claimType || "";
		this.ClaimValue = claimValue || "";
		this.RoleId = roleId || "";
		this.Role = role;
	}
}
