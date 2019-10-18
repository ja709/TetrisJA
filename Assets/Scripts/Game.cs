﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    public static bool startingAtLevelZero;
    public static int startingLevel;

    public int scoreOneLine = 40;
    public int scoreTwoLine = 100;
    public int scoreThreeLine = 300;
    public int scoreFourLine = 1200;

    public int currentLevel = 0;
    private int numLinesCleared = 0;

    public static float fallSpeed = 1.0f;

    public Text hud_score;
    public Text hud_level;
    public Text hud_line;

    private int numberOfRowsThisTurn = 0;

    public static int currentScore = 0;

    private GameObject previewTetromino;
    private GameObject nextTetromino;

    private bool gameStarted = false;

    private Vector2 previewTetrominoPosition = new Vector2(-6.5f, 17);

    // Use this for initialization
    void Start () {

        currentLevel = startingLevel;

        SpawnNext();

	}

    void Update() {
        UpdateScore();
        UpdateUI();
        UpdateLevel();
        UpdateSpeed();
    }

    void UpdateLevel() {
        if ((startingAtLevelZero == true) || (startingAtLevelZero == false && numLinesCleared / 10 > startingLevel))
            currentLevel = numLinesCleared / 10;
        
    }
    void UpdateSpeed() {

        fallSpeed = 1.0f - ((float)currentLevel * 0.1f);
        
    }

    public void UpdateUI() {
        hud_score.text = currentScore.ToString();
        hud_level.text = currentLevel.ToString();
        hud_line.text = numLinesCleared.ToString();
    }

    public void UpdateScore() {

        if (numberOfRowsThisTurn > 0) {

            if (numberOfRowsThisTurn == 1)
            {
                ClearedOneLine();

            }
            else if (numberOfRowsThisTurn == 2)
            {
                ClearedTwoLine();

            }
            else if (numberOfRowsThisTurn == 3)
            {
                ClearedThreeLine();

            }
            else if (numberOfRowsThisTurn == 4) {
                ClearedFourLine();
                
            }
            numberOfRowsThisTurn = 0;
        }
    }

    public void ClearedOneLine() {
        currentScore += scoreOneLine + (currentLevel * 20);
        numLinesCleared++;
    }

    public void ClearedTwoLine()
    {
        currentScore += scoreTwoLine + (currentLevel * 25);
        numLinesCleared += 2;
    }
    public void ClearedThreeLine()
    {
        currentScore += scoreThreeLine + (currentLevel * 30);
        numLinesCleared += 3;
    }
    public void ClearedFourLine()
    {
        currentScore += scoreFourLine + (currentLevel * 40);
        numLinesCleared += 4;
    }


    public bool CheckAboveGrid(Tetromino tetromino) {

        for (int x = 0; x < gridWidth; ++x) {

            foreach (Transform mino in tetromino.transform) {

                Vector2 pos = Round(mino.position);

                if (pos.y > gridHeight - 1) {

                    return true;

                }

            }

        }

        return false;

    }


    public bool IsFull(int y) {

        for (int x = 0; x < gridWidth; ++x) {

            if (grid[x, y] == null) {

                return false;

            }

        }

        numberOfRowsThisTurn++;
        return true;
    }

    public void DeleteRow(int y) {

        for (int x = 0; x < gridWidth; ++x) {

            Destroy(grid[x, y].gameObject);

            grid[x, y] = null;

        }

    }

    public void MoveDown(int y) {

        for (int x = 0; x < gridWidth; ++x) {

            if (grid[x, y] != null) {

                grid[x, y - 1] = grid[x, y];

                grid[x, y] = null;

                grid[x, y - 1].position += new Vector3(0, -1, 0);

            }

        }

    }

    public void MoveAllDown(int y) {

        for (int i = y; i < gridHeight; ++i) {

            MoveDown(i);

        }

    }

    public void DeleteRow() {

        for (int y = 0; y < gridHeight; ++y) {

            if (IsFull(y)) {

                DeleteRow(y);

                MoveAllDown(y + 1);

                --y;

            }

        }

    }

    public void UpdateGrid(Tetromino tetromino) {

        for (int y = 0; y < gridHeight; ++y) {

            for (int x = 0; x < gridWidth; ++x) {

                if (grid[x, y] != null) {

                    if (grid[x, y].parent == tetromino.transform) {

                        grid[x, y] = null;

                    }

                }

            }

        }

        foreach (Transform mino in tetromino.transform) {

            Vector2 pos = Round(mino.position);

            if (pos.y < gridHeight) {

                grid[(int)pos.x, (int)pos.y] = mino;

            }

        }

    }

    public Transform GetTransformAt(Vector2 pos) {

        if (pos.y > gridHeight - 1)
        {

            return null;

        }
        else {

            return grid[(int)pos.x, (int)pos.y];

        }

    }


    public void SpawnNext() {
        if (!gameStarted)
        {
            gameStarted = true;
            nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(5.0f, 20.0f), Quaternion.identity);
            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            previewTetromino.GetComponent<Tetromino>().enabled= false;
        }
        else {

            previewTetromino.transform.localPosition = new Vector2(5.0f, 20.0f);
            nextTetromino = previewTetromino;
            nextTetromino.GetComponent<Tetromino>().enabled = true;

            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            previewTetromino.GetComponent<Tetromino>().enabled = false;

        }
        

    }

    public bool CheckInsideGrid(Vector2 pos) {

        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);

    }

    public Vector2 Round(Vector2 pos) {

        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));

    }

    string GetRandomTetromino() {
        int randomTetromino = Random.Range(1, 8);

        string randomTetrominoName = "Prefabs/Tetromino_T";

        switch (randomTetromino) {
            case 1:
                randomTetrominoName = "Prefabs/Tetromino_I";
                break;
            case 2:
                randomTetrominoName = "Prefabs/Tetromino_J";
                break;
            case 3:
                randomTetrominoName = "Prefabs/Tetromino_L";
                break;
            case 4:
                randomTetrominoName = "Prefabs/Tetromino_S";
                break;
            case 5:
                randomTetrominoName = "Prefabs/Tetromino_Z";
                break;
            case 6:
                randomTetrominoName = "Prefabs/Tetromino_Square";
                break;
            case 7:
                randomTetrominoName = "Prefabs/Tetromino_T";
                break;
        }
        return randomTetrominoName;
    }

    public void GameOver() {

        Application.LoadLevel("GameOver");

    }

}
