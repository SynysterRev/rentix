import { Component, input } from '@angular/core';
import { PropertyDTO } from '../../models/property.model';

@Component({
  selector: 'app-property-card',
  imports: [],
  templateUrl: './property-card.html',
  styleUrl: './property-card.scss',
})
export class PropertyCard {
  property = input<PropertyDTO>();
}
