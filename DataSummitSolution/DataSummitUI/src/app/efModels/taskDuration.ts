import { Timestamp } from "rxjs/internal/operators/timestamp";
import { Drawing } from "../drawings/models/drawing";
import { TimeSpan } from "../shared/models/timespan";

export class TaskDuration {

	TaskDurationId: number;
	Name: string;
	TimeStamp: Date | string;
	DrawingId: number;
	Duration: TimeSpan;
	Drawing: Drawing;

	constructor(taskDurationId?: number, name?: string, timeStamp?: Date | string,
		drawingId?: number, duration?: TimeSpan, drawing?: Drawing)
	{
		this.TaskDurationId = taskDurationId || 0;
		this.Name = name || "";
		this.TimeStamp = timeStamp || new Date(Date.now());
		this.DrawingId = drawingId || 0;
		this.Duration = duration;
		this.Drawing = drawing;
	}
}
