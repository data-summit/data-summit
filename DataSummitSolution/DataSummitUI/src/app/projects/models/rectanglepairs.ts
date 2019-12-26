import { RectanglePair } from "./rectanglepair"

export class RectanglePairs {

    ProjectId: number
    Height: number
    Width: number
    Pairs: RectanglePair[]
    // Pairs: string | RectanglePair[]

    constructor(projectId?: number, height?: number, 
        width?: number, rectanglePairs?: RectanglePair[]) {
            this.ProjectId = projectId || 0
            this.Height = height || 0
            this.Width = width || 0 
            //this.Pairs = rectanglePairs || ''
            this.Pairs = rectanglePairs
        }
}