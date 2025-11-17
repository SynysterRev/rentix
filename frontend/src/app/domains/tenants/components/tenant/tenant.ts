import { Component, DestroyRef, inject, signal } from '@angular/core';
import { TenantDTO } from '../../../../shared/models/tenant.model';
import { TenantService } from '../../services/tenant';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { TenantCard } from '../tenant-card/tenant-card';

@Component({
  selector: 'app-tenant',
  imports: [TenantCard],
  templateUrl: './tenant.html',
  styleUrl: './tenant.scss',
})
export class Tenant {
  private tenantService = inject(TenantService);
  private destroyRef = inject(DestroyRef);
  tenants = signal<TenantDTO[]>([]);

  constructor() {
    this.loadTenants();
  }

  private loadTenants() {
    this.tenantService.getTenants()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(response => {
        this.tenants.set(response);
      });
  }
}

