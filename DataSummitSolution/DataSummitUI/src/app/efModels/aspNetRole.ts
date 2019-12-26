import { AspNetRoleClaim } from "./aspNetRoleClaim";
import { AspNetUserRole } from "./aspNetUserRole";

export class AspNetRole {

	Id: string;
	ConcurrencyStamp: string;
	Name: string;
	NormalizedName: string;
	AspNetRoleClaims: AspNetRoleClaim[];
	AspNetUserRoles: AspNetUserRole[];

	constructor(id?: string, concurrencyStamp?: string, name?: string,
		normalizedName?: string, aspNetRoleClaims?: AspNetRoleClaim[], aspNetUserRoles?: AspNetUserRole[])
	{
		this.Id = id || "";
		this.ConcurrencyStamp = concurrencyStamp || "";
		this.Name = name || "";
		this.NormalizedName = normalizedName || "";
		this.AspNetRoleClaims = aspNetRoleClaims;
		this.AspNetUserRoles = aspNetUserRoles;
	}
}
