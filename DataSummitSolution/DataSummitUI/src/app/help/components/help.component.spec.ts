import { TestBed, async } from '@angular/core/testing';
import { HelpComponent } from "./help.component";

describe('Help Component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                HelpComponent
            ]
        }).compileComponents();
    }));

    it('Should create the component', async(() => {
        const fixture = TestBed.createComponent(HelpComponent)
        const comp = fixture.debugElement.componentInstance;
        expect(comp).toBeTruthy;
    }))
    it('should have property title set to Help Component', async(()=> {
        const fixture = TestBed.createComponent(HelpComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app.title).toEqual('Help Component')
    }))
})