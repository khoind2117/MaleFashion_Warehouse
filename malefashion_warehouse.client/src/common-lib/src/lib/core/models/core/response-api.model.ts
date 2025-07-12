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
    params: Pageable = {},
    moveKeysToParent: string[] = [],
    isSortUpdateDateDefault = false,
    valueSortDefault = ColsConfigMaster.UPDATED_DATE,
    isNoPage = false
  ) {
    params = params || {};

    // Parse filters
    const filters = params.filters || {};
    this.criteria = Object.entries(filters).reduce((rev, [key, obj]) => {
      if (!moveKeysToParent.includes(key)) {
          const value = (obj as Filter).value;
          return {
              ...rev,
              [key]: value !== false ? value || undefined : false,
          };
      }
      return rev;
    }, {});

    this.page = params.page || 0;
    this.size = params.size || TABLE_CONFIG.ROWS;

    // Sort logic
    if (params?.sortField && params?.sortOrder !== 0) {
      this.sortBy = params.sortField;
      this.sortDirection = params.sortOrder === 1 ? SortEnum.ASC : SortEnum.DESC;
    } else if (isSortUpdateDateDefault) {
      this.sortBy = valueSortDefault;
      this.sortDirection = SortEnum.DESC;
    }

    // Move keys to parent
    moveKeysToParent?.forEach((key) => {
      this[key] = (params?.filters[key] as Filter)?.value || undefined;
    });

    // Remove pagination if needed
    if (isNoPage) {
      delete this.page;
      delete this.size;
    }
  }
}

