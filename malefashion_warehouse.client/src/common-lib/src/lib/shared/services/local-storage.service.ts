import {Injectable} from "@angular/core";
import {SafeAny} from "../../core";

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
    get<T = any>(key: string): T | null {
        try {
            const data = localStorage.getItem(key);
            return data ? (JSON.parse(data) as T) : null;
        } catch (e) {
            console.error('LocalStorage Get Error:', e);
            return null;
        }
    }

    set(key: string, value: any): void {
        try {
            localStorage.setItem(key, JSON.stringify(value));
        } catch (e) {
            console.error('LocalStorage Set Error:', e);
        }
    }

    clear(key: string): void {
        localStorage.removeItem(key);
    }
}
