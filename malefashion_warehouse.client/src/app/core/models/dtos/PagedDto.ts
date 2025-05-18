export interface PagedDto<T> {
    items: T[];
    totalRecords: number;
    pageSize: number;
    pageNumber: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}
 