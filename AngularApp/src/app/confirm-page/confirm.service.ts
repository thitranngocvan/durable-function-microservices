import { Injectable } from '@angular/core';
import { ICarModel } from '../book-page/car.model';
import { appConfig } from '../app-config.const';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  selectedCar: ICarModel;
  constructor(
    private httpClient: HttpClient
  ) { }

  setSelectedCar(selectedCar) {
    this.selectedCar = selectedCar;
  }

  getSelectedCar() {
    return this.selectedCar;
  }

  clearSelectedCar() {
    this.selectedCar = null;
  }

  bookCar(confirmModel) {
    const url = `${appConfig.baseUrl}/api/CarsBookingApi_HttpStart`;
    return this.httpClient.post<any>(url, confirmModel);
  }

  poolCheck(instanceId) {
    let url = `${appConfig.baseUrl}/${appConfig.poolingEndpoint}`;
    url = url.replace('{{cars_instance_id}}', instanceId);
    url = url.replace('{{auth_code}}', appConfig.authCode);
    console.log(url);
    return this.httpClient.get<any>(url);
  }

}
