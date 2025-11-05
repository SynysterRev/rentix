import { AddressDTO } from "../../../shared/models/address.model"

export interface PropertyDTO {
    id: number,
    name: string,
    totalRent: number,
    tenantsNames: string[],
    propertyStatus: number,
    address: AddressDTO,
    isAvailable: boolean
}
