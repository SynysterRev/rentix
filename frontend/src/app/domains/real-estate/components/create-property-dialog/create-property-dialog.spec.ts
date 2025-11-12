import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePropertyDialog } from './create-property-dialog';

describe('CreatePropertyDialog', () => {
  let component: CreatePropertyDialog;
  let fixture: ComponentFixture<CreatePropertyDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreatePropertyDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePropertyDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
