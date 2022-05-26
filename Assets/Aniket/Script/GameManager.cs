using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public PlayerState playerState;
    public static GameManager instance;
    [HideInInspector] public int playerLevel;
    [SerializeField] GameObject[] players;
    void Awake()
    {
        instance = this;
    }   
    public enum PlayerState
    {
        Stop,
        Move
    }     
    public void ChangePlayer()
    {
        playerLevel++;
        if (playerLevel < players.Length)
        {
            Instantiate(players[playerLevel], PlayerManager.instance.transform.position, transform.rotation);
            Destroy(PlayerManager.instance.gameObject);
        }
    }
}

