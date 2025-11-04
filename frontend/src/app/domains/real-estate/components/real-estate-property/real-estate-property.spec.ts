import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RealEstateProperty } from './real-estate-property';

describe('RealEstateProperty', () => {
  let component: RealEstateProperty;
  let fixture: ComponentFixture<RealEstateProperty>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RealEstateProperty]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RealEstateProperty);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
