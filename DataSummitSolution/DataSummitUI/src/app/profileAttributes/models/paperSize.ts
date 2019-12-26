import { Drawing } from "../../drawings/models/drawing";
import { ProfileAttribute } from "./profileAttribute";

export class PaperSize {

	number: number;
	Name: string;
	PixelWidth: number;
	PixelHeight: number;
	PhysicalWidth?: number;
	PhysicalHeight?: number;
	Drawings: Drawing[];
	ProfileAttributes: ProfileAttribute[];

	constructor(number?: number, name?: string, pixelWidth?: number,
		pixelHeight?: number, physicalWidth?: number, physicalHeight?: number,
		drawings?: Drawing[], profileAttributes?: ProfileAttribute[])
	{
		this.number = number || 0;
		this.Name = name || "";
		this.PixelWidth = pixelWidth || 0;
		this.PixelHeight = pixelHeight || 0;
		this.PhysicalWidth = physicalWidth || 0 || null;
		this.PhysicalHeight = physicalHeight || 0 || null;
		this.Drawings = drawings;
		this.ProfileAttributes = profileAttributes;
	}
}
