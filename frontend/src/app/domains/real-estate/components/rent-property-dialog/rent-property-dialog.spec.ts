import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RentPropertyDialog } from './rent-property-dialog';

describe('RentPropertyDialog', () => {
  let component: RentPropertyDialog;
  let fixture: ComponentFixture<RentPropertyDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RentPropertyDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RentPropertyDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
