using UnityEngine;

public class PlayerUniqueAction : MonoBehaviour
{
    public virtual void Action(GameObject attackObj, Animator anim, float attackCnt)    //virtual�F���z�֐�
    {

    }
}

public class PlayerUniqueActionCat : PlayerUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {
        if (0.2f <= attackCnt)
        {
            //�Ђ������U��
            attackObj.SetActive(true);
        }
        //attackObj.SetActive(false);
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
        //    //�A�C�X�{�[��
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
        if (0.4f <= attackCnt)
        {
            attackObj.SetActive(true);
        }
    }
}