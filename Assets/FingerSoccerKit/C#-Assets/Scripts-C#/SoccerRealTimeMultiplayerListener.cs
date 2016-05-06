using UnityEngine;
using System.Collections;
using GooglePlayGames.BasicApi.Multiplayer;

public class SoccerRealTimeMultiplayerListener : RealTimeMultiplayerListener
{

    private static SoccerRealTimeMultiplayerListener instance;

    private SoccerRealTimeMultiplayerListener() { }

    public static SoccerRealTimeMultiplayerListener Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SoccerRealTimeMultiplayerListener();
            }
            return instance;
        }
    }

    public void OnRoomSetupProgress(float progress)
    {
        Debug.Log("OnRoomSetup " + progress);
    }

    public void OnRoomConnected(bool success)
    {
        Debug.Log("OnRoomConnected " + success);
        if (success)
        {
            // Successfully connected to room!
            // ...start playing game...
            Application.LoadLevel("Game-c#");
        }
        else
        {
            // Error!
            // ...show error message to user...
        }
    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    public void OnParticipantLeft(Participant participant)
    {

    }

    public void OnPeersConnected(string[] participantIds)
    {

    }

    public void OnPeersDisconnected(string[] participantIds)
    {

    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        Debug.Log("OnRealTimeMessageReceived");
    }

}
