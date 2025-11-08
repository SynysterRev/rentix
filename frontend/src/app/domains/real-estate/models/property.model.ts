import { AddressDTO } from "../../../shared/models/address.model"
import { TenantDTO } from "../../../shared/models/tenant.model"

export interface PropertyDTO {
    id: number,
    name: string,
    totalRent: number,
    tenants: TenantDTO[],
    propertyStatus: number,
    address: AddressDTO,
    isAvailable: boolean
}
