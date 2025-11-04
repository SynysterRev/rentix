import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PropertyDTO } from '../models/property.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private headers = { withCredentials: true }

  getProperties(): Observable<PropertyDTO[]> {
    return this.http.get<PropertyDTO[]>(`${this.apiUrl}/property`, this.headers);
  }

  getPropertyDetails(id: number): Observable<PropertyDTO> {
    return this.http.get<PropertyDTO>(`${this.apiUrl}/property/${id}`, this.headers);
  }

}
