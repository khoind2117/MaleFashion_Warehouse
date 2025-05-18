import { ColorDto } from "../color/colorDto";

export interface PagedProductDto {
    id: number;
    name: string;
    slug: string;
    price: number;

    colorDtos: ColorDto[];
}