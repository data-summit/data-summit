import { Component, OnInit, ViewChild } from '@angular/core';
import { CompanyTableRow } from '../models/companyTableRow';
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

    companies: CompanyTableRow[];
    selectedCompany: CompanyTableRow;
    companyForm: FormGroup;
    headers: string[];
    loading: boolean;

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
            "Company Templates",
            "Projects",
            "Company Number",
            "Actions"
        ]
    }

    getCompanies() {
        this.loading = true;
        this.api.get("api/companies")
        .pipe(take(1))
            .subscribe((result: CompanyTableRow[]) => {
                this.companies = [];
                for (let i = 0; i < result.length; i++) {
                    let c = <CompanyTableRow>result[i];
                    this.companies.push(c);
                }
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error)
                this.loading = false;
            })
    }

    addCompany(company?: CompanyTableRow)
    {
        if (!company)
        { this.selectedCompany = new CompanyTableRow(); }
        else
        { this.selectedCompany = company; }
        this.companyModal.show();
    }

    editCompany(company?: CompanyTableRow)
    {
        this.addCompany(company);
    }

    deleteCompany(company?: CompanyTableRow)
    {
        if (company.CompanyId > 0)
        {
            this.api.delete(`api/companies/delete`, company.CompanyId)
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
        this.selectedCompany = new CompanyTableRow();
    }

    saveCompany()
    {
        if (this.selectedCompany.CompanyId === 0) //New entry
        {
            this.api.post("api/companies/create", this.selectedCompany)
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
            this.api.put("api/companies/update", this.selectedCompany)
            .pipe(take(1))
                .subscribe(result => {
                    this.companyModal.hide();
                    this.getCompanies();
                    console.log(result)
                }, error => {
                    console.log(error)
                })
        }

        this.getCompanies();
    }

    goToTemplates(company?: CompanyTableRow)
    {
        this.router.navigate(['companies', company.CompanyId, 'profileversions']);
    }

    goToProjects(company?: CompanyTableRow)
    {
        this.router.navigate(['companies', company.CompanyId, 'projects']);
    }
}
