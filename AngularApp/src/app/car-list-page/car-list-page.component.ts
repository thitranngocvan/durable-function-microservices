import { Component, OnInit } from '@angular/core';
import { CarListService } from './car-list.service';
import { ICarModel } from './car.model';
import { Router } from '@angular/router';
import { ConfirmService } from '../confirm-page/confirm.service';

@Component({
  selector: 'app-car-list-page',
  templateUrl: './car-list-page.component.html',
  styleUrls: ['./car-list-page.component.scss']
})
export class CarListPageComponent implements OnInit {
  cars: ICarModel[];
  constructor(
    private carService: CarListService,
    private confirmService: ConfirmService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.cars = this.carService.getCars();
    if (!this.cars || this.cars.length === 0) {
      this.router.navigate(['/search']);
      return;
    }
  }

  bookThisCar(car: ICarModel) {
    console.log(car);
    this.confirmService.setSelectedCar(car);
    this.router.navigate(['/confirm']);
  }
}
