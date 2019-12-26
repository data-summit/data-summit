import { Gender } from "./gender";

export class Employee {

	EmployeeId: number;
	FirstName: string;
	MiddleNames: string;
	Surname: string;
	Title: string;
	TitleOfCourtesy: string;
	JobTitle: string;
	BirthDate?: Date | string;
	HireDate?: Date | string;
	Notes: string;
	ReportsTo?: number;
	Photo: string;
	PhotoPath: string;
	GenderId?: number;
	CreatedDate?: Date | string;
	Gender: Gender;

	constructor(employeeId?: number, firstName?: string, middleNames?: string,
		surname?: string, title?: string, titleOfCourtesy?: string,
		jobTitle?: string, birthDate?: Date | string, hireDate?: Date | string,
		notes?: string, reportsTo?: number, photo?: string,
		photoPath?: string, genderId?: number, createdDate?: Date | string,
		gender?: Gender)
	{
		this.EmployeeId = employeeId || 0;
		this.FirstName = firstName || "";
		this.MiddleNames = middleNames || "";
		this.Surname = surname || "";
		this.Title = title || "";
		this.TitleOfCourtesy = titleOfCourtesy || "";
		this.JobTitle = jobTitle || "";
		this.BirthDate = birthDate || new Date(Date.now()) || null;
		this.HireDate = hireDate || new Date(Date.now()) || null;
		this.Notes = notes || "";
		this.ReportsTo = reportsTo || 0 || null;
		this.Photo = photo || "";
		this.PhotoPath = photoPath || "";
		this.GenderId = genderId || 0 || null;
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.Gender = gender;
	}
}
