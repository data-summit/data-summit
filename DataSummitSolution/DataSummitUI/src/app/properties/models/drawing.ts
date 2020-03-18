export class Drawing {

    DrawingId: number
    FileName: string
    BlobUrl: string
    ContainerName: string
    Success: boolean
    ProjectId: number
    ProfileVersionId?: number
    AzureConfidence?: number
    GoogleConfidence?: number
    AmazonConfidence?: number
    Processed?: boolean
    CreatedDate?: Date
    UserId?: number
    Type: string
    UploadDuration?: number
    File?: FormData
    PaperSizeId: number
    PaperOrientationId: number

    constructor(drawingId?: number, name?: string, fileName?: string, 
                blobName?: string, success?: boolean, projectId?: number, 
                profileVersionId?: number,  azureConfidence?: number, 
                googleConfidence?: number, amazonConfidence?: number, 
                processed?: boolean, createdDate?: Date, userId?: number, 
                type?: string, uploadDuration?: number, file?: FormData,
                paperSizeId?: number, paperOrientationId?: number)
    {
        this.DrawingId = drawingId;
        this.ContainerName = name;
        this.FileName = fileName;
        this.BlobUrl = blobName;
        this.Success = success;
        this.ProjectId = projectId;
        this.ProfileVersionId = profileVersionId;
        this.AzureConfidence = azureConfidence;
        this.GoogleConfidence = googleConfidence;
        this.AmazonConfidence = amazonConfidence;
        this.Processed = processed;
        this.CreatedDate = createdDate;
        this.UserId = userId;
        this.Type = type;
        this.UploadDuration = uploadDuration;
        this.File = file;
        this.PaperSizeId = paperSizeId;
        this.PaperOrientationId = paperOrientationId;
    }
}