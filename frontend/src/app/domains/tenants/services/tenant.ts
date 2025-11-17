import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { TenantDTO } from '../../../shared/models/tenant.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TenantService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private headers = { withCredentials: true }

  getTenants(): Observable<TenantDTO[]> {
    return this.http.get<TenantDTO[]>(`${this.apiUrl}/tenants`, this.headers);
  }
}
