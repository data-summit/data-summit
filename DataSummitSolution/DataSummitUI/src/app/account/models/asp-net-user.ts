export class AspNetUser {

    id: string;
    accessFailedCount: number;
    concurrencyStamp: string;
    email: string;
    emailConfirmed: boolean;
    lockoutEnabled: boolean;
    lockoutEnd: Date;
    normalizedEmail: string;
    normalizedUserName: string;
    passwordHash: string;
    phoneNumber: string;
    phoneNumberConfirmed: boolean;
    securityStamp: string;
    twoFactorEnabled: boolean;
    userName: string;
}