import { Component, OnInit } from '@angular/core';
import { Order } from '../../models/order';
import { PageSize } from 'src/app/enums/pageSize.enum';
import { Urgency } from 'src/app/enums/urgency.enum';
import { ContainerType } from 'src/app/enums/containerType.enum';
import { ApiService } from 'src/app/shared/services/api.service';
import { take } from 'rxjs/operators';

@Component({
    selector: 'ds-collection-order-form',
    templateUrl: 'collection-order-form.component.html'
})
export class CollectionOrderFormComponent implements OnInit {

    orders: Order[] = [];
    headers: string[]

    limitationMethod: number = 1
    limitationQuantity: number = 0
    limitationFrequency: number = 3

    constructor(private api: ApiService) { }

    ngOnInit() {
        this.headers = [
            "No. Documents",
            "Page Size",
            "Container Type",
            "Urgency"
        ]
        this.orders.push(new Order(0, PageSize.a3, ContainerType.box, Urgency.threeDays))
    }

    addLine() {
        let newOrder: Order = new Order(0, PageSize.unknown, ContainerType.unknown, Urgency.unknown)
        this.orders.push(newOrder)
    }

    order() {
        //Do something
    }

    pageSizeList: { label: string, value: any }[] = [
        { label: "-- please select --", value: PageSize.unknown },
        { label: "A4", value: PageSize.a4 },
        { label: "A3", value: PageSize.a3 },
        { label: "A2", value: PageSize.a2 },
        { label: "A1", value: PageSize.a1 },
        { label: "A0", value: PageSize.a0 },
    ]

    urgencyList: { label: string, value: any }[] = [
        { label: "-- please select --", value: Urgency.unknown },
        { label: "Next day", value: Urgency.asap },
        { label: "3 business days", value: Urgency.threeDays },
        { label: "1 Week", value: Urgency.oneWeek },
        { label: "2 Weeks", value: Urgency.twoWeeks },
        { label: "One month", value: Urgency.oneMonth }
    ]

    containerList: { label: string, value: any }[] = [
        { label: "-- please select --", value: ContainerType.unknown },
        { label: "Folder", value: ContainerType.folder },
        { label: "Box", value: ContainerType.box },
        { label: "Crate", value: ContainerType.crate }
    ]

}