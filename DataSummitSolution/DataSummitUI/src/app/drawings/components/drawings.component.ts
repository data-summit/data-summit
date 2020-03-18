import { Component, OnInit, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take, catchError } from 'rxjs/operators';
import { Drawing } from '../models/drawing';
import { DrawingUpload } from '../models/drawingupload'
import { FileUpload } from 'primeng/fileupload';
import { FormBuilder } from '@angular/forms';
import { ProfileVersion } from '../../profileVersion/models/profileVersion'

@Component({
    selector: 'ds-drawings',
    templateUrl: 'drawings.component.html'
})

export class DrawingsComponent implements OnInit {

    @ViewChild('drawingModal', { static: false }) drawingModal;
    @ViewChild('uploadControl', { static: false }) uploadControl: FileUpload

    companyId: number;
    projectId: number;
    drawings: Drawing[];
    selectedDrawing: Drawing;
    drawingsForUpload: DrawingUpload[];
    templates: ProfileVersion[];
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
        this.getDrawings(this.projectId);
        this.initDrawingsTable();
    }

    initDrawingsTable() {
        this.headers = [
            "Name",
            "Container Url",
            "Properties",
            "Created Date",
            "Actions"
        ];
    }

    getDrawings(id: number) {
        this.loading = true;
        this.api.get("api/drawings/" + id.toString(), id)
            .pipe(take(1))
                .subscribe((result: Drawing[]) => {
                    this.drawings = [];
                    for (let i = 0; i < result.length; i++) {
                        let p = <Drawing>result[i];
                        this.drawings.push(p);
                };
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error);
                this.loading = false;
            })
    }

    addDrawing() {
        this.api.get(`api/profileversions/${this.companyId}`, this.companyId)
            .pipe(take(1))
                .subscribe((result: ProfileVersion[]) => {
                    this.templates = [];
                    for (let i = 0; i < result.length; i++) {
                        let p = <ProfileVersion>result[i];
                        this.templates.push(p);
                };
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
                this.drawingModal.show();
            }, error => {
                console.log(error)
                this.loading = false;
            });
    }

    onChange(event){
        if (typeof event === 'string')         //Ensure id is number if received as a string
        { this.templateId = Number(event); }
        else
        { this.templateId = event; }
    }

    readDrawings(event) {
        this.drawingsForUpload = []
        let fileList: FileList = event.files;
        if (fileList.length > 0) {
            for (let i = 0; i < fileList.length; i++) {
                let file: File = fileList[i];
                let reader = new FileReader();
                reader.readAsArrayBuffer(file);
                
                reader.onload = (e) => {

                    let draw = new DrawingUpload();
                    draw.ProjectId= this.projectId;
                    draw.TemplateId = this.templateId;
                    draw.FileName = file.name;
                    draw.FileType = file.type;
                    
                    draw.UserId = 1; //TODO This is Tom James' ID from the data generic data
                    //This needs to be replaced by the logged in users id once OAuth protection is working
                    //Drawings are controlled by user if they're "under trial" 
                    //Drawings are by company if the user is not "under trial"

                    var binary = '';
                    var bytes = new Uint8Array(reader.result as ArrayBuffer);
                    for (var i = 0; i < bytes.byteLength; i++) {
                        binary += String.fromCharCode(bytes[i]);
                    }
                    
                    draw.File = btoa(binary);
                    
                    // let formData: FormData = new FormData();
                    // formData.append('file', file);
                    // draw.File = formData;

                    this.drawingsForUpload.push(draw);

                    this.api.postImage(`api/drawings/`, this.drawingsForUpload)
                        .pipe(take(1))
                            .subscribe(result => {
                                console.log(result);
                                this.uploadControl.clear();
                                this.getDrawings(this.projectId);
                                this.drawingModal.hide();
                            }, error => {
                                console.log(error);
                                this.uploadControl.clear();
                                this.getDrawings(this.projectId);
                                this.drawingModal.hide();
                            });
                }
                //reader.readAsArrayBuffer(file);
            }
        }
    }

    upload() {
        this.uploadControl.upload()            
    }

    deleteDrawing(drawing) {
        //todo: api call:
        let index = this.drawings.findIndex(d => d == drawing)
        if(index > -1) {
            this.drawings.splice(index, 1)
        }
    }

    goToProperties(drawingId: number) {
        this.router.navigate([`companies/${this.companyId}/projects/${this.projectId}/drawings/${drawingId}/properties`])
    }
}