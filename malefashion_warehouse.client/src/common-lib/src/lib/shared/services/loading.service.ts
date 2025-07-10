import {Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs";

@Injectable({ providedIn: 'root' })
export class LoadingService {
    private _loadingSubject = new BehaviorSubject<boolean>(false);
    loading$ = this._loadingSubject.asObservable();
    private _requestCount = 0;

    show() {
        this._requestCount++;
        this._loadingSubject.next(true);
    }

    hide() {
        this._requestCount = Math.max(0, this._requestCount - 1);
        if (this._requestCount === 0) {
            this._loadingSubject.next(false);
        }
    }

    reset() {
        this._requestCount = 0;
        this._loadingSubject.next(false);
    }
}
