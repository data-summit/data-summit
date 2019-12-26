import { Component } from '@angular/core';
import { NotifyService } from './shared/services/notify.service';
import { Message } from 'primeng/primeng';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Get there';
  messagesSubscription: Subscription;
  messages: Message[];

  constructor(private notifyService: NotifyService) {
    this.messagesSubscription = this.notifyService.messages$.subscribe(
      messages => {
        this.messages = messages;
      }
    );
  }
}
