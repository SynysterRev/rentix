import { Component, DestroyRef, inject, input, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PropertyDTO } from '../../models/property.model';
import { PropertyService } from '../../services/property';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-property-details',
  imports: [CommonModule],
  templateUrl: './property-details.html',
  styleUrl: './property-details.scss',
})
export class PropertyDetails {
  produitId: number = 0;

  private propertyService = inject(PropertyService);
  private destroyRef = inject(DestroyRef);
  property = signal<PropertyDTO | null>(null);

  constructor(private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      const newId = Number(params.get('id'));
      if (newId !== this.produitId) {
        this.loadProduit(newId);
        console.log(this.property());
      }
    });
    console.log(this.property());
  }

  loadProduit(id: number) {
    this.propertyService.getPropertyDetails(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(response => {
        this.property.set(response);
        console.log(this.property());
      });
  }
}

