import {Injectable, OnDestroy} from "@angular/core";
import {Subject} from "rxjs";

@Injectable()
export class DestroyService implements OnDestroy {
  public destroy$ = new Subject<void>();

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
