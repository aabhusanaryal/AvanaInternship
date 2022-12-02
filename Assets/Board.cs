using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Board : MonoBehaviour
{
    private GameObject renderIcon; // icon that will be rendered on top of a cell, either O or X

    public GameObject SquarePrefab;
    private GameObject[,] blocks = new GameObject[3, 3];

    public GameObject[] Os = new GameObject[3];
    public GameObject[] Xs = new GameObject[3];
    public GameObject textComment; // prefab of GO containing TextMesh component
    TextMesh remarks; // actual TextMesh component

    // store the data about values in each cell
    private int[,] gridData = new int[3, 3]; // all the values inside this 2D array is init'ed to -1 during initBoard()
                                             // -1 = empty cell; 0 = O; 1 = X


    // Position of the board
    private float x = -1;
    private float y = 1;

    private int currentPlayer = 0; // 0 for O and 1 for X
    private string winner;
    public bool playWithComputer = true;
    private bool isComputerPlaying = false; // is computer making it's move right now?
    private bool gameOver = false;
    void initBoard()
    {
        // This function creates a 3x3 grid at the center (almost) of the screen
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gridData[i, j] = -1;
                blocks[i, j] = Instantiate(SquarePrefab, new Vector3(x + (1 + 0.1f) * i, y - (1 + 0.1f) * j, 0), Quaternion.identity);
            }
        }
    }

    void switchPlayer()
    {
        // Switch the currentPlayer
        currentPlayer = currentPlayer == 1 ? 0 : 1;
    }

    void updateRemarks()
    {
        string player = currentPlayer == 0 ? "O" : "X";
        if (!gameOver)
        {
            remarks.text = $"{ player }'s turn";
        }
        else if (isTie())
        {
            remarks.text= "It's a tie!";
        }
        else remarks.text = $"{ winner } won!";
        
        
    }
    void clickBlock(int i, int j)
    {
        // This function renders the icon of currentPlayer in the clicked block, add the player's mark to the gridData and also switch player at the end
                Debug.Log($"Clicked on: {i}, {j}");
        gridData[i, j] = currentPlayer;
        if (currentPlayer == 0)
        {
            // renderIcon should be a random O
            renderIcon = Os[Random.Range(0, 2)];
        }
        else
        {
            // renderIcon should be a random X
            renderIcon = Xs[Random.Range(0, 2)];
        }
        Instantiate(renderIcon, new Vector3(x + (1 + 0.1f) * i, y - (1 + 0.1f) * j, 0), Quaternion.identity);
        int winner = checkWin();
        //isTie();
        if (winner == 0 || winner == 1 || isTie())
        {
            endGame(winner);
        }
        switchPlayer();

        // Computer makes a move
        if(currentPlayer == 1 && playWithComputer && !gameOver)
        {
            isComputerPlaying = true; // will be set to false inside computerMove function
            // Adding a random delay to make it natural
            Invoke("computerMove", Random.Range(0.3f, 1.2f));


        }
    }

    void computerMove()
    {
        int[] bestPlace = new int[3];
        // 0th idx is best score, 1st and 2nd idx are (i,j) for position with best score
        bestPlace[0] = -999; // score
        // For every available spot
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                if(gridData[i,j] == -1)
                {
                    gridData[i, j] = 1;
                    int score = minimax(false);
                    gridData[i, j] = -1;
                    if (score > bestPlace[0])
                    {
                        bestPlace[0] = score;
                        bestPlace[1] = i;
                        bestPlace[2] = j;
                    }
                }
            }
        }
        Debug.Log($"Compuer clicking on: {bestPlace[1]}, {bestPlace[2]}");
        clickBlock(bestPlace[1], bestPlace[2]);
        isComputerPlaying = false;
    }

    int calcScore()
    {
        int win = checkWin();
        if (win == 0) return -10; // If O (human player) wins, it's a score of -10 to the AI
        else if (win == 1) return 10; // If X (AI player) wins, it's a score of 10 to the AI
        else return 0; // In case nobody has won yet
    }

    int minimax(bool isMaximising)
    {
        int score = calcScore();
        // return score if the game has ended
        if (score != 0 ||  isTie())
        {
            return score;
        }

        if (isMaximising)
        {
            int bestScore = -999;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gridData[i, j] == -1)
                    {
                        gridData[i, j] = 1;
                        score = minimax(false);
                        gridData[i, j] = -1;
                        if (score > bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
            }
            return bestScore;
        }

        else
        {
            int bestScore = 999;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gridData[i, j] == -1)
                    {
                        gridData[i, j] = 0;
                        score = minimax(true);
                        gridData[i, j] = -1;
                        if (score < bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
            }
            return bestScore;
        }
    }

    public static void newGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void endGame(int winningPlayer)
    {
        gameOver = true;
        winner = winningPlayer == 0 ? "O" : "X";
    }

    int checkWin()
    {
        // This function returns 0 if O wins, 1 if X wins and -1 if noone has won yet
        // Checking rows and cols
       for(int i=0; i<3; i++)
        {
            // Check every column
            if (gridData[i, 0] == gridData[i, 1] && gridData[i, 0] == gridData[i, 2])
                return gridData[i, 0];
            // Check every row
            else if (gridData[0, i] == gridData[1, i] && gridData[0, i] == gridData[2, i])
                return gridData[0, i];
        }
        // Checking diagonals
        if (gridData[0, 0] == gridData[1, 1] && gridData[0, 0] == gridData[2, 2])
            return gridData[0, 0];
        if (gridData[0, 2] == gridData[1, 1] && gridData[0, 2] == gridData[2, 0])
            return gridData[0, 2];
        return -1;
    }

    bool isTie()
    {
        bool boardFilled = true;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if(gridData[i,j] == -1)
                {
                    boardFilled = false;
                    break;
                }
            }
        }
        if(checkWin() == -1 && boardFilled)
        {
            //Debug.Log("It's a tie!");
        }
        return (checkWin() == -1 && boardFilled);
    }
    // Start is called before the first frame update
    void Start()
    {
        // Initializing the grid for board

        GameObject tempTextBox = (GameObject)Instantiate(textComment, new Vector3(-1.5f, -3.5f, 0), Quaternion.identity);

        //Grabs the TextMesh component from the game object
        remarks = tempTextBox.transform.GetComponent<TextMesh>();

        //Sets the text.
        updateRemarks();
        initBoard();

    }

    // Update is called once per frame
    void Update()
    {
        updateRemarks();
        if (!gameOver && Input.GetMouseButtonDown(0) && !isComputerPlaying)
        {
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    BoxCollider2D collider2D = blocks[i, j].GetComponent<BoxCollider2D>();
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (collider2D.OverlapPoint(mousePos) && gridData[i, j] == -1)
                    {
                        clickBlock(i, j);
                    }
                }
            }
        }
    }
}
