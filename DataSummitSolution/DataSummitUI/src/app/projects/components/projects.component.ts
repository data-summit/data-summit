import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { Project } from '../models/project';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    selector: 'ds-projects',
    templateUrl: 'projects.component.html'
})
export class ProjectsComponent implements OnInit {

    @ViewChild('projectModal', { static: false }) projectModal
    
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
            "Documents",
            "Project Templates",
            "Company Templates",
            "Created Date",
            "Actions"
        ]}
    
    getProjects(id: number) {
        this.loading = true;
        this.api.get("api/projects/" + id.toString(), id)
            .pipe(take(1))
                .subscribe((result: any[]) => {
                    this.projects = [];
                    for (let i = 0; i < result.length; i++) {
                        let p = new Project(result[i].projectId, 
                            result[i].name, 
                            this.companyId,
                            result[i].createdDate);
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

    addProject(project?: Project)
    {
        if (!project)
        { this.selectedProject = new Project() }
        else
        { this.selectedProject = project; }
        this.projectModal.show();
    }
    
    editProject(project?: Project)
    {
        this.addProject(project);
    }

    goToDocuments(projectId: number) {
        this.router.navigate(['companies', this.companyId, 'projects', projectId, 'documents']);
    }

    goToProjectTemplates(projectId: number) {
        this.router.navigate(['companies', this.companyId, 'projects', projectId, 'profileversions']);
    }

    goToCompanyTemplates() {
        this.router.navigate(['companies', this.companyId, 'profileversions']);
    }

    createTemplate() {
        this.router.navigate(['companies', this.companyId, 'profileversions', 'create'])
    }

    saveProject() 
    {
        this.selectedProject.CompanyId = this.companyId;
        if (this.selectedProject.ProjectId == 0) //New entry
        { 
            this.api.post("api/projects/create", this.selectedProject)
                .pipe(take(1))
                .subscribe(result => {
                    this.projectModal.hide();
                    this.getProjects(this.companyId);
                    console.log(result);
                }, error => {
                    console.log(error);
                });
        }
        else //updated entry
        { 
            this.api.put("api/projects/update", this.selectedProject)
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
        project.CompanyId = this.companyId;
        if (project.ProjectId > 0)
        { 
            this.api.delete("api/projects/delete", project.ProjectId)
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