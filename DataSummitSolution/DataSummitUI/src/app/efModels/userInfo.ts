import { UserInfoType } from "./userInfoType";

export class UserInfo {

	UserInfoId: number;
	Value: string;
	CreatedDate?: Date | string;
	Id: string;
	UserInfoTypeId?: number;
	IdNavigation: number;
	UserInfoType: UserInfoType;

	constructor(userInfoId?: number, value?: string, createdDate?: Date | string,
		id?: string, userInfoTypeId?: number, idNavigation?: number,
		userInfoType?: UserInfoType)
	{
		this.UserInfoId = userInfoId || 0;
		this.Value = value || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.Id = id || "";
		this.UserInfoTypeId = userInfoTypeId || 0 || null;
		this.IdNavigation = idNavigation || 0;
		this.UserInfoType = userInfoType || null;
	}
}
