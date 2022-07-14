using UnityEngine;

public class ThumbnailsList : MonoBehaviour
{
    public Sprite mainCharacter;
    public Sprite whiteGirlBlondeHair;
    public Sprite blackGirlBlondeHair;
    public Sprite blackDudeNoseRing;
    public Sprite whiteDudeAthlete;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Sprite GetThumbnail(string name)
    {
        switch (name)
        {
            case "Karen":
                return blackGirlBlondeHair;
            default:
                return whiteDudeAthlete;
        }
    }
}
