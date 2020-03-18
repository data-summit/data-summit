import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { Property } from '../models/property';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DrawingProperty } from '../models/drawingProperty';
import { Drawing } from '../models/drawing';
import { ProfileVersion } from '../../profileVersion/models/profileVersion';
import { ProfileAttribute } from '../../profileAttributes/models/profileAttribute';
import { Sentence } from '../../drawings/models/sentence';
import { DrawingData } from '../models/drawingData';

@Component({
    selector: 'ds-properties',
    templateUrl: 'properties.component.html'
})

export class PropertiesComponent implements OnInit {

    @ViewChild('propertyModal', { static: false }) propertyModal;

    companyId: number;
    projectId: number;
    drawingId: number;
    properties: Property[];
    drawingProperties: DrawingData;
    selectedProperties: Property;
    headers: string[];
    drawingForm: FormGroup;
    loading: boolean;
    saveProfileAttribute: any;

    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        //Get companyId from URL
        this.companyId = this.route.snapshot.params['companyId']
        if (typeof this.companyId == 'string')         //Ensure id is number if received as a string
        { this.companyId = Number(this.companyId); }
        //Get projectId from URL
        this.projectId = this.route.snapshot.params['projectId']
        if (typeof this.projectId == 'string')         //Ensure id is number if received as a string
        { this.projectId = Number(this.projectId); }
        //Get drawingId from URL
        this.drawingId = this.route.snapshot.params['drawingId']
        if (typeof this.drawingId == 'string')         //Ensure id is number if received as a string
        { this.drawingId = Number(this.drawingId); }

        this.drawingProperties = new DrawingData();
        this.initPropertiesTable();
        this.initPropertiesForm();
        this.getProperties(this.drawingId);
    }

    initPropertiesForm() {
        this.drawingForm = this.fb.group({
            Name: this.fb.control('', Validators.required),
            CreateDate: this.fb.control('')
        });
        this.selectedProperties = new Property();
    }

    initPropertiesTable() {
        this.headers = [
            'Standard Name',
            'Name',
            'Name X',
            'Name Y',
            'Value',
            'Value X',
            'Value Y',
            'Actions'
        ]; }

    getProperties(id: number) {
        this.loading = true;
        this.api.get(`api/properties/${id}`)
            .pipe(take(1))
                .subscribe((result: DrawingData) => {
                    let d = <DrawingData>result;
                    this.drawingProperties = d;
                    console.log(location.origin.toString() + this.router.url.toString());
            }, error => {
                console.log("Error occurred");
                console.log(error);
            });
        this.loading = false;
    }

    goToAttributes(drawingId: number) {
        this.router.navigate(['companies/profileattributes', drawingId]);
    }

    createProperties() {
        this.router.navigate(['drawings', 'create', 'createprofileversion', this.companyId]);
    }

    addOrEditProperties(drawing?: Property) {
        if (drawing == null) { this.selectedProperties = new Property(); } else { this.selectedProperties = drawing; }
        this.selectedProperties.PropertyId = this.drawingId;
        this.propertyModal.show();
    }

    hideDialog() {
        this.propertyModal.hide();
        this.selectedProperties = new Property();
    }

    deleteProperties(property?: Property) {
        if (property.PropertyId > 0) {
            this.api.delete('api/drawings/' + property.PropertyId.toString(), property.PropertyId)
                .pipe(take(1))
                .subscribe(result => {
                    this.getProperties(this.companyId);
                    console.log(result);
                }, error => {
                    console.log(error);
                });
        }
    }
}
