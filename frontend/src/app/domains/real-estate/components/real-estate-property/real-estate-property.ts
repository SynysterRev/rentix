import { Component, DestroyRef, inject, signal } from '@angular/core';
import { PropertyService } from '../../services/property';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PropertyDTO } from '../../models/property.model';
import { PropertyCard } from '../property-card/property-card';

@Component({
  selector: 'app-real-estate-property',
  imports: [
    PropertyCard
  ],
  templateUrl: './real-estate-property.html',
  styleUrl: './real-estate-property.scss',
})
export class RealEstateProperty {
  private propertyService = inject(PropertyService);
  private destroyRef = inject(DestroyRef);
  properties = signal<PropertyDTO[]>([]);

  constructor() {
    this.loadProperties();
  }

  private loadProperties() {
    this.propertyService.getProperties()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(response => {
        this.properties.set(response);
      });
  }
}
