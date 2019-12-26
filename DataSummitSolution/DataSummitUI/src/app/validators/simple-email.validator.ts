import { AbstractControl, ValidatorFn } from '@angular/forms';

export function SimpleEmailValidator() : ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
        const emailValue: string = control.value;
        const reg = new RegExp(/^\S+@\S+\.\S+$/)

        if (reg.test(emailValue) || (emailValue == "" || !emailValue)) {
            return null;
        } else {
            return { simpleEmail: true }
        }
    }
}
