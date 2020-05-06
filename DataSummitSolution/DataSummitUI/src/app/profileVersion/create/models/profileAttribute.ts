import { ProfileVersion } from "./profileVersion";
import { StandardAttribute } from "./standardAttribute";

export class ProfileAttribute {

	ProfileAttributeId: number;
	Name: string;
	NameX: number;
	NameY: number;
	NameWidth: number;
	NameHeight: number;
	PaperSizeId: number;
	BlockPositionId: number;
	ProfileVersionId: number;
	CreatedDate?: Date | string;
	UserId?: number;
	Value: string;
	ValueX?: number;
	ValueY?: number;
	ValueWidth?: number;
	ValueHeight?: number;
	StandardAttributeId?: number;
	BlockPosition?: number;
	PaperSize?: number;
	ProfileVersion?: ProfileVersion;
	StandardAttribute?: StandardAttribute;

	constructor(profileAttributeId?: number, name?: string, nameX?: number,
		nameY?: number, nameWidth?: number, nameHeight?: number,
		paperSizeId?: number, blockPositionId?: number, profileVersionId?: number,
		createdDate?: Date | string, userId?: number, value?: string,
		valueX?: number, valueY?: number, valueWidth?: number,
		valueHeight?: number, standardAttributeId?: number, blockPosition?: number,
		paperSize?: number, profileVersion?: ProfileVersion, standardAttribute?: StandardAttribute)
	{
		this.ProfileAttributeId = profileAttributeId || 0;
		this.Name = name || "";
		this.NameX = nameX || 0;
		this.NameY = nameY || 0;
		this.NameWidth = nameWidth || 0;
		this.NameHeight = nameHeight || 0;
		this.PaperSizeId = paperSizeId || 0;
		this.BlockPositionId = blockPositionId || 0;
		this.ProfileVersionId = profileVersionId || 0;
		this.CreatedDate = createdDate || null;
		this.UserId = userId || 0 || null;
		this.Value = value || "";
		this.ValueX = valueX || 0 || null;
		this.ValueY = valueY || 0 || null;
		this.ValueWidth = valueWidth || 0 || null;
		this.ValueHeight = valueHeight || 0 || null;
		this.StandardAttributeId = standardAttributeId || 0 || null;
		this.BlockPosition = blockPosition;
		this.PaperSize = paperSize;
		this.ProfileVersion = profileVersion;
		this.StandardAttribute = standardAttribute;
	}
}