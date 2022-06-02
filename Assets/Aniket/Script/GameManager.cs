using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public PlayerState playerState;
    public static GameManager instance;
    [HideInInspector] public int playerLevel;
    [SerializeField] GameObject[] players;
    float cooldown;
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
        if (cooldown < 0f)
        {
            playerLevel++;
            if (playerLevel < players.Length)
            {
                GameObject go = PlayerManager.instance.gameObject;
                GameObject go_new = Instantiate(players[playerLevel], go.transform.position, transform.rotation);
                GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = go_new.transform;
                Destroy(go);
                cooldown = 1f;
            }
        }
    }
    private void Update()
    {
        cooldown -= Time.deltaTime;
    }
}

