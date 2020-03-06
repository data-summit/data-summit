import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ApiService } from '../../../shared/services/api.service';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
    selector: 'ds-twofactor',
    templateUrl: 'twofactor.component.html'
})

export class TwoFactorComponent implements OnInit {

    qrCodeString: string;
    authCode: string;
    manualCode: string;

    show = false;

    twoFactorAuthForm: FormGroup;

    constructor(private apiService: ApiService,
        private fb: FormBuilder,
        private http: HttpClient,
        private authService: AuthService) {
    }

    ngOnInit() {
    }

    setQrCode = (qrCodePath) => {
      this.qrCodeString = qrCodePath && qrCodePath[1];
      this.manualCode = qrCodePath && qrCodePath[0];

      this.show = true;
    }

    getQrCodes() {
      const secret = 'DataSummitUISecret';
      const email = sessionStorage.getItem('userEmail');

      const promise = this.authService.getTwoFactorCodes(email, secret, this.setQrCode);
    }

    signIn() {
      const secret = 'DataSummitUISecret'; // TODO get secret from elsewhere
      this.authService.validateUserCode(this.authCode, secret);
    }
}
