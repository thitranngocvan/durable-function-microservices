import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatInputModule } from '@angular/material/input';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SearchPageComponent } from './search-page/search-page.component';
import { Routes, RouterModule } from '@angular/router';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { HttpClientModule } from '@angular/common/http';
import { ConfirmPageComponent } from './confirm-page/confirm-page.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CarListPageComponent } from './car-list-page/car-list-page.component';
import { LoadingComponent } from './loading/loading.component';

const routes: Routes = [
  { path: 'search', component: SearchPageComponent },
  { path: 'car-list', component: CarListPageComponent },
  { path: 'confirm', component: ConfirmPageComponent },
  { path: '**', redirectTo: 'search' },
];

@NgModule({
   declarations: [
      AppComponent,
      SearchPageComponent,
      CarListPageComponent,
      ConfirmPageComponent,
      LoadingComponent
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      AppRoutingModule,
      HttpClientModule,
      ReactiveFormsModule,
      MatInputModule,
      MatDatepickerModule,
      MatNativeDateModule,
      MatButtonModule,
      MatCheckboxModule,
      RouterModule.forRoot(routes)
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
