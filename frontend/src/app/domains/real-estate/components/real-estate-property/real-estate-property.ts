import { Component, DestroyRef, inject, signal } from '@angular/core';
import { PropertyService } from '../../services/property';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PropertyDetailsDTO, PropertyDTO } from '../../models/property.model';
import { PropertyCard } from '../property-card/property-card';
import { LucideAngularModule, Plus } from 'lucide-angular';
import { MatDialog } from '@angular/material/dialog';
import { CreatePropertyDialog } from '../create-property-dialog/create-property-dialog';
import { PropertyDetails } from '../property-details/property-details';

@Component({
  selector: 'app-real-estate-property',
  imports: [
    PropertyCard,
    LucideAngularModule
  ],
  templateUrl: './real-estate-property.html',
  styleUrl: './real-estate-property.scss',
})
export class RealEstateProperty {
  private propertyService = inject(PropertyService);
  private destroyRef = inject(DestroyRef);
  properties = signal<PropertyDTO[]>([]);


  readonly Plus = Plus;

  constructor(private dialog: MatDialog) {
    this.loadProperties();
  }

  private loadProperties() {
    this.propertyService.getProperties()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(response => {
        this.properties.set(response);
      });
  }

  createProperty() {
    const dialogRef = this.dialog.open(CreatePropertyDialog, {
      // width: '400px',
      // height: '300px',
      disableClose: true, // Prevent closing by clicking outside
    });

    dialogRef.afterClosed().subscribe((data: PropertyDetailsDTO) => {
      const property: PropertyDTO = {
          id: data.id,
          name: data.name,
          totalRent: data.rentWithoutCharges + data.rentCharges,
          tenantsNames: [],
          propertyStatus: data.propertyStatus,
          address: data.address,
          isAvailable: data.isAvailable
      }
      this.properties.update(list =>[...list, property]);
    })
  }
}
