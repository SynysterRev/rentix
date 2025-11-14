import { Component, NgModule, ChangeDetectionStrategy, DestroyRef, signal, inject } from '@angular/core';
import { MatDialogRef, MatDialogContent, MatDialogActions } from '@angular/material/dialog';
import { AddressCreateDTO } from '../../../../shared/models/address.model';
import { PropertyCreateDTO } from '../../models/property.model';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule, MatLabel, MatHint } from '@angular/material/input';
import { MatFormField } from '@angular/material/input';
import { MatCardModule } from "@angular/material/card";
import { PropertyService } from '../../services/property';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PropertyStatus } from '../../../../shared/models/property-status.model';

@Component({
  selector: 'app-create-property-dialog',
  imports: [ReactiveFormsModule, MatFormField, MatDialogActions, MatInputModule, MatCardModule],
  templateUrl: './create-property-dialog.html',
  styleUrl: './create-property-dialog.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class CreatePropertyDialog {
  private propertyService = inject(PropertyService);
  private destroyRef = inject(DestroyRef);

  createPropertyForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(3)]),
    maxRent: new FormControl('', [Validators.required]),
    rentNoCharges: new FormControl('', [Validators.required]),
    rentCharges: new FormControl('', [Validators.required]),
    deposit: new FormControl('', [Validators.required]),
    surface: new FormControl('', [Validators.required]),
    numberRooms: new FormControl('', [Validators.required]),
    street: new FormControl('', [Validators.required]),
    complement: new FormControl('', [Validators.required]),
    postalCode: new FormControl('', [Validators.required, Validators.maxLength(5)]),
    city: new FormControl('', [Validators.required]),
    country: new FormControl('', [Validators.required]),
  });

  constructor(public dialogRef: MatDialogRef<CreatePropertyDialog>) { }

  closeDialog() {
    this.dialogRef.close();
  }

  onSubmitCreatePropertyForm() {
    if (this.createPropertyForm.valid) {
      const address: AddressCreateDTO = {
        street: this.createPropertyForm.value.street!,
        city: this.createPropertyForm.value.city!,
        postalCode: this.createPropertyForm.value.postalCode!.toString(),
        country: this.createPropertyForm.value.country!,
        complement: this.createPropertyForm.value.complement!,
      };

      const property: PropertyCreateDTO = {
        name: this.createPropertyForm.value.name!,
        maxRent: Number(this.createPropertyForm.value.maxRent!),
        rentNoCharges: Number(this.createPropertyForm.value.rentNoCharges!),
        rentCharges: Number(this.createPropertyForm.value.rentCharges),
        deposit: Number(this.createPropertyForm.value.deposit),
        propertyStatus: PropertyStatus.Available,
        surface: Number(this.createPropertyForm.value.surface),
        numberRooms: Number(this.createPropertyForm.value.numberRooms),
        addressId: null,
        addressDto: address,
        landlordId: 'd176a09f-321c-43e1-8247-20fc6a3cc266'
      };

      this.propertyService.createProperty(property)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe(response => {
          this.dialogRef.close(response);
        });

    } else {
      console.log('Form is invalid :  ');
    }
  }

}
