import { AddressCreateDTO, AddressDTO } from "../../../shared/models/address.model"
import { PropertyStatus } from "../../../shared/models/property-status.model"
import { TenantDTO } from "../../../shared/models/tenant.model"

export interface PropertyDTO {
    id: number,
    name: string,
    totalRent: number,
    tenantsNames: string[],
    propertyStatus: PropertyStatus,
    address: AddressDTO,
    isAvailable: boolean
}

export interface PropertyDetailsDTO {
    id: number,
    name: string,
    maxRent: number,
    rentWithoutCharges: number,
    rentCharges: number,
    leaseStartDate: Date,
    leaseEndDate: Date,
    tenants: TenantDTO[],
    propertyStatus: PropertyStatus,
    address: AddressDTO,
    isAvailable: boolean
}

export interface PropertyCreateDTO {
    name: string,
    maxRent: number,
    rentNoCharges: number,
    rentCharges: number,
    deposit: number,
    propertyStatus: PropertyStatus,
    surface: number,
    numberRooms: number,
    addressId: number | null,
    addressDto: AddressCreateDTO,
    landlordId: string
}