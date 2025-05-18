import { HttpClient } from "@angular/common/http";
import {Observable, throwError} from "rxjs";
import { Pageable, PageableResponse, ResponseApi, SafeAny } from "../../core";
import {Injectable} from "@angular/core";
import {catchError} from "rxjs/operators";

@Injectable({
    providedIn: 'root',
})
export class ApiService {
    constructor(
        private _httpClient: HttpClient,
    ) {}

    searchPagination<T>(url: string, body: Pageable): Observable<ResponseApi<PageableResponse<T[]>>> {
        return this._httpClient.post<ResponseApi<PageableResponse<T[]>>>(url, body);
    }

    get<T>(url: string, data: string | number = ''): Observable<ResponseApi<T>> {
        let urlFinal = `${url}`;
        if (data) {
            urlFinal += `/${data}`;
        }

        return this._httpClient.get<ResponseApi<T>>(urlFinal);
    }

    getByParams<T>(url: string, params = {}): Observable<ResponseApi<T>> {
        // Filter out parameters with undefined or null values
        const filteredParams = Object.fromEntries(
            Object.entries(params).filter(([_, v]) => v !== undefined && v !== null)
        );

        return this._httpClient.get<ResponseApi<T>>(url, { params: filteredParams as SafeAny });
    }

    getAll<T>(url: string, params = {}): Observable<ResponseApi<PageableResponse<T[]>>> {
        return this._httpClient.get<ResponseApi<PageableResponse<T[]>>>(url, { params });
    }

    getAllNotPaging<T>(url: string): Observable<ResponseApi<T[]>> {
        return this._httpClient.get<ResponseApi<T[]>>(url);
    }

    insert<T>(url: string, body: T): Observable<ResponseApi<T>> {
        return this._httpClient.post<ResponseApi<T>>(url, body);
    }

    post<T>(url: string, body: SafeAny, options = {}): Observable<ResponseApi<T>> {
        return this._httpClient.post<ResponseApi<T>>(`${url}`, body, options);
    }

    update<T>(url:string, body: SafeAny, option = {}): Observable<ResponseApi<T>> {
        return this._httpClient.put<ResponseApi<T>>(`${url}`, body, option);
    }

    patch<T>(url: string, body: T = null as SafeAny, option = {}): Observable<ResponseApi<T>> {
        return this._httpClient.patch<ResponseApi<T>>(`${url}`, body, option);
    }

    delete<T>(url: string, id: string | number): Observable<ResponseApi<boolean>> {
        return this._httpClient.delete<ResponseApi<boolean>>(`${url}/${id}`);
    }
}
