using UnityEngine;

public class LoadController : MonoBehaviour {
    private bool gameLoaded = false;
    private GlobalVars globalVars;
    private DelayGramSerializer dgSerializer;
    private MessagesSerializer messagesSerializer;

    // Use this for initialization
    void Start()
    {
        LoadState();
    }
	
	// Update is called once per frame
	void Update () {
    }

    // Consider OnApplicationFocus also (when keyboard is brought up on android for instance)
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // Paused game
        {
            SaveState();
        }
        else // Resumed game
        {
            gameLoaded = false;
            LoadState();
        }
    }

    void SaveState()
    {
        if (dgSerializer != null)
        {
            dgSerializer.SaveGame();
        }
        if (globalVars != null)
        {
            globalVars.SaveGame();
        }
    }

    void LoadState()
    {
        if (!gameLoaded)
        {
            globalVars = GlobalVars.Instance;
            dgSerializer = DelayGramSerializer.Instance;
            messagesSerializer = MessagesSerializer.Instance;
            globalVars.LoadGame();
            dgSerializer.LoadGame();
            messagesSerializer.LoadGame();

            gameLoaded = true;
        }
    }
}
