import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { DocumentProperty } from '../models/documentProperty';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'ds-properties',
    templateUrl: 'properties.component.html'
})

export class PropertiesComponent implements OnInit {

    @ViewChild('documentPropertyModal', { static: false }) documentPropertyModal;

    @Input() companyId: number;
    projectId: number;
    documentId: number;
    headers: string[];
    selectedDocumentProperty: DocumentProperty;
    documentPropertiesForm: FormGroup;
    loading: boolean;
    saveProfileAttribute: any;
    enableEdit = false;
    enableEditIndex = null;
    documentProperties: Array<DocumentProperty>

    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder,
        private route: ActivatedRoute) { }

    ngOnInit() {
        //Get companyId from URL
        this.companyId = this.route.snapshot.params['companyId']
        if (typeof this.companyId == 'string')         //Ensure id is number if received as a string
        { this.companyId = Number(this.companyId); }
        //Get documentId from URL
        this.documentId = this.route.snapshot.params['documentId']
        if (typeof this.documentId == 'string')         //Ensure id is number if received as a string
        { this.documentId = Number(this.documentId); }

        this.initPropertiesTable();
        this.initPropertiesForm();
        this.getDocumentProperties(this.documentId);
    }

    initPropertiesForm() {
        this.documentPropertiesForm = this.fb.group({
            Name: this.fb.control('', Validators.required),
            CreateDate: this.fb.control('')
        });
        this.selectedDocumentProperty = new DocumentProperty();
    }

    initPropertiesTable() {
        this.headers = [
            'Standard Name',
            'Name',
            'Value',
            'Confidence',
            'Actions'
        ]; }

    getDocumentProperties(id: number) {
        this.loading = true;        
        this.api.get(`api/properties/document/${id}`)
            .pipe(take(1))
                .subscribe((result: any[]) => {
                    this.documentProperties = [];

                    for (let i = 0; i < result.length; i++) {
                        let p = new DocumentProperty(result[i].sentenceId, 
                            result[i].standardName, 
                            result[i].name, 
                            result[i].wordValue, 
                            result[i].confidence);

                        this.documentProperties.push(p);
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
        this.documentPropertyModal.hide();
        this.selectedDocumentProperty = new DocumentProperty();
    }

    goToAttributes(documentId: number) {
        this.router.navigate(['companies/profileattributes', documentId]);
    }

    createProperties() {
        this.router.navigate(['documents', 'create', 'createprofileversion', this.companyId]);
    }

    addDocumentProperty(documentProperty?: DocumentProperty) {
        if (!documentProperty)
        { this.selectedDocumentProperty = new DocumentProperty() }
        else
        { this.selectedDocumentProperty = documentProperty; }
        this.documentPropertyModal.show();
    }

    editDocumentProperty(documentProperty?: DocumentProperty) {
        this.addDocumentProperty(documentProperty);
    }

    // deleteProperties(property?: Property) {
    //     if (property.PropertyId > 0) {
    //         this.api.delete('api/documents/' + property.PropertyId.toString(), property.PropertyId)
    //             .pipe(take(1))
    //             .subscribe(result => {
    //                 this.getProperties(this.companyId);
    //                 console.log(result);
    //             }, error => {
    //                 console.log(error);
    //             });
    //     }
    // }

    updateDocumentProperty(propertyId: string, propertyValue: string) {
        let documentProperty ={propertyId, propertyValue};
        console.log(documentProperty);
        
        if (documentProperty) {
            this.api.post('api/properties/update', documentProperty)
                .pipe(take(1))
                .subscribe(result => {
                    console.log(result);
                }, error => {
                    console.log(error);
                });
        }
    }

    saveDocumentProperty() 
    {
        //this.selectedDocumentProperty.CompanyId = this.companyId;
        if (!this.selectedDocumentProperty.Id) //New entry
        { 
            this.api.post("api/properties/create", this.selectedDocumentProperty)
                .pipe(take(1))
                .subscribe(result => {
                    this.documentPropertyModal.hide();
                    this.getDocumentProperties(this.documentId);
                    console.log(result);
                }, error => {
                    console.log(error);
                });
        }
        else //updated entry
        { 
            this.api.put("api/properties/update", this.selectedDocumentProperty)
            .pipe(take(1))
                .subscribe(result => {
                    this.documentPropertyModal.hide();
                    this.getDocumentProperties(this.documentId);
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }
    }
}
