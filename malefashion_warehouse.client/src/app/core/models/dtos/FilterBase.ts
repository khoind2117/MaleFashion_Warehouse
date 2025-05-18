export interface FilterBase {
    keyword?: string;
    orderBy?: string;
    isDescending?: boolean;
    currentPage: number;
    pageSize: number;
}