using UnityEngine;

public class WindAttack : MonoBehaviour
{
    //参照：https://teratail.com/questions/253539
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float rotatespeed = 360.0f;
    [SerializeField] float circleRadius = 1.0f;

    Transform target;

    public float distance = 2.0f;

    //移動　「IceBall.cs」からコピー
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
        //前方に移動させる
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    //吸い寄せる
    void Atract()
    {
        float rad = rotatespeed * Mathf.Deg2Rad * Time.time;    //Sinの引数はラジアンなのでRad2DegではなくDeg2Radを使う
        target.position = transform.position + new Vector3(Mathf.Cos(rad) * circleRadius * 1.5f, 0.0f, Mathf.Sin(rad) * circleRadius * 1.5f);
        if (0 < circleRadius) circleRadius -= moveSpeed * Time.deltaTime;   //半径を小さくしていく
    }

    //地面に沿うようにする　
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
