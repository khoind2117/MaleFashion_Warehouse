export interface Account {
  id: string;
  firstName: string;
  lastName: string;
  address: string;
  phoneNumber: string;
  email: string;
}

export enum UserStatus {
    ACTIVE = 'ACTIVE',
    INACTIVE = 'IN_ACTIVE',
}
