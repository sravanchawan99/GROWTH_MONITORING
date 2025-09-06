import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgChartsModule } from 'ng2-charts';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { GrowthMonitorComponent } from './growth-monitor/growth-monitor.component';

@NgModule({
  declarations: [
    AppComponent,
    GrowthMonitorComponent // declared here, so component should NOT be standalone
  ],
  imports: [
    BrowserModule,
    FormsModule,            // if template-driven is used elsewhere
    ReactiveFormsModule,    // needed for [formGroup]/formControlName
    HttpClientModule,       // needed for API calls via HttpClient
    NgChartsModule,
    RouterModule.forRoot([]) // keep forRoot only once in the root module
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
