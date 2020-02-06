import { Component, OnInit, ViewChild } from '@angular/core';
import { Company } from '../models/company';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/shared/services/api.service';
import { take } from 'rxjs/operators';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'ds-companies',
    templateUrl: 'companies.component.html'
})
export class CompaniesComponent implements OnInit {

    @ViewChild('companyModal', { static: false }) companyModal

    companies: Company[];
    selectedCompany: Company;
    companyForm: FormGroup;
    headers: string[];
    loading: boolean;
    // TODO fix this prperty
    company: any;

    constructor(private router: Router,
        private api: ApiService,
        private fb: FormBuilder) { }

    ngOnInit() {
        this.getCompanies();
        this.initCompaniesTable();
    }

    initCompaniesTable() {
        this.headers = [
            "Name",
            "Templates",
            "Projects",
            "Company Number",
            "Actions"
        ]
    }

    getCompanies() {
        this.loading = true;
        this.api.get("api/companies")
        .pipe(take(1))
            .subscribe((result: Company[]) => {
                this.companies = [];
                for (let i = 0; i < result.length; i++) {
                    let c = <Company>result[i];
                    this.companies.push(c);
                }
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error)
                this.loading = false;
            })
    }

    addOrEditCompany(company?: Company)
    {
        if (company == null)
        { this.selectedCompany = new Company(); }
        else
        { this.selectedCompany = company; }
        this.companyModal.show();
    }

    deleteCompany(company?: Company)
    {
        if (company.CompanyId > 0) //New entry
        {
            this.api.delete(`api/companies/${company.CompanyId}`, company.CompanyId)
                .pipe(take(1))
                .subscribe(result => {
                    this.companyModal.hide();
                    this.getCompanies();
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }
    }

    hideDialog()
    {
        this.companyModal.hide();
        this.selectedCompany = new Company();
    }

    saveCompany()
    {
        if (this.selectedCompany.CompanyId == 0) //New entry
        {
            this.api.post("api/companies", this.selectedCompany)
                .pipe(take(1))
                .subscribe(result => {
                    this.companyModal.hide();
                    this.getCompanies();
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }
        else    //updated entry
        {
            this.api.put("api/companies/"+ this.selectedCompany.CompanyId, this.selectedCompany)
            .pipe(take(1))
                .subscribe(result => {
                    this.companyModal.hide();
                    this.getCompanies();
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }
    }
    goToTemplates(company?: Company)
    {
        this.router.navigate([`companies/${company.CompanyId}/profileversions`])
    }
    goToProjects(company?: Company)
    {
        this.router.navigate([`companies//${company.CompanyId}/projects`])
    }
}
