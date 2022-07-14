using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    private CharacterSerializer _characterSerializer;
    private CharacterSpriteController _spriteController;
    [SerializeField]
    private bool _loadFromSave;

    private void Awake()
    {
        this._spriteController = GameObject.Find("CONTROLLER").GetComponent<CharacterSpriteController>();
    }

    // Use this for initialization
    void Start () {
        this._characterSerializer = CharacterSerializer.Instance;
        if (this._loadFromSave)
        {
            this._characterSerializer.SetCharacterCustomization(this);
        }
        if (!this._characterSerializer.IsLoaded())
        {
            this._characterSerializer.LoadGame();
        } else {
            this._characterSerializer.UpdateAllCharacters();
        }
    }

    // Update is called once per frame
    void Update() {
    }

    public void SetCharacterLook(CharacterProperties properties)
    {
        if (properties.gender == Gender.Female)
        {
            SetBodySprite(properties.bodySprite);
        }
        SetFaceSprite(properties.faceSprite);
        SetSkinColor(properties.skinColor.GetColor());
        SetHairColor(properties.hairColor.GetColor());
        SetShirtColor(properties.shirtColor.GetColor());
    }

    public void SetBodySprite(string spriteName)
    {
        if (this._spriteController)
        {
            var bodySprites = this._spriteController.BodySprites;
            foreach (Sprite sprite in bodySprites)
            {
                if (sprite.name == spriteName)
                {
                    this.transform.Find("Body").GetComponent<SpriteRenderer>().sprite = sprite;
                    return;
                }
            }
        }
    }
    public void SetFaceSprite(string spriteName)
    {
        if (this._spriteController)
        {
            var faceSprites = this._spriteController.FaceSprites;
            foreach (Sprite sprite in faceSprites)
            {
                if (sprite.name == spriteName)
                {
                    var head = this.transform.Find("Head");
                    head.Find("Face").GetComponent<SpriteRenderer>().sprite = sprite;
                    return;
                }
            }
        }
    }
    public void SetSkinColor(Color color)
    {
        this.transform.Find("Head").GetComponent<SpriteRenderer>().color = color;
        this.transform.Find("LeftArm").GetComponent<SpriteRenderer>().color = color;
        this.transform.Find("RightArm").GetComponent<SpriteRenderer>().color = color;
    }
    public void SetHairColor(Color color)
    {
        this.transform.Find("Body").GetComponent<SpriteRenderer>().color = color;
    }
    public void SetShirtColor(Color color)
    {
        this.transform.Find("Head").transform.Find("Hair").GetComponent<SpriteRenderer>().color = color;
    }
}
