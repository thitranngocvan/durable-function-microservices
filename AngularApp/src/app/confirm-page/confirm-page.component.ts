import { ConfirmService } from './confirm.service';
import { Component, OnInit } from '@angular/core';
import { ICarModel } from '../car-list-page/car.model';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { timer } from 'rxjs';
import { takeUntil, skipWhile, map, filter, take, concatMap, tap, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-confirm-page',
  templateUrl: './confirm-page.component.html',
  styleUrls: ['./confirm-page.component.scss']
})
export class ConfirmPageComponent implements OnInit {
  formGroup: FormGroup;
  selectedCar: ICarModel;
  bookingInfo: any;
  isBooking = false;
  constructor(
    private confirmService: ConfirmService,
    private router: Router,
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
    this.selectedCar = this.confirmService.getSelectedCar();
    if (!this.selectedCar) {
      this.router.navigate(['/search']);
      return;
    }

    console.log(this.selectedCar);
    this.buildForm();
  }

  buildForm() {
    this.formGroup = this.fb.group({
      driverName: [null, Validators.required],
      customerEmail: [null, Validators.required],
      customerFirstName: [null, Validators.required],
      customerLastName: [null, Validators.required],
    });
  }

  submit() {
    const formValue = this.formGroup.getRawValue();
    console.log(formValue);
    this.isBooking = true;
    const bookCarModel = {
      ...formValue,
      customerGuid: '00000000-0000-0000-0000-000000000000',
      bookingDetail: {
        pickupDate: new Date('2019-10-01T09:00:00'),
        returnDate: new Date('2019-10-15T09:00:00'),
        pickupLocation: {
          id: 123,
          name: 'AMS'
        },
        returnLocation: {
          id: 456,
          name: 'AMS'
        },
        currencyCode: 'USD'
      },
      carPrice: {
        ...this.selectedCar
      }
    };
    this.confirmService.bookCar(bookCarModel).subscribe(searchResponse => {
      console.log(searchResponse);
      const { id } = searchResponse;
      timer(0, 2000)
        .pipe(
          concatMap(() => this.confirmService.poolCheck(id)),
          filter(poolResult => {
            console.log(poolResult);
            this.bookingInfo = poolResult.output;
            return poolResult.runtimeStatus === 'Completed';
          }),
          take(1),
        ).subscribe((poolResult) => {
          console.log(poolResult);
          const { output } = poolResult;
          this.bookingInfo = output;
          this.isBooking = false;
        });
    });
  }

  ngOnDestroy() {
    this.confirmService.clearSelectedCar();
  }

}
