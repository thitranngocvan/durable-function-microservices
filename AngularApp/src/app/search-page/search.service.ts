import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ISearchModel } from './search.model';
import { appConfig } from '../app-config.const';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  constructor(
    private httpClient: HttpClient
  ) { }

  search(searchModel: ISearchModel): Observable<any> {
    return this.httpClient.post<any>(`${appConfig.baseUrl}/api/HttpStart_SearchCar`, searchModel);
  }

  poolCheckCar(instanceId: string): Observable<any> {
    let url = `${appConfig.baseUrl}/${appConfig.poolingEndpoint}`;
    url = url.replace('{{cars_instance_id}}', instanceId);
    url = url.replace('{{auth_code}}', appConfig.authCode);
    console.log(url);
    return this.httpClient.get<any>(url);
  }

}
