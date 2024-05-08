using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public float moveSpeed; // Tốc độ di chuyển của player
    //public float acceleration; // Tốc độ tăng tốc
    //public float deceleration; // Tốc độ giảm tốc
    public float velocityStart;

    private bool isMovingUp;
    private bool isMovingDown;
    private bool isMovingLeft;
    private bool isMovingRight;
    private float velocityMoveUp;
    private float velocityMoveDown;
    private float velocityMoveLeft;
    private float velocityMoveRight;


    [SerializeField] private LineRenderer lineCircle;

    List<Character> characters = new();

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnInit()
    {
        base.OnInit();
        velocityMoveUp = 0;
        velocityMoveDown = 0;
        velocityMoveLeft = 0;
        velocityMoveRight = 0;
        velocityStart = 5;
        moveSpeed = 20;
        /*acceleration = 10f;
        deceleration = .1f;*/
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || (Input.GetKey(KeyCode.W)))
        {
            isMovingUp = true;
        }
        else { isMovingUp = false; }

        if (Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.S)))
        {
            isMovingDown = true;
        }
        else { isMovingDown = false; }

        if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A)))
        {
            isMovingLeft = true;
        }
        else { isMovingLeft = false; }

        if (Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D)))
        {
            isMovingRight = true;
        }
        else { isMovingRight = false; }


        CaptureZone();
        //DisplayCaptureZone();
        //Test();
    }

    private void FixedUpdate()
    {
        if (isMovingUp)  // Len
        {
            velocityMoveUp += velocityStart * 0.1f * GameManager.instance.status.acceleration * Time.fixedDeltaTime;
            velocityMoveUp = Mathf.Clamp(velocityMoveUp, -moveSpeed, moveSpeed);
        }
        else
        {
            velocityMoveUp -= velocityMoveUp * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
            if (Mathf.Abs(velocityMoveUp) < 0.01f)
            {
                velocityMoveUp = 0f;
            }
        }

        if (isMovingDown)  //Xuong
        {
            velocityMoveDown += velocityStart * 0.1f * GameManager.instance.status.acceleration * Time.fixedDeltaTime;
            velocityMoveDown = Mathf.Clamp(velocityMoveDown, -moveSpeed, moveSpeed);
        }
        else
        {
            velocityMoveDown -= velocityMoveDown * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
            if (Mathf.Abs(velocityMoveDown) < 0.01f)
            {
                velocityMoveDown = 0f;
            }
        }


        if (isMovingLeft) //Trai
        {
            velocityMoveLeft += velocityStart * 0.1f * GameManager.instance.status.acceleration * Time.fixedDeltaTime;
            velocityMoveLeft = Mathf.Clamp(velocityMoveLeft, -moveSpeed, moveSpeed);
        }
        else
        {
            velocityMoveLeft -= velocityMoveLeft * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
            if (Mathf.Abs(velocityMoveLeft) < 0.01f)
            {
                velocityMoveLeft = 0f;
            }
        }

        if (isMovingRight)  // Phai
        {
            velocityMoveRight += velocityStart * 0.1f * GameManager.instance.status.acceleration * Time.fixedDeltaTime;
            velocityMoveRight = Mathf.Clamp(velocityMoveRight, -moveSpeed, moveSpeed);
        }
        else
        {
            velocityMoveRight -= velocityMoveRight * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
            if (Mathf.Abs(velocityMoveRight) < 0.01f)
            {
                velocityMoveRight = 0f;
            }
        }

        float velocityHorizontal = velocityMoveUp - velocityMoveDown;
        float velocityVertical = velocityMoveRight - velocityMoveLeft;

        tf.position += new Vector3(velocityVertical, velocityHorizontal, 0) * Time.fixedDeltaTime;
    }

    public void CaptureZone()
    {
        characters.Clear();
        Collider2D[] character = Physics2D.OverlapCircleAll(tf.position, captureZoneRadius, characterLayer);
        //Debug.Log(character.Length);
        for (int i = 0; i < character.Length; i++)
        {
            if (character[i].gameObject != this.gameObject)
            {
                Character c = character[i].GetComponent<Character>();
                characters.Add(c);
                c.isCapture = true;
                c.host = this;
            }
        }
    }

    public void DisplayCaptureZone()
    {
        lineCircle.enabled = true;
        float Theta = 0f;
        float ThetaScale = 0.01f;
        int Size = (int)((1f / ThetaScale) + 1f);
        lineCircle.positionCount = Size;
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = lineCircle.transform.localPosition.x + captureZoneRadius * Mathf.Cos(Theta);
            float y = lineCircle.transform.localPosition.y + captureZoneRadius * Mathf.Sin(Theta);
            lineCircle.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    public void Test()
    {
        characters.Clear();
        Collider2D[] character = Physics2D.OverlapCircleAll(tf.position, captureZoneRadius, characterLayer);
        for (int i = 0; i < character.Length; i++)
        {
            if (character[i].gameObject != this.gameObject)
            {
                Character c = character[i].GetComponent<Character>();
                characters.Add(c);
                Attract(c);
            }
        }
    }
}