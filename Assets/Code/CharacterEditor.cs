using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditor : MonoBehaviour {
    [SerializeField]
    private GameObject _randomPersonButton;
    private CharacterSerializer _characterSerializer;
    private CharacterSpriteController _spriteController;
    private List<Color> _skinColors;

    // Use this for initialization
    void Start ()
    {
        this._characterSerializer = CharacterSerializer.Instance;
        this._spriteController = GameObject.Find("CONTROLLER").GetComponent<CharacterSpriteController>();
        if (!this._characterSerializer.IsLoaded())
        {
            this._characterSerializer.LoadGame();
        }

        this._skinColors = new List<Color>();
        this.LoadSkinColors();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == this._randomPersonButton.name)
                {
                    // var bodySprite = GetRandomBodySprite();
                    // this._characterCustomization.SetBodySprite(bodySprite);
                    // this._characterSerializer.BodySprite = bodySprite;

                    var face = this.GetRandomFaceSprite();
                    this._characterSerializer.FaceSprite = face;

                    var skinColor = this.GetRandomSkinColor();
                    this._characterSerializer.SkinColor = skinColor;

                    var hairColor = this.GetRandomColor();
                    this._characterSerializer.HairColor = hairColor;

                    var shirtColor = this.GetRandomColor();
                    this._characterSerializer.ShirtColor = shirtColor;

                    this._characterSerializer.SaveGame();
                }
            }
        }
    }

    // private string GetRandomBodySprite()
    // {
    //     var bodySprites = this._characterCustomization.BodySprites;
    //     return bodySprites[Random.Range(0, bodySprites.Count - 1)].name;
    // }

    private string GetRandomFaceSprite()
    {
        var faceSprites = this._spriteController.FaceSprites;
        return faceSprites[Random.Range(0, faceSprites.Count - 1)].name;
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }

    Color GetRandomSkinColor()
    {
        var randomColor = Random.Range(0, this._skinColors.Count);
        return this._skinColors[randomColor];
    }

    private void LoadSkinColors()
    {
        // In increasing order of light to dark
        Color currentColor;
        ColorUtility.TryParseHtmlString("#FFF4D0FF", out currentColor);
        this._skinColors.Add(currentColor);
        ColorUtility.TryParseHtmlString("#FFE89EFF", out currentColor);
        this._skinColors.Add(currentColor);
        ColorUtility.TryParseHtmlString("#EACE70FF", out currentColor);
        this._skinColors.Add(currentColor);
        ColorUtility.TryParseHtmlString("#D2B656FF", out currentColor);
        this._skinColors.Add(currentColor);
        ColorUtility.TryParseHtmlString("#BA9E40FF", out currentColor);
        this._skinColors.Add(currentColor);
        ColorUtility.TryParseHtmlString("#967E2FFF", out currentColor);
        this._skinColors.Add(currentColor);
        ColorUtility.TryParseHtmlString("#6D570FFF", out currentColor);
        this._skinColors.Add(currentColor);
    }
}
