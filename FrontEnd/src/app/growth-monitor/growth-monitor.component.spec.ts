import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GrowthMonitorComponent } from './growth-monitor.component';

describe('GrowthMonitorComponent', () => {
  let component: GrowthMonitorComponent;
  let fixture: ComponentFixture<GrowthMonitorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GrowthMonitorComponent]
    });
    fixture = TestBed.createComponent(GrowthMonitorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
