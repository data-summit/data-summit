import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { DrawingProperty } from '../models/drawingProperty';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'ds-properties',
    templateUrl: 'properties.component.html'
})

export class PropertiesComponent implements OnInit {

    @ViewChild('drawingPropertyModal', { static: false }) drawingPropertyModal;

    @Input() companyId: number;
    projectId: number;
    drawingId: number;
    headers: string[];
    selectedDrawingProperty: DrawingProperty;
    drawingPropertiesForm: FormGroup;
    loading: boolean;
    saveProfileAttribute: any;
    enableEdit = false;
    enableEditIndex = null;
    drawingProperties: Array<DrawingProperty>

    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        //Get companyId from URL
        this.companyId = this.route.snapshot.params['companyId']
        if (typeof this.companyId == 'string')         //Ensure id is number if received as a string
        { this.companyId = Number(this.companyId); }
        //Get drawingId from URL
        this.drawingId = this.route.snapshot.params['drawingId']
        if (typeof this.drawingId == 'string')         //Ensure id is number if received as a string
        { this.drawingId = Number(this.drawingId); }

        this.initPropertiesTable();
        this.initPropertiesForm();
        this.getDrawingProperties(this.drawingId);
    }

    initPropertiesForm() {
        this.drawingPropertiesForm = this.fb.group({
            Name: this.fb.control('', Validators.required),
            CreateDate: this.fb.control('')
        });
        this.selectedDrawingProperty = new DrawingProperty();
    }

    initPropertiesTable() {
        this.headers = [
            'Standard Name',
            'Name',
            'Value',
            'Confidence',
            'Actions'
        ]; }

    getDrawingProperties(id: number) {
        this.loading = true;        
        this.api.get(`api/properties/drawing/${id}`)
            .pipe(take(1))
                .subscribe((result: any[]) => {
                    this.drawingProperties = [];

                    for (let i = 0; i < result.length; i++) {
                        let p = new DrawingProperty(result[i].sentenceId, 
                            result[i].standardName, 
                            result[i].name, 
                            result[i].wordValue, 
                            result[i].confidence);

                        this.drawingProperties.push(p);
                };
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error);
                this.loading = false;
            })
    }

    hideDialog()
    {
        this.drawingPropertyModal.hide();
        this.selectedDrawingProperty = new DrawingProperty();
    }

    goToAttributes(drawingId: number) {
        this.router.navigate(['companies/profileattributes', drawingId]);
    }

    createProperties() {
        this.router.navigate(['drawings', 'create', 'createprofileversion', this.companyId]);
    }

    addDrawingProperty(drawingProperty?: DrawingProperty) {
        if (!drawingProperty)
        { this.selectedDrawingProperty = new DrawingProperty() }
        else
        { this.selectedDrawingProperty = drawingProperty; }
        this.drawingPropertyModal.show();
    }

    editDrawingProperty(drawingProperty?: DrawingProperty) {
        this.addDrawingProperty(drawingProperty);
    }

    // deleteProperties(property?: Property) {
    //     if (property.PropertyId > 0) {
    //         this.api.delete('api/drawings/' + property.PropertyId.toString(), property.PropertyId)
    //             .pipe(take(1))
    //             .subscribe(result => {
    //                 this.getProperties(this.companyId);
    //                 console.log(result);
    //             }, error => {
    //                 console.log(error);
    //             });
    //     }
    // }

    updateDrawingProperty(propertyId: string, propertyValue: string) {
        let drawingProperty ={propertyId, propertyValue};
        console.log(drawingProperty);
        
        if (drawingProperty) {
            this.api.post('api/properties/update', drawingProperty)
                .pipe(take(1))
                .subscribe(result => {
                    console.log(result);
                }, error => {
                    console.log(error);
                });
        }
    }

    saveDrawingProperty() 
    {
        //this.selectedDrawingProperty.CompanyId = this.companyId;
        if (!this.selectedDrawingProperty.Id) //New entry
        { 
            this.api.post("api/properties/create", this.selectedDrawingProperty)
                .pipe(take(1))
                .subscribe(result => {
                    this.drawingPropertyModal.hide();
                    this.getDrawingProperties(this.drawingId);
                    console.log(result);
                }, error => {
                    console.log(error);
                });
        }
        else //updated entry
        { 
            this.api.put("api/properties/update", this.selectedDrawingProperty)
            .pipe(take(1))
                .subscribe(result => {
                    this.drawingPropertyModal.hide();
                    this.getDrawingProperties(this.drawingId);
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }
    }
}
