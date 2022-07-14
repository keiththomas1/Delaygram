using System;
using System.Collections.Generic;

public enum MessageTriggerType
{
    NewPost
}

public class MessagePost
{
    private static MessagePost _instance;
    private MessagesSerializer _messageSerializer;

    public static MessagePost Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MessagePost();
            }
            return _instance;
        }
    }

    private MessagePost()
    {
        this._messageSerializer = MessagesSerializer.Instance;
    }

    public void TriggerActivated(MessageTriggerType trigger)
    {
        switch(trigger)
        {
            case MessageTriggerType.NewPost:
                var newConversation = new Conversation();
                newConversation.messages = new List<Message>();
                var newMessage = new Message();
                newMessage.text = "Hi! This is just to test out how everything is working. " +
                    "So yeah, let's just keep talking. This and that. Here and there. I want to " +
                    "make sure that the message system can support a long message. So yeah that's " +
                    "about it. See ya later!";
                newMessage.type = MessageType.NPC;
                newMessage.timeSent = DateTime.Now;
                newConversation.messages.Add(newMessage);
                newConversation.personName = "Karen";
                newConversation.viewed = false;
                this._messageSerializer.AddConversation(newConversation);
                break;
            default:
                break;
        }
    }
}
