import { Component, DestroyRef, Inject, inject } from '@angular/core';
import { PropertyService } from '../../services/property';
import { MatStepperModule, MatStepper, MatStep } from '@angular/material/stepper';
import { MatFormField, MatInputModule, MatLabel } from "@angular/material/input";
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, FormGroup, FormGroupDirective, FormsModule, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgTemplateOutlet, NgClass } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { LucideAngularModule, Pencil, Check, FileText, UserRound, Save, ArrowBigRight, ArrowBigLeft, X, Folder } from "lucide-angular";
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LeaseCreateDTO } from '../../../../shared/models/lease.model';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, ErrorStateMatcher, MatNativeDateModule, NativeDateAdapter, provideNativeDateAdapter, ShowOnDirtyErrorStateMatcher } from '@angular/material/core';
import { TenantCreateDTO } from '../../../../shared/models/tenant.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LeaseService } from '../../../../shared/services/lease';
import { MyErrorStateMatcher } from '../../../../shared/utils/my-error-state-matcher';
import { FileUploadService } from '../../../../shared/services/file-upload';

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
    { provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher }
  ]
})

export class RentPropertyDialog {
  private leaseService = inject(LeaseService);
  private fileUploadService = inject(FileUploadService);
  private destroyRef = inject(DestroyRef);

  readonly Pencil = Pencil;
  readonly Check = Check;
  readonly FileText = FileText;
  readonly UserRound = UserRound;
  readonly Save = Save;
  readonly ArrowBigRight = ArrowBigRight;
  readonly ArrowBigLeft = ArrowBigLeft;
  readonly X = X;
  readonly Folder = Folder;

  matcher = new MyErrorStateMatcher();
  selectedFile!: File | null;

  createLeaseForm = new FormGroup({
    rentAmount: new FormControl('900', [Validators.required]),
    chargesAmount: new FormControl('100', [Validators.required]),
    startDate: new FormControl('01/11/2025', [Validators.required]),
    endDate: new FormControl('01/11/2026', [Validators.required]),
    deposit: new FormControl('100', [Validators.required]),
    notes: new FormControl(''),
  });

  createTenantForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(3)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(3)]),
    email: new FormControl('jean.michel@gmail.com', [Validators.required, Validators.email]),
    phoneNumber: new FormControl('0612131415', [Validators.required, Validators.minLength(10), Validators.maxLength(10)])
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<RentPropertyDialog>) { }

  closeDialog() {
    this.dialogRef.close();
  }

  createLease() {
    if (this.createTenantForm.valid && this.createLeaseForm.valid) {
      const formData = new FormData();
      const startDate = new Date(this.createLeaseForm.value.startDate!);
      const endDate = new Date(this.createLeaseForm.value.endDate!);

      formData.append("LeaseDocument", this.selectedFile!);
      formData.append("Deposit", this.createLeaseForm.value.deposit!);
      formData.append("rentAmount", this.createLeaseForm.value.rentAmount!);
      formData.append("chargesAmount", this.createLeaseForm.value.chargesAmount!);
      formData.append("startDate", startDate.toISOString());
      formData.append("endDate", endDate.toISOString());
      formData.append("notes", this.createLeaseForm.value.notes!);
      formData.append("isActive", "true");
      formData.append("Tenants[0].FirstName", this.createTenantForm.value.firstName!);
      formData.append("Tenants[0].LastName", this.createTenantForm.value.lastName!);
      formData.append("Tenants[0].Email", this.createTenantForm.value.email!);
      formData.append("Tenants[0].PhoneNumber", this.createTenantForm.value.phoneNumber!);

      this.leaseService.createLease(formData, this.data.propertyId)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe(response => {
          this.dialogRef.close(response);
        });

    } else {
      console.log('Form is invalid :  ');
    }
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }
}
