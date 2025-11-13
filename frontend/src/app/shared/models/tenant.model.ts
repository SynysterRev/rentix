export interface TenantDTO {
    id: number,
    firstName: string,
    lastName: string,
    email: string,
    phoneNumber: string,
    fullName: string
}

export interface TenantCreateDTO {
    firstName: string,
    lastName: string,
    email: string,
    phone: string,
}
