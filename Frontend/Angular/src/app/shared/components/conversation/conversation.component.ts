import {Component, Input, OnInit} from '@angular/core';
import {ChatService, ConversationDto, MessageDto, UserListDto} from "../../../clients/common";
import {AuthService} from "../../../services/auth/auth.service";
import {BaseComponent} from "../../../core/base-component";

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss']
})
export class ConversationComponent extends BaseComponent implements OnInit {

  messages: MessageDto[] = [];
  me: UserListDto;
  other: UserListDto;
  @Input() conversation: ConversationDto;

  constructor(
    private chatService: ChatService,
    private authService: AuthService
  ) {
    super();
  }

  ngOnInit() {
    this.loading = true;
    this.me = this.conversation.users.find(x => x.userId == this.authService.userIdentity.Id);
    this.other = this.conversation.users.find(x => x.userId != this.authService.userIdentity.Id);
    this.chatService.getMessages(this.conversation.id).subscribe(data => {
      this.messages = data;
      this.loading = false;
    });
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
}
