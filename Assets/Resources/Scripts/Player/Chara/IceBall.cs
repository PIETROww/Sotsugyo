using UnityEngine;

public class IceBall : MonoBehaviour
{
    //éQè∆ÅFhttps://sleepygamersmemo.blogspot.com/2017/04/unity-raycast.html
    public float distanceFromSurface;
    public LayerMask groundLayer;
    public float speed = 5.0f;
    public int meltTime = 5;
    float meltCount = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        meltCount += Time.deltaTime;
        Melt();
    }

    private void FixedUpdate()
    {
        //à⁄ìÆÇ≥ÇπÇÈ
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //ínñ Ç…âàÇ§ÇÊÇ§Ç…Ç∑ÇÈ
    void CheckGround()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, groundLayer))
        {
            Vector3 newPos = transform.position;
            newPos.y = hitInfo.point.y + distanceFromSurface;
            transform.position = newPos;
        }
    }
     void Melt()
    {
        if(meltTime<meltCount)
        {
            Destroy(gameObject);
        }
    }
}
