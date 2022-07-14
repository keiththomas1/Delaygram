using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DelayGramSerializer
{
    private List<MonoBehaviour> followerListeners = new List<MonoBehaviour>();
    private static DelayGramSerializer instance;
    private DelayGramSaveVariables currentSave;
    private float[] likeTimePoints;
    private int likesThisSession = 0;

    // For saving/loading
    private string savePath;

    public static DelayGramSerializer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DelayGramSerializer();
            }
            return instance;
        }
    }

    private DelayGramSerializer()
    {
        Initialize();
    }

    // Use this for initialization
    void Initialize()
    {
        FillTimePoints();
        savePath = Application.persistentDataPath + "/DelayGram.dat";
    }

    public void RegisterFollowersListener(MonoBehaviour listener)
    {
        followerListeners.Add(listener);
    }

    public void UnregisterFollowersListener(MonoBehaviour listener)
    {
        followerListeners.Remove(listener);
    }

    public List<DelayGramPost> GetReverseChronologicalPosts()
    {
        List<DelayGramPost> returnList = new List<DelayGramPost>();
        if (currentSave.posts != null)
        {
            returnList = currentSave.posts;
            returnList.Reverse();
        }
        return returnList;
    }

    public List<DelayGramNotification> GetNotifications()
    {
        List<DelayGramNotification> returnList = new List<DelayGramNotification>();
        if (currentSave.notifications != null)
        {
            returnList = currentSave.notifications;
            returnList.Reverse();
        }
        return returnList;
    }

    public void FillTimePoints()
    {
        likeTimePoints = new float[10];
        likeTimePoints[0] = 0.1f;
        likeTimePoints[1] = 0.1f;
        likeTimePoints[2] = 0.1f;
        likeTimePoints[3] = 0.1f;
        likeTimePoints[4] = 0.1f;
        likeTimePoints[5] = 0.1f;
        likeTimePoints[6] = 0.1f;
        likeTimePoints[7] = 0.1f;
        likeTimePoints[8] = 0.1f;
    }

    public void AddFollowers(int followers)
    {
        currentSave.followers += followers;

        foreach (MonoBehaviour listener in followerListeners)
        {
            if (listener)
            {
                listener.BroadcastMessage("OnFollowersUpdated", currentSave.followers);
            } else {
                followerListeners.Remove(listener);
            }
        }

        SaveGame();
    }

    public int Followers
    {
        get { return currentSave.followers; }
    }

    public void AddEndorsement(string endorsement)
    {
        currentSave.endorsements.Add(endorsement);
        SaveGame();
    }

    public List<string> Endorsements
    {
        get { return currentSave.endorsements; }
    }

    public int GetLikes()
    {
        return likesThisSession;
    }

    public void ClearInfo()
    {
        likesThisSession = 0;
    }

    private void UpdateInformation()
    {
        var currentTime = DateTime.Now;
        var minsSinceLastUpdate = (currentTime - currentSave.lastUpdate).Minutes;
        if (minsSinceLastUpdate < 5) { return; }

        var totalLikes = 0;
        foreach (DelayGramPost post in currentSave.posts)
        {
            if (post.dateTime > DateTime.Now.Subtract(TimeSpan.FromDays(1.0f)))
            {
                // 1 like per 10 minutes, starting at 5 mins, 15 mins, etc.
                int newLikes = (minsSinceLastUpdate  + 5) / 10;
                if (currentSave.followers >= 10)
                {
                    newLikes *= (currentSave.followers + 10) / 10; // 1 extra like per 10 followers
                }
                post.likes += newLikes;
                totalLikes += newLikes;
            }
        }

        currentSave.lastUpdate = currentTime;
        SaveGame();

        likesThisSession = totalLikes;
    }

    public void SerializePost(DelayGramPost newPost)
    {
        if (currentSave.posts == null)
        {
            currentSave.posts = new List<DelayGramPost>();
        }
        currentSave.posts.Add(newPost);
        SaveGame();
    }

    public void SerializeNotification(DelayGramNotification newNotification)
    {
        if (currentSave.notifications == null)
        {
            currentSave.notifications = new List<DelayGramNotification>();
        }
        currentSave.notifications.Add(newNotification);
        SaveGame();
    }

    public DateTime NextPostTime
    {
        get { return currentSave.nextPostTime; }
        set
        {
            currentSave.nextPostTime = value;
            SaveGame();
        }
    }

    public void SerializePostCooldown(DateTime nextPostTime)
    {
        currentSave.nextPostTime = nextPostTime;
        SaveGame();
    }

    public void SaveGame()
    {
        Thread oThread = new Thread(new ThreadStart(SaveGameThread));
        oThread.Start();
    }

    public void SaveGameThread()
    {
        FileStream file = File.Open(savePath, FileMode.OpenOrCreate);

        if (file.CanWrite)
        {
            BinaryFormatter bf = new BinaryFormatter();
            currentSave.lastUpdate = DateTime.Now;
            bf.Serialize(file, currentSave);
            Debug.Log("Saved delay gram file");
        }
        else
        {
            Debug.Log("Problem opening " + file.Name + " for writing");
        }

        file.Close();
    }

    public bool LoadGame()
    {
        bool fileLoaded = false;
        if (File.Exists(savePath))
        {
            FileStream file = File.Open(savePath, FileMode.Open);

            if (file.CanRead)
            {
                BinaryFormatter bf = new BinaryFormatter();
                currentSave = (DelayGramSaveVariables)bf.Deserialize(file);
                Debug.Log("Save game loaded from " + savePath);
                fileLoaded = true;
            }

            file.Close();
        }

        if (!fileLoaded)
        {
            currentSave = new DelayGramSaveVariables();
            currentSave.lastUpdate = DateTime.Now;
            currentSave.followers = 0;
            currentSave.posts = new List<DelayGramPost>();
            currentSave.notifications = new List<DelayGramNotification>();
            currentSave.endorsements = new List<string>();
            currentSave.nextPostTime = DateTime.Now;
            SaveGame();
        }

        UpdateInformation();
        return fileLoaded;
    }
}

[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;
    public SerializableVector3(Vector3 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }
}

[Serializable]
public class DelayGramPost
{
    public DateTime dateTime;
    public string imageID;
    public string playerName;
    public string backgroundName;
    public SerializableVector3 avatarPosition;
    public CharacterProperties characterProperties;
    public int likes;
    public int dislikes;
}

[Serializable]
public class DelayGramNotification
{
    public string text;
    public DateTime dateTime;
}

// Can speed up in the future by turning bought items into a bool array.
[Serializable]
class DelayGramSaveVariables
{
    public DateTime lastUpdate;
    public int followers;
    public List<DelayGramPost> posts;
    public List<DelayGramNotification> notifications;
    public List<string> endorsements;
    public DateTime nextPostTime;
}