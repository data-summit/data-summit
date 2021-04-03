export class DocumentData {

    DocumentId: number
    Name: string
    ContainerUrl: string 
    CreatedDate: Date

    constructor(documentId?: number, 
                name?: string, 
                containerUrl?: string, 
                createdDate?: Date)
    {
        this.DocumentId = documentId  || null;
        this.Name = name || null;
        this.ContainerUrl = containerUrl || null;
        this.CreatedDate = createdDate || null;
    }
}