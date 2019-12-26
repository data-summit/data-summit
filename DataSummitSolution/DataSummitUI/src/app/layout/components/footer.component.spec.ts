import { TestBed, async } from '@angular/core/testing';
import { FooterComponent } from './footer.component';
describe('FooterComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        FooterComponent
      ]
    }).compileComponents();
  }));
  
  it('should create the app', async(() => {
    const fixture = TestBed.createComponent(FooterComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
  it('should be fixed to bottom', async(() => {
      const fixture = TestBed.createComponent(FooterComponent);
      fixture.detectChanges();
      const compiled: Element = fixture.debugElement.nativeElement
      expect(compiled.querySelector('div').classList.contains('fixed-bottom')).toBeTruthy;
  }))
});