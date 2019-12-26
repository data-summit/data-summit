import { ProfileAttribute } from '../profileAttributes/models/profileAttribute';

export class BlockPosition {

	BlockPositionId: number;
	Name: string;
	CreatedDate?: Date | string;
	UserId?: number;
	ProfileAttributes: ProfileAttribute[];

	constructor(blockPositionId?: number, name?: string, createdDate?: Date | string,
		userId?: number, profileAttributes?: ProfileAttribute[])
	{
		this.BlockPositionId = blockPositionId || 0;
		this.Name = name || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
		this.ProfileAttributes = profileAttributes;
	}
}
