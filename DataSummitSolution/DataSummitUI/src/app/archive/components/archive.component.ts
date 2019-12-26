import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IndustryType } from '../../enums/industry-type.enum';

@Component({
    selector: 'ds-archive',
    templateUrl: 'archive.component.html'
})

export class ArchiveComponent implements OnInit {

    title: string = 'Archive Component'
    constructor(private router: Router) { }

    ngOnInit() { }

    setIndustry(ind: string) {
        let industry: IndustryType
        switch (ind) {
            case "construction":
                industry = IndustryType.construction
                break
            case "finance":
                industry = IndustryType.finance
                break
            case "energy":
                industry = IndustryType.energy
                break
            case "healthcare":
                industry = IndustryType.healthcare
                break
            case "pharmaceutical":
                industry = IndustryType.pharmaceutical
                break
            case "government":
                industry = IndustryType.government
                break
            case "legal":
                industry = IndustryType.legal
                break
            case "manufacturing":
                industry = IndustryType.manufacturing
                break
            case "IT":
                industry = IndustryType.IT
                break
            default:
                industry = IndustryType.unknown
                break
        }
        this.router.navigate([`/archive/details`, industry])
    }
}