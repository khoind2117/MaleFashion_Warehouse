import { FilterBase } from "../FilterBase";

export interface ProductFilterDto extends FilterBase {
    sortBy?: string;
    maincategoryId?: number;
    subCategoryId?: number;
    sizeId?: number;
    colorId?: number;
}