import { Component, OnInit, EventEmitter, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, NgControl } from '@angular/forms';
import { PasswordMatchValidation } from "../../validators/password-match.validator";
import { HttpClient } from 'selenium-webdriver/http';
import { Router } from '@angular/router';
import { TelephoneCodeService } from '../../shared/datasets/telephone-codes';
import { TelephoneCode } from 'src/app/shared/models/telephone-codes';
import { EMAIL_VALIDATOR, EmailValidator } from '@angular/forms/src/directives/validators';
import { RegisterFormService } from 'src/app/shared/services/register-form.service';
import { CarouselComponent } from 'angular-bootstrap-md';
import { BehaviorSubject, Subscription } from 'rxjs';

@Component({
  selector: 'ds-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, AfterViewInit, OnDestroy {

  roleType: string = 'Archivist';
  applicationForm: FormGroup;
  allCodes: TelephoneCode[];

  controls: boolean = false;
  interval: number = 0
  muted: boolean = true;
  firstPlay: boolean = true;

  canPlay = new BehaviorSubject<boolean>(false);
  sub: Subscription;

  @ViewChild('carouselEl') carouselEl: CarouselComponent
  @ViewChild('videoEl') videoEl: any

  constructor(private fb: FormBuilder,
      private router: Router,
      private telCode: TelephoneCodeService,
      private registerFormService: RegisterFormService) { }

  ngOnInit() {
    this.initForm();
    this.allCodes = this.telCode.getPhoneCodes();
    this.allCodes.sort();
  }

  //For auto playing the video
  ngAfterViewInit() {
    this.sub = this.canPlay.subscribe(canPlay=> {
      if(canPlay) {
        this.videoEl.nativeElement.play()
      }
    })
  }


  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  setCanPlayObs() {
    this.canPlay.next(true);
  }

  initForm() {
    this.applicationForm = this.fb.group({
      firstName: this.fb.control('', Validators.required),
      lastName: this.fb.control('', Validators.required),
      email: this.fb.control('', Validators.required),
      telCode: this.fb.control(''),
      phone: this.fb.control(''),
      password: this.fb.control('', Validators.required),
      confirmPassword: this.fb.control('', Validators.required),
      // city: this.fb.control('', Validators.required)
    })
    this.applicationForm.setValidators(PasswordMatchValidation.MatchPassword)
    let savedForm = this.registerFormService.registerForm;
      if (savedForm) {
        this.applicationForm.patchValue(savedForm.value)
      }
  }

  submitForm() {
    this.registerFormService.registerForm = this.applicationForm
    this.router.navigate(['/archive'])
  }

  clearForm() {
    this.applicationForm.reset()
  }

  setCarouselOpts(showControls: boolean, speed: number) {
    this.controls = showControls;
    this.interval = speed;
  }

  onVideoPaused() {
    this.setCarouselOpts(true, 5000);
  }

  onVideoPlayed() {
    this.setCarouselOpts(false, 0);
  }

  onVideoEnd() {
    this.setCarouselOpts(true, 5000);
    this.carouselEl.nextSlide();
  }

  unMuteFirstPlay() {
    if(this.firstPlay) {
      this.muted = false;
      this.firstPlay = false;
    }
  }

}
