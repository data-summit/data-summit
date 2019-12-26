import { Injectable } from "@angular/core";
import { IndustryType } from "../../enums/industry-type.enum";
import { IndustryVariables } from "../models/industry-variables";
import { FormGroup } from "@angular/forms";
import { RegisteredUser } from "src/app/account/models/registered-user";

@Injectable()
export class RegisterFormService {

    registeredUser: RegisteredUser

    _registerForm: FormGroup;
    _detailsForm: FormGroup

    get registerForm() {
        return this._registerForm;
    }
    set registerForm(value: FormGroup) {
        this._registerForm = value;
    }

    get detailsForm() {
        return this._detailsForm;
    }
    set detailsForm(value: FormGroup) {
        this._detailsForm = value;
    }

    getRegisterObject() {
        const obj = Object.assign(this.detailsForm.value, this.registerForm.value)
        return obj
    }
}