import { HttpClient } from "@angular/common/http";
import { ApiService } from "./api.service";
import { catchError, map, Observable, of } from "rxjs";
import {MasterRequestPageable, PageableResponse, SafeAny} from "../../core";
import {environment} from "@env";

export class BaseHttpService<T> {
    protected baseUrl: string;

    constructor(protected _apiService: ApiService) {
        this.baseUrl = environment.apiUrl;
    }

    getPaging(params?: SafeAny, moveKeysToParent: string[] = [], isSortDefault = false): Observable<PageableResponse<T[]>> {
        const filter = new MasterRequestPageable(params, moveKeysToParent, isSortDefault);

        return this._apiService.searchPagination<T[]>(`${this.baseUrl}/paging`, filter)
            .pipe(
                map((data) => {
                    return data;
                }),
            ) as SafeAny;
    }

    getAll(): Observable<T[]> {
      return this._apiService.getAll<T[]>(`${this.baseUrl}/all`)
        .pipe(
          map(({data}) => {
            return data as T[];
          })
        )
    }
}
