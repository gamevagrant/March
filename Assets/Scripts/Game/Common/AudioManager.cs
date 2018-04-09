using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    public static AudioManager instance = null;

    public AudioSource efxSource;

    [Header("Play")]
    public AudioClip swap;
    public AudioClip swapBack;
    public AudioClip cookieCrush;
    public AudioClip bomb;
    public AudioClip bombExplode;
    public AudioClip colRowBreaker;
    public AudioClip colRowBreakerExplode;
    public AudioClip rainbow;
    public AudioClip rainbowExplode;
    public AudioClip drop;
    public AudioClip coinPay;
    public AudioClip coinAdd;
    public AudioClip UIPopupLevelSkipped;
    public AudioClip waffleExplode;
    public AudioClip collectTarget;
    public AudioClip cageExplode;
    public AudioClip gingerbreadExplode;
    public AudioClip gingerbread;
    public AudioClip marshmallowExplode;
    public AudioClip collectibleExplode;
    public AudioClip chocolateExplode;
    public AudioClip rockCandyExplode;
    public AudioClip amazing;
    public AudioClip exellent;
    public AudioClip great;
    public AudioClip star1;
    public AudioClip star2;
    public AudioClip star3;

    // UI
    [Header("UI")]
    public AudioClip buttonClick;
    public AudioClip UIPopupTarget;
    public AudioClip UIPopupCompleted;
    public AudioClip UIPopupWin;
    public AudioClip UIPopupLose;
    public AudioClip UIPopupNoMatches;

    [Header("Booster")]
    public AudioClip singleBooster;
    public AudioClip rowBooster;
    public AudioClip columnBooster;
    public AudioClip rainbowBooster;
    public AudioClip ovenBooster;

    [Header("Check")]
    public bool playingCookieCrush;
    public bool playingBomb;
    public bool playingBombExplode;
    public bool playingColRowBreaker;
    public bool playingColRowBreakerExplode;
    public bool playingRainbow;
    public bool playingRainbowExplode;
    public bool playingDrop;
    public bool playingWaffleExplode;
    public bool playingCageExplode;
    public bool playingMarshmallowExplode;
    public bool playingChocolateExplode;
    public bool playingRockCandyExplode;

    float delay = 0.3f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingleSound(AudioClip clip)
    {
        if (PlayerPrefs.GetInt("sound_on") == 1)
        {
            efxSource.PlayOneShot(clip);
        }
    }

    #region Play

    public void SwapBackAudio()
    {
        PlaySingleSound(swapBack);
    }

    public void SwapAudio()
    {
        PlaySingleSound(swap);
    }

    public void CookieCrushAudio()
    {
        if (playingCookieCrush == false)
        {
            playingCookieCrush = true;

            PlaySingleSound(cookieCrush);

            StartCoroutine(ResetCookieCrushAudio());
        }
    }

    IEnumerator ResetCookieCrushAudio()
    {
        yield return new WaitForSeconds(delay);

        playingCookieCrush = false;
    }

    public void BombBreakerAudio()
    {
        if (playingBomb == false)
        {
            playingBomb = true;

            PlaySingleSound(bomb);

            StartCoroutine(ResetBombAudio());
        }
    }

    IEnumerator ResetBombAudio()
    {
        yield return new WaitForSeconds(delay);

        playingBomb = false;
    }

    public void BombExplodeAudio()
    {
        if (playingBombExplode == false)
        {
            playingBombExplode = true;

            PlaySingleSound(bombExplode);

            StartCoroutine(ResetBombExplodeAudio());
        }
    }

    IEnumerator ResetBombExplodeAudio()
    {
        yield return new WaitForSeconds(delay);

        playingBombExplode = false;
    }

    public void ColRowBreakerAudio()
    {
        if (playingColRowBreaker == false)
        {
            playingColRowBreaker = true;

            PlaySingleSound(colRowBreaker);

            StartCoroutine(ResetColRowBreakerAudio());
        }
    }

    IEnumerator ResetColRowBreakerAudio()
    {
        yield return new WaitForSeconds(delay);

        playingColRowBreaker = false;
    }

    public void ColRowBreakerExplodeAudio()
    {
        if (playingColRowBreakerExplode == false)
        {
            playingColRowBreakerExplode = true;

            PlaySingleSound(colRowBreakerExplode);

            StartCoroutine(ResetColRowBreakerExplodeAudio());
        }
    }

    IEnumerator ResetColRowBreakerExplodeAudio()
    {
        yield return new WaitForSeconds(delay);

        playingColRowBreakerExplode = false;
    }

    public void RainbowAudio()
    {
        if (playingRainbow == false)
        {
            playingRainbow = true;

            PlaySingleSound(rainbow);

            StartCoroutine(ResetRainbowAudio());
        }
    }

    IEnumerator ResetRainbowAudio()
    {
        yield return new WaitForSeconds(delay);

        playingRainbow = false;
    }

    public void RainbowExplodeAudio()
    {
        if (playingRainbowExplode == false)
        {
            playingRainbowExplode = true;

            PlaySingleSound(rainbowExplode);

            StartCoroutine(ResetRainbowExplodeAudio());
        }
    }

    IEnumerator ResetRainbowExplodeAudio()
    {
        yield return new WaitForSeconds(delay);

        playingRainbowExplode = false;
    }

    public void DropAudio()
    {
        if (playingDrop == false)
        {
            playingDrop = true;

            PlaySingleSound(drop);

            StartCoroutine(ResetDropAudio());
        }
    }

    IEnumerator ResetDropAudio()
    {
        yield return new WaitForSeconds(delay);

        playingDrop = false;
    }

    public void WaffleExplodeAudio()
    {
        if (playingWaffleExplode == false)
        {
            playingWaffleExplode = true;

            PlaySingleSound(waffleExplode);

            StartCoroutine(ResetWaffleExplodeAudio());
        }
    }

    IEnumerator ResetWaffleExplodeAudio()
    {
        yield return new WaitForSeconds(delay);

        playingWaffleExplode = false;
    }

    public void CollectTargetAudio()
    {
        PlaySingleSound(collectTarget);
    }

    public void CageExplodeAudio()
    {
        if (playingCageExplode == false)
        {
            playingCageExplode = true;

            PlaySingleSound(cageExplode);

            StartCoroutine(ResetCageExplodeAudio());
        }
    }

    IEnumerator ResetCageExplodeAudio()
    {
        yield return new WaitForSeconds(delay);

        playingCageExplode = false;
    }

    public void GingerbreadExplodeAudio()
    {
        PlaySingleSound(gingerbreadExplode);
    }

    public void GingerbreadAudio()
    {
        PlaySingleSound(gingerbread);
    }

    public void MarshmallowExplodeAudio()
    {
        if (playingMarshmallowExplode == false)
        {
            playingMarshmallowExplode = true;

            PlaySingleSound(marshmallowExplode);

            StartCoroutine(ResetMarshmallowExplodeAudio());
        }
    }

    IEnumerator ResetMarshmallowExplodeAudio()
    {
        yield return new WaitForSeconds(delay);

        playingMarshmallowExplode = false;
    }

    public void ChocolateExplodeAudio()
    {
        if (playingChocolateExplode == false)
        {
            playingChocolateExplode = true;

            PlaySingleSound(chocolateExplode);

            StartCoroutine(ResetChocolateExplodeAudio());
        }
    }

    IEnumerator ResetChocolateExplodeAudio()
    {
        yield return new WaitForSeconds(delay);

        playingChocolateExplode = false;
    }

    public void CollectibleExplodeAudio()
    {
        PlaySingleSound(collectibleExplode);
    }

    public void RockCandyExplodeAudio()
    {
        if (playingRockCandyExplode == false)
        {
            playingRockCandyExplode = true;

            PlaySingleSound(rockCandyExplode);

            StartCoroutine(ResetRockCandyExplodeAudio());
        }
    }

    IEnumerator ResetRockCandyExplodeAudio()
    {
        yield return new WaitForSeconds(delay);

        playingRockCandyExplode = false;
    }

    #endregion

    #region UI

    public void ButtonClickAudio()
    {
        PlaySingleSound(buttonClick);
    }

    public void PopupTargetAudio()
    {
        PlaySingleSound(UIPopupTarget);
    }

    public void PopupCompletedAudio()
    {
        PlaySingleSound(UIPopupCompleted);
    }

    public void PopupWinAudio()
    {
        PlaySingleSound(UIPopupWin);
    }

    public void PopupLoseAudio()
    {
        PlaySingleSound(UIPopupLose);
    }

    public void CoinPayAudio()
    {
        PlaySingleSound(coinPay);
    }

    public void CoinAddAudio()
    {
        PlaySingleSound(coinAdd);
    }

    public void PopupLevelSkippedAudio()
    {
        PlaySingleSound(UIPopupLevelSkipped);
    }

    public void PopupNoMatchesAudio()
    {
        PlaySingleSound(UIPopupNoMatches);
    }

    #endregion

    #region Booster

    public void SingleBoosterAudio()
    {
        PlaySingleSound(singleBooster);
    }

    public void RowBoosterAudio()
    {
        PlaySingleSound(rowBooster);
    }

    public void ColumnBoosterAudio()
    {
        PlaySingleSound(columnBooster);
    }

    public void RainbowBoosterAudio()
    {
        PlaySingleSound(rainbowBooster);
    }

    public void OvenBoosterAudio()
    {
        PlaySingleSound(ovenBooster);
    }

    // font
    public void amazingAudio()
    {
        PlaySingleSound(amazing);
    }

    public void exellentAudio()
    {
        PlaySingleSound(exellent);
    }

    public void greatAudio()
    {
        PlaySingleSound(great);
    }

    // star
    public void Star1Audio()
    {
        PlaySingleSound(star1);
    }

    public void Star2Audio()
    {
        PlaySingleSound(star2);
    }

    public void Star3Audio()
    {
        PlaySingleSound(star3);
    }

    #endregion
}
