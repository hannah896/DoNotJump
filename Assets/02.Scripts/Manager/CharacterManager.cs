using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    private static PlayerController _controller;
    private static PlayerCondition _condition;
    private Player _player;

    public GameObject Choco;
    public GameObject Straw;

    public AnimationState state;

    public bool isInteract = false;
    public Player Player
    { get { return _player; } }
    public PlayerController Controller
    { get { return _controller; } }
    public PlayerCondition Condition
    { get { return _condition; } }

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

    private void OnValidate()
    {
        _controller = GetComponent<PlayerController>();
        _condition = GetComponent<PlayerCondition>();
        _player = GetComponent<Player>();
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
