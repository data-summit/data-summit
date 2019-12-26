
export class Category {

	CategoryId: number;
	Name: string;
	Description: string;
	Picture: string;
	CreatedDate?: Date | string;
	UserId?: number;

	constructor(categoryId?: number, name?: string, description?: string,
		picture?: string, createdDate?: Date | string, userId?: number)
	{
		this.CategoryId = categoryId || 0;
		this.Name = name || "";
		this.Description = description || "";
		this.Picture = picture || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
	}
}
