import { TestBed, async } from '@angular/core/testing';
import { ArchiveComponent } from "./archive.component";

describe('Archive Component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                ArchiveComponent
            ]
        }).compileComponents();
    }));

    it('Should create the component', async(() => {
        const fixture = TestBed.createComponent(ArchiveComponent)
        const comp = fixture.debugElement.componentInstance;
        expect(comp).toBeTruthy;
    }))
    it('should have property title set to Archive Component', async(()=> {
        const fixture = TestBed.createComponent(ArchiveComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app.title).toEqual('Archive Component')
    }))
})