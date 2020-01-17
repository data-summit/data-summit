import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { ProfileVersion } from '../models/profileVersion';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'ds-profileVersions',
    templateUrl: 'profileVersions.component.html'
})

export class ProfileVersionsComponent implements OnInit {
    
    @ViewChild('profileVersionModal', { static: false }) profileVersionModal

    @Input() companyId: number;
    profileVersionId: number;
    profileVersions: ProfileVersion[];
    headers: string[];
    selectedProfileVersion: ProfileVersion;
    profileVersionForm: FormGroup
    loading: boolean;

    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        //CompanyId
        this.companyId = this.route.snapshot.params['companyId']
        if (typeof this.companyId == 'string')         //Ensure companyId is number if received as a string
        { this.companyId = Number(this.companyId);}
        this.getProfileVersions(this.companyId);
        this.initProfileVersionTable();
        //this.initProfileVersionsForm();
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
                .subscribe((result: ProfileVersion[]) => {
                    this.profileVersions = [];
                    for (let i = 0; i < result.length; i++) {
                        let p = <ProfileVersion>result[i];
                        this.profileVersions.push(p);
                };
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error)
                this.loading = false;
            });
        }

    goToAttributes(profileVersion?: ProfileVersion) {
        this.router.navigate([`companies/${this.companyId.toString()}/profileversions/${profileVersion.ProfileVersionId}/profileAttributes`]);
    }

    createProfileVersion() {
        this.router.navigate([`companies/${this.companyId.toString()}/profileversions/create`])
        //this.router.navigate([`companies/${this.companyId.toString()}/profileversions/${profileVersion.ProfileVersionId}/Create`])
    }

    addOrEditProfileVersion(profileVersion?: ProfileVersion)
    {
        if (profileVersion == null)
        { this.selectedProfileVersion = new ProfileVersion(); }
        else
        { this.selectedProfileVersion = profileVersion; }
        this.profileVersionModal.show();
    }

    hideDialog()
    {
        this.profileVersionModal.hide();
        this.selectedProfileVersion = new ProfileVersion();
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

    deleteProfileVersion(profileVersion?: ProfileVersion)
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