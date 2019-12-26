import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IndustryVariablesService } from '../../../shared/services/industry-variables.service';
import { IndustryVariables } from '../../../shared/models/industry-variables';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-collection',
  templateUrl: './order-collection.component.html',
  styleUrls: ['./order-collection.component.scss']
})
export class OrderCollectionComponent implements OnInit {

  collectionForm: FormGroup
  industryInfo: IndustryVariables;

  constructor(private fb: FormBuilder,
      private industryVars: IndustryVariablesService,
      private router: Router) { }

  ngOnInit() {
    this.initReactiveForm();
    this.industryInfo = this.industryVars.getIndustryInfo()
  }

  initReactiveForm() {
    this.collectionForm = this.fb.group({
      line1: this.fb.control('', Validators.required),
      line2: this.fb.control(''),
      city: this.fb.control('', Validators.required),
      county: this.fb.control('', Validators.required),
      postcode: this.fb.control('', Validators.required),
      time: this.fb.control('', Validators.required),
      date: this.fb.control('', Validators.required),
      name: this.fb.control('', Validators.required),
      role: this.fb.control('', Validators.required)
    })
  }

  submitForm() {
    console.log(this.collectionForm)
    this.router.navigate(['archive/collection-order-form'])
  }

}
