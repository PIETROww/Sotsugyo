using UnityEngine;

public class WindAttack : MonoBehaviour
{
    //�Q�ƁFhttps://teratail.com/questions/253539
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float rotatespeed = 360.0f;
    [SerializeField] float circleRadius = 1.0f;

    Transform target;

    public float distance = 2.0f;

    //�ړ��@�uIceBall.cs�v����R�s�[
    public float distanceFromSurface;
    public LayerMask groundLayer;
    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Enemy").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Vector3.Distance(transform.position, target.position);
        if(dis>distance)
        {
            Atract();
        }

        CheckGround();
    }

    void FixedUpdate()
    {
        //�O���Ɉړ�������
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    //�z���񂹂�
    void Atract()
    {
        float rad = rotatespeed * Mathf.Deg2Rad * Time.time;    //Sin�̈����̓��W�A���Ȃ̂�Rad2Deg�ł͂Ȃ�Deg2Rad���g��
        target.position = transform.position + new Vector3(Mathf.Cos(rad) * circleRadius * 1.5f, 0.0f, Mathf.Sin(rad) * circleRadius * 1.5f);
        if (0 < circleRadius) circleRadius -= moveSpeed * Time.deltaTime;   //���a�����������Ă���
    }

    //�n�ʂɉ����悤�ɂ���@
    private void CheckGround()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, groundLayer))
        {
            Vector3 newPos = transform.position;
            newPos.y = hitInfo.point.y + distanceFromSurface;
            transform.position = newPos;
        }
    }
}
