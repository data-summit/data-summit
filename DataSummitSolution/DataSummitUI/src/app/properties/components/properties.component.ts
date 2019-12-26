import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { Property } from '../models/property';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'ds-properties',
    templateUrl: 'properties.component.html'
})

export class PropertiesComponent implements OnInit {
    
    @ViewChild('propertyModal') propertyModal

    @Input() companyId: number;
    drawingId: number;
    properties: Property[];
    headers: string[];
    selectedProperties: Property;
    drawingForm: FormGroup
    loading: boolean;

    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        //CompanyId
        this.companyId = this.route.snapshot.params['companyId']
        if (typeof this.companyId === 'string')         //Ensure companyId is number if received as a string
        { this.companyId = Number(this.companyId);}
        this.getPropertiess(this.companyId);
        this.initPropertiesTable();
        this.initPropertiessForm();
    }

    initPropertiessForm() {
        this.drawingForm = this.fb.group({
            Name: this.fb.control('', Validators.required),
            CreateDate: this.fb.control('')
        });
        this.selectedProperties = new Property();
    }

    initPropertiesTable() {
        this.headers = [
            "Name",
            "Profile Attributes",
            "Height",
            "Width",
            "Created Date",
            "Actions"
        ]}

    getPropertiess(id: number) {
        this.loading = true;
        this.api.get("api/profileversions/" + id.toString(), id)
            .pipe(take(1))
                .subscribe((result: Property[]) => {
                    this.properties = [];
                    for (let i = 0; i < result.length; i++) {
                        let p = <Property>result[i];
                        this.properties.push(p);
                };
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error)
                this.loading = false;
            });
        }

    goToAttributes(drawingId: number) {
        this.router.navigate(["companies/profileattributes", drawingId]);
    }
        
    createProperties() {
        this.router.navigate(['drawings', 'create', 'createprofileversion', this.companyId])
    }

    addOrEditProperties(drawing?: Property)
    {
        if (drawing == null)
        { this.selectedProperties = new Property(); }
        else
        { this.selectedProperties = drawing; }
        this.selectedProperties.PropertyId = this.drawingId;
        this.propertyModal.show();
    }

    hideDialog()
    {
        this.propertyModal.hide();
        this.selectedProperties = new Property();
    }

    deleteProperties(property?: Property)
    {
        if (property.PropertyId > 0) //New entry
        { 
            this.api.delete("api/drawings/" + property.PropertyId.toString(), property.PropertyId)
                .pipe(take(1))
                .subscribe(result => {
                    this.getPropertiess(this.companyId);
                    console.log(result);
                }, error => {
                    console.log(error);
                })
        }
    }
}