
export class PropertyData {

    ProfileAttributeId: number;
    Name: string;
    NameX: number;
    NameY: number;
    NameWidth: number;
    NameHeight: number;
    PaperSizeId: number;
    BlockPositionId: number;
    ProfileVersionId: number;
    CreatedDate: Date;
    UserId: number;
    Value: string;
    ValueX: number;
    ValueY: number;
    ValueWidth: number;
    ValueHeight: number;
    StandardAttributeId: number;
    StandardName: string;
    SentenceId: string;
    Words: string;
    Width: number;
    Height: number;
    Left: number;
    Top: number;
    Vendor: string;
    IsUsed: Boolean;
    Confidence: number;
    SlendernessRatio: number;
    DrawingId: number;
    PropertyId: number;

    constructor(profileAttributeId?: number,
        name?: string, nameX?: number, nameY?: number, nameWidth?: number, nameHeight?: number,
        paperSizeId?: number, blockPositionId?: number, profileVersionId?: number,
        createdDate?: Date, userId?: number,
        value?: string, valueX?: number, valueY?: number, valueWidth?: number, valueHeight?: number,
        standardAttributeId?: number, standardName?: string, sentenceId?: string,
        words?: string, width?: number, height?: number, left?: number, top?: number,
        vendor?: string, isUsed?: Boolean, confidence?: number, slendernessRatio?: number, drawingId?: number,
        propertyId?: number)
    {
        this.ProfileAttributeId = profileAttributeId || null; 
        this.Name = name || null; 
        this.NameX = nameX || null; 
        this.NameY = nameY || null; 
        this.NameWidth = nameWidth || null; 
        this.NameHeight = nameHeight || null; 
        this.PaperSizeId = paperSizeId || null; 
        this.BlockPositionId = blockPositionId || null; 
        this.ProfileVersionId = profileVersionId || null; 
        this.CreatedDate = createdDate || null; 
        this.UserId = userId || null; 
        this.Value = value || null; 
        this.ValueX = valueX || null; 
        this.ValueY = valueY || null; 
        this.ValueWidth = valueWidth || null; 
        this.ValueHeight = valueHeight || null; 
        this.StandardAttributeId = standardAttributeId || null; 
        this.StandardName = standardName || null; 
        this.SentenceId = sentenceId || null; 
        this.Words = words || null; 
        this.Width = width || null; 
        this.Height = height || null; 
        this.Left = left || null; 
        this.Top = top || null; 
        this.Vendor = vendor || null; 
        this.IsUsed = isUsed || null; 
        this.Confidence = confidence || null; 
        this.SlendernessRatio = slendernessRatio || null; 
        this.DrawingId = drawingId || null; 
        this.PropertyId = propertyId || null;
    }    
}