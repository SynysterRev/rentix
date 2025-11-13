import { TenantCreateDTO } from "./tenant.model";

export interface LeaseCreateDTO {
    propertyId: number,
    tenants: TenantCreateDTO,
    rentWithoutCharges: number,
    rentCharges: number,
    leaseStartDate: string,
    leaseEndDate: string,
    deposit: number,
    note: string
    // document:
}
