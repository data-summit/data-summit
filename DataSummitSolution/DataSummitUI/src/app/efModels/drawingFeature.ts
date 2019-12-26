import { Point } from "../profileVersion/create/models/point";
import { Drawing } from "../drawings/models/drawing";

export class DrawingFeature {

	DrawingFeatureId: number;
	Vendor: string;
	Value: string;
	DrawingId: number;
	Left?: number;
	Top?: number;
	Width?: number;
	Height?: number;
	Feature?: number;
	Center?: number;
	Drawing: Drawing;
	Points: Point[];

	constructor(drawingFeatureId?: number, vendor?: string, value?: string,
		drawingId?: number, left?: number, top?: number,
		width?: number, height?: number, feature?: number,
		center?: number, drawing?: Drawing, points?: Point[])
	{
		this.DrawingFeatureId = drawingFeatureId || 0;
		this.Vendor = vendor || "";
		this.Value = value || "";
		this.DrawingId = drawingId || 0;
		this.Left = left || 0 || null;
		this.Top = top || 0 || null;
		this.Width = width || 0 || null;
		this.Height = height || 0 || null;
		this.Feature = feature || 0 || null;
		this.Center = center || 0 || null;
		this.Drawing = drawing;
		this.Points = points;
	}
}
