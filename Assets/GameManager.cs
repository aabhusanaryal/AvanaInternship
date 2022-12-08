using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int score = 0;
    public GameObject textbox;
    public Player player;
    public GameObject playBtn;
    private Text textEl;
    private bool paused;
    public void IncreaseScore()
    {
        score++;
        textEl.text = score.ToString();
        Debug.Log(score);
    }

    public void Play()
    {
        paused = false;
        Time.timeScale = 1f;
        player.enabled = true;
        playBtn.SetActive(false);
        score = 0;
        textEl.text = score.ToString();

        // Removing older pipes (if any) before starting a game
        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        player.enabled = false;
        playBtn.SetActive(true);
    }

    public void GameOver()
    {
        Pause();
    }
    void Awake()
    {
        Pause();
        textEl = textbox.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(paused && Input.GetKeyDown(KeyCode.Space)){
            Play();
        }
    }
}
