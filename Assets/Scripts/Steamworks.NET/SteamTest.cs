using Steamworks;
using UnityEngine;

public class SteamTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!SteamManager.Initialized)
        {
            Debug.Log("Steam Connection Failed");
            return;
        }

        string name = SteamFriends.GetPersonaName();
        Debug.Log(name);
    }

    public void TestAchievement()
    {
        if(SteamManager.Initialized)
        {
            SteamUserStats.SetAchievement("ACH_START_GAME");
            SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");

            SteamUserStats.StoreStats();

            Debug.Log("Test Achievement");
        }

    }
}
