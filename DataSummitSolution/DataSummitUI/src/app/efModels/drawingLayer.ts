import { Drawing } from "../drawings/models/drawing";

export class DrawingLayer {

	DrawingLayerId: number;
	Name: string;
	DrawingId: number;
	Drawing: Drawing;

	constructor(drawingLayerId?: number, name?: string, drawingId?: number,
		drawing?: Drawing)
	{
		this.DrawingLayerId = drawingLayerId || 0;
		this.Name = name || "";
		this.DrawingId = drawingId || 0;
		this.Drawing = drawing;
	}
}
