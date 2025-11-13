import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { LeaseCreateDTO } from '../models/lease.model';

@Injectable({
  providedIn: 'root'
})
export class LeaseService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private headers = { withCredentials: true }

  createLease(lease: LeaseCreateDTO){
    return this.http.post<LeaseCreateDTO>(`${this.apiUrl}/lease`, lease, this.headers);
  }
}
