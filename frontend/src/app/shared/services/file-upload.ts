import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private headers = { withCredentials: true };

  loadTestFile() {
    return this.http.get('assets/test.txt', { responseType: 'blob' });
  }
}
