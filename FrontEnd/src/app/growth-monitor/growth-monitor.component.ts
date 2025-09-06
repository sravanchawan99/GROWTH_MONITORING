// growth-monitor.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GrowthService } from '../services/growth.service';

@Component({
  selector: 'app-growth-monitor',
  templateUrl: './growth-monitor.component.html',
  styleUrls: ['./growth-monitor.component.css']
})
export class GrowthMonitorComponent implements OnInit {
  form!: FormGroup;
  loading = false;
  apiResult: { height: number; weight: number; zone: string } | null = null;
  errorMsg = '';

  private fb = inject(FormBuilder);
  private growthService = inject(GrowthService);

  ngOnInit(): void {
    this.form = this.fb.group({
      height: [null, [Validators.required, Validators.min(1)]],
      weight: [null, [Validators.required, Validators.min(1)]],
    }); [17]
  }

  onSubmit(): void {
    this.errorMsg = '';
    this.apiResult = null;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return; [17]
    }

    const { height, weight } = this.form.value;
    this.loading = true;

    this.growthService.predictZone(height, weight).subscribe({
      next: (res) => {
        // Expected: { height: 108.0, weight: 12.0, zone: 'Red' }
        this.apiResult = res;
        this.loading = false; [2]
      },
      error: () => {
        this.errorMsg = 'Failed to fetch growth zone. Please try again.';
        this.loading = false; [7]
      },
    }); [2][13]
  }

  zoneClass(): string {
    const z = (this.apiResult?.zone || '').toLowerCase();
    return z === 'red' ? 'zone-red'
         : z === 'yellow' ? 'zone-yellow'
         : z === 'green' ? 'zone-green'
         : 'zone-unknown'; [18][19]
  }
}
