import { Sentence } from "./sentence";

export class Rectangle {

	RectangleId: number;
	Height: number;
	Left: number;
	Top: number;
	Width: number;
	SentenceId: number;
	Sentence: Sentence;

	constructor(rectangleId?: number, height?: number, left?: number,
		top?: number, width?: number, sentenceId?: number,
		sentence?: Sentence)
	{
		this.RectangleId = rectangleId || 0;
		this.Height = height || 0;
		this.Left = left || 0;
		this.Top = top || 0;
		this.Width = width || 0;
		this.SentenceId = sentenceId || 0;
		this.Sentence = sentence;
	}
}
