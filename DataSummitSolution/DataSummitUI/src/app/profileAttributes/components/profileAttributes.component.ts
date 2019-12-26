import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ProfileAttribute } from '../models/profileAttribute';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from 'src/app/shared/services/api.service';
import { take } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { validateConfig } from '@angular/router/src/config';
import { projection } from '@angular/core/src/render3/instructions';

@Component({
    selector: 'ds-profileAttributes',
    templateUrl: 'profileAttributes.component.html'
})
export class ProfileAttributesComponent implements OnInit {

    @ViewChild('profileAttributeModal') profileAttributeModal;
    
    @Input() profileVersionId: number;
    profileAttributes: ProfileAttribute[];
    headers: string[];
    selectedProfileAttribute: ProfileAttribute;
    profileAttributeForm: FormGroup
    loading: boolean;
    companyId: any;
    
    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        this.profileVersionId = this.route.snapshot.params['profileVersionId']
        this.companyId = this.route.snapshot.params['companyId']
        if (typeof this.profileVersionId === 'string')         //Ensure id is number if received as a string
        { this.profileVersionId = Number(this.profileVersionId); }
        this.getProfileAttributes(this.profileVersionId);
        this.initProfileAttributesTable();
        //this.initProfileAttributeForm();
    }
        
    mapAttributes()
    {
        this.router.navigate([`companies/${this.companyId.toString()}/profileversions/${this.profileVersionId}/standardAttributes`]);
        //this.router.navigate(["standardAttributes", this.profileVersionId]);
    }

    initProfileAttributesTable() {
        this.headers = [
            "Standard Attribute",
            "Name",
            "X",
            "Y",
            "Width",
            "Height",
            "Created",
            "Actions"
        ]}
    
    getProfileAttributes(id: number) {
        this.loading = true;
        this.api.get("api/profileAttributes/" + id.toString(), id)  
            .pipe(take(1))
                .subscribe((result: ProfileAttribute[]) => {
                this.profileAttributes = [];
                for (let i = 0; i < result.length; i++) {
                    let p = <ProfileAttribute>result[i];
                    this.profileAttributes.push(p);
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
        this.profileAttributeModal.hide();
        this.selectedProfileAttribute = new ProfileAttribute();
    }

    addOrEditProfileAttribute(profileAttribute?: ProfileAttribute)
    {
        if (profileAttribute == null)
        { this.selectedProfileAttribute = new ProfileAttribute(); }
        else
        { this.selectedProfileAttribute = profileAttribute; }
        this.profileAttributeModal.show();
    }
    
    saveProfileAttribute()
    {
        if (this.selectedProfileAttribute.ProfileVersionId == 0) //New entry
        { 
            this.api.post("api/profileattributes", this.selectedProfileAttribute)
                .pipe(take(1))
                .subscribe(result => {
                    this.profileAttributeModal.hide();
                    this.getProfileAttributes(this.profileVersionId);
                    console.log(result);
                }, error => {
                    console.log(error);
                })
        }
        else    //updated entry
        { 
            this.api.put("api/profileversions/"+ this.profileVersionId, this.selectedProfileAttribute)
            .pipe(take(1))
                .subscribe(result => {
                    this.profileAttributeModal.hide();
                    this.getProfileAttributes(this.profileVersionId);
                    console.log(result);
                }, error => {
                    console.log(error);
                })
        }
    }

    deleteProfileAttribute(profileAttribute?: ProfileAttribute)
    {
        if (profileAttribute.ProfileAttributeId > 0) //New entry
        { 
            this.api.delete("api/profileAttributes/" + profileAttribute.ProfileAttributeId.toString(), profileAttribute.ProfileAttributeId)
                .pipe(take(1))
                .subscribe(result => {
                    this.profileAttributeModal.hide();
                    this.getProfileAttributes(this.profileVersionId);
                    console.log(result);
                }, error => {
                    console.log(error);
                })
        }
    }
}