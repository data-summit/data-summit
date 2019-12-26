
export class Product {

	ProductId: number;
	Name: string;
	SupplierId?: number;
	CategoryId?: number;
	QuantityPerUnit: string;
	UnitPrice?: number;
	UnitsInStock?: number;
	UnitsOnOrder?: number;
	ReorderLevel?: number;
	Discontinued: boolean;
	CreatedDate?: Date | string;
	UserId?: number;

	constructor(productId?: number, name?: string, supplierId?: number,
		categoryId?: number, quantityPerUnit?: string, unitPrice?: number,
		unitsInStock?: number, unitsOnOrder?: number, reorderLevel?: number,
		discontinued?: boolean, createdDate?: Date | string, userId?: number)
	{
		this.ProductId = productId || 0;
		this.Name = name || "";
		this.SupplierId = supplierId || 0 || null;
		this.CategoryId = categoryId || 0 || null;
		this.QuantityPerUnit = quantityPerUnit || "";
		this.UnitPrice = unitPrice || 0 || null;
		this.UnitsInStock = unitsInStock || 0 || null;
		this.UnitsOnOrder = unitsOnOrder || 0 || null;
		this.ReorderLevel = reorderLevel || 0 || null;
		this.Discontinued = discontinued || false;
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
	}
}
