using TMPro;
using UnityEngine;

public class GamePlayPage : MonoBehaviour
{
    [SerializeField] private RectTransform _health;
    [SerializeField] private TMP_Text _labelPlayScore;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPlayScore(int playScore)
    {
        _labelPlayScore.text = playScore.ToString();
    }

    public void ShowHealth(int health)
    {
        for (int i = 0; i < _health.childCount; i++)
        {
            _health.GetChild(i).gameObject.SetActive((i + 1) <= health);
        }
    }
}
