using UnityEngine;

public class StarManager : MonoBehaviour
{
    [SerializeField]
    private Star[] stars;

    [SerializeField]
    private float scoreGoal;
    void Start()
    {
        foreach(var star in stars)
        {
            star.SetState(Star.StarState.Empty);
        }
    }

    public void UpdateStars(float score)
    {   
        if (score > scoreGoal) score = scoreGoal;

        float fullStarScore = scoreGoal / (stars.Length * 2);
        float starScore = score / (stars.Length * 2);
        float starCount = (stars.Length * 2) / (scoreGoal / score);

        int currentStar = 0;
        starCount = Mathf.Floor(starCount);

        while (starCount > 0)
        {
            if (stars[currentStar].GetState() == Star.StarState.Empty)
            {
                stars[currentStar].SetState(Star.StarState.Half);
                starCount--;
            }
            else if (stars[currentStar].GetState() == Star.StarState.Half)
            {
                stars[currentStar].SetState(Star.StarState.Filled);
                starCount--;
            }
            else
            {
                currentStar++;
            }
        }

        
    }

}
