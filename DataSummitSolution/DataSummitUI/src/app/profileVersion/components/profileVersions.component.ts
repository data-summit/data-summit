import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { TemplateTableRow } from '../models/templateTableRow';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'ds-profileVersions',
    templateUrl: 'profileVersions.component.html'
})

export class ProfileVersionsComponent implements OnInit {
    
    @ViewChild('profileVersionModal', { static: false }) profileVersionModal

    @Input() companyId: number;
    profileVersionId: number;
    profileVersions: Array<TemplateTableRow>;
    headers: string[];
    selectedProfileVersion: TemplateTableRow;
    profileVersionForm: FormGroup
    loading: boolean;

    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        this.companyId = this.route.snapshot.params['companyId']
        if (typeof this.companyId == 'string') //Ensure companyId is number if received as a string
        { this.companyId = Number(this.companyId);}
        this.getProfileVersions(this.companyId);
        this.initProfileVersionTable();
    }

    initProfileVersionTable() {
        this.headers = [
            "Name",
            "Profile Attributes",
            "Height",
            "Width",
            "Created Date",
            "Actions"
        ]}

    getProfileVersions(id: number) {
        this.loading = true;
        this.api.get("api/profileversions/" + id.toString(), id)
            .pipe(take(1))
                .subscribe((result: any[]) => {
                    this.profileVersions = [];
                    for (let i = 0; i < result.length; i++) {
                        let t = new TemplateTableRow(result[i].profileVersionId, 
                            result[i].templateName, 
                            result[i].companyId, 
                            result[i].width, 
                            result[i].height, 
                            result[i].createdDate);

                        this.profileVersions.push(t);
                };
                this.loading = false;
            }, error => {
                console.log(error)
                this.loading = false;
            });
        }

    goToAttributes(profileVersion?: TemplateTableRow) {
        this.router.navigate([`companies/${this.companyId.toString()}/profileversions/${profileVersion.ProfileVersionId}/profileAttributes`]);
    }

    createProfileVersion() {
        this.router.navigate([`companies/${this.companyId.toString()}/profileversions/create`])
    }

    addOrEditProfileVersion(profileVersion?: TemplateTableRow)
    {
        if (profileVersion == null)
        { this.selectedProfileVersion = new TemplateTableRow(); }
        else
        { this.selectedProfileVersion = profileVersion; }
        this.profileVersionModal.show();
    }

    hideDialog()
    {
        this.profileVersionModal.hide();
        this.selectedProfileVersion = new TemplateTableRow();
    }

    saveProfileVersion()
    {
        if (this.selectedProfileVersion.ProfileVersionId == 0) //New entry
        { 
            this.api.post("api/profileversions", this.selectedProfileVersion)
                .pipe(take(1))
                .subscribe(result => {
                    this.profileVersionModal.hide();
                    this.getProfileVersions(this.companyId);
                    console.log(result);
                }, error => {
                    console.log(error);
                })
        }
        else    //updated entry
        { 
            this.api.put("api/profileversions/"+ this.companyId, this.selectedProfileVersion)
            .pipe(take(1))
                .subscribe(result => {
                    this.profileVersionModal.hide();
                    this.getProfileVersions(this.companyId);
                    console.log(result);
                }, error => {
                    console.log(error);
                })
        }
    }

    deleteProfileVersion(profileVersion?: TemplateTableRow)
    {
        if (profileVersion.ProfileVersionId > 0) //New entry
        { 
            this.api.delete("api/profileVersions/" + profileVersion.ProfileVersionId.toString(), profileVersion.ProfileVersionId)
                .pipe(take(1))
                .subscribe(result => {
                    this.getProfileVersions(this.companyId);
                    console.log(result);
                }, error => {
                    console.log(error);
                })
        }
    }
}