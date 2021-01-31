using UnityEngine;

public enum ProType
{
    rock, arrow, fireball
}

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int attackStrength;

    [SerializeField]
    private ProType projectileType;

    public int AttackStrength
    {
        get
        {
            return attackStrength;
        }
    }

    public ProType ProjectileType
    {
        get
        {
            return projectileType;
        }
    }
}
