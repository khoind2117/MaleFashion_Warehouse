import { SafeAny } from "./common.model";

/**
 * Pagination request params
 */
export interface Pageable {
    page?: number;
    size?: number;
    sortOrder?: number;
    sortField?: string;
    filters?: NonNullable<unknown>;
    conditionalFilters?: Filter[];
    totalRecords?: number;

    [key: string]: SafeAny;
}

export interface Filter {
    field: string;
    matchMode: FilterMatchMode;
    value: SafeAny;
}

export enum FilterMatchMode {
    StartsWith = 1,
    EndsWith = 2,
    Equals = 3,
    NotEquals = 4,
    Contains = 5,
    NotContains = 6,
}

/**
 * Pagination response
 */
export interface PageableResponse<T> {
    content?: T;
    empty?: boolean;
    first?: boolean; // set to true if it's the first page otherwise false
    last?: boolean; // set to true if it's the last page otherwise false
    size?: number; // the number of records per page, this was passed from client via param size
    sort?: Sort; // the sorting paramenter for the page
    totalElements?: number; // The total number of rows/records.
    totalPages?: number; // The total number of pages which was derived from (totalElements / size)
    numberOfElements?: number; // The page number sent by the client
    number?: number;
}

export interface Sort {
    empty?: boolean;
    sorted?: boolean;
    unsorted?: boolean;
}