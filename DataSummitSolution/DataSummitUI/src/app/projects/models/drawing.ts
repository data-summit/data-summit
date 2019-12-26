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
}