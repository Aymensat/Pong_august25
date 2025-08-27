using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    int leftScore = 0;
    int rightScore = 0;

    [SerializeField] TextMeshProUGUI leftScoreText;
    [SerializeField] TextMeshProUGUI rightScoreText;


    private void Start()
    {

    }

    private void OnEnable()
    {
        EventBus.Instance.OnGoal += ScoreHandler; 

    }

    private void OnDisable()
    {
        EventBus.Instance.OnGoal -= ScoreHandler; 
    }

    private void ScoreHandler(int x)
    {
        if (x == 1)
        {
            leftScore++;
            leftScoreText.text = leftScore + "";
        }
        else if (x == -1)
        {
            rightScore++;
            rightScoreText.text = rightScore + "";
        }


    }

    public  void  ResetScore()
    {
        leftScore = rightScore =  0;
        leftScoreText.text = rightScoreText.text =  "0";
    }







}
