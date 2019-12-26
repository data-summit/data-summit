export class DrawingUpload {

    ProjectId: number
    TemplateId: number
    UserId: number
    File: string
    FileName: string
    FileType: string

    constructor(projectId?: number, templateId?: number, userId?: number,
                file?: string, fileName?: string, fileType?: string)
    {
        this.ProjectId = projectId || 0;
        this.TemplateId = templateId || 0;
        this.UserId = userId || 0;
        this.File = file || "";
        this.FileName = fileName || "";
        this.FileType = fileType || "";
    }
}