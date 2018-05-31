using System;
using Assets.Scripts.Common;
using Core.March.Config;
using UnityEngine;

public enum MAP_LEVEL_STATUS
{
    LOCKED,
    CURRENT,
    OPENED
}

public enum TILE_TYPE
{
    NONE,
    PASS_THROUGH,
    LIGHT_TILE,
    DARD_TILE
}

// waffle
public enum WAFFLE_TYPE
{
    NONE,
    WAFFLE_1,
    WAFFLE_2,
    WAFFLE_3 // do not use
}

public enum ITEM_TYPE
{
    NONE,

    BLANK,

    RAMDOM,
    RAINBOW,

    COOKIE_1,

    COOKIE_2,
    
    COOKIE_3,
    
    COOKIE_4,
    
    COOKIE_5,
    
    COOKIE_6,

    COLUMN_BREAKER,
    ROW_BREAKER,
    BOMB_BREAKER,
    PLANE_BREAKER,

    MARSHMALLOW,

    CHERRY,

    GINGERBREAD_RANDOM,
    GINGERBREAD_1,
    GINGERBREAD_2,
    GINGERBREAD_3,
    GINGERBREAD_4,
    GINGERBREAD_5,
    GINGERBREAD_6,

    CHOCOLATE_1_LAYER,
    CHOCOLATE_2_LAYER,
    CHOCOLATE_3_LAYER,
    CHOCOLATE_4_LAYER,
    CHOCOLATE_5_LAYER,
    CHOCOLATE_6_LAYER,

    ROCK_CANDY_RANDOM,
    ROCK_CANDY,

    COLLECTIBLE_1,
    COLLECTIBLE_2,
    COLLECTIBLE_3,
    COLLECTIBLE_4,
    COLLECTIBLE_5,
    COLLECTIBLE_6,
    COLLECTIBLE_7,
    COLLECTIBLE_8,
    COLLECTIBLE_9,
    COLLECTIBLE_10,
    COLLECTIBLE_11,
    COLLECTIBLE_12,
    COLLECTIBLE_13,
    COLLECTIBLE_14,
    COLLECTIBLE_15,
    COLLECTIBLE_16,
    COLLECTIBLE_17,
    COLLECTIBLE_18,
    COLLECTIBLE_19,
    COLLECTIBLE_20,

    APPLEBOX,
}

public enum APPLEBOX_TYPE
{
    NONE,
    STATUS_0,
    STATUS_1,
    STATUS_2,
    STATUS_3,
    STATUS_4,
    STATUS_5,
    STATUS_6,
    STATUS_7,
    STATUS_8,
}

// cage
public enum CAGE_TYPE
{
    NONE,
    CAGE_1,
    CAGE_2,
}

//ice
public enum ICE_TYPE
{
    NONE,
    ICE_1,
    ICE_2,
}

//grass
public enum GRASS_TYPE
{
    NONE,
    GRASS_1
}

//jelly
public enum JELLY_TYPE
{
    NONE,
    JELLY_1,
    JELLY_2,
    JELLY_3
}

// packagebox
public enum PACKAGEBOX_TYPE
{
    NONE,
    PACKAGEBOX_1,
    PACKAGEBOX_2,
    PACKAGEBOX_3,
    PACKAGEBOX_4,
    PACKAGEBOX_5,
    PACKAGEBOX_6
}

//baffle
public enum BAFFLE_TYPE
{
    NONE,
    BAFFLE_BOTTOM,
    BAFFLE_RIGHT,
}

public enum GAME_STATE
{
    PREPARING_LEVEL,
    WAITING_USER_SWAP,
    PRE_WIN_AUTO_PLAYING,
    OPENING_POPUP,
    NO_MATCHES_REGENERATING,
    DESTROYING_ITEMS
}

public enum BOOSTER_TYPE
{
    NONE = 0,
    SINGLE_BREAKER,
    COLUMN_BREAKER,
    ROW_BREAKER,
    RAINBOW_BREAKER,
    OVEN_BREAKER,
    BEGIN_FIVE_MOVES,
    BEGIN_RAINBOW_BREAKER,
    BEGIN_BOMB_BREAKER,
	BEGIN_PLANE_BREAKER
}

public enum FIND_DIRECTION
{
    NONE = 0,
    ROW,
    COLUMN
}

public enum BREAKER_EFFECT
{
    NORMAL = 0,
    BOMB_ROWCOL_BREAKER,
    BIG_BOMB_BREAKER,
    CROSS,
    CROSS_X_BREAKER,
    BOMB_X_BREAKER,
    PLANE_CHANGE_BREAKER,
    BIG_PLANE_BREAKER,
    NONE
}

public enum TARGET_TYPE
{
    NONE = 0,
    SCORE, // do not use
    COOKIE,
    MARSHMALLOW,
    WAFFLE,
    COLLECTIBLE,
    COLUMN_ROW_BREAKER,
    BOMB_BREAKER,
    X_BREAKER,
    CAGE,
    RAINBOW,
    GINGERBREAD,
    CHOCOLATE,
    ROCK_CANDY,
    GRASS,
    CHERRY,
    PACKAGEBOX,
    APPLEBOX,
}

public enum SWAP_DIRECTION
{
    NONE, 
    TOP,
    RIGHT,
    BOTTOM,
    LEFT,
    SELFCLICK,
}

public class Configure : MonoSingleton<Configure>
{
    public event Action OnSoundChange;
    public event Action OnMusicChange; 

    [Header("Global Settings")]
    private bool musicOn;
    private bool soundOn;

    public bool SoundOn
    {
        get { return soundOn; }
        set {
            if (soundOn != value)
            {
                soundOn = value;
                if (OnSoundChange != null)
                {
                    OnSoundChange();
                }
            }

        }
    }

    public bool MusicOn
    {
        get { return musicOn; }
        set {
            if (musicOn != value)
            {
                musicOn = value;
                if (OnMusicChange != null)
                {
                    OnMusicChange();
                }
            }
        }
    }

    [Header("Configuration")]
    public float swapTime;
    public float destroyTime;
    public float dropTime;
    public float changingTime;
    public float hintDelay;

    [Header("")]
    public int scoreItem;
    public int finishedScoreItem;

    [Header("")]
    public int bonus1Star;
    public int bonus2Star;
    public int bonus3Star;

    [Header("")]
    public int package1Amount;
    public int package2Amount;

    [Header("")]
    public int beginFiveMovesLevel;
    public int beginRainbowLevel;
    public int beginBombBreakerLevel;

    [Header("")]
    public int beginFiveMovesCost1;
    public int beginFiveMovesCost2;
    [Header("")]
    public int beginRainbowCost1;
    public int beginRainbowCost2;
    [Header("")]
    public int beginBombBreakerCost1;
    public int beginBombBreakerCost2;

    // play
    [Header("")]
    public int keepPlayingCost;
    public int skipLevelCost;

    [Header("")]
    public int singleBreakerCost1;
    public int singleBreakerCost2;

    [Header("")]
    public int rowBreakerCost1;
    public int rowBreakerCost2;

    [Header("")]
    public int columnBreakerCost1;
    public int columnBreakerCost2;

    [Header("")]
    public int rainbowBreakerCost1;
    public int rainbowBreakerCost2;

    [Header("")]
    public int ovenBreakerCost1;
    public int ovenBreakerCost2;

    [Header("")]
    public int plusMoves = 5;
    public bool showHint;

    [Header("Check")]
    // map config
    public int autoPopup;

    [Header("")]

    // play config
    public bool beginFiveMoves;
    public bool beginRainbow;
    public bool beginBombBreaker;

    [Header("")]
    //public bool touchIsSwallowed;

    // settings
    public static int maxCookies = 6;

    // max level
    public int maxLevel = 100;
    
    [Header("Check to disable debug")]
    public bool checkSwap; // TEST ONLY

    [Header("Facebook Leaderboard")]
    public bool FBLeaderboard;
    public int FBLoginCoin;

    [Header("Encouraging Popup")]
    public int encouragingPopup;

	[Header("Life")]
	public int life;
	public float timer;
	public string exitDateTime;

	[Header("")]
	public int maxLife;
	public int lifeRecoveryHour;
	public int lifeRecoveryMinute;
	public int lifeRecoverySecond;
    public int recoveryCostPerLife;

    public string ServerUrl;
    public string AssetBundleServerUrl;

    // game data
    public static string game_data = "cookie.dat";
    public static string opened_level = "opened_level";
    public static string level_statistics = "level_statistics";
    public static string level_star = "level_star";
    public static string level_score = "level_score";
    public static string level_number = "level_number";
    public static string player_coin = "player_coin";
    public static string single_breaker = "single_breaker";
    public static string row_breaker = "row_breaker";
    public static string column_breaker = "column_breaker";
    public static string rainbow_breaker = "rainbow_breaker";
    public static string oven_breaker = "oven_breaker";
    public static string begin_five_moves = "begin_five_moves";
    public static string begin_rainbow = "begin_rainbow";
    public static string begin_bomb_breaker = "begin_bomb_breaker";

    #region Prefab

    public static string NodePrefab = "Prefabs/PlayScene/Node";
    public static string LightTilePrefab = "Prefabs/PlayScene/TileLayer/LightTile";
    public static string DarkTilePrefab = "Prefabs/PlayScene/TileLayer/DarkTile";
    public static string NoneTilePrefab = "Prefabs/PlayScene/TileLayer/NoneTile";
    public static string GrassPrefab = "Prefabs/Grass/Grass";
    public static string CollectorPrefab = "Prefabs/Collector";

    public static string UITargetCellPrefab = "Prefabs/Popup/UITargetCell";
    public static string EnhancementItemPrefab = "Prefabs/Popup/EnhancementItem";

    public static string TileBorderTop()
    {
        return "Prefabs/PlayScene/TileLayer/Top/";
    }

    public static string TileBorderBottom()
    {
        return "Prefabs/PlayScene/TileLayer/Bottom/";
    }

    public static string TileBorderLeft()
    {
        return "Prefabs/PlayScene/TileLayer/Left/";
    }

    public static string TileBorderRight()
    {
        return "Prefabs/PlayScene/TileLayer/Right/";
    }

    public static string Cookie1BombBreaker()
    {
        return "Prefabs/Items/bomb_breaker";
    }

    public static string Cookie1ColumnBreaker()
    {
        return "Prefabs/Items/column_breaker";
    }

    public static string Cookie1RowBreaker()
    {
        return "Prefabs/Items/row_breaker";
    }

    public static string PlaneBreaker()
    {
        return "Prefabs/Items/plane_breaker";
    }

    public static string Blank()
    {
        return "Prefabs/Items/blank";
    }

    public static string CookieRainbow()
    {
        return "Prefabs/Items/rainbow";
    }

    public static string Marshmallow()
    {
        return "Prefabs/Items/marshmallow";
    }

    public static string Cherry()
    {
        return "Prefabs/Items/cherry";
    }

    public static string Gingerbread1()
    {
        return "Prefabs/Items/gingerbread_1";
    }

    public static string Gingerbread2()
    {
        return "Prefabs/Items/gingerbread_2";
    }

    public static string Gingerbread3()
    {
        return "Prefabs/Items/gingerbread_3";
    }

    public static string Gingerbread4()
    {
        return "Prefabs/Items/gingerbread_4";
    }

    public static string Gingerbread5()
    {
        return "Prefabs/Items/gingerbread_5";
    }

    public static string Gingerbread6()
    {
        return "Prefabs/Items/gingerbread_6";
    }

    public static string GingerbreadGeneric()
    {
        return "Prefabs/Items/gingerbread_generic";
    }

    public static string Chocolate1()
    {
        return "Prefabs/Items/chocolate_1";
    }

    public static string Chocolate2()
    {
        return "Prefabs/Items/chocolate_2";
    }

    public static string Chocolate3()
    {
        return "Prefabs/Items/chocolate_3";
    }

    public static string Chocolate4()
    {
        return "Prefabs/Items/chocolate_4";
    }

    public static string Chocolate5()
    {
        return "Prefabs/Items/chocolate_5";
    }

    public static string Chocolate6()
    {
        return "Prefabs/Items/chocolate_6";
    }

    public static string RockCandy1()
    {
        return "Prefabs/Items/rock_candy_1";
    }

    public static string RockCandy2()
    {
        return "Prefabs/Items/rock_candy_2";
    }

    public static string RockCandy3()
    {
        return "Prefabs/Items/rock_candy_3";
    }

    public static string RockCandy4()
    {
        return "Prefabs/Items/rock_candy_4";
    }

    public static string RockCandy5()
    {
        return "Prefabs/Items/rock_candy_5";
    }

    public static string RockCandy6()
    {
        return "Prefabs/Items/rock_candy_6";
    }

    public static string RockCandyGeneric()
    {
        return "Prefabs/Items/rock_candy_1";
    }

    public static string ItemsPath = "Prefabs/Items";

    public static string CollectibleBox()
    {
        return "Prefabs/Items/collectible_box";
    }

    public static string ColumnRowBreaker()
    {
        return "Prefabs/Items/column_row_breaker";
    }

    public static string GenericBombBreaker()
    {
        return "Prefabs/Items/generic_bomb_breaker";
    }

    public static string GenericXBreaker()
    {
        return "Prefabs/Items/generic_x_breaker";
    }

    public static string CookieExplosion()
    {
        return "Effects/Cookie Explosion";
    }

    public static string BombBreakerExplosion()
    {
        return "Effects/BombExplosion";
    }

    public static string RainbowExplosion()
    {
        return "Effects/Rainbow";
    }

    public static string RingExplosion()
    {
        return "Effects/Moves Ring";
    }

    public static string ColRowBreaker()
    {
        return "Effects/Striped";
    }

    public static string MarshmallowExplosion()
    {
        return "Effects/Marshmallow Explosion";
    }

    public static string ChocolateExplosion()
    {
        return "Effects/Chocolate Explosion";
    }

    public static string BoosterActive()
    {
        return "Effects/Booster Active";
    }

	public static string PackageBox1Effects()
	{
		return "Effects/Packagebox";
	}

	public static string Jelly1Effects()
	{
		return "Effects/Jelly";
	}

    public static string ColumnBreakerAnimation()
    {
        return "StripeAnim/StripeAnim";
    }

    public static string BigColumnBreakerAnimation1()
    {
        return "StripeAnim/BigStripeAnim";
    }

    public static string Waffle1()
    {
        return "Prefabs/Items/waffle_1";
    }

    public static string Waffle2()
    {
        return "Prefabs/Items/waffle_2";
    }

    public static string Waffle3()
    {
        return "Prefabs/Items/waffle_3";
    }

    public static string Cage1()
    {
        return "Prefabs/Cage/cage_1";
    }

    public static string Cage2()
    {
        return "Prefabs/Cage/cage_2";
    }

    public static string Jelly1()
    {
        return "Prefabs/Jelly/jelly_1";
    }

    public static string Jelly2()
    {
        return "Prefabs/Jelly/jelly_2";
    }
    public static string Jelly3()
    {
        return "Prefabs/Jelly/jelly_3";
    }

    public static string PackageBox1()
    {
        return "Prefabs/PackageBox/packagebox_1";
    }
    public static string PackageBox2()
    {
        return "Prefabs/PackageBox/packagebox_2";
    }
    public static string PackageBox3()
    {
        return "Prefabs/PackageBox/packagebox_3";
    }
    public static string PackageBox4()
    {
        return "Prefabs/PackageBox/packagebox_4";
    }
    public static string PackageBox5()
    {
        return "Prefabs/PackageBox/packagebox_5";
    }
    public static string PackageBox6()
    {
        return "Prefabs/PackageBox/packagebox_6";
    }

    public static string Applebox8Main()
    {
        return "Prefabs/Items/applebox_8_main";
    }
    public static string Applebox7Main()
    {
        return "Prefabs/Items/applebox_7_main";
    }
    public static string Applebox6Main()
    {
        return "Prefabs/Items/applebox_6_main";
    }
    public static string Applebox5Main()
    {
        return "Prefabs/Items/applebox_5_main";
    }
    public static string Applebox4Main()
    {
        return "Prefabs/Items/applebox_4_main";
    }
    public static string Applebox3Main()
    {
        return "Prefabs/Items/applebox_3_main";
    }
    public static string Applebox2Main()
    {
        return "Prefabs/Items/applebox_2_main";
    }
    public static string Applebox1Main()
    {
        return "Prefabs/Items/applebox_1_main";
    }
    public static string Applebox0Main()
    {
        return "Prefabs/Items/applebox_0_main";
    }
    public static string AppleboxOther()
    {
        return "Prefabs/Items/applebox_other";
    }
    public static string Apple()
    {
        return "Prefabs/Items/apple";
    }

    public static string Ice1()
    {
        return "Prefabs/Ice/ice_1";
    }

    public static string Ice2()
    {
        return "Prefabs/Ice/ice_2";
    }

    public static string bafflebottom()
    {
        return "Prefabs/Baffle/baffle_bottom";
    }

    public static string baffleright()
    {
        return "Prefabs/Baffle/baffle_right";
    }

    public static string Cake(string name)
    {
        return "Cakes/" + name;
    }

    public static string StarGold()
    {
        return "Prefabs/PlayScene/UI/StarGold";
    }

    public static string Mask()
    {
        return "Prefabs/PlayScene/Mask";
    }   

    public static string ProgressGoldStar()
    {
        return "Prefabs/PlayScene/UI/StarGold";
    }

    public static string ChoseIcon()
    {
        return "Sprites/Cookie/UI/Play/icon_chose";
    }

    #endregion

    public static string ServerPath = "Config/ServerConfig";
    public static string AssetBundleServerPath = "Config/AssetBundleServerConfig";

    public static string DebugCanvasPath = "Debug/Debug";
    public static string DebugLevelChoosePanelPath = "Debug/LevelChoosePanel";
    public static string ReporterPath = "Debug/Reporter";

    public static string LevelBundlePath = "play/level";
    public static string ConfigurePath = "core/config";
    public static string FilmBackgroundPath = "film/bg";

    public static string SceneBackgroundPath = "scene/background";
    public static string SceneBuildingPath = "scene/building";
    public static string SceneNPCPath = "scene/npc";
    public static string ScenePlayerPath = "scene/player";

    public static string StoryBackground = "story/bg";
    public static string StoryPerson = "story/person";
    public static string StoryDialogPerson = "story/dialogperson";
    public static string LoseTargetCellPrefab = "Prefabs/Popup/LoseTargetCell";
    public static string LoseStepBuyItemCellPrefab = "Prefabs/Popup/LoseStepBuyItemCell";

    protected override void Init()
    {
        base.Init();

        SoundOn = PlayerPrefs.GetInt(PlayerPrefEnums.SoundOn) == 1;
        MusicOn = PlayerPrefs.GetInt(PlayerPrefEnums.MusicOn) == 1;

        var config = JsonUtility.FromJson<ServerConfig>(Resources.Load<TextAsset>(ServerPath).text);
        ServerUrl = config.Current.Url;

        var abConfig = JsonUtility.FromJson<ServerConfig>(Resources.Load<TextAsset>(AssetBundleServerPath).text);
        AssetBundleServerUrl = abConfig.Current.Url;
    }

    void OnApplicationQuit()
	{
		SaveLifeInfo();
	}

	public void SaveLifeInfo()
	{
        PlayerPrefs.SetFloat(PlayerPrefEnums.Timer, timer);

        PlayerPrefs.SetInt(PlayerPrefEnums.Life, life);

        PlayerPrefs.SetString(PlayerPrefEnums.ExitDateTime, DateTime.Now.ToString());

        PlayerPrefs.Save();
	}

    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveLifeInfo();
        }
        else
        {
            if (GameObject.Find("LifeBar"))
            {
                instance.exitDateTime = PlayerPrefs.GetString(PlayerPrefEnums.ExitDateTime, new DateTime().ToString());
                instance.timer = PlayerPrefs.GetFloat(PlayerPrefEnums.Timer, 0f);
                instance.life = PlayerPrefs.GetInt(PlayerPrefEnums.Life, instance.maxLife);

                GameObject.Find("LifeBar").GetComponent<Life>().runTimer = false;
            }
        }
    }
}
