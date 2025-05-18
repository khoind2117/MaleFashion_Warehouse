import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppRoutingApi } from 'src/app/app-routing-api';
import { Observable } from 'rxjs';
import { ProductFilterDto } from '../models/dtos/product/ProductFilterDto';
import { PagedDto } from '../models/dtos/PagedDto';
import { PagedProductDto } from '../models/dtos/product/PagedProductDto';

@Injectable({
  providedIn: 'root'
})
export class ProductService extends BaseService {

  constructor( http: HttpClient ) {
    super(http, AppRoutingApi.Product.Router_Prefix);
  }

  getAll() {
    return this.http.get(this.routerPrefix + "/get-all")
  }

  getPaged(productFilterDto?: ProductFilterDto): Observable<any> {
    let params = new HttpParams();

    if (productFilterDto) {
      Object.keys(productFilterDto).forEach((key) => {
        const value = (productFilterDto as any)[key];
        if (value !== null && value !== undefined) {
          params = params.set(key, value);
        }
      });
    }

    return this.http.get(this.routerPrefix + "/paged", {
      params: params,
      responseType: 'json',
    })
  }
}
