
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    private GameData gData;
    private GameDataRepository gDataRepository;

    // private int score = 0;
    // private int lives = 3;x

    private TMP_Text scoreText;
    
 

    // Start is called before the first frame update
    void Start()
    {
        

        scoreText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
       
    }

    public void AddScore(int scoreToAdd)
    {
        gData.score += scoreToAdd;
        gDataRepository.SaveGame(gData);
    }


    void Update()
    {
        scoreText.text = "Score: " + gData.score;
       
    }
}
