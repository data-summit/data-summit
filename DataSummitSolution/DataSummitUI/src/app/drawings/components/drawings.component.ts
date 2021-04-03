import { Component, OnInit, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take, catchError } from 'rxjs/operators';
import { DocumentData } from '../models/documentdata';
import { DocumentUpload } from '../models/documentupload'
import { FileUpload } from 'primeng/fileupload';
import { FormBuilder } from '@angular/forms';

@Component({
    selector: 'ds-documents',
    templateUrl: 'documents.component.html'
})

export class DocumentsComponent implements OnInit {

    @ViewChild('documentModal', { static: false }) documentModal;
    @ViewChild('uploadControl', { static: false }) uploadControl: FileUpload

    companyId: number;
    projectId: number;
    documentTableRows: Array<DocumentData>;
    selectedDocument: DocumentData;
    documentsForUpload: DocumentUpload[];
    templateId: number;
    headers: string[];
    loading: boolean;

    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        this.companyId = this.route.snapshot.params['companyId'];
        if (typeof this.companyId == 'string')         //Ensure id is number if received as a string
        { this.companyId = Number(this.companyId); }
        this.projectId = this.route.snapshot.params['projectId'];
        if (typeof this.projectId == 'string')         //Ensure id is number if received as a string
        { this.projectId = Number(this.projectId); }
        this.getDocuments(this.projectId);
        this.initDocumentsTable();
    }

    initDocumentsTable() {
        this.headers = [
            "Name",
            "Container Url",
            "Properties",
            "Created Date",
            "Actions"
        ];
    }

    getDocuments(id: number) {
        this.loading = true;        
        this.api.get("api/documents/" + id.toString(), id)
            .pipe(take(1))
                .subscribe((result: any[]) => {
                    this.documentTableRows = [];

                    for (let i = 0; i < result.length; i++) {
                        let d = new DocumentData(result[i].documentId, 
                            result[i].name, 
                            result[i].containerUrl, 
                            result[i].createdDate);
                        this.documentTableRows.push(d);
                };
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error);
                this.loading = false;
            })
    }

    // addDocument() {
    //     this.api.get(`api/profileversions/${this.companyId}`, this.companyId)
    //         .pipe(take(1))
    //             .subscribe((result: ProfileVersion[]) => {
    //                 this.templates = [];
    //                 for (let i = 0; i < result.length; i++) {
    //                     let p = <ProfileVersion>result[i];
    //                     this.templates.push(p);
    //             };
    //             console.log(location.origin.toString() + this.router.url.toString());
    //             this.loading = false;
    //             this.documentModal.show();
    //         }, error => {
    //             console.log(error)
    //             this.loading = false;
    //         });
    // }

    onChange(event){
        if (typeof event === 'string')         //Ensure id is number if received as a string
        { this.templateId = Number(event); }
        else
        { this.templateId = event; }
    }

    readDocuments(event) {
        this.documentsForUpload = []
        let fileList: FileList = event.files;
        if (fileList.length > 0) {
            for (let i = 0; i < fileList.length; i++) {
                let file: File = fileList[i];
                let reader = new FileReader();
                reader.readAsArrayBuffer(file);
                
                reader.onload = (e) => {

                    let draw = new DocumentUpload();
                    draw.ProjectId= this.projectId;
                    draw.TemplateId = this.templateId;
                    draw.FileName = file.name;
                    draw.FileType = file.type;
                    
                    draw.UserId = 1; //TODO This is Tom James' ID from the data generic data
                    //This needs to be replaced by the logged in users id once OAuth protection is working
                    //Documents are controlled by user if they're "under trial" 
                    //Documents are by company if the user is not "under trial"

                    var binary = '';
                    var bytes = new Uint8Array(reader.result as ArrayBuffer);
                    for (var i = 0; i < bytes.byteLength; i++) {
                        binary += String.fromCharCode(bytes[i]);
                    }
                    
                    draw.File = btoa(binary);
                    
                    this.documentsForUpload.push(draw);

                    this.api.postImage(`api/documents/`, this.documentsForUpload)
                        .pipe(take(1))
                            .subscribe(result => {
                                console.log(result);
                                this.uploadControl.clear();
                                this.getDocuments(this.projectId);
                                this.documentModal.hide();
                            }, error => {
                                console.log(error);
                                this.uploadControl.clear();
                                this.getDocuments(this.projectId);
                                this.documentModal.hide();
                            });
                }
            }
        }
    }

    upload() {
        this.uploadControl.upload()            
    }

    deleteDocument(document) {
        //todo: api call:
        let index = this.documentTableRows.findIndex(d => d == document)
        if(index > -1) {
            this.documentTableRows.splice(index, 1)
        }
    }

    goToProperties(documentId: number) {
        this.router.navigate([`companies/${this.companyId}/projects/${this.projectId}/documents/${documentId}/properties`])
    }
}