using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    #region Variables
    [SerializeField]
    private AudioClip rock, arrow, fireball, die, newGame, gameOver, buildTower, hit;


    #endregion

    public AudioClip Rock
    {
        get
        {
            return rock;
        }
    }

    public AudioClip Arrow
    {
        get
        {
            return arrow;
        }
    }

    public AudioClip Fireball
    {
        get
        {
            return fireball;
        }
    }

    public AudioClip Die
    {
        get
        {
            return die;
        }
    }

    public AudioClip NewGame
    {
        get
        {
            return newGame;
        }
    }

    public AudioClip Gameover
    {
        get
        {
            return gameOver;
        }
    }

    public AudioClip BuildTower
    {
        get
        {
            return buildTower;
        }
    }
    public AudioClip Hit
    {
        get
        {
            return hit;
        }
    }

}
