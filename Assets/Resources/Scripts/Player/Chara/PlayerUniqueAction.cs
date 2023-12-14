using UnityEngine;

public class PlayerUniqueAction : MonoBehaviour
{
    public virtual void Action(GameObject attackObj, Animator anim, float attackCnt)    //virtual：仮想関数
    {

    }
}

public class PlayerUniqueActionCat : PlayerUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {
        //ひっかき攻撃
        attackObj.SetActive(true);
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
    //public bool oneShot = false;
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {
        //if (!oneShot && 0.5f <= attackCnt)
        //{
        //    //アイスボール
        //    Instantiate(attackObj, this.gameObject.transform.position, Quaternion.identity);
        //    oneShot = true;
        //}
        Instantiate(attackObj, this.gameObject.transform.position, Quaternion.identity);
    }
}

public class PlayerUniqueActionSheep : PlayerUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {
        attackObj.SetActive(true);
    }
}