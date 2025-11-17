import { Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { LucideAngularModule, UsersRound, MapPin } from "lucide-angular";
import { TenantDTO } from '../../../../shared/models/tenant.model';

@Component({
  selector: 'app-tenant-card',
  imports: [MatCardModule, LucideAngularModule, RouterLink],
  templateUrl: './tenant-card.html',
  styleUrl: './tenant-card.scss',
})
export class TenantCard {
  readonly UsersRound = UsersRound;
  readonly MapPin = MapPin;
  tenant = input<TenantDTO>();
}
