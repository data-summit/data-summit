import { RectanglePair } from "./rectanglepair"

export class RectanglePairs {

    CompanyId: number
    Height: number
    Width: number
    Pairs: RectanglePair[]
    TemplateName: string

    constructor(companyId?: number, height?: number, 
        width?: number, rectanglePairs?: RectanglePair[],
        templateName?: string) {
            this.CompanyId = companyId || 0
            this.Height = height || 0
            this.Width = width || 0 
            this.Pairs = rectanglePairs
            this.TemplateName = templateName || ''
    }
}