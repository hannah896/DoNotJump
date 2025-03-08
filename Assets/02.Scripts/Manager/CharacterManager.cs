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

    public AnimationState state;

    public Player Player
    { get { return _player; } set { _player = value; } }
    public PlayerController Controller
    { get { return _controller; } set { _controller = value; } }
    public PlayerCondition Condition
    { get { return _condition; } set { _condition = value; } }

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
