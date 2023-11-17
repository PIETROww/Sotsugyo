using UnityEngine;

public class Cat : Character
{
    public override void Attack(GameObject attackRange)
    {
        attackRange.SetActive(true);
    }
}
