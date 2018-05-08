﻿using System;
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

    COOKIE_RAMDOM,
    COOKIE_RAINBOW,

    COOKIE_1,

    COOKIE_2,
    
    COOKIE_3,
    
    COOKIE_4,
    
    COOKIE_5,
    
    COOKIE_6,

    COOKIE_COLUMN_BREAKER,
    COOKIE_ROW_BREAKER,
    COOKIE_BOMB_BREAKER,
    COOKIE_PLANE_BREAKER,

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
    public bool touchIsSwallowed;

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

    // node
    public static string NodePrefab()
    {
        return "Prefabs/PlayScene/Node";
    }

    // tile
    public static string LightTilePrefab()
    {
        return "Prefabs/PlayScene/TileLayer/LightTile";
    }

    public static string DarkTilePrefab()
    {
        return "Prefabs/PlayScene/TileLayer/DarkTile";
    }

    public static string NoneTilePrefab()
    {
        return "Prefabs/PlayScene/TileLayer/NoneTile";
    }

    //grass

    public static string GrassPrefab()
    {
        return "Prefabs/Grass/Grass";
    }

    // tile border
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

    // cookie1
    public static string Cookie1()
    {
        return "Prefabs/Cookies/blue";
    }

    public static string Cookie1BombBreaker()
    {
        return "Prefabs/Cookies/blue_bomb_breaker";
    }

    public static string Cookie1ColumnBreaker()
    {
        return "Prefabs/Cookies/blue_column_breaker";
    }

    public static string Cookie1RowBreaker()
    {
        return "Prefabs/Cookies/blue_row_breaker";
    }

    public static string Cookie1XBreaker()
    {
        return "Prefabs/Cookies/blue_x_breaker";
    }

    // cookie2
    public static string Cookie2()
    {
        return "Prefabs/Cookies/green";
    }

    public static string Cookie2BombBreaker()
    {
        return "Prefabs/Cookies/green_bomb_breaker";
    }

    public static string Cookie2ColumnBreaker()
    {
        return "Prefabs/Cookies/green_column_breaker";
    }

    public static string Cookie2RowBreaker()
    {
        return "Prefabs/Cookies/green_row_breaker";
    }

    public static string Cookie2XBreaker()
    {
        return "Prefabs/Cookies/green_x_breaker";
    }

    // cookie3
    public static string Cookie3()
    {
        return "Prefabs/Cookies/orange";
    }

    public static string Cookie3BombBreaker()
    {
        return "Prefabs/Cookies/orange_bomb_breaker";
    }

    public static string Cookie3ColumnBreaker()
    {
        return "Prefabs/Cookies/orange_column_breaker";
    }

    public static string Cookie3RowBreaker()
    {
        return "Prefabs/Cookies/orange_row_breaker";
    }

    public static string Cookie3XBreaker()
    {
        return "Prefabs/Cookies/orange_x_breaker";
    }

    // cookie4
    public static string Cookie4()
    {
        return "Prefabs/Cookies/purple";
    }

    public static string Cookie4BombBreaker()
    {
        return "Prefabs/Cookies/purple_bomb_breaker";
    }

    public static string Cookie4ColumnBreaker()
    {
        return "Prefabs/Cookies/purple_column_breaker";
    }

    public static string Cookie4RowBreaker()
    {
        return "Prefabs/Cookies/purple_row_breaker";
    }

    public static string Cookie4XBreaker()
    {
        return "Prefabs/Cookies/purple_x_breaker";
    }

    // cookie5
    public static string Cookie5()
    {
        return "Prefabs/Cookies/red";
    }

    public static string Cookie5BombBreaker()
    {
        return "Prefabs/Cookies/red_bomb_breaker";
    }

    public static string Cookie5ColumnBreaker()
    {
        return "Prefabs/Cookies/red_column_breaker";
    }

    public static string Cookie5RowBreaker()
    {
        return "Prefabs/Cookies/red_row_breaker";
    }

    public static string Cookie5XBreaker()
    {
        return "Prefabs/Cookies/red_x_breaker";
    }

    // cookie6
    public static string Cookie6()
    {
        return "Prefabs/Cookies/yellow";
    }

    public static string Cookie6BombBreaker()
    {
        return "Prefabs/Cookies/yellow_bomb_breaker";
    }

    public static string Cookie6ColumnBreaker()
    {
        return "Prefabs/Cookies/yellow_column_breaker";
    }

    public static string Cookie6RowBreaker()
    {
        return "Prefabs/Cookies/yellow_row_breaker";
    }

    public static string Cookie6XBreaker()
    {
        return "Prefabs/Cookies/yellow_x_breaker";
    }

    //breaker
    public static string PlaneBreaker()
    {
        return "Prefabs/Cookies/plane_breaker";
    }

    // blank
    public static string Blank()
    {
        return "Prefabs/Items/blank";
    }


    // rainbow
    public static string CookieRainbow()
    {
        return "Prefabs/Cookies/rainbow";
    }

    // marshmallow
    public static string Marshmallow()
    {
        return "Prefabs/Items/marshmallow";
    }

    // cherry
    public static string Cherry()
    {
        return "Prefabs/Cookies/cherry";
    }

    // gingerbread
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

    // chocolate
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

    // rock candy
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

    // collectible
    public static string Collectible1()
    {
        return "Prefabs/Items/collectible_1";
    }

    public static string Collectible2()
    {
        return "Prefabs/Items/collectible_2";
    }

    public static string Collectible3()
    {
        return "Prefabs/Items/collectible_3";
    }

    public static string Collectible4()
    {
        return "Prefabs/Items/collectible_4";
    }

    public static string Collectible5()
    {
        return "Prefabs/Items/collectible_5";
    }

    public static string Collectible6()
    {
        return "Prefabs/Items/collectible_6";
    }

    public static string Collectible7()
    {
        return "Prefabs/Items/collectible_7";
    }

    public static string Collectible8()
    {
        return "Prefabs/Items/collectible_8";
    }

    public static string Collectible9()
    {
        return "Prefabs/Items/collectible_9";
    }

    public static string Collectible10()
    {
        return "Prefabs/Items/collectible_10";
    }

    public static string Collectible11()
    {
        return "Prefabs/Items/collectible_11";
    }

    public static string Collectible12()
    {
        return "Prefabs/Items/collectible_12";
    }

    public static string Collectible13()
    {
        return "Prefabs/Items/collectible_13";
    }

    public static string Collectible14()
    {
        return "Prefabs/Items/collectible_14";
    }

    public static string Collectible15()
    {
        return "Prefabs/Items/collectible_15";
    }

    public static string Collectible16()
    {
        return "Prefabs/Items/collectible_16";
    }

    public static string Collectible17()
    {
        return "Prefabs/Items/collectible_17";
    }

    public static string Collectible18()
    {
        return "Prefabs/Items/collectible_18";
    }

    public static string Collectible19()
    {
        return "Prefabs/Items/collectible_19";
    }

    public static string Collectible20()
    {
        return "Prefabs/Items/collectible_20";
    }

    public static string CollectibleBox()
    {
        return "Prefabs/Items/collectible_box";
    }

    // cookie effects
    public static string BlueCookieExplosion()
    {
        return "Effects/Cookie Explosion (Blue)";
    }

    public static string GreenCookieExplosion()
    {
        return "Effects/Cookie Explosion (Green)";
    }

    public static string OrangeCookieExplosion()
    {
        return "Effects/Cookie Explosion (Orange)";
    }

    public static string PurpleCookieExplosion()
    {
        return "Effects/Cookie Explosion (Purple)";
    }

    public static string RedCookieExplosion()
    {
        return "Effects/Cookie Explosion (Red)";
    }

    public static string YellowCookieExplosion()
    {
        return "Effects/Cookie Explosion (Yellow)";
    }

    // breaker explosion
    public static string BreakerExplosion1()
    {
        return "Effects/Explosion (Blue)";
    }

    public static string BreakerExplosion2()
    {
        return "Effects/Explosion (Green)";
    }

    public static string BreakerExplosion3()
    {
        return "Effects/Explosion (Orange)";
    }

    public static string BreakerExplosion4()
    {
        return "Effects/Explosion (Purple)";
    }

    public static string BreakerExplosion5()
    {
        return "Effects/Explosion (Red)";
    }

    public static string BreakerExplosion6()
    {
        return "Effects/Explosion (Yellow)";
    }

    // generic
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

    // rainbow explosion
    public static string RainbowExplosion()
    {
        return "Effects/Rainbow";
    }

    // ring
    public static string RingExplosion()
    {
        return "Effects/Moves Ring";
    }

    // column explosion
    public static string ColRowBreaker1()
    {
        return "Effects/Striped (Blue)";
    }

    // marshmallow explosion
    public static string MarshmallowExplosion()
    {
        return "Effects/Marshmallow Explosion1";
    }

    // chocolate explosion
    public static string ChocolateExplosion()
    {
        return "Effects/Chocolate Explosion";
    }

    public static string ColRowBreaker2()
    {
        return "Effects/Striped (Green)";
    }

    public static string ColRowBreaker3()
    {
        return "Effects/Striped (Orange)";
    }

    public static string ColRowBreaker4()
    {
        return "Effects/Striped (Purple)";
    }

    public static string ColRowBreaker5()
    {
        return "Effects/Striped (Red)";
    }

    public static string ColRowBreaker6()
    {
        return "Effects/Striped (Yellow)";
    }

    // booster
    public static string BoosterActive()
    {
        return "Effects/Booster Active";
    }

	//PackageBox Effects
	public static string PackageBox1Effects()
	{
		return "Effects/Packagebox_1";
	}

	//Jelly Effects
	public static string Jelly1Effects()
	{
		return "Effects/Jelly_1";
	}

    // column breaker animation
    public static string ColumnBreakerAnimation1()
    {
        return "StripeAnim/StripeAnim1";
    }

    public static string ColumnBreakerAnimation2()
    {
        return "StripeAnim/StripeAnim2";
    }

    public static string ColumnBreakerAnimation3()
    {
        return "StripeAnim/StripeAnim3";
    }

    public static string ColumnBreakerAnimation4()
    {
        return "StripeAnim/StripeAnim4";
    }

    public static string ColumnBreakerAnimation5()
    {
        return "StripeAnim/StripeAnim5";
    }

    public static string ColumnBreakerAnimation6()
    {
        return "StripeAnim/StripeAnim6";
    }

    // big column breaker animation
    public static string BigColumnBreakerAnimation1()
    {
        return "StripeAnim/BigStripeAnim1";
    }

    public static string BigColumnBreakerAnimation2()
    {
        return "StripeAnim/BigStripeAnim2";
    }

    public static string BigColumnBreakerAnimation3()
    {
        return "StripeAnim/BigStripeAnim3";
    }

    public static string BigColumnBreakerAnimation4()
    {
        return "StripeAnim/BigStripeAnim4";
    }

    public static string BigColumnBreakerAnimation5()
    {
        return "StripeAnim/BigStripeAnim5";
    }

    public static string BigColumnBreakerAnimation6()
    {
        return "StripeAnim/BigStripeAnim6";
    }

    // waffle
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

    // cage
    public static string Cage1()
    {
        return "Prefabs/Cage/cage_1";
    }

    public static string Cage2()
    {
        return "Prefabs/Cage/cage_2";
    }

    // jelly
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

    // packagebox
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

    //applebox
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


    //ice
    public static string Ice1()
    {
        return "Prefabs/Ice/ice_1";
    }

    public static string Ice2()
    {
        return "Prefabs/Ice/ice_2";
    }

    //baffle
    public static string bafflebottom()
    {
        return "Prefabs/Baffle/baffle_bottom";
    }

    public static string baffleright()
    {
        return "Prefabs/Baffle/baffle_right";
    }

    // cake
    public static string Cake(string name)
    {
        return "Cakes/" + name;
    }

    // star
    public static string StarGold()
    {
        return "Prefabs/PlayScene/UI/StarGold";
    }

    // mask
    public static string Mask()
    {
        return "Prefabs/PlayScene/Mask";
    }   

    // Help
    public static string Level1Step1()
    {
        return "Prefabs/PlayScene/Help/Level1Step1";
    }

    public static string Level1Step2()
    {
        return "Prefabs/PlayScene/Help/Level1Step2";
    }

    public static string Level1Step3()
    {
        return "Prefabs/PlayScene/Help/Level1Step3";
    }

    public static string Level2Step1()
    {
        return "Prefabs/PlayScene/Help/Level2Step1";
    }

    public static string Level2Step2()
    {
        return "Prefabs/PlayScene/Help/Level2Step2";
    }

    public static string Level2Step3()
    {
        return "Prefabs/PlayScene/Help/Level2Step3";
    }

    public static string Level2Step4()
    {
        return "Prefabs/PlayScene/Help/Level2Step4";
    }

    public static string Level2Step5()
    {
        return "Prefabs/PlayScene/Help/Level2Step5";
    }

    public static string Level3Step1()
    {
        return "Prefabs/PlayScene/Help/Level3Step1";
    }

    public static string Level3Step2()
    {
        return "Prefabs/PlayScene/Help/Level3Step2";
    }

    public static string Level3Step3()
    {
        return "Prefabs/PlayScene/Help/Level3Step3";
    }

    public static string Level3Step4()
    {
        return "Prefabs/PlayScene/Help/Level3Step4";
    }

    public static string Level3Step5()
    {
        return "Prefabs/PlayScene/Help/Level3Step5";
    }

    public static string Level4Step1()
    {
        return "Prefabs/PlayScene/Help/Level4Step1";
    }

    public static string Level4Step2()
    {
        return "Prefabs/PlayScene/Help/Level4Step2";
    }

    public static string Level4Step3()
    {
        return "Prefabs/PlayScene/Help/Level4Step3";
    }

    public static string Level4Step4()
    {
        return "Prefabs/PlayScene/Help/Level4Step4";
    }

    public static string Level4Step5()
    {
        return "Prefabs/PlayScene/Help/Level4Step5";
    }

    public static string Level5Step1()
    {
        return "Prefabs/PlayScene/Help/Level5Step1";
    }

    public static string Level5Step2()
    {
        return "Prefabs/PlayScene/Help/Level5Step2";
    }

    public static string Level6Step1()
    {
        return "Prefabs/PlayScene/Help/Level6Step1";
    }
    public static string Level6Step2()
    {
        return "Prefabs/PlayScene/Help/Level6Step2";
    }
    public static string Level6Step3()
    {
        return "Prefabs/PlayScene/Help/Level6Step3";
    }
    public static string Level6Step4()
    {
        return "Prefabs/PlayScene/Help/Level6Step4";
    }

	public static string Level7Step1()
	{
		return "Prefabs/PlayScene/Help/Level7Step1";
	}
	public static string Level7Step2()
	{
		return "Prefabs/PlayScene/Help/Level7Step2";
	}

    public static string Level8Step1()
    {
        return "Prefabs/PlayScene/Help/Level8Step1";
    }

    public static string Level9Step1()
    {
        return "Prefabs/PlayScene/Help/Level9Step1";
    }
    public static string Level9Step2()
    {
        return "Prefabs/PlayScene/Help/Level9Step2";
    }
    public static string Level12Step1()
    {
        return "Prefabs/PlayScene/Help/Level12Step1";
    }
    public static string Level18Step1()
    {
        return "Prefabs/PlayScene/Help/Level18Step1";
    }

    // Progress gold star
    public static string ProgressGoldStar()
    {
        return "Prefabs/PlayScene/UI/StarGold";
    }

    // Win pop up star explode
    public static string StarExplode()
    {
        return "Effects/StarExplode";
    }

    public static string ChoseIcon()
    {
        return "Sprites/Cookie/UI/Play/icon_chose";
    }

    #endregion

    public static string ServerPath = "Config/ServerConfig";

    public static string DebugCanvasPath = "Debug/Debug";
    public static string DebugLevelChoosePanelPath = "Debug/LevelChoosePanel";
    public static string ReporterPath = "Debug/Reporter";

    public static string LevelBundlePath = "play/level";
    public static string ConfigurePath = "core/config";
    public static string SceneBackgroundPath = "scene/background";
    public static string SceneBuildingPath = "scene/building";
    public static string SceneNPCPath = "scene/npc";
    public static string ScenePlayerPath = "scene/player";
    public static string FilmBackgroundPath = "film/bg";

    protected override void Init()
    {
        base.Init();

        SoundOn = PlayerPrefs.GetInt(PlayerPrefEnums.SoundOn) == 1;
        MusicOn = PlayerPrefs.GetInt(PlayerPrefEnums.MusicOn) == 1;

        var config = JsonUtility.FromJson<ServerConfig>(Resources.Load<TextAsset>(ServerPath).text);
        ServerUrl = config.Current.Url;
        AssetBundleServerUrl = config.AssetBundleServer.Url;
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
