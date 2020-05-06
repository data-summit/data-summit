export class DrawingData {

    DrawingId: number
    Name: string
    ContainerUrl: string 
    CreatedDate: Date

    constructor(drawingId?: number, 
                name?: string, 
                containerUrl?: string, 
                createdDate?: Date)
    {
        this.DrawingId = drawingId  || null;
        this.Name = name || null;
        this.ContainerUrl = containerUrl || null;
        this.CreatedDate = createdDate || null;
    }
}