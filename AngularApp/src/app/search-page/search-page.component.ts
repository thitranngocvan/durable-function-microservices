import { Component, OnInit } from '@angular/core';
import { SearchService } from './search.service';
import { timer } from 'rxjs';
import { takeUntil, skipWhile, map, filter, take, concatMap, tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ConfirmService } from '../confirm-page/confirm.service';
import { CarListService } from '../car-list-page/car-list.service';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss']
})
export class SearchPageComponent implements OnInit {
  isSearching = false;
  constructor(
    private searchService: SearchService,
    private confirmService: ConfirmService,
    private carService: CarListService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.carService.clearCars();
    this.confirmService.clearSelectedCar();
  }

  search() {
    this.isSearching = true;
    this.searchService.search({
      pickupDate: new Date('2019-10-01T09:00:00'),
      returnDate: new Date('2019-10-15T09:00:00'),
      pickupLocation: 'AMS',
      returnLocation: 'AMS',
      currencyCode: 'USD'
    }).subscribe(searchResponse => {
      console.log(searchResponse);
      const { id } = searchResponse;
      timer(0, 2000)
        .pipe(
          concatMap(() => this.searchService.poolCheckCar(id)),
          filter(poolResult => {
            console.log(poolResult);
            return poolResult.runtimeStatus === 'Completed';
          }),
          take(1),
        ).subscribe((poolResult) => {
          console.log(poolResult);
          const { output } = poolResult;
          this.carService.setCars(output['CarPrices']);
          this.router.navigate(['/car-list']);
        });
    });
  }

}
