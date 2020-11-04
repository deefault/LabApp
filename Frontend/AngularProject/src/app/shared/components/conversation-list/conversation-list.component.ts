import {Component, EventEmitter, OnInit} from '@angular/core';
import {ChatService, ConversationWithLastMessageDto, MessageDto} from "../../../clients/common";
import {SignalRService} from "../../../services/signalr/signalr.service";
import {EventBusService} from "../../../services/event-bus.service";
import {NewMessage} from "../../../models/events/new-message";

@Component({
  selector: 'app-conversation-list',
  templateUrl: './conversation-list.component.html',
  styleUrls: ['./conversation-list.component.scss']
})
export class ConversationListComponent implements OnInit {

  loading: boolean = true;
  items: ConversationWithLastMessageDto[] = []
  selected: ConversationWithLastMessageDto;
  selectedChange: EventEmitter<ConversationWithLastMessageDto> = new EventEmitter<ConversationWithLastMessageDto>();

  constructor(
    private chatService: ChatService,
    private signalRService: SignalRService,
    private bus: EventBusService
  ) {
  }

  ngOnInit() {
    this.chatService.getConversations().subscribe(data => {
      this.items = data;
      this.loading = false;
    });
    this.signalRService.newMessage.subscribe((data: NewMessage) => {
      let toUpdate = this.items.find(x => x.id == data.ConversationId);
      if (toUpdate) {
        toUpdate.lastMessage.text = data.Text;
        toUpdate.lastMessage.sent = data.Sent;
        toUpdate.lastMessage.userId = data.UserId
        toUpdate.lastMessageUser = data.User;
      }
    });
    
    this.bus.changeNewMessageNumber.subscribe(data => {
      let toUpdate = this.items.find(x => x.id == data.conversationId);
      if (toUpdate) toUpdate.newMessages += data.num;
    })
  }
}
