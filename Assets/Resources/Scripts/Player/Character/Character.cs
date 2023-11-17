using UnityEngine;

public enum Animals
{
    cat,
    duck,
    penguin,
    sheep
}
public abstract class Character : MonoBehaviour
{
    public int speed;
    public abstract void Attack(GameObject attackRange);
}
