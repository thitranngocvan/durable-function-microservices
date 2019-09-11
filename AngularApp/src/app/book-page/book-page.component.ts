import { Component, OnInit } from '@angular/core';
import { BookService } from './book.service';
import { ICarModel } from './car.model';
import { Router } from '@angular/router';
import { ConfirmService } from '../confirm-page/confirm.service';

@Component({
  selector: 'app-book-page',
  templateUrl: './book-page.component.html',
  styleUrls: ['./book-page.component.scss']
})
export class BookPageComponent implements OnInit {
  cars: ICarModel[];
  constructor(
    private carService: BookService,
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
