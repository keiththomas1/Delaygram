using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostHelper {

	// Use this for initialization
	public PostHelper() {
	}

    public void PopulatePostFromData(GameObject post, DelayGramPost data) {
        var postPicture = post.transform.Find("Picture");
        if (postPicture)
        {
            GameObject avatar;
            switch (data.characterProperties.gender)
            {
                case Gender.Male:
                    avatar = postPicture.transform.Find("MaleAvatar").gameObject;
                    postPicture.transform.Find("FemaleAvatar").gameObject.SetActive(false);
                    break;
                case Gender.Female:
                default:
                    avatar = postPicture.transform.Find("FemaleAvatar").gameObject;
                    postPicture.transform.Find("MaleAvatar").gameObject.SetActive(false);
                    break;
            }
            avatar.SetActive(true);
            avatar.transform.localPosition = new Vector3(
                data.avatarPosition.x,
                data.avatarPosition.y,
                data.avatarPosition.z);
            var avatarCustomization = avatar.GetComponent<CharacterCustomization>();
            avatarCustomization.SetCharacterLook(data.characterProperties);
        }

        switch (data.backgroundName)
        {
            case "City":
                var beachBackground = GameObject.Instantiate(Resources.Load("Posts/Backgrounds/CityBackground") as GameObject);
                beachBackground.transform.parent = postPicture.transform;
                beachBackground.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
                break;
            case "Beach":
            default:
                var cityBackground = GameObject.Instantiate(Resources.Load("Posts/Backgrounds/BeachBackground") as GameObject);
                cityBackground.transform.parent = postPicture.transform;
                cityBackground.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
                break;
        }

        var likesText = post.transform.Find("LikeText");
        float likesPercentage = 100.0f;
        float dislikePercentage = 0.0f;
        if (data.likes + data.dislikes > 0)
        {
            likesPercentage = Mathf.Floor(((float)data.likes) / ((float)(data.likes + data.dislikes)) * 100.0f);
            dislikePercentage = Mathf.Ceil(((float)data.dislikes) / ((float)(data.likes + data.dislikes)) * 100.0f);
        }
        likesText.GetComponent<TextMeshPro>().text = likesPercentage.ToString() + "%";

        var likeDislikeBar = post.transform.Find("LikeDislikeBar");
        var likeBar = likeDislikeBar.transform.Find("LikeBar");
        if (likeBar)
        {
            likeBar.transform.localScale = new Vector3(
                likesPercentage / 100.0f,
                likeBar.transform.localScale.y,
                likeBar.transform.localScale.z);
        }
        var dislikeBar = likeDislikeBar.transform.Find("DislikeBar");
        if (dislikeBar)
        {
            dislikeBar.transform.localScale = new Vector3(
                dislikePercentage / 100.0f,
                dislikeBar.transform.localScale.y,
                dislikeBar.transform.localScale.z);
        }
    }
}
