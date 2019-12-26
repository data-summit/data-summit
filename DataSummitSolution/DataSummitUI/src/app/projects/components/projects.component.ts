import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { Project } from '../models/project';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { validateConfig } from '@angular/router/src/config';
import { projection } from '@angular/core/src/render3/instructions';

@Component({
    selector: 'ds-projects',
    templateUrl: 'projects.component.html'
})
export class ProjectsComponent implements OnInit {

    @ViewChild('projectModal') projectModal
    
    @Input() companyId: number;
    projects: Project[];
    headers: string[];
    selectedProject: Project;
    projectForm: FormGroup
    loading: boolean;
    
    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        this.companyId = this.route.snapshot.params['companyId'];
        if (typeof this.companyId === 'string')         //Ensure id is number if received as a string
        { this.companyId = Number(this.companyId); }
        this.getProjects(this.companyId);
        this.initProjectsTable();
        this.initProjectForm();
    }
    
    initProjectForm() {
        this.projectForm = this.fb.group({
            Name: this.fb.control('', Validators.required),
            CreateDate: this.fb.control('')
        });
        this.selectedProject = new Project();
    }
    
    initProjectsTable() {
        this.headers = [
            "Name",
            "Drawings",
            "Project Templates",
            "Created Date",
            "Actions"
        ]}
    
    getProjects(id: number) {
        this.loading = true;
        this.api.get("api/projects/" + id.toString(), id)
            .pipe(take(1))
                .subscribe((result: Project[]) => {
                    this.projects = [];
                    for (let i = 0; i < result.length; i++) {
                        let p = <Project>result[i];
                        this.projects.push(p);
                };
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error)
                this.loading = false;
            });
    }

    hideDialog()
    {
        this.projectModal.hide();
        this.selectedProject = new Project();
    }

    addOrEditProject(project?: Project)
    {
        if (project == null)
        { this.selectedProject = new Project(); }
        else
        { this.selectedProject = project; }
        this.selectedProject.CompanyId = this.companyId;
        this.projectModal.show();
    }
    
    goToDrawings(projectId: number) {
        this.router.navigate([`companies/${this.companyId}/projects/${projectId}/drawings`]);
    }

    goToTemplates(projectId: number) {
        this.router.navigate(['projects', 'templates', projectId]);
    }

    createTemplate() {
        this.router.navigate([`companies/${this.companyId.toString()}/profileversions/create`])
    }

    saveProject() 
    {
        if (this.selectedProject.ProjectId == 0) //New entry
        { 
            this.api.post("api/projects", this.selectedProject)
                .pipe(take(1))
                .subscribe(result => {
                    this.projectModal.hide();
                    this.getProjects(this.companyId);
                    console.log(result);
                }, error => {
                    console.log(error);
                });
        }
        else    //updated entry
        { 
            this.api.put("api/projects/" + this.companyId, this.selectedProject)
            .pipe(take(1))
                .subscribe(result => {
                    this.projectModal.hide();
                    this.getProjects(this.companyId);
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }
    }
    
    deleteProject(project?: Project)
    {
        if (project.ProjectId > 0) //New entry
        { 
            this.api.delete("api/projects/" + project.ProjectId.toString(), project.ProjectId)
                .pipe(take(1))
                .subscribe(result => {
                    this.projectModal.hide();
                    this.getProjects(this.companyId);
                    console.log(result);
                }, error => {
                    console.log(error);
                })
        }
    }
}