using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    private PlayerController _controller;
    private PlayerCondition _condition;
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
                if (!FindFirstObjectByType(typeof(CharacterManager)))
                {
                    _instance = new GameObject("Player").AddComponent<CharacterManager>();
                }
            }
            return _instance;
        }
    }

    private void OnValidate()
    {
        if (_controller == null) _controller = GetComponent<PlayerController>();
        if (_condition == null) _condition = GetComponent<PlayerCondition>();
        if (_player == null) _player = GetComponent<Player>();
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
