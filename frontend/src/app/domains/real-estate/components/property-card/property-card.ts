import { Component, input } from '@angular/core';
import { PropertyDTO } from '../../models/property.model';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { LucideAngularModule, UsersRound, MapPin } from "lucide-angular";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-property-card',
  imports: [MatCardModule, LucideAngularModule, RouterLink],
  templateUrl: './property-card.html',
  styleUrl: './property-card.scss',
})
export class PropertyCard {
  readonly UsersRound = UsersRound;
  readonly MapPin = MapPin;
  property = input<PropertyDTO>();
}
