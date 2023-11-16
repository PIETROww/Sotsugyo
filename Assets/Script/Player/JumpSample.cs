using UnityEngine;

public class JumpSample : MonoBehaviour
{
    //参照：https://kurokumasoft.com/2022/04/12/unity-jump/
    //作者を偽らなければ、自由に使っていいとのこと

    [SerializeField]
    Rigidbody rigidBody;
    [SerializeField, Min(0)]
    float jumpPower = 5f;
    [SerializeField]
    AnimationCurve jumpCurve = new();
    [SerializeField, Min(0)]
    float maxJumpTime = 1f;
    [SerializeField]
    float groundCheckRadius = 0.4f;
    [SerializeField]
    float groundCheckOffsetY = 0.07f;
    [SerializeField]
    float groundCheckDistance = 0.2f;
    [SerializeField]
    LayerMask groundLayers = 0;
    [SerializeField]
    string JumpButtonName = "Jump";

    bool isGrounded = false;
    bool jumping = false;
    float jumpTime = 0;
    RaycastHit hit;
    Transform thisTransform;

    void Start()
    {
        thisTransform = this.transform;
    }

    void Update()
    {
        isGrounded = CheckGroundStatus();
        Debug.Log(isGrounded);

        // ジャンプの開始判定
        if (isGrounded && Input.GetButton(JumpButtonName))
        {
            jumping = true;
        }

        // ジャンプ中の処理
        if (jumping)
        {
            if (Input.GetButtonUp(JumpButtonName) || jumpTime >= maxJumpTime)
            {
                jumping = false;
                jumpTime = 0;
            }
            else if (Input.GetButton(JumpButtonName))
            {
                jumpTime += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        Jump();
    }

    void Jump()
    {
        if (!jumping)
        {
            return;
        }

        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);

        // ジャンプの速度をアニメーションカーブから取得
        float t = jumpTime / maxJumpTime;
        float power = jumpPower * jumpCurve.Evaluate(t);

        if (t >= 1)
        {
            jumping = false;
            jumpTime = 0;
        }

        rigidBody.AddForce(power * Vector3.up, ForceMode.Impulse);
    }

    // 接地判定
    bool CheckGroundStatus()
    {
        bool isHit = false;
        if(Physics.SphereCast(
            thisTransform.position + groundCheckOffsetY * Vector3.up,
            groundCheckRadius,
            Vector3.down,
            out hit,
            groundCheckDistance,
            groundLayers,
            QueryTriggerInteraction.Ignore
            ))
        {
            isHit = true;
        }
        return isHit;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(thisTransform.position + groundCheckOffsetY * Vector3.up, groundCheckRadius);
        //Gizmos.DrawRay(thisTransform.position + groundCheckOffsetY * Vector3.up, groundCheckDistance * Vector3.down);


        //if (CheckGroundStatus())
        //{
        //    // SphereCastがヒットした場合、ヒットしたポイントを可視化します：chatGPT
        //    Debug.DrawRay(thisTransform.position, Vector3.down * hit.distance, Color.red);
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(thisTransform.position + Vector3.down * hit.distance, groundCheckRadius);
        //}

    }
}
