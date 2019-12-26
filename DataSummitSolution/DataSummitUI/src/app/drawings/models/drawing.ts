export class Drawing {

    DrawingId: number
    Name: string
    FileName: string
    BlobName: string 
    Success: boolean
    ProjectId: number
    ProfileVersionId?: number
    MicrosoftConfidence?: number
    GoogleConfidence?: number
    AmazonConfidence?: number
    Processed?: boolean
    CreatedDate: Date
    UserId?: number
    Type: string
    UploadDuration?: number
    File?: FormData

    constructor(drawingId?: number, name?: string, fileName?: string, 
                blobName?: string, success?: boolean, projectId?: number, 
                profileVersionId?: number,  microsoftConfidence?: number, 
                googleConfidence?: number, amazonConfidence?: number, 
                processed?: boolean, createdDate?: Date, userId?: number, 
                type?: string, uploadDuration?: number, file?: FormData)
    {
        this.DrawingId = drawingId;
        this.Name = name;
        this.FileName = fileName;
        this.BlobName = blobName;
        this.Success = success;
        this.ProjectId = projectId;
        this.ProfileVersionId = profileVersionId;
        this.MicrosoftConfidence = microsoftConfidence;
        this.GoogleConfidence = googleConfidence;
        this.AmazonConfidence = amazonConfidence;
        this.Processed = processed;
        this.CreatedDate = createdDate;
        this.UserId = userId;
        this.Type = type;
        this.UploadDuration = uploadDuration;
        this.File = file;
        
    }
}