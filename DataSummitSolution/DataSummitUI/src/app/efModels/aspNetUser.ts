import { TimeSpan } from "../shared/models/timespan";
import { Company } from "../companies/models/company";
import { Gender } from "./gender";
import { UserType } from "./userType";
import { AspNetUserClaim } from "./aspNetUserClaim";
import { AspNetUserLogin } from "./aspNetUserLogin";
import { AspNetUserRole } from "./aspNetUserRole";
import { UserInfo } from "./userInfo";

export class AspNetUser {

	Id: string;
	AccessFailedCount: number;
	ConcurrencyStamp: string;
	Email: string;
	EmailConfirmed: boolean;
	LockoutEnabled: boolean;
	LockoutEnd?: TimeSpan;
	NormalizedEmail: string;
	NormalizedUserName: string;
	PasswordHash: string;
	PhoneNumber: string;
	PhoneNumberConfirmed: boolean;
	SecurityStamp: string;
	TwoFactorEnabled: boolean;
	UserName: string;
	IsTrial?: boolean;
	Title: string;
	FirstName: string;
	MiddleNames: string;
	Surname: string;
	DateOfBirth?: Date | string;
	PhotoPath: string;
	Photo: string;
	GenderId?: number;
	CreatedDate?: Date | string;
	PositionTitle: string;
	UserTypeId?: number;
	CompanyId?: number;
	Company: Company;
	Gender: Gender;
	UserType: UserType;
	AspNetUserClaims: AspNetUserClaim[];
	AspNetUserLogins: AspNetUserLogin[];
	AspNetUserRoles: AspNetUserRole[];
	UserInfo: UserInfo[];

	constructor(id?: string, accessFailedCount?: number, concurrencyStamp?: string,
		email?: string, emailConfirmed?: boolean, lockoutEnabled?: boolean,
		lockoutEnd?: TimeSpan, normalizedEmail?: string, normalizedUserName?: string,
		passwordHash?: string, phoneNumber?: string, phoneNumberConfirmed?: boolean,
		securityStamp?: string, twoFactorEnabled?: boolean, userName?: string,
		isTrial?: boolean, title?: string, firstName?: string,
		middleNames?: string, surname?: string, dateOfBirth?: Date | string,
		photoPath?: string, photo?: string, genderId?: number,
		createdDate?: Date | string, positionTitle?: string, userTypeId?: number,
		companyId?: number, company?: Company, gender?: Gender,
		userType?: UserType, aspNetUserClaims?: AspNetUserClaim[], aspNetUserLogins?: AspNetUserLogin[],
		aspNetUserRoles?: AspNetUserRole[], userInfo?: UserInfo[])
	{
		this.Id = id || "";
		this.AccessFailedCount = accessFailedCount || 0;
		this.ConcurrencyStamp = concurrencyStamp || "";
		this.Email = email || "";
		this.EmailConfirmed = emailConfirmed || false;
		this.LockoutEnabled = lockoutEnabled || false;
		this.LockoutEnd = lockoutEnd || null;
		this.NormalizedEmail = normalizedEmail || "";
		this.NormalizedUserName = normalizedUserName || "";
		this.PasswordHash = passwordHash || "";
		this.PhoneNumber = phoneNumber || "";
		this.PhoneNumberConfirmed = phoneNumberConfirmed || false;
		this.SecurityStamp = securityStamp || "";
		this.TwoFactorEnabled = twoFactorEnabled || false;
		this.UserName = userName || "";
		this.IsTrial = isTrial || false;
		this.Title = title || "";
		this.FirstName = firstName || "";
		this.MiddleNames = middleNames || "";
		this.Surname = surname || "";
		this.DateOfBirth = dateOfBirth || new Date(Date.now()) || null;
		this.PhotoPath = photoPath || "";
		this.Photo = photo;
		this.GenderId = genderId || 0 || null;
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.PositionTitle = positionTitle || "";
		this.UserTypeId = userTypeId || 0 || null;
		this.CompanyId = companyId || 0 || null;
		this.Company = company;
		this.Gender = gender;
		this.UserType = userType;
		this.AspNetUserClaims = aspNetUserClaims;
		this.AspNetUserLogins = aspNetUserLogins;
		this.AspNetUserRoles = aspNetUserRoles;
		this.UserInfo = userInfo;
	}
}
