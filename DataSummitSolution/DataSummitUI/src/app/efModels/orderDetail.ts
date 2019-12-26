import { Order } from "../archive/models/order";

export class OrderDetail {

	OrderId: number;
	ProductId: number;
	UnitPrice: number;
	Quantity: number;
	Discount: number;
	CreatedDate?: Date | string;
	UserId?: number;
	Order: Order;

	constructor(orderId?: number, productId?: number, unitPrice?: number,
		quantity?: number, discount?: number, createdDate?: Date | string,
		userId?: number, order?: Order)
	{
		this.OrderId = orderId || 0;
		this.ProductId = productId || 0;
		this.UnitPrice = unitPrice || 0;
		this.Quantity = quantity || 0;
		this.Discount = discount || 0;
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
		this.Order = order;
	}
}
