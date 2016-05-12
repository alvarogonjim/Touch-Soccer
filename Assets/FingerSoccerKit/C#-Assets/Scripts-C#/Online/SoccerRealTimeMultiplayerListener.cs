using UnityEngine;
using System.Collections;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;
using System;
using GooglePlayGames;
using System.Collections.Generic;

public class SoccerRealTimeMultiplayerListener : RealTimeMultiplayerListener
{

    public enum State
    {
        WAIT,
        MY_TURN
    }
    private IFormatter formatter = new BinaryFormatter();
    private static SoccerRealTimeMultiplayerListener instance;
    private OnlineAI ai;
    private State currentState = State.WAIT;
    private bool playerOneDecided = false;

    private SoccerRealTimeMultiplayerListener() {}

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

    public OnlineAI AI
    {
        get{return ai;}
        set{ai = value;}
    }

    public State CurrentState
    {
        get{return currentState;}
        set{currentState = value;}
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
            DecideWhoIsPlayerOne();
            Application.LoadLevel("Game-c#");

            //Application.LoadLevel("Game-c#");
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
        OnlineMessage message = deserializeMessage(data);
        if (!playerOneDecided)
        {
            Debug.LogFormat("OnRealTimeMessageReceived - decide p1 - %s, %s", PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId, message.participantIdPlayerOne);
            GlobalGameManager.amIPlayerOne = PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId.Equals(message.participantIdPlayerOne);
            CurrentState = GlobalGameManager.amIPlayerOne ? State.MY_TURN : State.WAIT;
        }
        else
        {
            ReceiveShoot(message.unitIndex, message.force);
        }
        //switch (CurrentState)
        //{
        //    case State.WAIT:
                
        //        break;
        //    case State.MY_TURN:
        //        Debug.Log("OnRealTimeMessageReceived - mensaje en mi turno? WTF!!");
        //        break;
        //    default:
        //        Debug.Log("OnRealTimeMessageReceived - WTF!!");
        //        break;
        //}
    }

    private byte[] serializeMessage(OnlineMessage message)
    {
        byte[] bytes;
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, message);
            bytes = stream.ToArray();
        }
        return bytes;
    }

    private OnlineMessage deserializeMessage(byte[] data)
    {
        OnlineMessage message;
        using (var ms = new MemoryStream(data))
        {
            using (var ds = new DeflateStream(ms, CompressionMode.Decompress, true))
            {
                message = (OnlineMessage)formatter.Deserialize(ds);
            }
        }
        return message;
    }

    /**
    Realiza lógica de decidir quién es el jugador 1.
    */
    private void DecideWhoIsPlayerOne()
    {
        bool iDecide = doIDecideWhoIsPlayerOne();
        if (iDecide)
        {
            GlobalGameManager.amIPlayerOne = UnityEngine.Random.value <= 0.5;
            Debug.LogFormat("DecideWhoIsPlayerOne - decide p1 - %s", GlobalGameManager.amIPlayerOne);
            if (GlobalGameManager.amIPlayerOne)
            {
                OnlineMessage message = new OnlineMessage();
                message.participantIdPlayerOne = PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, serializeMessage(message));
                playerOneDecided = true;
                CurrentState = State.MY_TURN;
            }
        }
    }

    /**
    Indica si "yo" decido quien es el jugador 1. 
    Se ordenan los participantId, si yo soy el primero lanzo un random y se decide si soy o no.
    */
    private bool doIDecideWhoIsPlayerOne()
    {
        List<Participant> participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
        //Sólo habrá 2 participants
        int compare = participants[0].ParticipantId.CompareTo(participants[1].ParticipantId);
        string firstParticipantId = compare <= 0 ? participants[0].ParticipantId : participants[1].ParticipantId;
        return firstParticipantId.Equals(PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId);
    }

    internal void SendShoot(int unitIndex, Vector3 outPower)
    {
        OnlineMessage message = new OnlineMessage();
        message.unitIndex = unitIndex;
        message.force = outPower;
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, serializeMessage(message));
        CurrentState = State.WAIT;
    }

    internal void ReceiveShoot(int unitIndex, Vector3 outPower)
    {
        ai.Shoot(unitIndex, outPower);
        CurrentState = State.MY_TURN;
    }
}
