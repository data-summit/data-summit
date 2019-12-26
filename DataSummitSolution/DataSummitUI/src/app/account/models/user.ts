import { Address } from "./address";

export class User {
    
    id: number
    userId: number
    positionTitle: string
    firstName: string
    surname: string
    middleNames: string
    title: string
    photoPath: string
    photo: any
    companyId: number
    genderId: number
    isTrial: boolean
    createdDate: Date
    userTypeId: number

    address: Address[]
}