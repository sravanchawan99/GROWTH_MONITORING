import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GrowthService {
  private apiUrl = 'https://fa-growthmonitoring.azurewebsites.net/api/predict_zone';

  constructor(private http: HttpClient) {}

  predictZone(height: number, weight: number): Observable<any> {
    return this.http.post<any>(this.apiUrl, { height, weight });
  }
}
