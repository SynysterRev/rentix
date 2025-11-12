import { AddressDTO } from "../../../shared/models/address.model"
import { TenantDTO } from "../../../shared/models/tenant.model"

export interface PropertyDTO {
    id: number,
    name: string,
    totalRent: number,
    tenantsNames: string[],
    propertyStatus: number,
    address: AddressDTO,
    isAvailable: boolean
}

export interface PropertyDetailsDTO {
    id: number,
    name: string,
    maxRent: number,
    leaseStartDate: Date,
    leaseEndDate: Date,
    tenants: TenantDTO[],
    propertyStatus: number,
    address: AddressDTO,
    isAvailable: boolean
}