import { AspNetUser } from "../account/models/asp-net-user";
import { Employee } from "./employee";

export class Gender {

	GenderId: number;
	Type: string;
	CreatedDate?: Date | string;
	UserId: string;
	AspNetUsers: AspNetUser[];
	Employees: Employee[];

	constructor(genderId?: number, type?: string, createdDate?: Date | string,
		userId?: string, aspNetUsers?: AspNetUser[], employees?: Employee[])
	{
		this.GenderId = genderId || 0;
		this.Type = type || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || "";
		this.AspNetUsers = aspNetUsers;
		this.Employees = employees;
	}
}
