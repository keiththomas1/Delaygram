﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Page
{
    Home,
    Profile,
    Post,
    Explore,
    Messages
}

public class UIController : MonoBehaviour {
    [SerializeField]
    private GameObject _bottomNavBackground;
    [SerializeField]
    private GameObject _homeButton;
    [SerializeField]
    private GameObject _profileButton;
    [SerializeField]
    private GameObject _postButton;
    [SerializeField]
    private GameObject _exploreButton;
    [SerializeField]
    private GameObject _messagesButton;

    [SerializeField]
    private Sprite _homeButtonUnselected;
    [SerializeField]
    private Sprite _homeButtonSelected;
    [SerializeField]
    private Sprite _profileButtonUnselected;
    [SerializeField]
    private Sprite _profileButtonSelected;
    [SerializeField]
    private Sprite _postButtonUnselected;
    [SerializeField]
    private Sprite _postButtonSelected;
    [SerializeField]
    private Sprite _exploreButtonUnselected;
    [SerializeField]
    private Sprite _exploreButtonSelected;
    [SerializeField]
    private Sprite _messagesButtonUnselected;
    [SerializeField]
    private Sprite _messagesButtonSelected;

    private HomeScreenController _homeController;
    private ProfileScreenController _profileController;
    private PostScreenController _postController;
    private ExploreScreenController _exploreController;
    private MessagesScreenController _messagesController;
    private EventController _eventController;
    private SoundController _soundController;

    private Page _currentPage;

    // Use this for initialization
    void Start () {
        this._bottomNavBackground.GetComponent<Image>().enabled = true;
        this._homeButton.GetComponent<Button>().onClick.AddListener(this.OnHomeClick);
        this._profileButton.GetComponent<Button>().onClick.AddListener(this.OnProfileClick);
        this._postButton.GetComponent<Button>().onClick.AddListener(this.OnPostClick);
        this._exploreButton.GetComponent<Button>().onClick.AddListener(this.OnExploreClick);
        this._messagesButton.GetComponent<Button>().onClick.AddListener(this.OnMessagesClick);

        this._homeController = GetComponent<HomeScreenController>();
        this._profileController = GetComponent<ProfileScreenController>();
        this._postController = GetComponent<PostScreenController>();
        this._exploreController = GetComponent<ExploreScreenController>();
        this._messagesController = GetComponent<MessagesScreenController>();
        this._eventController = GetComponent<EventController>();
        this._soundController = GameObject.Find("SoundController").GetComponent<SoundController>();

        this.OnProfileClick();
    }
	
	// Update is called once per frame
	void Update () {
    }

    public bool BackOut()
    {
        return false;
    }

    public Page GetCurrentPage()
    {
        return this._currentPage;
    }

    /* Private methods */

    private void OnHomeClick()
    {
        GenerateHomePage();
        this._soundController.PlayClickSound(1);
        UpdateButtonState();
        this._eventController.ClearNotifications(this._currentPage);
    }
    private void OnProfileClick()
    {
        GenerateProfilePage();
        this._soundController.PlayClickSound(1);
        UpdateButtonState();
        this._eventController.ClearNotifications(this._currentPage);
    }
    private void OnPostClick()
    {
        GeneratePostPage();
        this._soundController.PlayClickSound(1);
        UpdateButtonState();
        this._eventController.ClearNotifications(this._currentPage);
    }
    private void OnExploreClick()
    {
        GenerateExplorePage();
        this._soundController.PlayClickSound(1);
        UpdateButtonState();
        this._eventController.ClearNotifications(this._currentPage);
    }
    private void OnMessagesClick()
    {
        GenerateMessagesPage();
        this._soundController.PlayClickSound(1);
        UpdateButtonState();
        this._eventController.ClearNotifications(this._currentPage);
    }

    private void GenerateHomePage()
    {
        if (this._currentPage != Page.Home)
        {
            DestroyPage(this._currentPage);
            this._homeController.EnterScreen();
            this._currentPage = Page.Home;
        }
    }
    private void GenerateProfilePage()
    {
        if (this._currentPage != Page.Profile)
        {
            DestroyPage(this._currentPage);
            this._profileController.EnterScreen();
            this._currentPage = Page.Profile;
        }
    }
    private void GeneratePostPage()
    {
        if (this._currentPage != Page.Post)
        {
            DestroyPage(this._currentPage);
            this._postController.EnterScreen();
            this._currentPage = Page.Post;
        }
    }
    private void GenerateExplorePage()
    {
        if (this._currentPage != Page.Explore)
        {
            DestroyPage(this._currentPage);
            this._exploreController.EnterScreen();
            this._currentPage = Page.Explore;
        }
    }
    private void GenerateMessagesPage()
    {
        if (this._currentPage != Page.Messages)
        {
            DestroyPage(this._currentPage);
            this._messagesController.EnterScreen();
            this._currentPage = Page.Messages;
        }
    }

    private void DestroyPage(Page page)
    {
        switch (page)
        {
            case Page.Home:
                this._homeController.DestroyPage();
                break;
            case Page.Profile:
                this._profileController.DestroyPage();
                break;
            case Page.Post:
                this._postController.DestroyPage();
                break;
            case Page.Explore:
                this._exploreController.DestroyPage();
                break;
            case Page.Messages:
                this._messagesController.DestroyPage();
                break; 
        }
    }

    private void UpdateButtonState()
    {
        this._homeButton.GetComponent<Image>().sprite = (this._currentPage == Page.Home) ? this._homeButtonSelected : this._homeButtonUnselected;
        this._profileButton.GetComponent<Image>().sprite = (this._currentPage == Page.Profile) ? this._profileButtonSelected : this._profileButtonUnselected;
        this._postButton.GetComponent<Image>().sprite = (this._currentPage == Page.Post) ? this._postButtonSelected : this._postButtonUnselected;
        this._exploreButton.GetComponent<Image>().sprite = (this._currentPage == Page.Explore) ? this._exploreButtonSelected : this._exploreButtonUnselected;
        this._messagesButton.GetComponent<Image>().sprite = (this._currentPage == Page.Messages) ? this._messagesButtonSelected : this._messagesButtonUnselected;
    }
}
