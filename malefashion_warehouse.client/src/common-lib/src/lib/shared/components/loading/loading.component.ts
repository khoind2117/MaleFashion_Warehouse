import { Component } from "@angular/core";
import {CommonModule} from "@angular/common";
import {LoadingService} from "../../services";

@Component({
    selector: 'app-loading',
    standalone: true,
    imports: [
        CommonModule
    ],
    templateUrl: './loading.component.html',
    styleUrls: ['./loading.component.scss'],
})
export class LoadingComponent {
    constructor(
        public loadingService: LoadingService,
    ) {
    }
}
