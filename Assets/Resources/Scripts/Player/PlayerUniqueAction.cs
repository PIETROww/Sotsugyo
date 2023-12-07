using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUniqueAction : MonoBehaviour
{
    public virtual void Action(GameObject attackObj, Animator anim, float attackCnt)
    {

    }
}

public class PlayerUniqueActionCat : PlayerUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {

    }
}

public class PlayerUniqueActionDuck : PlayerUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {

    }
}

public class PlayerUniqueActionPenguin : PlayerUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {

    }
}

public class PlayerUniqueActionSheep : PlayerUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt    )
    {

    }
}