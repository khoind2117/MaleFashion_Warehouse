import {ColsConfigMaster, Filter, Pageable, SafeAny} from ".";
import {TABLE_CONFIG} from "../../consts";

export class ResponseApi<T> {
  ts?: number;

  status?: number;

  success?: boolean;

  data?: T;

  message?: string;

  error?: string;

  [key: string]: SafeAny;
}

export class MasterRequestApi<T> {
  criteria: T;

  [key: string]: SafeAny;

  constructor(body: T, moveKeysToParent: string[] = []) {
    this.criteria = body;
    moveKeysToParent?.forEach((key) => {
      if ((body as Record<string, SafeAny>)[key]) {
        (this as Record<string, SafeAny>)[key] =
          (body as Record<string, SafeAny>)[key] || undefined;
        (body as Record<string, SafeAny>)[key] = undefined;
      }
    });
  }
}

export enum SortEnum {
  ASC = 'ASC',
  DESC = 'DESC',
}

export class MasterRequestPageable {
  // criteria => filter
  criteria: NonNullable<unknown> | undefined;
  page: number;
  size: number;
  sortBy: string | undefined;
  sortDirection: SortEnum | undefined;

  // custom filed for table constrain
  [key: string]: SafeAny;

  constructor(
    params: Pageable,
    moveKeysToParent: string[] = [],
    isSortUpdateDateDefault = false,
    valueSortDefault = ColsConfigMaster.UPDATED_DATE,
    isNoPage = false
  ) {
    // parse model filter of primeng to my custom filter
    if (params?.filters) {
      this.criteria = Object.entries(params?.filters).reduce((rev, [key, obj]) => {
        if (!moveKeysToParent?.find((k) => k === key)) {
          return {
            ...rev,
            [key]: (obj as Filter).value !== false ? (obj as Filter).value || undefined : false,
          };
        }
        return rev;
      }, {});
    }
    this.page = params.page || 0;
    this.size = params.size || TABLE_CONFIG.ROWS;
    if (params?.sortField) {
      this.sortBy = params?.sortField || undefined;
      this.sortDirection = params?.sortOrder === 1 ? SortEnum.ASC : SortEnum.DESC;
    }
    // default sort by updated by
    if (!params?.sortField && isSortUpdateDateDefault) {
      this.sortBy = valueSortDefault;
      this.sortDirection = SortEnum.DESC;
    }
    moveKeysToParent?.forEach((key) => {
      this[key] = (params?.filters[key] as Filter)?.value || undefined;
    });

    if (isNoPage) {
      delete this.page;
      delete this.size;
    }
  }
}

