import {Injectable} from "@angular/core";
import {SafeAny} from "../../core";

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  set(name: string, value: SafeAny) {
    localStorage.setItem(name, value);
  }

  get(name: string) {
    return localStorage.getItem(name) || '{}';
  }

  clear(name: string) {
    return localStorage.removeItem(name);
  }
}
