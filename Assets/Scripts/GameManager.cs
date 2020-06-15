using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Shader shader;

    [SerializeField]
    private GameObject cube;

    private static int level = 1;

    private static int numberOfLevel = 0;


    public int numberOfCube;

    [SerializeField]
    private GameObject[] bonusList;

    public bool isGameOver;

    private bool isWin;
    private bool isWinNext;
    private bool isPause;
    private static bool isMenu = true;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject winNextPanel;

    [SerializeField]
    private GameObject winPanel;

    [SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private GameObject pausePanel;


    // Start is called before the first frame update
    void Awake()
    {

        numberOfCube = 0;
        isWin = false;
        isWinNext = false;
        LoadLevel();
        gameOverPanel.SetActive(false);
        winNextPanel.SetActive(false);
        winPanel.SetActive(false);


        isGameOver = false;
        if (isMenu)
            Time.timeScale = 0f;
        else
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1f;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (numberOfCube == 0 && !isWin)
        {
            isWinNext = true;
            if (numberOfLevel != level)
                LoadWinNextPanel();
            if (numberOfLevel == level)
            {
                isWin = true;
                level = 1;
                LoadWinPanel();
            }
        }

        if (isGameOver)
        {
            LoadGameOverPanel();
        }

        if ((isGameOver || isWin || isMenu || isPause) && Input.GetKeyDown(KeyCode.R))
        {
            isGameOver = false;
            isWin = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if ((isMenu || isPause) && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            isMenu = false;
            pausePanel.SetActive(false);
            menuPanel.SetActive(false);
            isPause = false;
        }

        if (!isWin && isWinNext && Input.GetKeyDown(KeyCode.Space))
        {
            level++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if ((isWin || isGameOver || isPause) && Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPause)
            {
                Time.timeScale = 1f;
                pausePanel.SetActive(false);
                isPause = false;
            }
            else
            {
                Time.timeScale = 1f;
                isPause = true;
                LoadPausePanel();
            }
        }
    }

    private void LoadLevel()
    {
        bool start = false;
        string end = "end";
        string[] lines = File.ReadAllLines("Assets/levels.txt");

        float startCubeX = -4f;
        float startCubeZ = 6f;
        foreach (string line in lines)
        {
            string[] elements = line.Split(' ');
            if (elements.Length == 2 && level == Int32.Parse(elements[1]))
            {
                if(elements[0] == "levels")
                    numberOfLevel = Int32.Parse(elements[1]);
                else if (level == Int32.Parse(elements[1]))
                    start = true;

            } else if (start && end == elements[0])
            {
                start = false;
                break;
            } else if (start && elements.Length == 6)
            {
                int numberBrick = NumberOfBrick(elements);
                int position = Random.Range(1, numberBrick);
                int pos = 1;
                for (int i = 0; i < elements.Length; i++)
                {
                    if (elements[i] == "B")
                    {
                        numberOfCube++;
                        CreateCube(new Vector3(startCubeX, 0.5f, startCubeZ), (pos == position ? GetBonus(elements[5]) : null));
                        pos++;
                    }
                    startCubeX += 2f;
                }
            }
            if (start)
            {
                startCubeZ -= 1f;
                startCubeX = -4f;
            }
        }
    }

    private GameObject GetBonus(String name)
    {
        foreach (GameObject go in bonusList)
            if (go.name == name)
                return go;
        return null;
    }

    private int NumberOfBrick(string[] elements)
    {
        int n = 0;
        for (int i = 0; i < elements.Length; i++)
            if (elements[i] == "B")
                n++;
        return n;
    }

    private void CreateCube(Vector3 pos, GameObject bonus)
    {
        GameObject emptyObejct = Instantiate(cube, transform);
        emptyObejct.transform.position = pos;
        emptyObejct.GetComponent<Brick>().bonus = bonus;
    }

    private void LoadGameOverPanel()
    {
        winPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        winPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    private void LoadWinNextPanel()
    {
        winNextPanel.SetActive(true);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    private void LoadWinPanel()
    {
        winNextPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void LoadPausePanel()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }
}
