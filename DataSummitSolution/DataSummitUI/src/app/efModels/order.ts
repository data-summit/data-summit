import { Company } from "../companies/models/company";
import { OrderDetail } from "./orderDetail";

export class Order {

	OrderId: number;
	CompanyId?: number;
	EmployeeId?: number;
	OrderDate?: Date | string;
	RequiredDate?: Date | string;
	ShippedDate?: Date | string;
	ShipVia?: number;
	Freight?: number;
	ShipName: string;
	ShipAddress: string;
	ShipCity: string;
	ShipRegion: string;
	ShipPostalCode: string;
	ShipCountry: string;
	CreatedDate?: Date | string;
	UserId?: number;
	Company: Company;
	OrderDetails: OrderDetail[];

	constructor(orderId?: number, companyId?: number, employeeId?: number,
		orderDate?: Date | string, requiredDate?: Date | string, shippedDate?: Date | string,
		shipVia?: number, freight?: number, shipName?: string,
		shipAddress?: string, shipCity?: string, shipRegion?: string,
		shipPostalCode?: string, shipCountry?: string, createdDate?: Date | string,
		userId?: number, company?: Company, orderDetails?: OrderDetail[])
	{
		this.OrderId = orderId || 0;
		this.CompanyId = companyId || 0 || null;
		this.EmployeeId = employeeId || 0 || null;
		this.OrderDate = orderDate || new Date(Date.now()) || null;
		this.RequiredDate = requiredDate || new Date(Date.now()) || null;
		this.ShippedDate = shippedDate || new Date(Date.now()) || null;
		this.ShipVia = shipVia || 0 || null;
		this.Freight = freight || 0 || null;
		this.ShipName = shipName || "";
		this.ShipAddress = shipAddress || "";
		this.ShipCity = shipCity || "";
		this.ShipRegion = shipRegion || "";
		this.ShipPostalCode = shipPostalCode || "";
		this.ShipCountry = shipCountry || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
		this.Company = company;
		this.OrderDetails = orderDetails;
	}
}
