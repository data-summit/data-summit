import { UserInfo } from "./userInfo";

export class UserInfoType {

	UserInfoTypeId: number;
	Type: string;
	CreatedDate?: Date | string;
	UserId?: number;
	UserInfo: UserInfo[];

	constructor(userInfoTypeId?: number, type?: string, createdDate?: Date | string,
		userId?: number, userInfo?: UserInfo[])
	{
		this.UserInfoTypeId = userInfoTypeId || 0;
		this.Type = type || "";
		this.CreatedDate = createdDate || new Date(Date.now()) || null;
		this.UserId = userId || 0 || null;
		this.UserInfo = userInfo;
	}
}
