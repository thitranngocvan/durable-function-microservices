import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CarListService {
  cars: any[] = [];
  constructor() { }

  setCars(cars) {
    this.cars = cars;
  }

  getCars() {
    return this.cars;
  }

  clearCars() {
    this.cars = [];
  }

}
