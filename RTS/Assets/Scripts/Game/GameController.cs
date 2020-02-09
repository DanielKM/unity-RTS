using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class GameController : MonoBehaviour
{    
    static bool ListenForFriendsMessages { get; set; }
    static bool IsOverlayEnabled { get; }

    void Awake()
    {
        try
        {
            Steamworks.SteamClient.Init( 480 );
        }
        catch ( System.Exception e )
        {
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
        }
        var playername = SteamClient.Name;
        var playersteamid = SteamClient.SteamId;
        int value = 0;
        // Debug.Log(SteamClient.Name);
        SteamScreenshots.TriggerScreenshot();
        Steamworks.SteamUserStats.SetStat( "deaths", value );
        foreach ( var a in SteamUserStats.Achievements )
        {
            // Debug.Log( $"{a.Name} ({a.State})" );
        }	
        foreach ( var player in SteamFriends.GetFriends() )
        {
            // Debug.Log( $"{player.Name}" );
        }

    }

    void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }


    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
        Steamworks.SteamClient.Shutdown();
    }

    public void TriggerAchievement(string achievementId)
    {
    }
    
}