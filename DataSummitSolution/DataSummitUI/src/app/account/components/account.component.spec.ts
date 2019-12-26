import { TestBed, async } from '@angular/core/testing';
import { AccountComponent } from "./account.component";

describe('Account Component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AccountComponent
            ]
        }).compileComponents();
    }));

    it('Should create the component', async(() => {
        const fixture = TestBed.createComponent(AccountComponent)
        const comp = fixture.debugElement.componentInstance;
        expect(comp).toBeTruthy;
    }))
    it('should have property title set to Account Component', async(()=> {
        const fixture = TestBed.createComponent(AccountComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app.title).toEqual('Account Component')
    }))
})