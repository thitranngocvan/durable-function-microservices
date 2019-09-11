export interface ICarModel {
  Car: ICarDetailModel;
  Price: number;
  PriceRateId: string;
  CurrencyCode: string;
}

export interface ICarDetailModel {
  ImageUrl: string;
  CarClassId: number;
  Doors: string;
  Seats: string;
  Id: string;
  Name: string;
  PartitionKey: string;
  RowKey: string;
  Timestamp: string;
  ETag: string;
}
