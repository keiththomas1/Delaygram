using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _bodySprites;
    [SerializeField]
    private List<Sprite> _faceSprites;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    }

    public List<Sprite> BodySprites
    {
        get { return this._bodySprites; }
    }

    public List<Sprite> FaceSprites
    {
        get { return this._faceSprites; }
    }
}
