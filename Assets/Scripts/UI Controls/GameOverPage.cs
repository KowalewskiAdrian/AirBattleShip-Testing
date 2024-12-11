using TMPro;
using UnityEngine;

public class GameOverPage : MonoBehaviour
{
    [SerializeField] private TMP_Text _labelHighScore;
    [SerializeField] private TMP_Text _labelOverScore;
    [SerializeField] private GameObject _Congratulations;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameOver(int playScore, int highScore, bool congratulations)
    {
        _labelHighScore.text = $"High Score :{highScore.ToString()}";
        _labelOverScore.text = $"Your Score :{playScore.ToString()}";
        _Congratulations.SetActive(congratulations);
    }


}
