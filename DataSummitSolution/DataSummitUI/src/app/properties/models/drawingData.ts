import { PropertyData } from "./propertyData";

export class DrawingData {

    DrawingId: number;
    FileName: string;
    BlobUrl: string;
    ContainerName: string;
    Success: Boolean;
    ProjectId: number;
    ProfileVersionId: number;
    AzureConfidence: number;
    GoogleConfidence: number;
    AmazonConfidence: number;
    PaperSizeId: number;
    PaperOrientationId: number;
    Processed: Boolean;
    CreatedDate: Date;
    UserId: string;
    Type: string;
    Name: string;
    CompanyId: number;
    Width: number;
    Height: number;
    WidthOriginal: number;
    HeightOriginal: number;
    X: number;
    Y: number;
    Properties: PropertyData[];

    constructor(drawingId?: number, fileName?: string, blobUrl?: string,
        containerName?: string, success?: Boolean, projectId?: number,
        profileVersionId?: number, azureConfidence?: number, googleConfidence?: number,
        amazonConfidence?: number, paperSizeId?: number, paperOrientationId?: number,
        processed?: Boolean, createdDate?: Date, userId?: string, type?: string,
        name?: string, companyId?: number, width?: number, height?: number,
        widthOriginal?: number, heightOriginal?: number, x?: number, y?: number,
        properties?: PropertyData[])
    {
        this.DrawingId = drawingId || null; 
        this.FileName = fileName || null; 
        this.BlobUrl = blobUrl || null; 
        this.ContainerName = containerName || null; 
        this.Success = success || null; 
        this.ProjectId = projectId || null; 
        this.ProfileVersionId = profileVersionId || null; 
        this.AzureConfidence = azureConfidence || null; 
        this.GoogleConfidence = googleConfidence || null; 
        this.AmazonConfidence = amazonConfidence || null; 
        this.PaperSizeId = paperSizeId || null; 
        this.PaperOrientationId = paperOrientationId || null; 
        this.Processed = processed || null; 
        this.CreatedDate = createdDate || null; 
        this.UserId = userId || null; 
        this.Type = type || null; 
        this.Name = name || null; 
        this.CompanyId = companyId || null; 
        this.Width = width || null; 
        this.Height = height || null; 
        this.WidthOriginal = widthOriginal || null; 
        this.HeightOriginal = heightOriginal || null; 
        this.X = x || null; 
        this.Y = y || null; 
        this.Properties = properties || []; 
    }
}