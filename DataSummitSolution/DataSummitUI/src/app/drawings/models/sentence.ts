import { Rectangle } from "../../profileVersion/create/models/rectangle";
import { Drawing } from "./drawing";
import { Property } from "../../properties/models/property";

export class Sentence {

	SentenceId: number;
	Words: string;
	RectangleId: number;
	Vendor: string;
	IsUsed: boolean;
	Confidence?: number;
	DrawingId: number;
	Drawing: Drawing;
	Properties: Property[];
	Rectangles: Rectangle[];

	constructor(sentenceId?: number, words?: string, rectangleId?: number,
		vendor?: string, isUsed?: boolean, confidence?: number,
		drawingId?: number, drawing?: Drawing, properties?: Property[],
		rectangles?: Rectangle[])
	{
		this.SentenceId = sentenceId || 0;
		this.Words = words || "";
		this.RectangleId = rectangleId || 0;
		this.Vendor = vendor || "";
		this.IsUsed = isUsed || false;
		this.Confidence = confidence || null;
		this.DrawingId = drawingId || 0;
		this.Drawing = drawing;
		this.Properties = properties;
		this.Rectangles = rectangles;
	}
}
