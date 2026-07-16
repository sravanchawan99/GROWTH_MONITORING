import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GrowthService, GrowthResponse } from '../services/growth.service';
 
@Component({
  selector: 'app-growth-monitor',
  templateUrl: './growth-monitor.component.html',
  styleUrls: ['./growth-monitor.component.css']
})
export class GrowthMonitorComponent implements OnInit {
 
  form!: FormGroup;
 
  loading = false;
 
  errorMsg = '';
 
  apiResult: GrowthResponse | null = null;
 
  months: number[] = [];
 
  constructor(
    private fb: FormBuilder,
    private growthService: GrowthService
  ) { }
 
  ngOnInit(): void {
 
    // Generate months 1-60
    this.months = Array.from({ length: 60 }, (_, i) => i + 1);
 
    this.form = this.fb.group({
 
      gender: [
        '',
        Validators.required
      ],
 
      age: [
        '',
        Validators.required
      ],
 
      height: [
        '',
        [
          Validators.required,
          Validators.min(1)
        ]
      ],
 
      weight: [
        '',
        [
          Validators.required,
          Validators.min(0.1)
        ]
      ]
 
    });
 
  }
 
  onSubmit(): void {
 
    this.errorMsg = '';
    this.apiResult = null;
 
    if (this.form.invalid) {
 
      this.form.markAllAsTouched();
 
      return;
 
    }
 
    this.loading = true;
 
    const {
      gender,
      age,
      height,
      weight
    } = this.form.value;
 
    this.growthService.predictZone(
      gender,
      Number(age),
      Number(height),
      Number(weight)
    ).subscribe({
 
      next: (response) => {
 
        this.apiResult = response;
 
        this.loading = false;
 
      },
 
      error: (error) => {
 
        console.error(error);
 
        this.errorMsg =
          'Unable to calculate growth monitoring result. Please try again.';
 
        this.loading = false;
 
      }
 
    });
 
  }
 
  getZoneClass(status: string): string {
 
    switch ((status || '').toUpperCase()) {
 
      case 'NORMAL':
        return 'zone-green';
 
      case 'MAM':
        return 'zone-yellow';
 
      case 'SAM':
        return 'zone-red';
 
      default:
        return 'zone-unknown';
 
    }
 
  }
 
}