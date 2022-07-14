using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class MessagesScreenController : MonoBehaviour {
    private ThumbnailsList thumbnailsList;

    private GameObject page;
    private Transform pageScrollArea;
    private Transform popupScrollArea;
    private ScrollController scrollController;
    private GameObject messageContainer;

    private List<Conversation> activeConversations;
    private List<GameObject> createdStubs;
    private Conversation currentConversationWithDialog;
    private const float STUB_STARTING_X = -1.0f;
    private float stubStartingY;

	// Use this for initialization
	void Start () {
        thumbnailsList = GetComponent<ThumbnailsList>();
        activeConversations = new List<Conversation>();

        createdStubs = new List<GameObject>();
        stubStartingY = -0.7f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CheckClick(string colliderName)
    {
        if (messageContainer)
        {
            switch (colliderName)
            {
                case "Option0":
                    ConversationOptionSelected(0);
                    break;
                case "Option1":
                    ConversationOptionSelected(1);
                    break;
                case "Option2":
                    ConversationOptionSelected(2);
                    break;
                case "ExitButton":
                    GameObject.Destroy(messageContainer);
                    break;
            }
        }
        else
        {
            switch (colliderName)
            {
                default:
                    foreach (Conversation conversation in activeConversations)
                    {
                        if (colliderName == conversation.personName)
                        {
                            GenerateMessagePopup(conversation);
                        }
                    }
                    break;
            }
        }
    }

    public void EnterScreen()
    {
        page = GameObject.Instantiate(Resources.Load("Messages/DGMessagesPage") as GameObject);
        pageScrollArea = page.transform.Find("ScrollArea");

        // activeConversations = messagesController.GetActiveConversations();

        GenerateMessageStubs();
    }

    public void DestroyPage()
    {
        DestroyPopup();
        if (this.page)
        {
            GameObject.Destroy(page);
        }
    }

    public bool PopupActive()
    {
        return messageContainer;
    }

    /* Private methods */

    private void GenerateMessageStubs()
    {
        float currentYPosition = stubStartingY;
        foreach (Conversation conversation in activeConversations)
        {
            var messages = conversation.messages;
            if (messages.Count != 0)
            {
                var firstMessage = messages[0];

                CreateMessageStub(conversation.personName, firstMessage, currentYPosition);
                currentYPosition -= 1.0f;
            }
        }
    }

    private void CreateMessageStub(string name, Message message, float yPosition)
    {
        var messageStub = GameObject.Instantiate(Resources.Load("Messages/MessageStub") as GameObject);
        messageStub.name = name;
        messageStub.transform.parent = pageScrollArea;
        messageStub.transform.localPosition = new Vector3(STUB_STARTING_X, yPosition, 0.0f);

        var thumbnail = messageStub.transform.Find("Thumbnail");
        if (thumbnail)
        {
            var thumbnailSprite = thumbnailsList.GetThumbnail(name);
            thumbnail.GetComponent<SpriteRenderer>().sprite = thumbnailSprite;
        }
        var nameText = messageStub.transform.Find("NameText");
        if (nameText)
        {
            nameText.GetComponent<TextMesh>().text = name;
        }
        var previewText = messageStub.transform.Find("MessagePreview");
        if (previewText)
        {
            previewText.GetComponent<TextMesh>().text = message.text;
        }
        var timeText = messageStub.transform.Find("TimeText");
        if (timeText)
        {
            timeText.GetComponent<TextMesh>().text = this.GetMessageTimeFromDateTime(message.timeSent);
        }

        createdStubs.Add(messageStub);
    }

    private string GetMessageTimeFromDateTime(DateTime postTime)
    {
        var timeSincePost = DateTime.Now - postTime;
        if (timeSincePost.Days > 0)
        {
            return timeSincePost.Days.ToString() + " days ago";
        }
        else if (timeSincePost.Hours > 0)
        {
            return timeSincePost.Hours.ToString() + " hours ago";
        }
        else
        {
            return timeSincePost.Minutes.ToString() + " mins ago";
        }
    }

    private void GenerateMessagePopup(Conversation conversation)
    {
        messageContainer = GameObject.Instantiate(Resources.Load("Messages/MessageContainer") as GameObject);
        messageContainer.transform.localPosition = new Vector3(-0.20f, 0.74f, -1.0f);
        popupScrollArea = messageContainer.transform.Find("ScrollArea");

        var scrollHeight = 2;
        scrollController = popupScrollArea.gameObject.AddComponent<ScrollController>();
        scrollController.UpdateScrollArea(
            popupScrollArea.gameObject, popupScrollArea.transform.localPosition.y, scrollHeight);

        const float xPosition = 0.0f;
        float yPosition = 0.0f;
        foreach (Message message in conversation.messages)
        {
            yPosition = AddMessageToPopup(conversation, message, xPosition, yPosition);
        }
    }

    private float AddMessageToPopup(Conversation conversation, Message message, float xPosition, float yPosition)
    {
        if (!popupScrollArea)
        {
            return 0;
        }
        var popupMessage = GameObject.Instantiate(Resources.Load("Messages/PopupMessage") as GameObject);
        popupMessage.transform.parent = popupScrollArea;
        popupMessage.transform.localPosition = new Vector3(xPosition, yPosition, 0.0f);

        var thumbnail = popupMessage.transform.Find("Thumbnail");
        var thumbnailSprite = thumbnailsList.GetThumbnail(conversation.personName);
        thumbnail.GetComponent<SpriteRenderer>().sprite = thumbnailSprite;

        var personName = popupMessage.transform.Find("PersonName");
        personName.GetComponent<TextMesh>().text = conversation.personName;

        var bodyText = popupMessage.transform.Find("BodyText");
        bodyText.GetComponent<TextMesh>().text = message.text;

        // yPosition -= CalculateMessageHeight(bodyLineCount);

        if (message.choices.Count > 0)
        {
            currentConversationWithDialog = conversation;
        }
        int optionCount = 0;
        foreach (string option in message.choices)
        {
            var dialogOption = GameObject.Instantiate(Resources.Load("Messages/DialogOption") as GameObject);
            dialogOption.transform.parent = popupScrollArea;
            dialogOption.transform.localPosition = new Vector3(xPosition - 0.3f, yPosition + 2.2f, -1.0f);
            dialogOption.name = "Option" + optionCount.ToString();

            var dialogOptionBack = dialogOption.transform.Find("DialogOptionBack");
            if (dialogOptionBack)
            {
                dialogOptionBack.localScale = new Vector3(1.0f, 1, 1.0f);
            }

            var dialogOptionText = dialogOption.transform.Find("DialogText");
            if (dialogOptionText)
            {
                dialogOptionText.GetComponent<TextMesh>().text = option;
            }

            // yPosition -= CalculateDialogHeight(1); // optionLineCount);
            optionCount++;
        }

        return yPosition;
    }

    private void ConversationOptionSelected(int index)
    {
        string response = "";
        foreach (Message message in currentConversationWithDialog.messages)
        {
            if (message.choices.Count > index)
            {
                response = message.choices[index];

                break;
            }
        }

        // messagesController.AddDialogToConversation(response, currentConversationWithDialog.name);
    }

    public void DestroyPopup()
    {
        if (this.messageContainer)
        {
            GameObject.Destroy(this.messageContainer);
        }
    }
}
