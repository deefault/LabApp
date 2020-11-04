import {AfterViewInit, Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {ChatService, ConversationDto, MessageDto, UserListDto} from "../../../clients/common";
import {AuthService} from "../../../services/auth/auth.service";
import {BaseComponent} from "../../../core/base-component";
import {fromEvent, Observable, of} from "rxjs";
import {EventBusService} from "../../../services/event-bus.service";
import {SignalRService} from "../../../services/signalr/signalr.service";
import {debounceTime, tap} from "rxjs/operators";

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss']
})
export class ConversationComponent extends BaseComponent implements OnInit, AfterViewInit, OnChanges {

  messages: MessageDto[] = [];
  me: UserListDto;
  other: UserListDto;
  @Input() conversation: ConversationDto;

  @Input() conversationId: number;

  lastReadMessage: MessageDto = null;
  readonly enableReadAll: boolean = true;
  private getMessages: Observable<Array<MessageDto>>;

  chatEl: ElementRef;

  //scroll: number = 0;

  constructor(
    private chatService: ChatService,
    private authService: AuthService,
    private eventBus: EventBusService,
    private signalR: SignalRService,
  ) {
    super();
  }

  ngOnInit() {
    if (!this.conversation && !this.conversationId) throw new Error("No input parameters!!!");
    this.subscribeForReadMessages();

    this.signalR.newMessage.subscribe(m => {
      this.messages.push(m);
    })

    this.loading = true;
    let conversationSubscription: Observable<ConversationDto> = !this.conversation && this.conversationId
      ? this.chatService.getConversation(this.conversationId)
      : of(this.conversation);

    this.reloadConversation(conversationSubscription);
  }

  ngAfterViewInit() {
    if (this.getMessages) {
      this.getMessages.subscribe(_ => this.scrollToLastRead());
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["conversationId"]) {
      this.reloadConversation(this.chatService.getConversation(changes["conversationId"].currentValue))
    }
  }

  private reloadConversation(conversationSubscription: Observable<ConversationDto>) {
    this.loading = true;
    conversationSubscription.pipe(tap(data => {
      this.conversation = data;
      this.me = this.conversation.users.find(x => x.userId == this.authService.userIdentity.Id);
      this.other = this.conversation.users.find(x => x.userId != this.authService.userIdentity.Id);
    })).subscribe((conversation) => {
      this.getMessages = this.chatService.getMessages(conversation.id);
      this.getMessages.subscribe(messages => {
        this.messages = messages;
        this.setLastMessage();
        this.loading = false;
      })
    });
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
        this.eventBus.changeNewMessageNumber.emit({conversationId: this.conversation.id, num: -count});
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

  private subscribeForReadMessages() {
    let el = document.getElementById("chat-comp");
    fromEvent(el, 'mouseover').pipe(
      debounceTime(500)
    ).subscribe(x => this.readAll());
  }
}
