import { Component, DestroyRef, inject, input, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PropertyDTO } from '../../models/property.model';
import { PropertyService } from '../../services/property';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { LucideAngularModule, CircleAlert, MapPin, Euro, FileText, UsersRound, Mail, Phone, Calendar } from "lucide-angular";

@Component({
  selector: 'app-property-details',
  imports: [LucideAngularModule],
  templateUrl: './property-details.html',
  styleUrl: './property-details.scss',
})
export class PropertyDetails {
  propertyId: number = 0;
  private propertyService = inject(PropertyService);
  private destroyRef = inject(DestroyRef);
  property = signal<PropertyDTO | null>(null);
  readonly MapPin = MapPin;
  readonly CircleAlert = CircleAlert;
  readonly Euro = Euro;
  readonly FileText = FileText;
  readonly UsersRound = UsersRound;
  readonly Mail = Mail;
  readonly Phone = Phone;
  readonly Calendar = Calendar;

  constructor(private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      const newId = Number(params.get('id'));
      if (newId !== this.propertyId) {
        this.loadProduit(newId);
      }
    });
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

