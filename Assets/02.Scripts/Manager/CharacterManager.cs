using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    private static PlayerController _controller;
    private Player _player;
    
    public Player Player
    { get { return _player; } set { _player = value; } }
    public PlayerController Controller
    { get { return _controller; } set { _controller = value; } }

    public AnimationState state;

    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("Player").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
