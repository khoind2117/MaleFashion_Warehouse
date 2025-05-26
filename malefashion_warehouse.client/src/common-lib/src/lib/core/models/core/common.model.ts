export type SafeAny = any;

export enum ColsConfigMaster {
  NO = 'no',
  ACTION = 'action',
  NAME = 'name',
  CODE = 'code',
  DESCRIPTION = 'description',
  STATUS = 'status',
  CREATED_BY = 'createdBy',
  CREATED_DATE = 'createdDate',
  UPDATED_BY = 'updatedBy',
  UPDATED_DATE = 'updatedDate',
  TYPE = 'type',
  REQUIRED = 'required',
  ACTIVE_STATUS = 'activeStatus',
  START_DATE = 'startDate',
  END_DATE = 'endDate',
  NOTE = 'note',
  EMAIL = 'email',
  PHONE = 'phone',
  ID = 'id',
  ADDRESS = 'address',
  RELEASE_DATE = 'releasedDate',
  RELEASE_BY = 'releasedBy',
}

export class MapRouting {
    fullPath: string;
    absolutePath: string;

    constructor(rootPath: any, absolutePath: any) {
        this.absolutePath = absolutePath || rootPath;
        this.fullPath = absolutePath ? `/${rootPath}/${absolutePath}` : `/${rootPath}`;
    }
}
