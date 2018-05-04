using UnityEngine;
using UnityEngine.UI;

public class UITop : MonoBehaviour
{
    public Text levelText;
    public Text movesText;

    int moves;

    void Start()
    {
        levelText.text = LanguageManager.instance.GetValueByKey("200014") + LevelLoader.instance.level;

        moves = LevelLoader.instance.moves;

        if (Configure.instance.beginFiveMoves == true)
        {
            moves += Configure.instance.plusMoves;
        }

        movesText.text = moves.ToString();
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

    public void Set5Moves(int addsteps)
    {
        var explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RingExplosion()) as GameObject);
        explosion.transform.position = movesText.gameObject.transform.position;

        moves += addsteps;

        movesText.text = moves.ToString();
    }
}
