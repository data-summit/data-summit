import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderCollectionComponent } from './order-collection.component';

describe('OrderCollectionComponent', () => {
  let component: OrderCollectionComponent;
  let fixture: ComponentFixture<OrderCollectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderCollectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderCollectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
