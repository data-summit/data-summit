import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { RouterTestingModule } from '@angular/router/testing'
import { HeaderComponent } from './layout/components/header.component';
import { FooterComponent } from './layout/components/footer.component';
import { Component } from '@angular/core';
describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        HeaderComponent,
        FooterComponent,
        MockHomeComponent
      ],
      imports: [
        RouterTestingModule.withRoutes([
          { path: "", redirectTo: "/home", pathMatch: "full" },
          { path: "home", component: MockHomeComponent },
          { path: "scan", loadChildren: "./scan/scan.module#ScanModule" },
          { path: "archive", loadChildren: "./archive/archive.module#ArchiveModule" },
          { path: "account", loadChildren: "./account/account.module#AccountModule" },
          { path: "help", loadChildren: "./help/help.module#HelpModule" },
      ])
      ]
    }).compileComponents();
  }));
  it('should create the app', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
  it(`should have as title 'app'`, async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('Get there');
  }));
  it('should render the app-header tag', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('app-header').toBeTruthy);
  }));
  it('should render the app-footer tag', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('app-footer').toBeTruthy);
  }));
  it('should render the router-outlet', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('router-outlet').toBeTruthy);
  }));
});

@Component({
  selector: 'home',
  template: ''
})
class MockHomeComponent {
}