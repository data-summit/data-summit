import { PageSize } from "src/app/enums/pageSize.enum";
import { ContainerType } from "src/app/enums/containerType.enum";
import { Urgency } from "src/app/enums/urgency.enum";

export class Order {
    
    count: number;
    pageSize: PageSize;
    containerType: ContainerType;
    urgency: Urgency

    constructor(count: number, pageSize: PageSize, containerType: ContainerType, urgency?: Urgency) {
        this.count = count;
        this.pageSize = pageSize;
        this.containerType = containerType;
        this.urgency = urgency || Urgency.oneWeek
    }
}