import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
 
export interface GrowthResponse {
  Wasting: string;
  Underweight: string;
  Stunting: string;
  NWL: number;
  NWA: number;
  NLA: number;
}
 
@Injectable({
  providedIn: 'root'
})
export class GrowthService {
 
  private apiUrl = 'http://localhost:7071/api/GetZscore';
 
  constructor(private http: HttpClient) {}
 
  predictZone(
    gender: string,
    age: number,
    height: number,
    weight: number
  ): Observable<GrowthResponse> {
 
    return this.http.post<GrowthResponse>(this.apiUrl, {
      height,
      age,
      gender,
      weight
    });
 
  }
}