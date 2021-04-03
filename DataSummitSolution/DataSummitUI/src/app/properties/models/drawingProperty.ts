export class DocumentProperty {

    Id: string;
    StandardName: string;
    Name: string;
    Value: string;
    Confidence: number;

    constructor(Id?: string,
        standardName?: string,
        name?: string, 
        value?: string,
        confidence?: number)
    {
        this.Id = Id || null; 
        this.StandardName = standardName || null; 
        this.Name = name || null; 
        this.Value = value || null; 
        this.Confidence = confidence || null; 
    }    
}