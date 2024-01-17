using UnityEngine;

public class CharaUniqueAction : MonoBehaviour
{
    public virtual void Action(GameObject attackObj, Animator anim, float attackCnt)    //virtual：仮想関数
    {

    }
}

public class CharaUniqueActionCat : CharaUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {
        //ひっかき攻撃
        attackObj.SetActive(true);
    }
}

public class CharaUniqueActionDuck : CharaUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {
        //Instantiate(attackObj, gameObject.transform.position, Quaternion.identity);
    }
}

public class CharaUniqueActionPenguin : CharaUniqueAction
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
        Instantiate(attackObj, gameObject.transform.position, gameObject.transform.rotation);
    }
}

public class CharaUniqueActionSheep : CharaUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {
        attackObj.SetActive(true);
        //attackObj.SetActive(false);
    }
}