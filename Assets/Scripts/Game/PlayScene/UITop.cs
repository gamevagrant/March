using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITop : MonoBehaviour 
{
    public Text levelText;
    public Text scoreText;
    public Text movesText;
    public Image progess;
    public Image cake;
    public Image progressStar1;
    public Image progressStar2;
    public Image progressStar3;

    float progress = 0f;
    float progress1 = 0.33f;
    float progress2 = 0.66f;
    float progress3 = 1.00f;
    int star1;
    int star2;
    int star3;

    bool greeting1;
    bool greeting2;
    bool greeting3;

    float duration = 0.5f;
    int start;
    int moves;

	void Start () 
    {
		levelText.text = LanguageManager.instance.GetValueByKey ("200014") + LevelLoader.instance.level.ToString ();
        scoreText.text = "0";

        moves = LevelLoader.instance.moves;

        if (Configure.instance.beginFiveMoves == true)
        {
            moves += Configure.instance.plusMoves;
        }

        movesText.text = moves.ToString();

        star1 = LevelLoader.instance.score1Star;
        star2 = LevelLoader.instance.score2Star;
        star3 = LevelLoader.instance.score3Star;

        progess.fillAmount = 0;

        var name = "cake_" + LevelLoader.instance.cake + "_1";
        cake.sprite = Resources.Load<Sprite>(Configure.Cake(name));
	}

    public void UpdateScoreAmount(int score)
    {
        StartCoroutine("StartUpdateScore", score);
    }

    IEnumerator StartUpdateScore(int target)
    {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            scoreText.text = ((int)Mathf.Lerp((float)start, (float)target, (timer / duration))).ToString();

            yield return null;
        }

        start = target;

        scoreText.text = target.ToString();
    }

    public void DecreaseMoves(bool effect = false)
    {
        if (effect == true)
        {
            var explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RingExplosion()) as GameObject);
            explosion.transform.position = movesText.gameObject.transform.position;
        }

        if (moves > 0)
        {
            moves--;

            movesText.text = moves.ToString();
        }        
    }

    // when user runs out of moves and click keep playing
    public void Set5Moves(int addsteps)
    {
        var explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RingExplosion()) as GameObject);
        explosion.transform.position = movesText.gameObject.transform.position;

        moves += addsteps;

        movesText.text = moves.ToString();
    }

    public void UpdateProgressBar(int score)
    {
        if (score < star1)
        {
            progress = ((float)score / (float)star1) * progress1;
        }
        else if (star1 <= score && score < star2)
        {
            progress = progress1 + (((float)score - (float)star1) / ((float)star2 - (float)star1)) * (progress2 - progress1);

            if (greeting1 == false)
            {
                greeting1 = true;
                
                //print("cake state 2");

                var name = "cake_" + LevelLoader.instance.cake + "_2";
                cake.sprite = Resources.Load<Sprite>(Configure.Cake(name));

                iTween.PunchScale(cake.gameObject, new Vector3(0.5f, 0.5f, 0), 2.0f);

                // change progress star to gold
                //progressStar1.sprite = Resources.Load<GameObject>(Configure.ProgressGoldStar()).GetComponent<SpriteRenderer>().sprite; ;
                StartCoroutine(Star2Gold(progressStar1));
            }
        }
        else if (star2 <= score && score < star3)
        {
            progress = progress2 + (((float)score - (float)star2) / ((float)star3 - (float)star2)) * (progress3 - progress2);

            if (greeting2 == false)
            {
                greeting2 = true;

                //print("cake state 3");

                var name = "cake_" + LevelLoader.instance.cake + "_3";
                cake.sprite = Resources.Load<Sprite>(Configure.Cake(name));

                iTween.PunchScale(cake.gameObject, new Vector3(0.5f, 0.5f, 0), 2.0f);

                // change progress star to gold
                //progressStar2.sprite = Resources.Load<GameObject>(Configure.ProgressGoldStar()).GetComponent<SpriteRenderer>().sprite; ;
                StartCoroutine(Star2Gold(progressStar2));
            }
        }
        else if (score >= star3)
        {
            progress = progress3;

            if (greeting3 == false)
            {
                greeting3 = true;
                
                //print("cake state 4");

                var name = "cake_" + LevelLoader.instance.cake + "_4";
                cake.sprite = Resources.Load<Sprite>(Configure.Cake(name));

                iTween.PunchScale(cake.gameObject, new Vector3(0.5f, 0.5f, 0), 2.0f);

                // change progress star to gold
                //progressStar3.sprite = Resources.Load<GameObject>(Configure.ProgressGoldStar()).GetComponent<SpriteRenderer>().sprite;
                StartCoroutine(Star2Gold(progressStar3));
            }
        }

        //Debug.Log("Updating progress bar");

        StartCoroutine("StartUpdateProgress", progress);
    }

    IEnumerator StartUpdateProgress(float progress)
    {
        float start = progess.fillAmount;

        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            progess.fillAmount = Mathf.Lerp(start, progress, (timer / duration));

            yield return null;
        }
    }

    IEnumerator Star2Gold(Image progressStar)
    {
        yield return new WaitForSeconds(duration);

        progressStar.sprite = Resources.Load<GameObject>(Configure.ProgressGoldStar()).GetComponent<SpriteRenderer>().sprite;
    }
}
