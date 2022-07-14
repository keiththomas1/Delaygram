using UnityEngine;
using System;
using System.Collections.Generic;

public class NotifyScreenController : MonoBehaviour {
    private DelayGramSerializer serializer;
    private ScrollController scrollController;
    private GameObject page;
    private GameObject scrollArea;
    private List<GameObject> notificationObjects;
    private List<GameObject> dateHeaderObjects;
    private Queue<int> pendingNotifications;

    void Awake()
    {
        serializer = DelayGramSerializer.Instance;
        notificationObjects = new List<GameObject>();
        dateHeaderObjects = new List<GameObject>();
        pendingNotifications = new Queue<int>();
    }

    void Update()
    {
    }

    public void CheckClick(string colliderName)
    {
        switch (colliderName)
        {
            default:
                break;
        }
    }

    public void EnterScreen()
    {
        page = GameObject.Instantiate(Resources.Load("Notify/NotifyPage") as GameObject);
        scrollArea = page.transform.Find("ScrollArea").gameObject;

        while (pendingNotifications.Count > 0)
        {
            AddNewNotification(pendingNotifications.Dequeue());
        }

        var notifications = serializer.GetNotifications();
        notifications.Sort((a, b) => b.dateTime.CompareTo(a.dateTime));
        DisplayNotifications(notifications);
    }

    public void DestroyPage()
    {
        GameObject.Destroy(page);
    }

    public void AddNotificationToQueue(int newNotification)
    {
        pendingNotifications.Enqueue(newNotification);
    }

    private void AddNewNotification(int newNotifications)
    {
        var notificationString = newNotifications.ToString() + " people liked your photo";

        var dgNotification = new DelayGramNotification();
        dgNotification.text = notificationString;
        dgNotification.dateTime = DateTime.Now;
        serializer.SerializeNotification(dgNotification);
    }

    private void DisplayNotifications(List<DelayGramNotification> notifications)
    {
        float xPosition = 0.0f;
        float originalYPosition = 2.3f;
        float yPosition = originalYPosition;
        DateTime lastDate = DateTime.MinValue; 
        foreach (var notification in notifications)
        {
            var newDate = notification.dateTime.Date;
            if (newDate != lastDate)
            {
                var dateHeader = GenerateDateHeader(newDate, yPosition);
                dateHeader.transform.parent = scrollArea.transform;
                lastDate = newDate;
                yPosition -= 0.5f;
            }
            var notificationObject = GenerateNotification(notification.text, xPosition, yPosition);
            notificationObjects.Add(notificationObject);
            notificationObject.transform.parent = scrollArea.transform;
            yPosition -= 0.5f;
        }

        scrollController = scrollArea.AddComponent<ScrollController>();
        scrollController.UpdateScrollArea(scrollArea, scrollArea.transform.localPosition.y, -yPosition);
    }

    private GameObject GenerateDateHeader(DateTime date, float yPosition)
    {
        var dateHeader = GameObject.Instantiate(Resources.Load("Notify/DGNotifyDateHeader") as GameObject);
        var dateHeaderText = dateHeader.transform.Find("DateText");
        if (dateHeaderText)
        {
            dateHeaderText.GetComponent<TextMesh>().text = date.ToString("d");
        }
        dateHeader.transform.position = new Vector3(-1.1f, yPosition, -1.0f);
        dateHeaderObjects.Add(dateHeader);
        return dateHeader;
    }

    private GameObject GenerateNotification(string notificationString, float x, float y)
    {
        var notification = GameObject.Instantiate(Resources.Load("Notify/DGNotifyTextBack") as GameObject);
        var notifyText = notification.transform.Find("NotifyText");
        if (notifyText)
        {
            notifyText.GetComponent<TextMesh>().text = notificationString;
        }
        notification.transform.position = new Vector3(x, y, -1.0f);

        return notification;
    }
}
