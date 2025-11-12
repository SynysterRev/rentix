import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PropertyDetailsDTO, PropertyDTO } from '../models/property.model';
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

  getPropertyDetails(id: number): Observable<PropertyDetailsDTO> {
    return this.http.get<PropertyDetailsDTO>(`${this.apiUrl}/property/${id}`, this.headers);
  }

  deleteProperty(id: number) : Observable<PropertyDetailsDTO>{
    return this.http.delete<PropertyDetailsDTO>(`${this.apiUrl}/property/${id}`, this.headers);
  }
}
