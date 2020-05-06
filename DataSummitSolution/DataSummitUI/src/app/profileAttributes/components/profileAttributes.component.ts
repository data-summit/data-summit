import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ProfileAttribute } from '../models/profileAttribute';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from 'src/app/shared/services/api.service';
import { take } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';


@Component({
    selector: 'ds-profileAttributes',
    templateUrl: 'profileAttributes.component.html'
})
export class ProfileAttributesComponent implements OnInit {

    @ViewChild('profileAttributeModal', { static: false }) profileAttributeModal;
    
    @Input() profileVersionId: number;
    profileAttributes: ProfileAttribute[];
    headers: string[];
    selectedProfileAttribute: ProfileAttribute;
    profileAttributeForm: FormGroup
    loading: boolean;
    companyId: number;
    
    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        //Get companyId from URL
        this.companyId = this.route.snapshot.params['companyId']
        if (typeof this.companyId == 'string')         //Ensure id is number if received as a string
        { this.companyId = Number(this.companyId); }
        //Get profileVersionId from URL
        this.profileVersionId = this.route.snapshot.params['profileVersionId']
        if (typeof this.profileVersionId == 'string')         //Ensure id is number if received as a string
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
                .subscribe((result: any[]) => {
                this.profileAttributes = [];
                for (let i = 0; i < result.length; i++) {
                    let p = new ProfileAttribute(result[i].profileAttributeId,
                        result[i].standardAttributeName,
                        result[i].name,
                        result[i].x,
                        result[i].y,
                        result[i].width,
                        result[i].height,
                        result[i].createdDate);
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
        if (this.selectedProfileAttribute.ProfileAttributeId == 0) //New entry
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
            this.api.put("api/profileattributes/"+ this.profileVersionId, this.selectedProfileAttribute)
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