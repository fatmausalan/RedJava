using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactercontrol : MonoBehaviour
{
    public float jumpForce = 300;
    public float speed = 5;
    public Sprite[] waitingAni;
    public Sprite[] runningAni;
    public Sprite[] jumpingAni;

    int waitingAniCount = 0;
    int runningAniCount = 0;

    float waitingAniTime = 0;
    float runningAniTime = 0;

    SpriteRenderer spriteRenderer;
    Rigidbody2D phy;
    Vector3 vec;
    Vector3 cameraFirstPos;
    Vector3 cameraLastPos;
    float horizontal = 0;
    bool jumpBoundary = false;
    GameObject camera;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        phy = GetComponent<Rigidbody2D>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraFirstPos = camera.transform.position - transform.position;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!jumpBoundary)
            {
                jumpBoundary = true;
                phy.AddForce(new Vector2(0, jumpForce));
            }
        }
        
    }
    void LateUpdate()
    {
        cameraControl();
    }
    void FixedUpdate()
    {
        characterMovement();
        characterAnimation();
    }
    void characterMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vec = new Vector3(horizontal*speed, phy.velocity.y,0);
        phy.velocity = vec;

    }
    void characterAnimation()
    {
        if (!jumpBoundary)
        {
            if (horizontal == 0)
            {
                waitingAniTime += Time.deltaTime;
                if (waitingAniTime > 0.05f)
                {
                    spriteRenderer.sprite = waitingAni[waitingAniCount++];
                    if (waitingAniCount == waitingAni.Length)
                    {
                        waitingAniCount = 0;
                    }
                    waitingAniTime = 0;
                }

            }
            else if (horizontal > 0)
            {
                runningAniTime += Time.deltaTime;
                if (runningAniTime > 0.1f)
                {
                    spriteRenderer.sprite = runningAni[runningAniCount++];
                    if (runningAniCount == runningAni.Length)
                    {
                        runningAniCount = 0;
                    }
                    runningAniTime = 0;
                }
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            }
            else if (horizontal < 0)
            {
                runningAniTime += Time.deltaTime;
                if (runningAniTime > 0.1f)
                {
                    spriteRenderer.sprite = runningAni[runningAniCount++];
                    if (runningAniCount == runningAni.Length)
                    {
                        runningAniCount = 0;
                    }
                    runningAniTime = 0;
                }
                transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);
            }

        }
        else
        {
            if(phy.velocity.y > 0)
            {
                spriteRenderer.sprite = jumpingAni[0];
            }
            else
            {
                spriteRenderer.sprite = jumpingAni[1];
            }
            if(horizontal > 0)
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            }
            else if(horizontal < 0)
            {
                transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);

            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        jumpBoundary = false;
    }

    void cameraControl()
    {
        cameraLastPos = cameraFirstPos + transform.position;
        camera.transform.position = Vector3.Lerp(camera.transform.position, cameraLastPos, 0.08f);
    }
}
