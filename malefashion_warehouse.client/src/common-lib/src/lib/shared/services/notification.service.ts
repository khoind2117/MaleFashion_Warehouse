import {Injectable} from "@angular/core";
import {MessageService} from "primeng/api";
import {Message} from "primeng/message";
import {Severity} from "../../core";

@Injectable({
    providedIn: 'root'
})
export class NotificationService {
    constructor(
        private _messageService: MessageService,
    ){}

    show(severity: Severity, summary: string, detail?: string, config?: Partial<Message>){
        this._messageService.add({
            severity,
            summary,
            detail,
            life: config?.life ?? 3000,
            closable: config?.closable ?? true,
            styleClass: config?.styleClass,
            ...config,
        })
    }

    clear() {
        this._messageService.clear();
    }
}
