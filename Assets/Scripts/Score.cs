using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TMP_Text scoreText;
    private float totalScore;
    void Start()
    {
        scoreText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void AddScore(int scoreToAdd)
    {
        totalScore += scoreToAdd;
        
    }
    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score " + totalScore;
    }
}
