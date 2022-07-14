using UnityEngine;
using System;
using TMPro;

public delegate void CreatePostCallBack(DelayGramPost post);

public class NewPostController : MonoBehaviour
{
    private SoundController soundController;
    private DelayGramSerializer dgSerializer;
    private CharacterSerializer characterSerializer;
    private GlobalVars _globalVars;
    private MessagePost _messagePost;

    private GameObject _postPopupWindow;
    private Transform scrollArea;
    private GameObject _avatar;
    private CreatePostCallBack _postCallBack;

    private string _currentBackground;

    static private string characters = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private enum NewPostState
    {
        BackgroundSelection,
        Cropping
    }
    private NewPostState _currentPostState;

    void Awake()
    {
        this.soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
        this.dgSerializer = DelayGramSerializer.Instance;
        this.characterSerializer = CharacterSerializer.Instance;
        this._globalVars = GlobalVars.Instance;
        this._messagePost = MessagePost.Instance;

        this._currentPostState = NewPostState.BackgroundSelection;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                switch (hit.collider.name)
                {
                    default:
                        break;
                }
            }
        }
    }

    public bool PopupActive()
    {
        return this._postPopupWindow;
    }

    public void CreatePopup(CreatePostCallBack callBack)
    {
        this._postCallBack = callBack;
        var postPopupWindowPrefab = Resources.Load("Posts/NewPostPopup") as GameObject;
        if (postPopupWindowPrefab)
        {
            this._postPopupWindow = GameObject.Instantiate(postPopupWindowPrefab);
            this._postPopupWindow.transform.position = new Vector3(2.24f, -.04f, -3.0f);

            var newPost = this._postPopupWindow.transform.Find("NewPost");
            var picture = newPost.transform.Find("Picture");
            switch (this.characterSerializer.Gender)
            {
                case Gender.Male:
                    this._avatar = picture.transform.Find("MaleAvatar").gameObject;
                    break;
                case Gender.Female:
                default:
                    this._avatar = picture.transform.Find("FemaleAvatar").gameObject;
                    break;
            }
        }
    }

    public void CheckClick(string colliderName)
    {
        switch (colliderName)
        {
            case "NewPostDoneButton":
                this.soundController.PlayLikeSound();
                this.CreateNewPost();
                this.DestroyPopup();
                break;
            case "NewPostExitButton":
                this.soundController.PlayLikeSound();
                this.DestroyPopup();
                break;
            case "BeachBackground":
                this._currentBackground = "Beach";
                this.GotoNewState(NewPostState.Cropping);
                break;
            case "CityBackground":
                this._currentBackground = "City";
                this.GotoNewState(NewPostState.Cropping);
                break;
            case "BackButton":
                switch(this._currentPostState)
                {
                    case NewPostState.BackgroundSelection:
                        this.DestroyPopup();
                        break;
                    case NewPostState.Cropping:
                        this.GotoNewState(NewPostState.BackgroundSelection);
                        break;
                }
                break;
        }
    }

    public void DestroyPopup()
    {
        GameObject.Destroy(this._postPopupWindow);
    }

    public string GetRandomImageID()
    {
        string id = "";

        for (int i = 0; i < 20; i++)
        {
            var randomIndex = UnityEngine.Random.Range(0, characters.Length);
            id += characters[randomIndex];
        }

        return id;
    }

    private void GotoNewState(NewPostState newState)
    {
        switch (newState)
        {
            case NewPostState.BackgroundSelection:
                break;
            case NewPostState.Cropping:
                this._postPopupWindow.transform.Find("BeachBackground").gameObject.SetActive(false);
                this._postPopupWindow.transform.Find("CityBackground").gameObject.SetActive(false);
                this._postPopupWindow.transform.Find("ChooseText").GetComponent<TextMeshPro>().text
                    = "Edit your photo:";
                this._postPopupWindow.transform.Find("NewPostDoneButton").gameObject.SetActive(true);

                var post = this._postPopupWindow.transform.Find("NewPost");
                post.gameObject.SetActive(true);
                var picture = post.transform.Find("Picture");
                switch (this._currentBackground)
                {
                    case "Beach":
                        picture.transform.Find("BeachBackground").gameObject.SetActive(true);
                        break;
                    case "City":
                        picture.transform.Find("CityBackground").gameObject.SetActive(true);
                        break;
                }
                break;
        }

        this._currentPostState = newState;
    }

    private void CreateNewPost()
    {
        this.dgSerializer.NextPostTime = DateTime.Now.AddMinutes(20.0f);

        var delayGramPost = this.CreateNewPostDataStructure();

        this._messagePost.TriggerActivated(MessageTriggerType.NewPost);
        this._postCallBack(delayGramPost);
    }

    private DelayGramPost CreateNewPostDataStructure()
    {
        var newPost = new DelayGramPost();
        newPost.playerName = this._globalVars.PlayerName;
        newPost.imageID = GetRandomImageID();
        newPost.backgroundName = this._currentBackground;
        newPost.avatarPosition = new SerializableVector3(this._avatar.transform.localPosition);
        newPost.characterProperties = this.characterSerializer.CurrentCharacterProperties;
        newPost.likes = 0;
        newPost.dislikes = 0;
        newPost.dateTime = DateTime.Now;

        this.dgSerializer.SerializePost(newPost);
        return newPost;
    }
}
