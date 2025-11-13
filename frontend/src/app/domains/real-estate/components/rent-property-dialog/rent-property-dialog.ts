import { Component, DestroyRef, Inject, inject } from '@angular/core';
import { PropertyService } from '../../services/property';
import { MatStepperModule, MatStepper, MatStep } from '@angular/material/stepper';
import { MatFormField, MatInputModule, MatLabel } from "@angular/material/input";
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, FormGroup, FormGroupDirective, FormsModule, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgTemplateOutlet, NgClass } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { LucideAngularModule, Pencil, Check, FileText, UserRound, Save, ArrowBigRight, ArrowBigLeft, X } from "lucide-angular";
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LeaseCreateDTO } from '../../../../shared/models/lease.model';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, ErrorStateMatcher, MatNativeDateModule, NativeDateAdapter, provideNativeDateAdapter, ShowOnDirtyErrorStateMatcher } from '@angular/material/core';
import { TenantCreateDTO } from '../../../../shared/models/tenant.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LeaseService } from '../../../../shared/services/lease';
import { MyErrorStateMatcher } from './my-error-state-matcher';

@Component({
  selector: 'app-rent-property-dialog',
  imports: [MatStepper,
    MatStep,
    MatFormFieldModule,
    MatFormField,
    MatLabel,
    MatIconModule,
    MatStepperModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    LucideAngularModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule],
  templateUrl: './rent-property-dialog.html',
  styleUrl: './rent-property-dialog.scss',
  providers: [
    { provide: STEPPER_GLOBAL_OPTIONS, useValue: { displayDefaultIndicatorType: false } },
    provideNativeDateAdapter(),
    {provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher}
  ]
})



export class RentPropertyDialog {
  private leaseService = inject(LeaseService);
  private destroyRef = inject(DestroyRef);

  readonly Pencil = Pencil;
  readonly Check = Check;
  readonly FileText = FileText;
  readonly UserRound = UserRound;
  readonly Save = Save;
  readonly ArrowBigRight = ArrowBigRight;
  readonly ArrowBigLeft = ArrowBigLeft;
  readonly X = X;

  matcher = new MyErrorStateMatcher();

  createLeaseForm = new FormGroup({
    rentWithoutCharges: new FormControl('', [Validators.required]),
    rentCharges: new FormControl('', [Validators.required]),
    leaseStartDate: new FormControl('', [Validators.required]),
    leaseEndDate: new FormControl('', [Validators.required]),
    deposit: new FormControl('', [Validators.required]),
    note: new FormControl(''),
  });

  createTenantForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(3)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(3)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    phone: new FormControl('', [Validators.required, Validators.maxLength(10)])
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<RentPropertyDialog>) { }

  closeDialog() {
    this.dialogRef.close();
  }

  createLease() {
    if (this.createTenantForm.valid && this.createLeaseForm.valid) {
      const tenant: TenantCreateDTO = {
        firstName: this.createTenantForm.value.firstName!,
        lastName: this.createTenantForm.value.lastName!,
        email: this.createTenantForm.value.email!,
        phone: this.createTenantForm.value.phone!,
      };

      const lease: LeaseCreateDTO = {
        propertyId: this.data.propertyId!,
        tenants: tenant,
        rentWithoutCharges: Number(this.createLeaseForm.value.rentWithoutCharges!),
        rentCharges: Number(this.createLeaseForm.value.rentCharges!),
        leaseStartDate: this.createLeaseForm.value.leaseStartDate!,
        leaseEndDate: this.createLeaseForm.value.leaseEndDate!,
        deposit: Number(this.createLeaseForm.value.deposit!),
        note: this.createLeaseForm.value.note!
      }

      this.leaseService.createLease(lease)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe(response => {
          this.dialogRef.close(response);
        });

    } else {
      console.log('Form is invalid :  ');
    }
  }
}
