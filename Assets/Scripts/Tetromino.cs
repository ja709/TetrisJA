using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{

    float fall = 0;
    private float fallSpeed;

    public bool allowRotation = true;
    public bool limitRotation = false;
    public string prefabName;

    private float continuousVerticalSpeed = 0.05f;
    private float continousHorizontalSpeed = 0.1f;
    private float buttonDownWaitMax = 0.2f;



    private float verticalTimer = 0;
    private float horizontalTimer = 0;
    private float buttonDownWaitTimerHorizontal = 0;
    private float buttonDownWaitTimerVertical = 0;

    private bool moveImmediateHorizontal = false;
    private bool moveImmediateVertical = false;

    public int individualScore = 100;

    private float individualScoreTime;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UserInput();

        UpdateIndividualScore();
        UpdateFallSpeed();
    }

    void UpdateFallSpeed()
    {

        fallSpeed = Game.fallSpeed;

    }

    void UpdateIndividualScore()
    {

        if (individualScoreTime < 1)
        {

            individualScoreTime += Time.deltaTime;
        }
        else
        {
            individualScoreTime = 0;

            individualScore = Mathf.Max(individualScore - 10, 0);
        }
    }

    void UserInput()
    {

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {

            moveImmediateHorizontal = false;


            horizontalTimer = 0;

            buttonDownWaitTimerHorizontal = 0;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            moveImmediateVertical = false;
            verticalTimer = 0;
            buttonDownWaitTimerVertical = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotate();
        }
        if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            MoveDown();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
        }
    }

    void MoveLeft()
    {
        if (moveImmediateHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += buttonDownWaitMax;
                return;
            }
            if (horizontalTimer < continousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }
        if (!moveImmediateHorizontal)
            moveImmediateHorizontal = true;

        horizontalTimer = 0;

        transform.position += new Vector3(-1, 0, 0);

        if (CheckValidPosition())
        {

            FindObjectOfType<Game>().UpdateGrid(this);

        }
        else
        {
            transform.position += new Vector3(1, 0, 0);
        }
    }

    void MoveRight()
    {
        if (moveImmediateHorizontal)
        {

            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += buttonDownWaitMax;
                return;
            }

            if (horizontalTimer < continousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!moveImmediateHorizontal)
            moveImmediateHorizontal = true;

        horizontalTimer = 0;

        transform.position += new Vector3(1, 0, 0);

        if (CheckValidPosition())
        {

            FindObjectOfType<Game>().UpdateGrid(this);

        }
        else
        {
            transform.position += new Vector3(-1, 0, 0);
        }
    }

    void MoveDown()
    {
        if (moveImmediateVertical)
        {
            if (buttonDownWaitTimerVertical < buttonDownWaitMax)
            {
                buttonDownWaitTimerVertical += buttonDownWaitMax;
                return;
            }
            if (verticalTimer < continuousVerticalSpeed)
            {

                verticalTimer += Time.deltaTime;

                return;

            }
        }

        if (!moveImmediateVertical)
            moveImmediateVertical = true;

        verticalTimer = 0;

        transform.position += new Vector3(0, -1, 0);

        if (CheckValidPosition())
        {

            FindObjectOfType<Game>().UpdateGrid(this);

        }
        else
        {
            transform.position += new Vector3(0, 1, 0);

            FindObjectOfType<Game>().DeleteRow();

            if (FindObjectOfType<Game>().CheckAboveGrid(this))
            {

                FindObjectOfType<Game>().GameOver();

            }
            FindObjectOfType<Game>().SpawnNext();

            Game.currentScore += individualScore;

            enabled = false;
        }

        fall = Time.time;

    }

    void rotate()
    {
        if (allowRotation)
        {
            if (limitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
            if (CheckValidPosition())
            {

                FindObjectOfType<Game>().UpdateGrid(this);

            }
            else
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, -90);
                }
            }
        }
    }

    void HardDown()
    {

        while (CheckValidPosition())
        {

            transform.position += new Vector3(0, -1, 0);

        }

        transform.position += new Vector3(0, 1, 0);

        if (CheckValidPosition())
        {

            FindObjectOfType<Game>().UpdateGrid(this);

        }
        else
        {

            FindObjectOfType<Game>().DeleteRow();

            if (FindObjectOfType<Game>().CheckAboveGrid(this))
            {

                FindObjectOfType<Game>().GameOver();

            }

            FindObjectOfType<Game>().SpawnNext();

            Game.currentScore += individualScore;

            enabled = false;
        }
    }


    bool CheckValidPosition()
    {
        foreach (Transform mino in transform)
        {

            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);

            if (FindObjectOfType<Game>().CheckInsideGrid(pos) == false)
            {
                return false;
            }

            if (FindObjectOfType<Game>().GetTransformAt(pos) != null && FindObjectOfType<Game>().GetTransformAt(pos).parent != transform)
            {

                return false;

            }
        }
        return true;

    }
}
