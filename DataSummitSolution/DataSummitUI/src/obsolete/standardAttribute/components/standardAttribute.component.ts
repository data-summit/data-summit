import { Component, OnInit, ViewChild } from '@angular/core';
import { StandardAttribute } from '../models/standardAttribute';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/shared/services/api.service';
import { take } from 'rxjs/operators';
import { FormGroup, FormBuilder } from '@angular/forms';

const reg = '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?';

@Component({
    selector: 'ds-standardAttribute',
    templateUrl: 'standardAttribute.component.html'
})
export class StandardAttributeComponent implements OnInit {

    @ViewChild('standardAttributeModal', { static: false }) standardAttributeModal

    standardAttributes: StandardAttribute[] = [];
    selectedStandardAttribute: StandardAttribute;
    standardAttributeForm: FormGroup;
    headers: string[];
    loading: boolean;

    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder) { }

    ngOnInit() {
        this.getStandardAttributeNames();
        this.initStandardAttributeTable();
    }

    initStandardAttributeTable() {
        this.headers = [
            "Standard Attribute",
            "Template Attribute",
            "Actions"
        ]
    }
    
    getStandardAttributeNames() {
        this.standardAttributes.push(new StandardAttribute(1, "Document Title"));
        this.standardAttributes.push(new StandardAttribute(2, "StandardAttribute Number"));
        this.standardAttributes.push(new StandardAttribute(3, "StandardAttribute Name"));
        this.standardAttributes.push(new StandardAttribute(4, "StandardAttribute Address"));
        this.standardAttributes.push(new StandardAttribute(5, "Document Number"));
        this.standardAttributes.push(new StandardAttribute(6, "Document Revision"));
        this.standardAttributes.push(new StandardAttribute(7, "Document Date"));
        this.standardAttributes.push(new StandardAttribute(8, "Design Stage"));
        this.standardAttributes.push(new StandardAttribute(9, "Discipline"));
        this.standardAttributes.push(new StandardAttribute(10, "Size"));
        this.standardAttributes.push(new StandardAttribute(11, "Scale"));
        this.standardAttributes.push(new StandardAttribute(12, "Drafter"));
        this.standardAttributes.push(new StandardAttribute(13, "Checker"));
        this.standardAttributes.push(new StandardAttribute(14, "Approver"));
        this.standardAttributes.push(new StandardAttribute(15, "Zone"));
        this.standardAttributes.push(new StandardAttribute(16, "Security"));
        this.standardAttributes.push(new StandardAttribute(17, "Other"));
        this.standardAttributes.push(new StandardAttribute(101, "Drafting Company"));
    }

    getStandardAttributes(profileVersionId: number)
    {
        this.loading = true;
        this.api.get("api/standardAttributes/" + profileVersionId.toString(), profileVersionId)
            .pipe(take(1))
                .subscribe((result: StandardAttribute[]) => {
                    this.standardAttributes = [];
                    for (let i = 0; i < result.length; i++) {
                        let p = <StandardAttribute>result[i];
                        this.standardAttributes.push(p);
                };
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error)
                this.loading = false;
            });
    }
    
    addOrEditStandardAttribute(standardAttribute?: StandardAttribute)
    {
        if (standardAttribute == null)
        { this.selectedStandardAttribute = new StandardAttribute(); }
        else
        { this.selectedStandardAttribute = standardAttribute; }
        this.standardAttributeModal.show();
    }

    deleteStandardAttribute(standardAttribute?: StandardAttribute)
    {
        if (standardAttribute.StandardAttributeId > 0) //New entry
        { 
            this.api.delete(`api/standardAttribute/${standardAttribute.StandardAttributeId}`, standardAttribute.StandardAttributeId)
                .pipe(take(1))
                .subscribe(result => {
                    this.standardAttributeModal.hide();
                    this.getStandardAttributeNames();
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }
    }

    hideDialog()
    {
        this.standardAttributeModal.hide();
        this.selectedStandardAttribute = new StandardAttribute();
    }

    saveStandardAttribute()
    {
        if (this.selectedStandardAttribute.StandardAttributeId == 0) //New entry
        { 
            this.api.post("api/standardAttribute", this.selectedStandardAttribute)
                .pipe(take(1))
                .subscribe(result => {
                    this.standardAttributeModal.hide();
                    this.getStandardAttributeNames();
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }
        else    //updated entry
        { 
            this.api.put("api/standardAttribute/"+ this.selectedStandardAttribute.StandardAttributeId, this.selectedStandardAttribute)
            .pipe(take(1))
                .subscribe(result => {
                    this.standardAttributeModal.hide();
                    this.getStandardAttributeNames();
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }
    }
    goToTemplates(standardAttribute?: StandardAttribute)
    {
        this.router.navigate(['standardAttribute/profileversions', standardAttribute.StandardAttributeId])
    }
    goToStandardAttributes(standardAttribute?: StandardAttribute)
    {
        //":companyId/profileversions/:profileVersionId/standardAttributes",
        this.router.navigate(['standardAttribute/standardAttributes', standardAttribute.StandardAttributeId])
    }
}
