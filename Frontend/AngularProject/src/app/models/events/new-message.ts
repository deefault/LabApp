import {UserListDto} from "../../clients/common";

export class NewMessage {
    MessageId: number;
    UserId: number;
    User: UserListDto;
    Sent: Date;
    ConversationId: number
    Users: number[] = []
    Text: string
}