import { TenantCreateDTO } from "./tenant.model";

export interface LeaseDTO {

}

export interface LeaseCreateDTO {
    tenants: TenantCreateDTO[],
    rentAmount: number,
    chargesAmount: number,
    startDate: string,
    endDate: string,
    Deposit: number,
    notes: string,
    isActive: boolean,
    leaseDocument: File
}
