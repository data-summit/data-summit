import { Injectable, EventEmitter } from "@angular/core";
import { Message } from "primeng/primeng";
import { Subject, BehaviorSubject } from "rxjs";
import {MessageService} from 'primeng/api';

@Injectable()
export class NotifyService {

  private _importUpdateNotification = new Subject();

  constructor(private messageService: MessageService) {}


  private messages = new Subject<Message[]>();
  //private toasts = new Subject<Message[]>();

  reloadNotificationRequest: EventEmitter<boolean> = new EventEmitter<boolean>();
  messages$ = this.messages.asObservable();
  //toasts$ = this.toasts.asObservable();

  get importUpdateNotification() {
    return this._importUpdateNotification;
  }

  info(detail: string, summary?: string) {
    this.setNotification("info", detail, summary);
  }

  success(detail: string, summary?: string) {
    this.setNotification("success", detail, summary);
  }

  warn(detail: string, summary?: string) {
    this.setNotification("warn", detail, summary);
  }

  toast(detail) {
    this.messageService.add({key:"messageAlert", severity:'info', summary: detail.from, detail: detail.message});
  }

  simpleToast(message: string) {
    this.messageService.add({key:"messageAlert", severity:'info', summary: "Alert", detail: message});
  }

  anyError(error: any, summary?: string) {
    let erm: any = null;
    if (error.status) {
      switch (error.status) {
        case 401:
          this.error("Unauthorised Request", summary);
          return;
        case 408:
          this.error("Request Timeout. Please retry.", summary);
          return;
      }
    }
    if (error.error) {
      if (error.error.Message || error.error.message) { erm = error.error; }
      if (erm && error.error.ExceptionMessage) {
        this.error(error.error.ExceptionMessage, erm.Message || erm.message || erm);
        return;
      }
      if (!erm) { erm = error.error;}
    }

    if (!erm) { erm = error; }

    this.error(erm.Message || erm.message || erm, summary);
  }

  error(detail: string, summary?: string) {
    this.setNotification("error", detail, summary);
  }

  protected setNotification(severity: string, detail: string, summary: string) {
    if (!summary) {
      summary = severity.toLocaleUpperCase();
      if (summary == "WARN") {
        summary = "WARNING"
      }
    }

    let message: Message[] = [];
      message.push({ severity: severity, summary: summary, detail: detail }); 
    this.setElement(message);
  };

  protected setElement(message) {
    let messageArr = []
    messageArr = [...message]
      this.messages.next(messageArr)
    }
    
  formatError(error: any, defaultMsg?: string): string {
    if (error) {
      if (error.Message) {
        return error.Message;
      }
      if (typeof error.text === "function") {
        return error.text();
      }
      if (error.message) {
        return error.message;
      }
      if (defaultMsg) {
        return defaultMsg;
      }
    }
    return "";
  }

  notifyImportUpdate(notification: any) {
    if (notification) {
      notification["receivedDateTime"] = Date.now();
      this._importUpdateNotification.next(notification);
    }
  }
}
