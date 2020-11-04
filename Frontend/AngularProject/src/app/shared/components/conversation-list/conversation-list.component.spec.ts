import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConversationListComponent } from './conversation-list.component';

describe('ConversationListComponent', () => {
  let component: ConversationListComponent;
  let fixture: ComponentFixture<ConversationListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConversationListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConversationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  let fake = [
    "{\"id\":1,\"name\":null,\"type\":1,\"lastMessage\":{\"id\":11,\"text\":\"werwerwerwer\",\"userId\":1,\"sent\":\"2020-11-01T19:27:56.121265\",\"conversationId\":1,\"attachments\":[]},\"lastMessageUser\":{\"userId\":1,\"name\":\"qweqwe\",\"surname\":\"qweqwe\",\"middlename\":\"222\",\"userType\":1,\"photoId\":null,\"thumbnail\":null},\"newMessages\":0}",
    "{\"id\":1,\"name\":null,\"type\":1,\"lastMessage\":{\"id\":11,\"text\":\"werwerwerwer\",\"userId\":1,\"sent\":\"2020-11-01T19:27:56.121265\",\"conversationId\":1,\"attachments\":[]},\"lastMessageUser\":{\"userId\":1,\"name\":\"qweqwe\",\"surname\":\"qweqwe\",\"middlename\":\"222\",\"userType\":1,\"photoId\":null,\"thumbnail\":null},\"newMessages\":3}",
    "{\"id\":1,\"name\":null,\"type\":1,\"lastMessage\":{\"id\":11,\"text\":\"werwerwerwer\",\"userId\":1,\"sent\":\"2020-11-01T19:27:56.121265\",\"conversationId\":1,\"attachments\":[]},\"lastMessageUser\":{\"userId\":1,\"name\":\"qweqwe\",\"surname\":\"qweqwe\",\"middlename\":\"222\",\"userType\":1,\"photoId\":null,\"thumbnail\":null},\"newMessages\":0}",
    "{\"id\":1,\"name\":null,\"type\":1,\"lastMessage\":{\"id\":11,\"text\":\"werwerwerwer\",\"userId\":1,\"sent\":\"2020-11-01T19:27:56.121265\",\"conversationId\":1,\"attachments\":[]},\"lastMessageUser\":{\"userId\":1,\"name\":\"qweqwe\",\"surname\":\"qweqwe\",\"middlename\":\"222\",\"userType\":1,\"photoId\":null,\"thumbnail\":null},\"newMessages\":2}",
    "{\"id\":1,\"name\":null,\"type\":1,\"lastMessage\":{\"id\":11,\"text\":\"werwerwerwer\",\"userId\":1,\"sent\":\"2020-11-01T19:27:56.121265\",\"conversationId\":1,\"attachments\":[]},\"lastMessageUser\":{\"userId\":1,\"name\":\"qweqwe\",\"surname\":\"qweqwe\",\"middlename\":\"222\",\"userType\":1,\"photoId\":null,\"thumbnail\":null},\"newMessages\":7}"
  ]
});
