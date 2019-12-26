import { Drawing } from "../drawings/models/drawing";

export class PaperOrientation {

	PaperOrientationId: number;
	Orientation: string;
	Drawings: Drawing[];

	constructor(paperOrientationId?: number, orientation?: string, drawings?: Drawing[])
	{
		this.PaperOrientationId = paperOrientationId || 0;
		this.Orientation = orientation || "";
		this.Drawings = drawings;
	}
}
