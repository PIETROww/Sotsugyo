using UnityEngine;

public class CharaUniqueAction : MonoBehaviour
{
    public virtual void Action(GameObject attackObj, Animator anim, float attackCnt)    //virtual�F���z�֐�
    {

    }
}

public class CharaUniqueActionCat : CharaUniqueAction
{
    public override void Action(GameObject attackObj, Animator anim, float attackCnt)
    {
        //�Ђ������U��
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
        //    //�A�C�X�{�[��
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