import { AspNetRole } from "./aspNetRole";
import { AspNetUser } from "../account/models/asp-net-user";

export class AspNetUserRole {

	UserId: string;
	RoleId: string;
	Role: AspNetRole;
	User: AspNetUser;

	constructor(userId?: string, roleId?: string, role?: AspNetRole,
		user?: AspNetUser)
	{
		this.UserId = userId || "";
		this.RoleId = roleId || "";
		this.Role = role;
		this.User = user;
	}
}
