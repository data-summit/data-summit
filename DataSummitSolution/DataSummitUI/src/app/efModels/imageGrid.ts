import { Drawing } from "../drawings/models/drawing";

export class ImageGrid {

	ImageGridId: number;
	Name: string;
	WidthStart: number;
	HeightStart: number;
	Width: number;
	Height: number;
	BlobUrl: string;
	Processed: boolean;
	Type: number;
	DrawingId: number;
	Drawing: Drawing;

	constructor(imageGridId?: number, name?: string, widthStart?: number,
		heightStart?: number, width?: number, height?: number,
		blobUrl?: string, processed?: boolean, type?: number,
		drawingId?: number, drawing?: Drawing)
	{
		this.ImageGridId = imageGridId || 0;
		this.Name = name || "";
		this.WidthStart = widthStart || 0;
		this.HeightStart = heightStart || 0;
		this.Width = width || 0;
		this.Height = height || 0;
		this.BlobUrl = blobUrl || "";
		this.Processed = processed || false;
		this.Type = type || 0;
		this.DrawingId = drawingId || 0;
		this.Drawing = drawing;
	}
}
