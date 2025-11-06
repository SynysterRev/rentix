import { Component, input } from '@angular/core';
import { PropertyDTO } from '../../models/property.model';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { LucideAngularModule, UsersRound } from "lucide-angular";

@Component({
  selector: 'app-property-card',
  imports: [MatCardModule, LucideAngularModule],
  templateUrl: './property-card.html',
  styleUrl: './property-card.scss',
})
export class PropertyCard {
  readonly UsersRound = UsersRound;
  property = input<PropertyDTO>();

  propertyIsAvailable(){
    return this.property()?.isAvailable;
  }
}
