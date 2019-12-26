import { AspNetUser } from "../account/models/asp-net-user";

export class UserType {

	UserTypeId: number;
	Value: string;
	CreatedDate: Date | string;
	AspNetUsers: AspNetUser[];

	constructor(userTypeId?: number, value?: string, createdDate?: Date | string,
		aspNetUsers?: AspNetUser[])
	{
		this.UserTypeId = userTypeId || 0;
		this.Value = value || "";
		this.CreatedDate = createdDate || new Date(Date.now());
		this.AspNetUsers = aspNetUsers;
	}
}
