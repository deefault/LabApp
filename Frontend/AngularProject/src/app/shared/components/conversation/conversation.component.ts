import {AfterViewInit, Component, Input, OnInit} from '@angular/core';
import {ChatService, ConversationDto, MessageDto, UserListDto} from "../../../clients/common";
import {AuthService} from "../../../services/auth/auth.service";
import {BaseComponent} from "../../../core/base-component";
import {Observable} from "rxjs";
import {EventBusService} from "../../../services/event-bus.service";

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss']
})
export class ConversationComponent extends BaseComponent implements OnInit, AfterViewInit {

  messages: MessageDto[] = [];
  me: UserListDto;
  other: UserListDto;
  @Input() conversation: ConversationDto;

  lastReadMessage: MessageDto = null;
  readonly enableReadAll: boolean = true;
  private getMessages: Observable<Array<MessageDto>>;
  //scroll: number = 0;

  constructor(
    private chatService: ChatService,
    private authService: AuthService,
    private eventBus: EventBusService,
  ) {
    super();
  }

  ngOnInit() {
    this.loading = true;
    this.me = this.conversation.users.find(x => x.userId == this.authService.userIdentity.Id);
    this.other = this.conversation.users.find(x => x.userId != this.authService.userIdentity.Id);

    this.getMessages = this.chatService.getMessages(this.conversation.id);
    this.getMessages.subscribe(data => {
      this.messages = data;
      this.setLastMessage();
      this.loading = false;
    });
  }
  
  ngAfterViewInit() {
    this.getMessages.subscribe(_ => this.scrollToLastRead());
  }

  setLastMessage() {
    if (this.conversation.lastReadMessage) {
      for (let i = 0; i < this.messages.length; i++) {
        if (this.messages[i].sent < this.conversation.lastReadMessage) {
          this.lastReadMessage = this.messages[i];
          return;
        }
      }
    }

    // no unread found
    this.lastReadMessage = null;
  }

  get allRead(): boolean {
    return this.lastReadMessage && this.lastReadMessage.id == this.messages[this.messages.length - 1].id;
  }

  getName(me: UserListDto) {
    return me.surname + " " + me.name;
  }

  sendMessage($event: { message: string; files: File[] }) {
    this.chatService.addMessage(this.conversation.id, new class implements MessageDto {
      text: string = $event.message;
    }).subscribe(data => {
      this.messages.push(data);
    });
  }

  readAll() {
    if (this.enableReadAll && !this.allRead) {
      this.chatService.readMessages(this.conversation.id).subscribe(count => {
        console.log("has read messages " + count);
        this.lastReadMessage = this.messages[this.messages.length - 1];
        this.eventBus.changeNewMessageNumber.emit(-count);
      });
    }
  }

  private scrollToLastRead() {
    // TODO
    if (!this.messages && !this.messages.length) return;
    let el: HTMLElement;
    if (this.lastReadMessage != null) {
      el = document.getElementById(`message-${this.lastReadMessage.id}`);
    } else {
      el = document.getElementById(`message-${this.messages[0].id}`);
    }
    //console.log(el);
    //console.log("scrolling to " + el.scrollHeight);
    //el.scrollTop = el.sc;
    //this.scroll = el.scrollHeight;
  }
}
