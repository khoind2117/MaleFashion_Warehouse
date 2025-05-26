import {MenuItem} from "primeng/api";

export interface RouteDataModel {
    title?: string;
    breadcrumb?: string | MenuItem;
    mode?: string;
    activeNav?: boolean;
    sidebarOverlay?: boolean;
    authorities?: string | string [];
}
