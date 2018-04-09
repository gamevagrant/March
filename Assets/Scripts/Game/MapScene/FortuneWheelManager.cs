using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public enum LUCKY_SPIN_REWARDS
{
    FIVE_COINS_REWARD,
    TEN_COINS_REWARD,
    TWENTY_COINS_REWARD,
    THIRTY_COINS_REWARD,
    SINGLE_BREAKER_REWARD,
    ROW_BREAKER_REWARD,
    COL_BREAKER_REWARD,
    RAINBOW_BREAKER_REWARD,
    OVEN_BREAKER_REWARD,
    BEGIN_FIVE_MOVE_REWARD,
    BEGIN_RAINBOW_REWARD,
    BEGIN_BOMB_BREAKER_REWARD
}

public class FortuneWheelManager : MonoBehaviour
{
    public GameObject PaidTurnButton; 				// This button is showed when you can turn the wheel for coins
    public GameObject FreeTurnButton;				// This button is showed when you can turn the wheel for free
	public GameObject Circle; 					// Rotatable GameObject on scene with reward objects
	public Text DeltaCoinsText; 				// Pop-up text with wasted or rewarded coins amount
	public Text CurrentCoinsText; 				// Pop-up text with wasted or rewarded coins amount
	public GameObject NextTurnTimerWrapper;
	public Text NextFreeTurnTimerText;			// Text element that contains remaining time to next free turn
    public Image CoinsDeltaImage;

    public int SpinCost = 25;
    public int boosterRewardCount = 2;

    public List<Sprite> awardImage;
    
    private LUCKY_SPIN_REWARDS Sector1 = LUCKY_SPIN_REWARDS.THIRTY_COINS_REWARD;
    private LUCKY_SPIN_REWARDS Sector2 = LUCKY_SPIN_REWARDS.SINGLE_BREAKER_REWARD; // single bomb
    private LUCKY_SPIN_REWARDS Sector3 = LUCKY_SPIN_REWARDS.ROW_BREAKER_REWARD; // row bomb
    private LUCKY_SPIN_REWARDS Sector4 = LUCKY_SPIN_REWARDS.TEN_COINS_REWARD;
    private LUCKY_SPIN_REWARDS Sector5 = LUCKY_SPIN_REWARDS.COL_BREAKER_REWARD; // col bomb
    private LUCKY_SPIN_REWARDS Sector6 = LUCKY_SPIN_REWARDS.RAINBOW_BREAKER_REWARD; // rainbow bomb
    private LUCKY_SPIN_REWARDS Sector7 = LUCKY_SPIN_REWARDS.TWENTY_COINS_REWARD;
    private LUCKY_SPIN_REWARDS Sector8 = LUCKY_SPIN_REWARDS.OVEN_BREAKER_REWARD; // oven
    private LUCKY_SPIN_REWARDS Sector9 = LUCKY_SPIN_REWARDS.BEGIN_FIVE_MOVE_REWARD; // begin five move
    private LUCKY_SPIN_REWARDS Sector10 = LUCKY_SPIN_REWARDS.FIVE_COINS_REWARD;
    private LUCKY_SPIN_REWARDS Sector11 = LUCKY_SPIN_REWARDS.BEGIN_RAINBOW_REWARD; // begin rainbow
    private LUCKY_SPIN_REWARDS Sector12 = LUCKY_SPIN_REWARDS.BEGIN_BOMB_BREAKER_REWARD; // begin bomb

	private bool _isStarted;					// Flag that the wheel is spinning
	private float[] _sectorsAngles;				// All sectors angles
	private float _finalAngle;					// The final angle is needed to calculate the reward
	private float _startAngle = 0;				// The first time start angle equals 0 but the next time it equals the last final angle
	private float _currentLerpRotationTime;		// Needed for spinning animation
	private int _turnCost = 25;			    // How much coins user waste when turn when wheel
	private int _currentCoinsAmount = 0;		// Started coins amount. In your project it should be picked up from CoinsManager or from PlayerPrefs and so on
	private int _previousCoinsAmount;

	// Here you can set time between two free turns
	private int _timerMaxHours = 24;
	private int _timerMaxMinutes = 0;
	private int _timerMaxSeconds = 0;

	// Remaining time to next free turn
	private int _timerRemainingHours = 0;
	private int _timerRemainingMinutes = 0;
	private int _timerRemainingSeconds = 0;

	private DateTime _nextFreeTurnTime;

	// Key name for storing in PlayerPrefs
	private const string LAST_FREE_TURN_TIME_NAME = "LastFreeTurnTimeTicks";

	// Set TRUE if you want to let players to turn the wheel for coins while free turn is not available yet
	private bool _isPaidTurnEnabled = true;

	// Set TRUE if you want to let players to turn the wheel for FREE from time to time
	private bool _isFreeTurnEnabled = true;

	// Flag that player can turn the wheel for free right now
	private bool _isFreeTurnAvailable = false;

	private void Awake ()
	{
        // define coin amount
        _currentCoinsAmount = GameData.instance.GetPlayerCoin();

        // spin cost
        _turnCost = SpinCost;

		_previousCoinsAmount = _currentCoinsAmount;
		// Show our current coins amount
		CurrentCoinsText.text = _currentCoinsAmount.ToString ();

		if (_isFreeTurnEnabled) {
			if (!PlayerPrefs.HasKey(LAST_FREE_TURN_TIME_NAME)) {
                //print("Very first free spin: " + DateTime.Now.AddDays(-1).Ticks.ToString());

                PlayerPrefs.SetString(LAST_FREE_TURN_TIME_NAME, DateTime.Now.AddDays(-7).Ticks.ToString());
			}

            // Start our timer to next free turn
            SetNextFreeTime();
		} else {
			NextTurnTimerWrapper.gameObject.SetActive (false);
		}
	}

	private void TurnWheelForFree() { TurnWheel (true);	}
	private void TurnWheelForCoins() { TurnWheel (false); }

	private void TurnWheel (bool isFree)
	{
		_currentLerpRotationTime = 0f;

		// Fill the necessary angles (for example if you want to have 12 sectors you need to fill the angles with 30 degrees step)
		_sectorsAngles = new float[] { 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };

		int fullTurnovers = 5;

		// Choose random final sector
		float randomFinalAngle = _sectorsAngles [UnityEngine.Random.Range (0, _sectorsAngles.Length)];

		// Set up how many turnovers our wheel should make before stop
		_finalAngle = -(fullTurnovers * 360 + randomFinalAngle);

		// Stop the wheel
		_isStarted = true;

		_previousCoinsAmount = _currentCoinsAmount;

		// Decrease money for the turn if it is not free turn
		if (!isFree) {
			_currentCoinsAmount -= _turnCost;

			// Show wasted coins
			DeltaCoinsText.text = String.Format ("-{0}", _turnCost);
			DeltaCoinsText.gameObject.SetActive (true);

            CoinsDeltaImage.overrideSprite = awardImage[0];

            // reduce coin sound
            AudioManager.instance.CoinPayAudio();

            // update game data
            GameData.instance.SavePlayerCoin(GameData.instance.GetPlayerCoin() - _turnCost);

            // update text label
            if (GameObject.Find("MapScene"))
            {
                GameObject.Find("MapScene").GetComponent<MapScene>().UpdateCoinAmountLabel();
            }

			// Animations for coins
			StartCoroutine (HideCoinsDelta ());
			StartCoroutine (UpdateCoinsAmount ());
		} else {
			// At this step you can save current time value to your server database as last used free turn
			// We can't save long int to PlayerPrefs so store this value as string and convert to long later
			PlayerPrefs.SetString (LAST_FREE_TURN_TIME_NAME, DateTime.Now.Ticks.ToString());

			// Restart timer to next free turn
			SetNextFreeTime ();
		}
	}

	public void TurnWheelButtonClick ()
	{
		if (_isFreeTurnAvailable) {
			TurnWheelForFree ();
		} else {
			// If we have enabled paid turns
			if (_isPaidTurnEnabled) {
				// If player have enough coins
				if (_currentCoinsAmount >= _turnCost) {
					TurnWheelForCoins ();
				}				
			}
		}
	}

	public void SetNextFreeTime() {
		// Reset the remaining time values
		_timerRemainingHours = _timerMaxHours;
		_timerRemainingMinutes = _timerMaxMinutes;
		_timerRemainingSeconds = _timerMaxSeconds;

		// Get last free turn time value from storage
		// We can't save long int to PlayerPrefs so store this value as string and convert to long
		_nextFreeTurnTime = new DateTime(Convert.ToInt64(PlayerPrefs.GetString (LAST_FREE_TURN_TIME_NAME, DateTime.Now.Ticks.ToString())))
								.AddHours(_timerMaxHours)
								.AddMinutes(_timerMaxMinutes)
								.AddSeconds(_timerMaxSeconds);

		_isFreeTurnAvailable = false;
	}

	private void GiveAwardByAngle ()
	{
		// Here you can set up rewards for every sector of wheel (clockwise)
		switch ((int)_startAngle) {
		// Sector 1
		case 0:
			RewardCoins (Sector1);
			break;
		// Sector 2
		case -330:
            RewardCoins(Sector2);
			break;
		// Sector 3
		case -300:
            RewardCoins(Sector3);
			break;
		// Sector 4
		case -270:
            RewardCoins(Sector4);
			break;
		// Sector 5
		case -240:
            RewardCoins(Sector5);
			break;
		// Sector 6
		case -210:
            RewardCoins(Sector6);
			break;
		// Sector 7
		case -180:
            RewardCoins(Sector7);
			break;
		// Sector 8
		case -150:
            RewardCoins(Sector8);
			break;
		// Sector 9
		case -120:
            RewardCoins(Sector9);
			break;
		// Sector 10
		case -90:
            RewardCoins(Sector10);
			break;
		// Sector 11
		case -60:
            RewardCoins(Sector11);
			break;
		// Sector 12
		case -30:
            RewardCoins(Sector12);
			break;
		default:
			break;
		}
	}

	private void ShowTurnButtons ()
	{
		if (_isFreeTurnAvailable)				// If have free turn
		{			
			ShowFreeTurnButton ();
			EnableFreeTurnButton ();

		} else { 								// If haven't free turn
			
			if (!_isPaidTurnEnabled)			// If our settings allow only free turns
			{
				ShowFreeTurnButton ();
				DisableFreeTurnButton ();		// Make button inactive while spinning or timer to next free turn

			} else { 							// If player can turn for coins
				ShowPaidTurnButton ();

				if (_isStarted || _currentCoinsAmount < _turnCost)
					DisablePaidTurnButton ();	// Make button non interactable if user has not enough money for the turn of if wheel is turning right now
				else
					EnablePaidTurnButton ();	// Can make paid turn right now
			}
		}
	}

	private void Update ()
	{
		// We need to show TURN FOR FREE button or TURN FOR COINS button
		ShowTurnButtons ();

		// Show timer only if we enable free turns
		if (_isFreeTurnEnabled)
			UpdateFreeTurnTimer ();

		if (!_isStarted)
			return;

		// Animation time
		float maxLerpRotationTime = 4f;

		// increment animation timer once per frame
		_currentLerpRotationTime += Time.deltaTime;

		// If the end of animation
		if (_currentLerpRotationTime > maxLerpRotationTime || Circle.transform.eulerAngles.z == _finalAngle) {
			_currentLerpRotationTime = maxLerpRotationTime;
			_isStarted = false;
			_startAngle = _finalAngle % 360;

			GiveAwardByAngle ();
			StartCoroutine (HideCoinsDelta ());
		} else {
			// Calculate current position using linear interpolation
			float t = _currentLerpRotationTime / maxLerpRotationTime;

			// This formula allows to speed up at start and speed down at the end of rotation.
			// Try to change this values to customize the speed
			t = t * t * t * (t * (6f * t - 15f) + 10f);

			float angle = Mathf.Lerp (_startAngle, _finalAngle, t);
			Circle.transform.eulerAngles = new Vector3 (0, 0, angle);	
		}
	}

	// Give the reward to player
    private void RewardCoins(LUCKY_SPIN_REWARDS award)
	{
        //print("Reward to player: " + award);

        int awardCoins = 0;

        switch (award)
        {
            case LUCKY_SPIN_REWARDS.FIVE_COINS_REWARD:
            case LUCKY_SPIN_REWARDS.TEN_COINS_REWARD:
            case LUCKY_SPIN_REWARDS.TWENTY_COINS_REWARD:
            case LUCKY_SPIN_REWARDS.THIRTY_COINS_REWARD:

                //print("Award coins");

                if (award == LUCKY_SPIN_REWARDS.FIVE_COINS_REWARD) awardCoins = 5;
                else if (award == LUCKY_SPIN_REWARDS.TEN_COINS_REWARD) awardCoins = 10;
                else if (award == LUCKY_SPIN_REWARDS.TWENTY_COINS_REWARD) awardCoins = 20;
                else if (award == LUCKY_SPIN_REWARDS.THIRTY_COINS_REWARD) awardCoins = 30;

                _currentCoinsAmount += awardCoins;
		        // Show animated delta coins
		        DeltaCoinsText.text = String.Format("+{0}", awardCoins);
		        DeltaCoinsText.gameObject.SetActive (true);
		        StartCoroutine (UpdateCoinsAmount ());

                // plus coin
                GameData.instance.SavePlayerCoin(_currentCoinsAmount);

                // play add coin sound
                AudioManager.instance.CoinAddAudio();

                // update text label
                if (GameObject.Find("MapScene"))
                {
                    GameObject.Find("MapScene").GetComponent<MapScene>().UpdateCoinAmountLabel();
                }

                break;

            case  LUCKY_SPIN_REWARDS.SINGLE_BREAKER_REWARD:
            case  LUCKY_SPIN_REWARDS.ROW_BREAKER_REWARD:
            case  LUCKY_SPIN_REWARDS.COL_BREAKER_REWARD:
            case  LUCKY_SPIN_REWARDS.RAINBOW_BREAKER_REWARD:
            case  LUCKY_SPIN_REWARDS.OVEN_BREAKER_REWARD:
            case  LUCKY_SPIN_REWARDS.BEGIN_FIVE_MOVE_REWARD:
            case  LUCKY_SPIN_REWARDS.BEGIN_RAINBOW_REWARD:
            case  LUCKY_SPIN_REWARDS.BEGIN_BOMB_BREAKER_REWARD:

                //print("Award booster");

                // Show animated delta coins
                DeltaCoinsText.text = String.Format("+{0}", boosterRewardCount);
                DeltaCoinsText.gameObject.SetActive(true);

                

                if (award == LUCKY_SPIN_REWARDS.SINGLE_BREAKER_REWARD)
                {
                    CoinsDeltaImage.overrideSprite = awardImage[1];
                    GameData.instance.SaveSingleBreaker(GameData.instance.GetSingleBreaker() + boosterRewardCount);
                }
                else if (award == LUCKY_SPIN_REWARDS.ROW_BREAKER_REWARD)
                {
                    CoinsDeltaImage.overrideSprite = awardImage[2];
                    GameData.instance.SaveRowBreaker(GameData.instance.GetRowBreaker() + boosterRewardCount);
                }
                else if (award == LUCKY_SPIN_REWARDS.COL_BREAKER_REWARD)
                {
                    CoinsDeltaImage.overrideSprite = awardImage[3];
                    GameData.instance.SaveColumnBreaker(GameData.instance.GetColumnBreaker() + boosterRewardCount);
                }
                else if (award == LUCKY_SPIN_REWARDS.RAINBOW_BREAKER_REWARD)
                {
                    CoinsDeltaImage.overrideSprite = awardImage[4];
                    GameData.instance.SaveRainbowBreaker(GameData.instance.GetRainbowBreaker() + boosterRewardCount);
                }
                else if (award == LUCKY_SPIN_REWARDS.OVEN_BREAKER_REWARD) 
                {
                    CoinsDeltaImage.overrideSprite = awardImage[5];
                    GameData.instance.SaveOvenBreaker(GameData.instance.GetOvenBreaker() + boosterRewardCount);
                }
                else if (award == LUCKY_SPIN_REWARDS.BEGIN_FIVE_MOVE_REWARD) 
                {
                    CoinsDeltaImage.overrideSprite = awardImage[6];
                    GameData.instance.SaveBeginFiveMoves(GameData.instance.GetBeginFiveMoves() + boosterRewardCount);
                }
                else if (award == LUCKY_SPIN_REWARDS.BEGIN_RAINBOW_REWARD)
                {
                    CoinsDeltaImage.overrideSprite = awardImage[7];
                    GameData.instance.SaveBeginRainbow(GameData.instance.GetBeginRainbow() + boosterRewardCount);
                }
                else if (award == LUCKY_SPIN_REWARDS.BEGIN_BOMB_BREAKER_REWARD)
                {
                    CoinsDeltaImage.overrideSprite = awardImage[8];
                    GameData.instance.SaveBeginBombBreaker(GameData.instance.GetBeginBombBreaker() + boosterRewardCount);
                }

                // play add coin sound
                AudioManager.instance.CoinAddAudio();

                break;

            default:
                print("Default award");
                break;
        }
	}

	// Hide coins delta text after animation
	private IEnumerator HideCoinsDelta ()
	{
		yield return new WaitForSeconds (1f);
		DeltaCoinsText.gameObject.SetActive (false);
	}

	// Animation for smooth increasing and decreasing the number of coins
	private IEnumerator UpdateCoinsAmount ()
	{
		const float seconds = 0.5f; // Animation duration
		float elapsedTime = 0;

		while (elapsedTime < seconds) {
			CurrentCoinsText.text = Mathf.Floor(Mathf.Lerp (_previousCoinsAmount, _currentCoinsAmount, (elapsedTime / seconds))).ToString ();
			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}
		 
		_previousCoinsAmount = _currentCoinsAmount;

		CurrentCoinsText.text = _currentCoinsAmount.ToString ();
	}

	// Change remaining time to next free turn every 1 second
	private void UpdateFreeTurnTimer () 
	{
		// Don't count the time if we have free turn already
		if (_isFreeTurnAvailable)
			return;

		// Update the remaining time values
		_timerRemainingHours = (int)(_nextFreeTurnTime - DateTime.Now).Hours;
		_timerRemainingMinutes = (int)(_nextFreeTurnTime - DateTime.Now).Minutes;
		_timerRemainingSeconds = (int)(_nextFreeTurnTime - DateTime.Now).Seconds;

		// If the timer has ended
		if (_timerRemainingHours <= 0 && _timerRemainingMinutes <= 0 && _timerRemainingSeconds <= 0) {
			NextFreeTurnTimerText.text = "  Ready!";
			// Now we have a free turn
			_isFreeTurnAvailable = true;
		} else {
			// Show the remaining time
			NextFreeTurnTimerText.text = "  " + String.Format ("{0:00}:{1:00}:{2:00}", _timerRemainingHours, _timerRemainingMinutes, _timerRemainingSeconds);
			// We don't have a free turn yet
			_isFreeTurnAvailable = false;
		}
	}

	private void EnableButton (GameObject button)
	{
        button.SetActive(true);
	}

    private void DisableButton(GameObject button)
	{
        button.SetActive(false);
	}

	// Function for more readable calls
	private void EnableFreeTurnButton () { EnableButton (FreeTurnButton); }
	private void DisableFreeTurnButton () {	DisableButton (FreeTurnButton);	}
	private void EnablePaidTurnButton () { EnableButton (PaidTurnButton); }
	private void DisablePaidTurnButton () { DisableButton (PaidTurnButton); }

	private void ShowFreeTurnButton ()
	{
		FreeTurnButton.gameObject.SetActive(true); 
		PaidTurnButton.gameObject.SetActive(false);
	}

	private void ShowPaidTurnButton ()
	{
		PaidTurnButton.gameObject.SetActive(true); 
		FreeTurnButton.gameObject.SetActive(false);
	}

    public void ButtonClickAudio()
    {
        AudioManager.instance.ButtonClickAudio();
    }
}

