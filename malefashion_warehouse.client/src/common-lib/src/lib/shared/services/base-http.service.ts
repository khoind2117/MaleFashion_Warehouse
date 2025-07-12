import { ApiService } from "./api.service";
import { map, Observable } from "rxjs";
import {MasterRequestPageable, PageableResponse, ResponseApi, SafeAny} from "../../core";
import {environment} from "@env";

export class BaseHttpService<T> {
    protected routerURL = '';
    protected baseUrl: string;

    constructor(protected _apiService: ApiService) {
        this.baseUrl = environment.apiUrl;
    }

    getPaging(params?: any, moveKeysToParent: string[] = [], isSortDefault = false): Observable<ResponseApi<PageableResponse<T[]>>> {
        const filter = new MasterRequestPageable(params, moveKeysToParent, isSortDefault);

        return this._apiService.searchPagination<T[]>(`${this.baseUrl}/${this.routerURL}/paged`, filter)
            .pipe(
                map((data) => {
                    return data;
                }),
            ) as any;
    }

    getAll(): Observable<T[]> {
      return this._apiService.getAll<T[]>(`${this.baseUrl}/all`)
        .pipe(
          map(({data}) => {
            return data as T[];
          })
        )
    }

    getById(id: number | string): Observable<T> {
        return this._apiService.get<T>(`${this.baseUrl}/${this.routerURL}/all`).pipe(
            map((res) => {
                return res.data;
            })
        );
    }
}
