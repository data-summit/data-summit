
export class EmployeeTerritory {

	EmployeeId: number;
	TerritoryId: string;
	CreatedDate?: Date | string;
	UserId?: number;

	constructor(employeeId?: number, territoryId?: string, createdDate?: Date | string,
		userId?: number)
	{
		this.EmployeeId = employeeId || 0;
		this.TerritoryId = territoryId || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
	}
}
