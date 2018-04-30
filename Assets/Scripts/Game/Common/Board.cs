using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using March.Core.WindowManager;
using UnityEngine;
using UnityEngine.UI;
using qy.config;
using qy;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
// Unity Ads
#endif

public class Board : MonoBehaviour 
{
    // public
    [Header("Nodes")]
    public List<Node> nodes = new List<Node>();

    [Header("Board variables")]    
    public GAME_STATE state;
    public bool lockSwap;
    public int moveLeft;
    public int dropTime;
    public int score;
    public int star;
    public List<int> targetLeftList;

    [Header("Booster")]
    public BOOSTER_TYPE booster;
    public List<Item> boosterItems = new List<Item>();
    public Item ovenTouchItem;

    [Header("Check")]
    public int destroyingItems;
    public int droppingItems;
    public int flyingItems;
    public int playingAnimation;
    public int matching;
    public int specialDestroying;
    public GAME_STATE originalStateInSDestroying;
    public bool isFirstMove;

    public bool needIncreaseBubble = false;

    [Header("")]
    public bool movingGingerbread;
    public bool generatingGingerbread;
    public bool skipGenerateGingerbread;
    public bool showingInspiringPopup;
    public int skipGingerbreadCount;
    
    [Header("Item Lists")]
    public List<Item> changingList;
    public List<Item> sameColorList;

    [Header("Swap")]
    public Item touchedItem;
    public Item swappedItem;
    public Item clickedItem;

    // UI
    [Header("UI")]
    public UITarget UITarget;
    public UITop UITop;

    // hint
    [Header("Hint")]
    public int checkHintCall;
    public int showHintCall;
    public List<Item> hintItems = new List<Item>();

    [Header("AppleBox")]
    public List<AppleBox> appleBoxes = new List<AppleBox>();

    [Header("PlanePlus")]
    public int planePlusNum = 0;
    // private
    Vector3 firstNodePosition;

    [Header("data")]
    public int allstep;
    public int FiveMoreTimes;
    public int minWinGold;
    public int winGold;

    void Awake()
    {
        // debug
        if (LevelLoader.instance.level == 0)
        {
            LevelLoader.instance.LoadLevel();
        }            
    }

	void Start () 
    {
        state = GAME_STATE.PREPARING_LEVEL;
        moveLeft = LevelLoader.instance.moves;
        isFirstMove = true;
        FiveMoreTimes = 0;

        targetLeftList.Clear();
        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            targetLeftList.Add(LevelLoader.instance.targetList[i].Amount);
        }

        allstep = 0;

        string itemid = (1000000 + LevelLoader.instance.level).ToString();
        //LevelItem levelconfig = LevelLoader.instance.LevelConfig.GetItemByID(itemid);
        MatchLevelItem levelconfig = GameMainManager.Instance.configManager.matchLevelConfig.GetItem(itemid);
        winGold = 0;
        if (levelconfig != null)
        {
            winGold = levelconfig.coin;
        }
        else
        {
            Debug.LogError("matchlevel表中找不当当前关卡配置！");
        }
        minWinGold = winGold;

        GenerateBoard();

        BeginBooster();

        // reset color of the random cookie to make sure there is no match in the original board
        GenerateNoMatches();

        // open target popup
        TargetPopup();
    }
	
	void Update () 
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}

        if (state == GAME_STATE.WAITING_USER_SWAP && lockSwap == false && moveLeft > 0)
        {
            if (needIncreaseBubble)
            {
                needIncreaseBubble = false;
                IncreaseBubble();

            }
            else
            {
                if (Configure.instance.touchIsSwallowed 
                    && GameObject.Find("Help") != null 
                    && GameObject.Find("Help").activeSelf
                    )
                {
                    return;
                }

                // no booster
                if (booster == BOOSTER_TYPE.NONE)
                {
                    // mouse down
                    if (Input.GetMouseButtonDown(0))
                    {
                        // hit the collier
                        Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        if (hit != null)
                        {
                            Item item = hit.gameObject.GetComponent<Item>();
                            if (item != null)
                            {
                                if (item.Exchangeable(SWAP_DIRECTION.NONE))
                                {
                                    ClickChoseItem(item);

                                }
                                else
                                {
                                    CancelChoseItem();
                                }
                            }
                            else
                            {
                                CancelChoseItem();
                            }
                        }
                        else
                        {
                            CancelChoseItem();
                        }
                    }
                    // mouse up
                    else if (Input.GetMouseButtonUp(0))
                    {
                        Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        if (hit != null)
                        {
                            var item = hit.gameObject.GetComponent<Item>();
                            if (item != null)
                            {
                                item.drag = false;
                                item.swapDirection = Vector3.zero;
                            }
                        }
                    }
                }
                // use booster
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        if (hit != null)
                        {
                            var item = hit.gameObject.GetComponent<Item>();
                            if (item != null)
                            {
                                DestroyBoosterItems(item);
                            }
                        }
                    }

                }
            }// if need create bubble

        } // if state is WAITING_USER_SWAP

        // fix freezing board
//        if (state == GAME_STATE.DESTROYING_ITEMS)
//        {
//            if (lockSwap == false && destroyingItems == 0 && droppingItems == 0 && flyingItems == 0 && matching == 0 && playingAnimation == 0)
//            {
//                state = GAME_STATE.WAITING_USER_SWAP;
//            }
//        }
    }
    #region Board

    void GenerateBoard()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                GameObject node = Instantiate(Resources.Load(Configure.NodePrefab())) as GameObject;
                node.transform.SetParent(gameObject.transform, false);
                node.name = "Node " + order;
                node.GetComponent<Node>().board = this;
                node.GetComponent<Node>().i = i;
                node.GetComponent<Node>().j = j;

                nodes.Add(node.GetComponent<Node>());
            } // end for j
        } // end for i


        //todo: 一次循环即可 去掉多次循环
        GenerateTileLayer();
        GenerateTileBorder();

        GenerateGrassLayer();

        GenerateWaffleLayer();

        GenerateItemLayer();

        GenerateJellyLayer();

        GeneratePackageBoxLayer();

        GenerateCageLayer();

        GenerateIceLayer();

        GenerateBaffleLayer();

        GenerateCollectibleBoxByColumn();
        GenerateCollectibleBoxByNode();
    }

    void GenerateTileLayer()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                var tileLayerData = LevelLoader.instance.tileLayerData;

                GameObject tile = null;
                switch (tileLayerData[order])
                {
                    case TILE_TYPE.NONE:
                        tile = Instantiate(Resources.Load(Configure.NoneTilePrefab())) as GameObject;
                        break;
                    case TILE_TYPE.PASS_THROUGH:
                        tile = Instantiate(Resources.Load(Configure.NoneTilePrefab())) as GameObject;
                        break;
                    //case TILE_TYPE.LIGHT_TILE:
                    //    tile = Instantiate(Resources.Load(Configure.LightTilePrefab())) as GameObject;
                    //    break;
                    //case TILE_TYPE.DARD_TILE:
                    //    tile = Instantiate(Resources.Load(Configure.DarkTilePrefab())) as GameObject;
                    //    break;
                }

                if ((i % 2 + j % 2) % 2 == 0)
                {
                    if (tile == null)
                        tile = Instantiate(Resources.Load(Configure.LightTilePrefab())) as GameObject;
                }
                else
                {
                    if (tile == null)
                        tile = Instantiate(Resources.Load(Configure.DarkTilePrefab())) as GameObject;
                }

                if (tile)
                {
                    tile.transform.SetParent(nodes[order].gameObject.transform);
                    tile.name = "Tile";
                    if (tile.GetComponent<SpriteRenderer>())
                    {
                        tile.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    }
                    tile.transform.localPosition = NodeLocalPosition(i, j);
                    tile.GetComponent<Tile>().type = tileLayerData[order];
                    tile.GetComponent<Tile>().node = nodes[order];

                    //if (tile.GetComponent<SpriteRenderer>()) tile.GetComponent<SpriteRenderer>().enabled = false;

                    nodes[order].tile = tile.GetComponent<Tile>();
                }

            } // end for j
        } // end for i
    }

    void GenerateTileBorder()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                nodes[order].tile.SetBorder();
            }
        }
    }

    //grass


    void GenerateGrassLayer()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);


                var grassLayerData = LevelLoader.instance.grassLayerData;

                GameObject grass = null;
                if (grassLayerData.Count == row * column)//兼容旧关卡格式
                {
                    switch (grassLayerData[order])
                    {
                        case GRASS_TYPE.GRASS_1:
                            grass = Instantiate(Resources.Load(Configure.GrassPrefab())) as GameObject;
                            break;
                        case GRASS_TYPE.NONE:
                            grass = null;
                            break;
                    }
                }
                if (grass)
                {
                    grass.transform.SetParent(nodes[order].gameObject.transform);
                    grass.name = "Grass";
                    grass.transform.localPosition = NodeLocalPosition(i, j);
                    grass.GetComponent<Grass>().type = 0;
                    grass.GetComponent<Grass>().node = nodes[order];
                    //if (tile.GetComponent<SpriteRenderer>()) tile.GetComponent<SpriteRenderer>().enabled = false;

                    nodes[order].grass = grass.GetComponent<Grass>();
                }
            }
        }
    }



// waffle
void GenerateWaffleLayer()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                var waffleLayerData = LevelLoader.instance.waffleLayerData;

                GameObject waffle = null;

                switch (waffleLayerData[order])
                {
                    case WAFFLE_TYPE.WAFFLE_1:
                        waffle = Instantiate(Resources.Load(Configure.Waffle1())) as GameObject;
                        break;
                    case WAFFLE_TYPE.WAFFLE_2:
                        waffle = Instantiate(Resources.Load(Configure.Waffle2())) as GameObject;
                        break;
                    case WAFFLE_TYPE.WAFFLE_3:
                        waffle = Instantiate(Resources.Load(Configure.Waffle3())) as GameObject;
                        break;
                }

                if (waffle)
                {
                    waffle.transform.SetParent(nodes[order].gameObject.transform);
                    waffle.name = "Waffle";
                    waffle.transform.localPosition = NodeLocalPosition(i, j);
                    waffle.GetComponent<Waffle>().type = waffleLayerData[order];
                    waffle.GetComponent<Waffle>().node = nodes[order];
                    waffle.GetComponent<SpriteRenderer>().sortingLayerName = "Waffle";

                    nodes[order].waffle = waffle.GetComponent<Waffle>();
                }
            }
        }
    }

    void GenerateItemLayer()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                var itemLayerData = LevelLoader.instance.itemLayerData;

                if (nodes[order].CanStoreItem())
                {
                    nodes[order].GenerateItem(itemLayerData[order]);

                    // add mask
                    var mask = Instantiate(Resources.Load(Configure.Mask())) as GameObject;
                    mask.transform.SetParent(nodes[order].transform);
                    mask.transform.localPosition = NodeLocalPosition(i, j);
                    mask.name = "Mask";
                }
            }
        }
    }

    void GenerateCageLayer()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        int beabletodestroy = 1;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                var cageLayerData = LevelLoader.instance.cageLayerData;

                GameObject cage = null;

                switch (cageLayerData[order])
                {
                    case CAGE_TYPE.CAGE_1:
                        cage = Instantiate(Resources.Load(Configure.Cage1())) as GameObject;
                        beabletodestroy = 1;
                        break;
                    case CAGE_TYPE.CAGE_2:
                        cage = Instantiate(Resources.Load(Configure.Cage2())) as GameObject;
                        beabletodestroy = 2;
                        break;
                }

                if (cage)
                {
                    cage.transform.SetParent(nodes[order].gameObject.transform);
                    cage.name = "Cage";
                    cage.transform.localPosition = NodeLocalPosition(i, j);
                    cage.GetComponent<Cage>().type = cageLayerData[order];
                    cage.GetComponent<Cage>().node = nodes[order];

                    if (nodes[order].item != null)
                    {
                        nodes[order].item.beAbleToDestroy += beabletodestroy;
                    }

                    nodes[order].cage = cage.GetComponent<Cage>();
                }
            }
        }
    }

    void GenerateJellyLayer()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;
        int beabletodestroy = 1;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                var jellyLayerData = LevelLoader.instance.jellyLayerData;

                GameObject jelly = null;

                if (jellyLayerData.Count == row * column)//兼容旧关卡格式
                {
                    switch (jellyLayerData[order])
                    {
                        case JELLY_TYPE.JELLY_1:
                            jelly = Instantiate(Resources.Load(Configure.Jelly1())) as GameObject;
                            beabletodestroy = 1;
                            break;
                        case JELLY_TYPE.JELLY_2:
                            jelly = Instantiate(Resources.Load(Configure.Jelly2())) as GameObject;
                            beabletodestroy = 2;
                            break;
                        case JELLY_TYPE.JELLY_3:
                            jelly = Instantiate(Resources.Load(Configure.Jelly3())) as GameObject;
                            beabletodestroy = 3;
                            break;
                    }

                }

                if (jelly)
                {
                    jelly.transform.SetParent(nodes[order].gameObject.transform);
                    jelly.name = "Jelly";
                    jelly.transform.localPosition = NodeLocalPosition(i, j);
                    jelly.GetComponent<Jelly>().type = jellyLayerData[order];
                    jelly.GetComponent<Jelly>().node = nodes[order];

                    if (nodes[order].item != null)
                    {
                        nodes[order].item.beAbleToDestroy += beabletodestroy;
                    }

                    nodes[order].jelly = jelly.GetComponent<Jelly>();
                }
            }
        }
    }

    void GeneratePackageBoxLayer()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;
        int beabletodestroy = 1;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                var packageboxLayerData = LevelLoader.instance.packageboxLayerData;

                GameObject packagebox = null;

                if (packageboxLayerData.Count == row * column)//兼容旧关卡格式
                {
                    switch (packageboxLayerData[order])
                    {
                        case PACKAGEBOX_TYPE.PACKAGEBOX_1:
                            packagebox = Instantiate(Resources.Load(Configure.PackageBox1())) as GameObject;
                            beabletodestroy = 1;
                            break;
                        case PACKAGEBOX_TYPE.PACKAGEBOX_2:
                            packagebox = Instantiate(Resources.Load(Configure.PackageBox2())) as GameObject;
                            beabletodestroy = 2;
                            break;
                        case PACKAGEBOX_TYPE.PACKAGEBOX_3:
                            packagebox = Instantiate(Resources.Load(Configure.PackageBox3())) as GameObject;
                            beabletodestroy = 3;
                            break;
                        case PACKAGEBOX_TYPE.PACKAGEBOX_4:
                            packagebox = Instantiate(Resources.Load(Configure.PackageBox4())) as GameObject;
                            beabletodestroy = 4;
                            break;
                        case PACKAGEBOX_TYPE.PACKAGEBOX_5:
                            packagebox = Instantiate(Resources.Load(Configure.PackageBox5())) as GameObject;
                            beabletodestroy = 5;
                            break;
                        case PACKAGEBOX_TYPE.PACKAGEBOX_6:
                            packagebox = Instantiate(Resources.Load(Configure.PackageBox6())) as GameObject;
                            beabletodestroy = 6;
                            break;
                    }

                }

                if (packagebox)
                {
                    packagebox.transform.SetParent(nodes[order].gameObject.transform);
                    packagebox.name = "PackageBox";
                    packagebox.transform.localPosition = NodeLocalPosition(i, j);
                    packagebox.GetComponent<PackageBox>().type = packageboxLayerData[order];
                    packagebox.GetComponent<PackageBox>().node = nodes[order];

                    if (nodes[order].item != null)
                    {
                        nodes[order].item.beAbleToDestroy += beabletodestroy;
                    }

                    nodes[order].packagebox = packagebox.GetComponent<PackageBox>();
                }
            }
        }
    }


    void GenerateIceLayer()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        int beabletodestroy = 1;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                var iceLayerData = LevelLoader.instance.iceLayerData;

                GameObject ice = null;

                if (iceLayerData.Count == row * column) //兼容旧关卡格式
                {
                    switch (iceLayerData[order])
                    {
                        case ICE_TYPE.ICE_1:
                            ice = Instantiate(Resources.Load(Configure.Ice1())) as GameObject;
                            beabletodestroy = 1;
                            break;
                        case ICE_TYPE.ICE_2:
                            ice = Instantiate(Resources.Load(Configure.Ice2())) as GameObject;
                            beabletodestroy = 2;
                            break;
                    }
                }
                if (ice)
                {
                    ice.transform.SetParent(nodes[order].gameObject.transform);
                    ice.name = "Ice";
                    ice.transform.localPosition = NodeLocalPosition(i, j);
                    ice.GetComponent<Ice>().type = iceLayerData[order];
                    ice.GetComponent<Ice>().node = nodes[order];

                    if (nodes[order].item != null)
                    {
                        nodes[order].item.beAbleToDestroy += beabletodestroy;
                    }

                    nodes[order].ice = ice.GetComponent<Ice>();
                }
            }
        }
    }

    void GenerateBaffleLayer()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                var baffleRightLayerData = LevelLoader.instance.baffleRightLayerData;

                GameObject baffleright = null;

                if (baffleRightLayerData.Count == row * column) //兼容旧关卡格式
                {
                    switch (baffleRightLayerData[order])
                    {
                        case BAFFLE_TYPE.BAFFLE_RIGHT:
                            baffleright = Instantiate(Resources.Load(Configure.baffleright())) as GameObject;
                            break;
                    }
                }
                if (baffleright)
                {
                    baffleright.transform.SetParent(nodes[order].gameObject.transform);
                    baffleright.name = "BaffleRight";
                    baffleright.transform.localPosition = NodeLocalPosition(i, j);
                    baffleright.GetComponent<Baffle>().type = baffleRightLayerData[order];
                    baffleright.GetComponent<Baffle>().node = nodes[order];

                    nodes[order].baffleright = baffleright.GetComponent<Baffle>();
                }


                var baffleBottomLayerData = LevelLoader.instance.baffleBottomLayerData;

                GameObject bafflebottom = null;

                if (baffleBottomLayerData.Count == row * column) //兼容旧关卡格式
                {
                    switch (baffleBottomLayerData[order])
                    {
                        case BAFFLE_TYPE.BAFFLE_BOTTOM:
                            bafflebottom = Instantiate(Resources.Load(Configure.bafflebottom())) as GameObject;
                            break;
                    }
                }
                if (bafflebottom)
                {
                    bafflebottom.transform.SetParent(nodes[order].gameObject.transform);
                    bafflebottom.name = "BaffleBottom";
                    bafflebottom.transform.localPosition = NodeLocalPosition(i, j);
                    bafflebottom.GetComponent<Baffle>().type = baffleBottomLayerData[order];
                    bafflebottom.GetComponent<Baffle>().node = nodes[order];

                    nodes[order].bafflebottom = bafflebottom.GetComponent<Baffle>();
                }

            }
        }
    }


    void GenerateCollectibleBoxByColumn()
    {
        bool hasCollectible = false;
        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.COLLECTIBLE)
            {
                hasCollectible = true;
                break;
            }
        }
        if (!hasCollectible)
        {
            return;
        }

        var row = LevelLoader.instance.row;

        foreach (var column in LevelLoader.instance.collectibleCollectColumnMarkers)
        {
            var node = GetNode(row - 1, column);

            if (node != null && node.CanStoreItem())
            {
				var box = Instantiate(Resources.Load("Prefabs/Items/Collector")) as GameObject;
				if (box)
				{
					box.transform.SetParent(node.gameObject.transform);
					box.name = "Box";
					box.transform.localPosition = NodeLocalPosition(node.i, node.j) + new Vector3(0, -1 * NodeSize() + 0.4f, 0);
					box.transform.localScale = new Vector3 (0.7f, 0.7f, 1.0f);
				}
            }
        }
    }

    void GenerateCollectibleBoxByNode()
    {
        bool hasCollectible = false;
        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.COLLECTIBLE)
            {
                hasCollectible = true;
                break;
            }
        }
        if (!hasCollectible)
        {
            return;
        }

        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var order = NodeOrder(i, j);

                if (LevelLoader.instance.collectibleCollectNodeMarkers.Contains(order))
                {
                    var node = GetNode(i, j);

                    if (node != null)
                    {
						var box = Instantiate(Resources.Load("Prefabs/Items/Collector")) as GameObject;
						if (box)
						{
							box.transform.SetParent(node.gameObject.transform);
							box.name = "Box";
							box.transform.localPosition = NodeLocalPosition(node.i, node.j) + new Vector3(0, -1 * NodeSize() + 0.4f, 0);
							box.transform.localScale = new Vector3 (0.7f, 0.7f, 1.0f);
						}
                    }
                }
            }
        }
    }

    #endregion

    #region Begin

    void BeginBooster()
    {
        for (int i = LevelLoader.instance.beginItemList.Count-1; i >= 0; i--)
        {
            BoosterEffect(LevelLoader.instance.beginItemList[i]);
        }
    }

    public void BoosterEffect(string itemId)
    {
        var items = GetListItems();
        var cookies = new List<Item>();

        foreach (var item in items)
        {
            if (item != null && item.IsCookie() && item.Movable())
            {
                cookies.Add(item);
            }
        }
        var cookie = cookies[Random.Range(0, cookies.Count - 1)];

        if (itemId == "200003") //rainbow
        {
            cookie.ChangeToRainbow();
        }
        else if (itemId == "200004")
        {
            int rdmBomb = 0;
            int rdmRocket = 0;

            while (rdmRocket == rdmBomb)
            {
                rdmRocket = Random.Range(0, cookies.Count - 1);
                rdmBomb = Random.Range(0, cookies.Count - 1);
            }

            cookies[rdmBomb].ChangeToBombBreaker();
            cookies[rdmRocket].ChangeToColRowBreaker();
        }
        else if (itemId == "200005")
        {
            this.planePlusNum = 1;

        }
        else if (itemId == "200008")
        {
            cookie.ChangeToColRowBreaker();
        }
        else if (itemId == "200009")
        {
            cookie.ChangeToBombBreaker();
        }
        else if (itemId == "200010")
        {
            cookie.ChangeToPlaneBreaker();
        }
    }


    #endregion

    #region Utility
    Vector3 CalculateFirstNodePosition()
    {
        var width = NodeSize();
        var height = NodeSize();
        var column = LevelLoader.instance.column;
        var row = LevelLoader.instance.row;

        var offset = new Vector3(2, 0, 0);

        return (new Vector3(-((column - 1) * width / 2), (row - 1) * height / 2, 0) + offset);
    }

    public float NodeSize()
    {
        return 1.7f;
    }

    public Vector3 NodeLocalPosition(int i, int j)
    {
        var width = NodeSize();
        var height = NodeSize();

        if (firstNodePosition == Vector3.zero)
        {
            firstNodePosition = CalculateFirstNodePosition();
        }

        var x = firstNodePosition.x + j * width;
        var y = firstNodePosition.y - i * height;

        return new Vector3(x, y, 0);
    }

    public int NodeOrder(int i, int j)
    {
        return (i * LevelLoader.instance.column + j);
    }

    public Node GetNode(int row, int column)
    {
        if (row < 0 || row >= LevelLoader.instance.row || column < 0 || column >= LevelLoader.instance.column)
        {
            return null;
        }
        return nodes[row * LevelLoader.instance.column + column];
    }  

    Vector3 ColumnFirstItemPosition(int i, int j)
    {
        Node node = GetNode(i, j);

        if (node != null)
        {
            var item = node.item;

            if (item != null && item.type != ITEM_TYPE.BLANK)
            {
                return item.gameObject.transform.position;
            }
            else
            {
                return ColumnFirstItemPosition(i + 1, j);
            }
        }
        else
        {
            return Vector3.zero;
        }
    }

    // return a list of items
    public List<Item> GetListItems()
    {
        var items = new List<Item>();

        foreach (var node in nodes)
        {
            if (node != null)
            {
                items.Add(node.item);
            }
        }

        return items;
    }

    public int GetMostColor()
    {
        var sameColorItems = new Dictionary<int, int>();

        foreach (var node in nodes)
        {
            if (node != null && node.item != null && node.item.IsCookie())
            {
                if (sameColorItems.ContainsKey(node.item.color))
                {
                    sameColorItems[node.item.color]++;
                }
                else
                {
                    sameColorItems.Add(node.item.color, 1);
                }
            }
        }
        int count = 0;

        int mostColor = 1;

        foreach (var sameItems in sameColorItems)
        {
            if (count < sameItems.Value)
            {
                count = sameItems.Value;
                mostColor = sameItems.Key;
            }
        }

        return mostColor;
    }

    #endregion

    #region Match

    // return the list of square matches on the board
    public List<List<Item>> GetSquareMatches()
    {
        var combines = new List<List<Item>>();

        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                if (GetNode(i, j) != null)
                {
                    List<Item> combine = GetNode(i, j).FindSquareMatches();

                    // combine can be null
                    if (combine != null)
                    {
                        if (combine.Count == 4)
                        {
                            combines.Add(combine);
                        }
                    }
                }
            }
        }

        return combines;
    }



    // re-generate the board to make sure there is no "pre-matches"
    void GenerateNoMatches()
    {
        //Debug.Log("Start generating matches");

        var combines = GetMatches();
        var squareCombines = GetSquareMatches();

        var runNum = 0;

        do
        {
            List<Item> rdmItems = new List<Item>();
            foreach (var combine in combines)
            {
                foreach (var item in combine)
                {
                    if (item != null)
                    {

                        if (!rdmItems.Contains(item))
                        {
                            rdmItems.Add(item);
                        }

                    }
                }
            }

            foreach (var combine in squareCombines)
            {
                foreach (var item in combine)
                {

                    if (item != null)
                    {
                        if (!rdmItems.Contains(item))
                        {
                            rdmItems.Add(item);
                        }
                    }

                }
            }

            // only re-generate color for random item
            int i = 0;
            foreach (var item in rdmItems)
            {
                if (item.OriginCookieType() == ITEM_TYPE.COOKIE_RAMDOM)
                {
                    item.GenerateColor(item.color + i);
                    i++;
                }
            }


            squareCombines = GetSquareMatches();
            combines = GetMatches();
            runNum++;
            if (runNum > 400)
            {
                Debug.Log("初始地图配置存在无法解决的初始可消除问题！！！ 检查关卡配置！！！");
                break;
            }



        } while (combines.Count > 0 || squareCombines.Count > 0);

        //Debug.Log("End generating matches");
    }

    // return the list of matches on the board
    public List<List<Item>> GetMatches(FIND_DIRECTION direction = FIND_DIRECTION.NONE, int matches = 3)
    {
        var combines = new List<List<Item>>();

        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                if (GetNode(i, j) != null)
                {
                    List<Item> combine = GetNode(i, j).FindMatches(direction, matches);

                    // combine can be null
                    if (combine != null)
                    {
                        if (combine.Count >= matches)
                        {
                            combines.Add(combine);
                        }
                    }
                }
            }
        }

        return combines;
    }

    public void FindMatches()
    {
        //print("find matches");

        StartCoroutine(DestroyMatches());
    }

    // destroy the matches on the board
    IEnumerator DestroyMatches()
    {
        matching++;

        while (true)
        {
            var destroyItemList = new List<Item>();

            var grasschangeList = new List<Node>();

            var combines = GetMatches();
            
            //Debug.Log("Number of combines: " + combines.Count);

            foreach (var combine in combines)
            {
                //Debug.Log("Combine count: " + combine.Count);

                if (combine.Count == 3 && combines.Count > 3)
                {
                    // item in match-3 can be a bomb-breaker/x-breaker
                    SetBombBreakerCombine(GetMatches(FIND_DIRECTION.ROW));
                }
                else if (combine.Count == 4)
                {
                    SetColRowBreakerCombine(combine);
                }
                else if (combine.Count >= 5)
                {
                    SetRainbowCombine(combine);
                }

                var isGrass = false;

                foreach (var item in combine)
                {
                    destroyItemList.Add(item);

                    if (item.node.grass != null)
                    {
                        isGrass = true;
                    }
                }
                if (isGrass)
                {
                    foreach (var item in combine)
                    {
                        if (!grasschangeList.Contains(item.node))
                        {
                            grasschangeList.Add(item.node);
                        }
                    }
                }

            } // end foreach combines

            var squareCombines = GetSquareMatches();

            foreach (var combine in squareCombines)
            {
                if (combine.Count == 4)
                {
                    SetPlaneCombine(combine);
                }
                bool isDestroy = false;
                foreach (var item in combine)
                {
                    if (!(item.next == ITEM_TYPE.NONE || item.next == ITEM_TYPE.COOKIE_PLANE_BREAKER))
                    {
                        isDestroy = true;
                    }
                }

                var isGrass = false;

                if (!isDestroy)
                {
                    foreach (var item in combine)
                    {
                        destroyItemList.Add(item);

                        if (item.node.grass != null)
                        {
                            isGrass = true;
                        }
                    }
                }

                if (isGrass)
                {
                    foreach (var item in combine)
                    {
                        if (!grasschangeList.Contains(item.node))
                        {
                            grasschangeList.Add(item.node);
                        }
                    }
                }
            }

            foreach (var node in grasschangeList)
            {
                node.ChangeToGrass();
            }


            foreach (var item in destroyItemList)
            {
                item.Destroy();
            }

            // wait until item destroy animation finish
            while (destroyingItems > 0 || playingAnimation > 0)
            {
                //Debug.Log("Destroying items");
                yield return new WaitForSeconds(0.1f);
            }

            // IMPORTANT: as describe in document Destroy is always delayed (but executed within the same frame).
            // So There is case destroyingItems = 0 BUT the item still exist that causes the GenerateNewItems function goes wrong
            yield return new WaitForEndOfFrame();

            for (int i = appleBoxes.Count-1; i >=0; i--)
            {
                appleBoxes[i].TryToDestroyBox();
            }

            yield return new WaitForEndOfFrame();

            // new items
            Drop();

            while (droppingItems > 0)
            {
                //Debug.Log("Dropping items");
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForEndOfFrame();

            // check if collect collectible

            if (GetSquareMatches().Count <= 0 && GetMatches().Count <= 0 && CollectCollectible() == false)
            {
                break;
            }

            // increase dropTime
            dropTime++;

        } // end while

        // wait until all flying items fly to top bar
        while (flyingItems > 0)
        {
            //Debug.Log("Flying items");
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForEndOfFrame();

        if (matching > 1)
        {
            matching--;
            yield break;
        }

        // check if level complete
        if (state == GAME_STATE.WAITING_USER_SWAP)
        {
            if (moveLeft > 0)
            {
                if (IsLevelCompleted())
                {
                    StartCoroutine(PreWinAutoPlay());
                }
                else
                {
                    if (MoveGingerbread())
                    {
                        yield return new WaitForSeconds(Configure.instance.swapTime);

                        yield return new WaitForSeconds(0.2f);

                        FindMatches();
                    }
                    
                    if (GenerateGingerbread())
                    {
                        yield return new WaitForSeconds(0.2f);

                        FindMatches();
                    }

                    if (Help.instance.help == false)
                    {
                        StartCoroutine(CheckHint());
                    }
                    else
                    {
                        Help.instance.Show();
                    }
                }
            }
            else if (moveLeft == 0)
            {
                if (IsLevelCompleted())
                {
                    SaveLevelInfo();

                    // show win popup
                    state = GAME_STATE.OPENING_POPUP;

                    WindowManager.instance.Show<WinPopupWindow>();

                    AudioManager.instance.PopupWinAudio();

//                    ShowAds();
                }
                else
                {
                    // show lose popup
                    state = GAME_STATE.OPENING_POPUP;

                    WindowManager.instance.Show<LosePopupWindow>();

                    AudioManager.instance.PopupLoseAudio();

//                    ShowAds();
                }
            }
        }

        matching--;

        // if dropTime >= 3 we should show some text like: grate, amazing, etc.
        if (dropTime >= Configure.instance.encouragingPopup && state == GAME_STATE.WAITING_USER_SWAP && showingInspiringPopup == false)
        {
            ShowInspiringPopup();
        }

        // when finish function we can swap again
        //yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.2f);
        lockSwap = false;
    }

    #endregion

    #region Drop

    void Drop()
    {
        SetDropTargets();

        GenerateNewItems(true, Vector3.zero);

        Move();

        DropItems();
    }

    // set drop target to the remain items
    void SetDropTargets()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int j = 0; j < column; j++)
        {
            //need to enumerate rows from bottom to top
            for (int i = row - 1; i >= 0; i--)
            {
                Node node = GetNode(i, j);

                if (node != null)
                {
                    Item item = node.item;

                    if (item != null)
                    {
                        // start calculating new target for the node
                        if (item.Droppable())
                        {
                            Node target = node.BottomNeighbor();

                            if (target != null && target.CanGoThrough() && target.CanDropIn() && !target.HasTopBaffle())
                            {
                                if (target.item == null || (target.item != null && target.item.type == ITEM_TYPE.BLANK))
                                {
                                    // check rows below at this time GetNode(i + 1, j) = target
                                    for (int k = i + 2; k < row; k++)
                                    {
                                        if (GetNode(k, j) != null)
                                        {
                                            if (GetNode(k, j).item != null && GetNode(k, j).item.type == ITEM_TYPE.BLANK)
                                            {
                                                if (GetNode(k, j).CanStoreItem() && GetNode(k, j).CanDropIn() && !GetNode(k, j).HasTopBaffle())
                                                {
                                                    target = GetNode(k, j);
                                                }
                                            }

                                            // if a node can not go through we do not need to check bellow
                                            if (GetNode(k, j).CanGoThrough() == false || !GetNode(k, j).CanDropIn() || GetNode(k, j).HasTopBaffle())
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                if (GetNode(k, j).item != null && GetNode(k, j).item.type != ITEM_TYPE.BLANK)
                                                {
                                                    if (GetNode(k, j).item.Droppable() == false)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                    // after have the target we swap items on nodes
                                if ((target.item != null && target.item.type == ITEM_TYPE.BLANK)&& target.ice == null && target.CanStoreItem())
                                {
                                    if (target.item != null && target.item.type == ITEM_TYPE.BLANK)
                                    {
                                        Destroy(target.item.gameObject);
                                    }
                                    target.item = item;
                                    target.item.gameObject.transform.SetParent(target.gameObject.transform);
                                    target.item.node = target;

                                    node.item = null;
                                    node.GenerateItem(ITEM_TYPE.BLANK);

                                    if ( node.ice != null)
                                    {
                                        target.ice = node.ice;
                                        target.ice.gameObject.transform.SetParent(target.gameObject.transform);
                                        target.ice.node = target;

                                        node.ice = null;

                                    }
                                }
                            } // end if target != null
                        } // end item dropable
                    } // end item != null
                } // end node != null
            } // end for i
        } // end for j
    }

    // after destroy and drop items then we generate new items
    void GenerateNewItems(bool IsDrop, Vector3 pos)
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        var marshmallowGenerated = false;

        for (int j = 0; j < column; j++) 
        {
            var space = -1;

            var itemPos = Vector3.zero;

            for (int i = row - 1; i >= 0; i--) 
            {
                if (GetNode(i, j) != null)
                {
                    if (((GetNode(i, j).item != null && GetNode(i, j).item.type == ITEM_TYPE.BLANK)) && GetNode(i, j).CanGenerateNewItem())
                    {
                        // if target is collectible the new item can be a collectible
                        var collectible = false;

                        // collectible is only generated on the highest row
                        if (i == 0)
                        {
                            // check if need to generate new collectible
                            if (CheckGenerateCollectible() != null && 
                                CheckGenerateCollectible().Count > 0 && 
                                (LevelLoader.instance.collectibleGenerateMarkers.Contains(j) || LevelLoader.instance.collectibleGenerateMarkers.Count == 0))
                            {
                                collectible = true;
                            }                            
                        }

                        // check if need to generate a new marshmallow
                        var marshmallow = false;

                        if (CheckGenerateMarshmallow())
                        {
                            marshmallow = true;
                        }

                        if (pos != Vector3.zero)
                        {
                            itemPos = pos + Vector3.up * NodeSize();
                        }
                        else
                        {
                            // calculate position of the new item
                            if (i > space)
                            {
                                space = i;
                            }

                            // can pass through node
                            var pass = 0;

                            for (int k = 0; k < row; k++)
                            {
                                var node = GetNode(k, j);

                                if (node != null && node.tile != null && node.tile.type == TILE_TYPE.PASS_THROUGH)
                                {
                                    pass++;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            itemPos = NodeLocalPosition(i, j) + Vector3.up * (space - pass + 1) * NodeSize();
                        }

                        if (GetNode(i, j).item != null && GetNode(i, j).item.type == ITEM_TYPE.BLANK)
                        {
                            Destroy(GetNode(i, j).item.gameObject);
                            GetNode(i, j).item = null;
                        }

                        //print("COOKIE: Generate new item");

                        // if target is collectible then generate a new collectible item
                        if (collectible && Random.Range(0, 2) == 1)
                        {
                            GetNode(i, j).GenerateItem(CheckGenerateCollectible()[Random.Range(0, CheckGenerateCollectible().Count)]);
                        }
                        // generate a marshmallow
                        else if (marshmallow && Random.Range(0, 2) == 1 && marshmallowGenerated == false)
                        {
                            marshmallowGenerated = true;

                            GetNode(i, j).GenerateItem(ITEM_TYPE.MARSHMALLOW);
                        }
                        // generate a new random cookie
                        else
                        {
                            GetNode(i, j).GenerateItem(ITEM_TYPE.COOKIE_RAMDOM);
                        }

                        // set position

                        var newItem = GetNode(i, j).item;
                        if (newItem != null)
                        {
                            if (IsDrop)
                            {
                                newItem.gameObject.transform.localPosition = itemPos;
                            }
                            else
                            {
                                newItem.gameObject.transform.localPosition = NodeLocalPosition(i, j);
                            }
                        }
                    }
                }
            }
        }
    }

    // move item to neighbor empty node
    void Move()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = row - 1; i >= 0; i--)
        {
            //need to enumerate rows from bottom to top
            for (int j = 0; j < column; j++)
            {
                Node node = GetNode(i, j);

                if (node != null)
                {
                    if (node.item != null && node.item.type == ITEM_TYPE.BLANK && node.CanStoreItem() && node.CanDropIn())
                    {
                        Node source = node.GetSourceNode();

                        if (source != null)
                        {
                            //Debug.Log("source node: " + source.name);

                            // new item position
                            var pos = ColumnFirstItemPosition(0, source.j);
                            
                            //print(pos);

                            // calculate move path
                            List<Vector3> path = node.GetMovePath();

                            if (source.transform.position != NodeLocalPosition(source.i, source.j))
                            {
                                // if source item is just generated
                                path.Add(NodeLocalPosition(source.i, source.j)+ transform.position);
                            }

                            if (node.item != null && node.item.type == ITEM_TYPE.BLANK && node.CanDropIn())
                            {
                                Destroy(node.item.gameObject);
                            }

                            node.item = source.item;
                            node.item.gameObject.transform.SetParent(node.gameObject.transform);
                            node.item.node = node;

                            source.item = null;
                            source.GenerateItem(ITEM_TYPE.BLANK);

                            if (source.ice != null)
                            {
                                node.ice = source.ice;
                                node.ice.gameObject.transform.SetParent(node.gameObject.transform);
                                node.ice.node = node;

                                source.ice = null;
                            }

                            if (path.Count > 1)
                            {
                                path.Reverse();

                                node.item.dropPath = path;
                            }

                            SetDropTargets();

                            GenerateNewItems(true, pos);
                            
                        } // end if source node != null
                    }
                } // end if node != null
            } // for j
        } // for i
    }

    // drop item to new position
    void DropItems()
    {
        StartCoroutine(DropItems2());
    }


    IEnumerator DropItems2()
    {
        //print("COOKIE: Drop items");

        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = row - 1; i >= 0; i--)
        {
            bool hasDrop = false;
            for (int j = 0; j < column; j++)
                {
                if (GetNode(i, j) != null)
                {
                    if (GetNode(i, j).item != null)
                    {
                        if (GetNode(i, j).item.isNeedDrop())
                        {
                            hasDrop = true;
                            GetNode(i, j).item.Drop();
                        }
                    }
                }
            }
            if (hasDrop)
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    #endregion

    #region Item

    // this function check all the items and set them to be bomb-breaker/x-breaker
    public void SetBombBreakerCombine(List<List<Item>> lists)
    {
        foreach (List<Item> list in lists)
        {
            foreach (Item item in list)
            {
                if (item != null && item.node != null)
                {
                    //print(item.node.name);

                    if (item.node.FindMatches(FIND_DIRECTION.COLUMN).Count > 2)
                    {
                        //todo : 优先级根据配置 改掉死代码
                        if (item.next == ITEM_TYPE.NONE || item.next == ITEM_TYPE.COOKIE_ROW_BREAKER || item.next == ITEM_TYPE.COOKIE_COLUMN_BREAKER || item.next == ITEM_TYPE.COOKIE_PLANE_BREAKER)
                        {
                            // L shape = bomb breaker
                            item.next = item.GetBombBreaker(item.type);

                        } // item.next = none
                    } // count > 2
                }
            }
        }
    }

    public void SetColRowBreakerCombine(List<Item> combine)
    {
        bool isSwap = false;

        //todo : 优先级根据配置 改掉死代码
        foreach (Item item in combine)
        {
            if (item.next != ITEM_TYPE.NONE )
            {
                isSwap = true;

                break;
            }
        }

        // next type is normal (drop then match) get first item in the combine
        if (!isSwap)
        {
            Item first = null;

            foreach (Item item in combine)
            {
                if (first == null)
                {
                    first = item;
                }
                else
                {
                    if (item.node.OrderOnBoard() < first.node.OrderOnBoard())
                    {
                        first = item;
                    }
                }
            }

            foreach (Item item in combine)
            {
                if (first.node.RightNeighbor())
                {
                    if (item.node.OrderOnBoard() == first.node.RightNeighbor().OrderOnBoard())
                    {
                        first.next = first.GetColumnBreaker(first.type);
                        break;
                    }
                }

                if (first.node.BottomNeighbor())
                {
                    if (item.node.OrderOnBoard() == first.node.BottomNeighbor().OrderOnBoard())
                    {
                        first.next = first.GetRowBreaker(first.type);
                        break;
                    }
                }
            }
        } // not swap
    }

    public void SetRainbowCombine(List<Item> combine)
    {

        bool isSwap = false;
        //todo : 优先级根据配置 改掉死代码
       // rainbow优先级最高
        foreach (Item item in combine)
        {
            if (item.next == ITEM_TYPE.COOKIE_RAINBOW)
            {
                isSwap = true;
                break;
                
            }
            
        }

        if (!isSwap)
        {
            Item first = null;

            foreach (Item item in combine)
            {
                if (first == null)
                {
                    first = item;
                }
                else
                {
                    if (item.node.OrderOnBoard() < first.node.OrderOnBoard())
                    {
                        first = item;
                    }
                }
            }

            foreach (Item item in combine)
            {
                if (first.node.RightNeighbor())
                {
                    if (item.node.OrderOnBoard() == first.node.RightNeighbor().OrderOnBoard())
                    {
                        combine[2].next = ITEM_TYPE.COOKIE_RAINBOW;
                        break;
                    }
                }

                if (first.node.BottomNeighbor())
                {
                    if (item.node.OrderOnBoard() == first.node.BottomNeighbor().OrderOnBoard())
                    {
                        first.next = ITEM_TYPE.COOKIE_RAINBOW;
                        break;
                    }
                }
            }
        }
    }

    public void SetPlaneCombine(List<Item> combine)
    {

        bool isSwap = false;
        //todo : 优先级根据配置 改掉死代码
        // rainbow优先级最高
        foreach (Item item in combine)
        {
            if (item.next != ITEM_TYPE.NONE)
            {
                isSwap = true;
                break;

            }

        }

        if (!isSwap)
        {
            Item first = null;

            foreach (Item item in combine)
            {
                if (first == null)
                {
                    first = item;
                }
                else
                {
                    if (item.node.OrderOnBoard() < first.node.OrderOnBoard())
                    {
                        first = item;
                    }
                }
            }
            first.next = first.GetPlaneBreaker(first.type);
//            foreach (Item item in combine)
//            {
//                if (first.node.RightNeighbor())
//                {
//                    if (item.node.OrderOnBoard() == first.node.RightNeighbor().OrderOnBoard())
//                    {
//                        first.next = first.GetColumnBreaker(first.type);
//                        break;
//                    }
//                }
//
//                if (first.node.BottomNeighbor())
//                {
//                    if (item.node.OrderOnBoard() == first.node.BottomNeighbor().OrderOnBoard())
//                    {
//                        first.next = first.GetRowBreaker(first.type);
//                        break;
//                    }
//                }
//            }
        }
    }

    // return items around
    public List<Item> ItemAround(Node node,int range)
    {
        List<Item> items = new List<Item>();

        for (int i = node.i - range; i <= node.i + range; i++)
        {
            for (int j = node.j - range; j <= node.j + range; j++)
            {
                //跳过四个角点
                if (i == node.i - range && j == node.j - range
                    || i == node.i - range && j == node.j + range
                    || i == node.i + range && j == node.j - range
                    || i == node.i + range && j == node.j + range
                )
                {
                    continue;
                }
                if (GetNode(i, j) != null)
                {
                    items.Add(GetNode(i, j).item);
                }
            }
        }   

        return items;
    }

    public List<Item> XCrossItems(Node node)
    {
        var items = new List<Item>();

        var row = LevelLoader.instance.row;

        for (int i = 0; i < row; i++)
        {
            if (i < node.i)
            {
                var crossLeft = GetNode(i, node.j - (node.i - i));
                var crossRight = GetNode(i, node.j + (node.i - i));

                if (crossLeft != null)
                {
                    if (crossLeft.item != null)
                    {
                        items.Add(crossLeft.item);
                    }
                }

                if (crossRight != null)
                {
                    if (crossRight.item != null)
                    {
                        items.Add(crossRight.item);
                    }
                }
            }
            else if (i == node.i)
            {
                if (node.item != null)
                {
                    items.Add(node.item);
                }
            }
            else if (i > node.i)
            {
                var crossLeft = GetNode(i, node.j - (i - node.i));
                var crossRight = GetNode(i, node.j + (i - node.i));

                if (crossLeft != null)
                {
                    if (crossLeft.item != null)
                    {
                        items.Add(crossLeft.item);
                    }
                }

                if (crossRight != null)
                {
                    if (crossRight.item != null)
                    {
                        items.Add(crossRight.item);
                    }
                }
            }
        }

        return items;
    }

    // return list of items in a column
    public List<Item> ColumnItems(int column)
    {
        var items = new List<Item>();

        var row = LevelLoader.instance.row;

        for (int i = 0; i < row; i++)
        {
            if (GetNode(i, column) != null)
            {
                items.Add(GetNode(i, column).item);
            }
        }

        return items;
    }

    // return list of items in a row
    public List<Item> RowItems(int row)
    {
        var items = new List<Item>();

        var column = LevelLoader.instance.column;

        for (int j = 0; j < column; j++)
        {
            if (GetNode(row, j) != null)
            {
                items.Add(GetNode(row, j).item);
            }
        }

        return items;
    }

    // return list of items in a column
    public List<Node> ColumnNodes(int column)
    {
        var nodes = new List<Node>();

        var row = LevelLoader.instance.row;

        for (int i = 0; i < row; i++)
        {
            if (GetNode(i, column) != null)
            {
                nodes.Add(GetNode(i, column));
            }
        }

        return nodes;
    }

    // return list of items in a row
    public List<Node> RowNodes(int row)
    {
        var nodes = new List<Node>();

        var column = LevelLoader.instance.column;

        for (int j = 0; j < column; j++)
        {
            if (GetNode(row, j) != null)
            {
                nodes.Add(GetNode(row, j));
            }
        }

        return nodes;
    }

    #endregion

    #region Destroy

    // destroy the whole board when swap 2 rainbow
    public void DoubleRainbowDestroy(bool isgrass)
    {
        StartCoroutine(DestroyWholeBoard(isgrass));
    }

    IEnumerator DestroyWholeBoard(bool isgrass)
    {
        var column = LevelLoader.instance.column;
        playingAnimation++;
        for (int i = 0; i < column; i++)
        {
            List<Item> items = ColumnItems(i);

            foreach (var item in items)
            {
                if (item != null && item.Destroyable())
                {
                    //item.type = item.GetCookie(item.type);
                    if (isgrass)
                    {
                        item.node.ChangeToGrass();
                    }

                    GameObject explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RainbowExplosion()) as GameObject);

                    if (explosion != null) explosion.transform.position = item.transform.position;

                    item.Destroy();
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
        playingAnimation--;
        FindMatches();
    }

    public void DestroyPlaneTargetList(Item item, bool isgrass = false)
    {
        StartCoroutine(StartDestroyPlaneTargetList(item,isgrass));
    }

    IEnumerator StartDestroyPlaneTargetList(Item item, bool isgrass = false)
    {
        //        var originalState = state;
        if (state != GAME_STATE.PRE_WIN_AUTO_PLAYING)
        {
            if (specialDestroying == 0)
            {
                originalStateInSDestroying = state;
            }
            specialDestroying++;
            state = GAME_STATE.DESTROYING_ITEMS;
        }

        if (item != null)
        {
            if (isgrass)
            {
                item.node.ChangeToGrass();
            }
            item.beAbleToDestroy++;
            item.Destroy();
        }
        if (state != GAME_STATE.PRE_WIN_AUTO_PLAYING)
        {
            specialDestroying--;
            if (specialDestroying == 0)
            {
                state = originalStateInSDestroying;
            }
        }

        while (destroyingItems > 0 || playingAnimation > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForEndOfFrame();

        Drop();


        while (droppingItems > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForEndOfFrame();

//        state = originalState;

        //        if (state == GAME_STATE.WAITING_USER_SWAP)
        {
           // FindMatches();
        }

//        specialDestroying--;
//        if (specialDestroying == 0)
//        {
//            state = originalStateInSDestroying;
//        }

    }





    // destroy all items of changing list
    public void DestroyChangingList(bool isgrass = false)
    {
        StartCoroutine(StartDestroyChangingList(isgrass));
    }

    IEnumerator StartDestroyChangingList(bool isgrass = false)
    {
        //print("Start destroy items in the list");
        if (state != GAME_STATE.PRE_WIN_AUTO_PLAYING)
        {
            if (specialDestroying == 0)
            {
                originalStateInSDestroying = state;
            }
            specialDestroying++;
            state = GAME_STATE.DESTROYING_ITEMS;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < changingList.Count; i++)
        {
            var item = changingList[i];

            if (item != null)
            {
                if (isgrass)
                {
                    item.node.ChangeToGrass();
                }
                item.Destroy();
                //yield return new WaitForSeconds(0.1f);
            }

            while (destroyingItems > 0 || playingAnimation > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForEndOfFrame();

            Drop();

            while (droppingItems > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForEndOfFrame();
        }

        changingList.Clear();
        if (state != GAME_STATE.PRE_WIN_AUTO_PLAYING)
        {
            specialDestroying--;
            if (specialDestroying == 0)
            {
                state = originalStateInSDestroying;
            }
        }
        //        if (state == GAME_STATE.WAITING_USER_SWAP)
        {
            FindMatches();
        }

//        specialDestroying--;
//        if (specialDestroying == 0)
//        {
//            state = originalStateInSDestroying;
//        }
    }

    public void DestroySameColorList(bool isgrass = false)
    {
        StartCoroutine(StartDestroySameColorList(isgrass));
    }

    IEnumerator StartDestroySameColorList(bool isgrass = false)
    {
        //print("Start destroy items in the same color list");
        if (state != GAME_STATE.PRE_WIN_AUTO_PLAYING)
        {
            if (specialDestroying == 0)
            {
                originalStateInSDestroying = state;
            }
            specialDestroying++;
            state = GAME_STATE.DESTROYING_ITEMS;
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < sameColorList.Count; i++)
        {
            var item = sameColorList[i];

            if (item != null && item.destroying == false)
            {
                GameObject explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RainbowExplosion()) as GameObject);

                if (explosion != null) explosion.transform.position = item.transform.position;

                if (isgrass)
                {
                    item.node.ChangeToGrass();
                }

                item.Destroy();

                yield return new WaitForSeconds(0.1f);
            }
        }

        sameColorList.Clear();
        if (state != GAME_STATE.PRE_WIN_AUTO_PLAYING)
        {
            specialDestroying--;
            if (specialDestroying == 0)
            {
                state = originalStateInSDestroying;
            }
        }
        //        if (state == GAME_STATE.WAITING_USER_SWAP)
        {
            FindMatches();
        }
    }

    public void DestroyNeighborItems(Item item)
    {
        if (!CanDestroyNeighbor(item))
        {
            return;
        }

        if (state == GAME_STATE.PRE_WIN_AUTO_PLAYING)
        {
            return;
        }

        DestroyMarshmallow(item);

        DestroyAppleBox(item);

        DestroyChocolate(item);

        DestroyRockCandy(item);

        DestroyJelly(item);

        DestroyPackageBox(item);

    }

    private bool CanDestroyNeighbor(Item item)
    {
        if (item.IsMarshmallow() ||
            item.IsAppleBox() ||
            item.IsCollectible() ||
            item.IsGingerbread() ||
            item.IsChocolate() ||
            item.IsRockCandy() ||
            item.IsCherry() ||
            item.IsBlank() ||
            item.node.jelly != null ||
            item.node.packagebox != null
        )
        {
            return false;
        }
        return true;
    }

    public void DestroyMarshmallow(Item item)
    {
        var marshmallows = new List<Item>();

        if (item.node.TopNeighbor() != null && item.node.TopNeighbor().bafflebottom == null  && item.node.TopNeighbor().item != null && item.node.TopNeighbor().item.IsMarshmallow())
        {
            marshmallows.Add(item.node.TopNeighbor().item);
        }

        if (item.node.RightNeighbor() != null && item.node.baffleright == null && item.node.RightNeighbor().item != null && item.node.RightNeighbor().item.IsMarshmallow())
        {
            marshmallows.Add(item.node.RightNeighbor().item);
        }

        if (item.node.BottomNeighbor() != null && item.node.bafflebottom == null && item.node.BottomNeighbor().item != null && item.node.BottomNeighbor().item.IsMarshmallow())
        {
            marshmallows.Add(item.node.BottomNeighbor().item);
        }

        if (item.node.LeftNeighbor() != null && item.node.LeftNeighbor().baffleright == null && item.node.LeftNeighbor().item != null && item.node.LeftNeighbor().item.IsMarshmallow())
        {
            marshmallows.Add(item.node.LeftNeighbor().item);
        }

        foreach (var marshmallow in marshmallows)
        {
            marshmallow.Destroy();
        }
    }

    public void DestroyAppleBox(Item item)
    {

        var appleboxes = new List<Item>();

        if (item.node.TopNeighbor() != null && item.node.TopNeighbor().bafflebottom == null && item.node.TopNeighbor().item != null && item.node.TopNeighbor().item.IsAppleBox())
        {
            appleboxes.Add(item.node.TopNeighbor().item);
        }

        if (item.node.RightNeighbor() != null && item.node.baffleright == null && item.node.RightNeighbor().item != null && item.node.RightNeighbor().item.IsAppleBox())
        {
            appleboxes.Add(item.node.RightNeighbor().item);
        }

        if (item.node.BottomNeighbor() != null && item.node.bafflebottom == null && item.node.BottomNeighbor().item != null && item.node.BottomNeighbor().item.IsAppleBox())
        {
            appleboxes.Add(item.node.BottomNeighbor().item);
        }

        if (item.node.LeftNeighbor() != null && item.node.LeftNeighbor().baffleright == null && item.node.LeftNeighbor().item != null && item.node.LeftNeighbor().item.IsAppleBox())
        {
            appleboxes.Add(item.node.LeftNeighbor().item);
        }

        foreach (var applebox in appleboxes)
        {
            applebox.Destroy();
        }
    }


    public void DestroyChocolate(Item item)
    {
        
        var chocolates = new List<Item>();

        if (item.node.TopNeighbor() != null && item.node.TopNeighbor().bafflebottom == null && item.node.TopNeighbor().item != null && item.node.TopNeighbor().item.IsChocolate())
        {
            chocolates.Add(item.node.TopNeighbor().item);
        }

        if (item.node.RightNeighbor() != null && item.node.baffleright == null && item.node.RightNeighbor().item != null && item.node.RightNeighbor().item.IsChocolate())
        {
            chocolates.Add(item.node.RightNeighbor().item);
        }

        if (item.node.BottomNeighbor() != null && item.node.bafflebottom == null && item.node.BottomNeighbor().item != null && item.node.BottomNeighbor().item.IsChocolate())
        {
            chocolates.Add(item.node.BottomNeighbor().item);
        }

        if (item.node.LeftNeighbor() != null && item.node.LeftNeighbor().baffleright == null && item.node.LeftNeighbor().item != null && item.node.LeftNeighbor().item.IsChocolate())
        {
            chocolates.Add(item.node.LeftNeighbor().item);
        }

        foreach (var chocolate in chocolates)
        {
            chocolate.Destroy();
        }
    }

    public void DestroyJelly(Item item)
    {

        var items = new List<Item>();

        if (item.node.TopNeighbor() != null && item.node.TopNeighbor().bafflebottom == null && item.node.TopNeighbor().item != null && item.node.TopNeighbor().jelly != null)
        {
            items.Add(item.node.TopNeighbor().item);
        }

        if (item.node.RightNeighbor() != null && item.node.baffleright == null && item.node.RightNeighbor().item != null && item.node.RightNeighbor().jelly != null)
        {
            items.Add(item.node.RightNeighbor().item);
        }

        if (item.node.BottomNeighbor() != null && item.node.bafflebottom == null && item.node.BottomNeighbor().item != null && item.node.BottomNeighbor().jelly != null)
        {
            items.Add(item.node.BottomNeighbor().item);
        }

        if (item.node.LeftNeighbor() != null && item.node.LeftNeighbor().baffleright == null && item.node.LeftNeighbor().item != null && item.node.LeftNeighbor().jelly != null)
        {
            items.Add(item.node.LeftNeighbor().item);
        }

        foreach (var itemtmp in items)
        {
            itemtmp.Destroy();
        }
    }

    public void DestroyRockCandy(Item item)
    {

        var rocks = new List<Item>();

        if (item.node.TopNeighbor() != null && item.node.TopNeighbor().bafflebottom == null && item.node.TopNeighbor().item != null && item.node.TopNeighbor().item.IsRockCandy())
        {
            rocks.Add(item.node.TopNeighbor().item);
        }

        if (item.node.RightNeighbor() != null && item.node.baffleright == null && item.node.RightNeighbor().item != null && item.node.RightNeighbor().item.IsRockCandy())
        {
            rocks.Add(item.node.RightNeighbor().item);
        }

        if (item.node.BottomNeighbor() != null && item.node.bafflebottom == null && item.node.BottomNeighbor().item != null && item.node.BottomNeighbor().item.IsRockCandy())
        {
            rocks.Add(item.node.BottomNeighbor().item);
        }

        if (item.node.LeftNeighbor() != null && item.node.LeftNeighbor().baffleright == null && item.node.LeftNeighbor().item != null && item.node.LeftNeighbor().item.IsRockCandy())
        {
            rocks.Add(item.node.LeftNeighbor().item);
        }

        foreach (var rock in rocks)
        {
            needIncreaseBubble = false;
            rock.Destroy();
        }
    }


    public void DestroyPackageBox(Item item)
    {

        var packageboxes = new List<Item>();

        if (item.node.TopNeighbor() != null && item.node.TopNeighbor().bafflebottom == null && item.node.TopNeighbor().item != null && item.node.TopNeighbor().packagebox != null)
        {
            packageboxes.Add(item.node.TopNeighbor().item);
        }

        if (item.node.RightNeighbor() != null && item.node.baffleright == null && item.node.RightNeighbor().item != null && item.node.RightNeighbor().packagebox != null)
        {
            packageboxes.Add(item.node.RightNeighbor().item);
        }

        if (item.node.BottomNeighbor() != null && item.node.bafflebottom == null && item.node.BottomNeighbor().item != null && item.node.BottomNeighbor().packagebox != null)
        {
            packageboxes.Add(item.node.BottomNeighbor().item);
        }

        if (item.node.LeftNeighbor() != null && item.node.LeftNeighbor().baffleright == null && item.node.LeftNeighbor().item != null && item.node.LeftNeighbor().packagebox != null)
        {
            packageboxes.Add(item.node.LeftNeighbor().item);
        }

        foreach (var packagetmp in packageboxes)
        {
            packagetmp.Destroy();
        }
    }


    #endregion

    #region Collect

    // if item is the target to collect
    public void CollectItem(Item item)
    {
        GameObject flyingItem = null;
        var order = 0;

        // cookie
        if (item.IsCookie())
        {
            for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
            {
                if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.COOKIE 
                    && LevelLoader.instance.targetList[i].color == item.color 
                    && targetLeftList[i] > 0
                    )
                {
                    targetLeftList[i]--;
                    flyingItem = new GameObject();
                    order = i;
                    break;
                }
            }
            if (flyingItem != null)
            {
                flyingItem.transform.position = item.transform.position;
                flyingItem.name = "Flying Cookie";
                flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                GameObject prefab = null;

                switch (item.color)
                {
                    case 1:
                        prefab = Resources.Load(Configure.Cookie1()) as GameObject;
                        break;
                    case 2:
                        prefab = Resources.Load(Configure.Cookie2()) as GameObject;
                        break;
                    case 3:
                        prefab = Resources.Load(Configure.Cookie3()) as GameObject;
                        break;
                    case 4:
                        prefab = Resources.Load(Configure.Cookie4()) as GameObject;
                        break;
                    case 5:
                        prefab = Resources.Load(Configure.Cookie5()) as GameObject;
                        break;
                    case 6:
                        prefab = Resources.Load(Configure.Cookie6()) as GameObject;
                        break;
                }

                if (prefab != null)
                {
                    spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
        // gingerbread
        else if (item.IsGingerbread())
        {
            for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
            {
                if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.GINGERBREAD
                    && targetLeftList[i] > 0
                )
                {
                    targetLeftList[i]--;
                    flyingItem = new GameObject();
                    order = i;
                    break;
                }
            }

            if (flyingItem != null)
            {
                flyingItem.transform.position = item.transform.position;
                flyingItem.name = "Flying Gingerbread";
                flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                GameObject prefab = Resources.Load(Configure.GingerbreadGeneric()) as GameObject;

                if (prefab != null)
                {
                    spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }    
        // marshmallow
        else if (item.IsMarshmallow())
        {
            for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
            {
                if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.MARSHMALLOW
                    && targetLeftList[i] > 0
                )
                {
                    targetLeftList[i]--;
                    flyingItem = new GameObject();
                    order = i;
                    break;
                }
            }

            if (flyingItem != null)
            {
                flyingItem.transform.position = item.transform.position;
                flyingItem.name = "Flying Marshmallow";
                flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                GameObject prefab = Resources.Load(Configure.Marshmallow()) as GameObject;

                if (prefab != null)
                {
                    spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
        // chocolate
        else if (item.IsChocolate())
        {
            for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
            {
                if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.CHOCOLATE
                    && targetLeftList[i] > 0
                )
                {
                    targetLeftList[i]--;
                    flyingItem = new GameObject();
                    order = i;
                    break;
                }
            }

            if (flyingItem != null)
            {
                flyingItem.transform.position = item.transform.position;
                flyingItem.name = "Flying Chocolate";
                flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                GameObject prefab = Resources.Load(Configure.Chocolate1()) as GameObject;

                if (prefab != null)
                {
                    spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
        // column_row_breaker
        else if (item.IsColumnBreaker(item.type) || item.IsRowBreaker(item.type))
        {
            for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
            {
                if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.COLUMN_ROW_BREAKER
                    && targetLeftList[i] > 0
                )
                {
                    targetLeftList[i]--;
                    flyingItem = new GameObject();
                    order = i;
                    break;
                }
            }

            if (flyingItem != null)
            {
                flyingItem.transform.position = item.transform.position;
                flyingItem.name = "Flying Column Row Breaker";
                flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                GameObject prefab = Resources.Load(Configure.ColumnRowBreaker()) as GameObject;

                if (prefab != null)
                {
                    spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
		// generic bomb breaker
		else if (item.IsBombBreaker(item.type))
		{
		    for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
		    {
		        if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.BOMB_BREAKER
                    && targetLeftList[i] > 0
		        )
		        {
		            targetLeftList[i]--;
		            flyingItem = new GameObject();
		            order = i;
		            break;
		        }
		    }

			if (flyingItem != null)
			{
				flyingItem.transform.position = item.transform.position;
				flyingItem.name = "Flying Bomb Breaker";
				flyingItem.layer = LayerMask.NameToLayer("On Top UI");

				SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

				GameObject prefab = Resources.Load(Configure.GenericBombBreaker()) as GameObject;

				if (prefab != null)
				{
					spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
				}
			}
		}
        // rainbow
        else if (item.type == ITEM_TYPE.COOKIE_RAINBOW)
        {
            for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
            {
                if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.RAINBOW
                    && targetLeftList[i] > 0
                )
                {
                    targetLeftList[i]--;
                    flyingItem = new GameObject();
                    order = i;
                    break;
                }
            }

            if (flyingItem != null)
            {
                flyingItem.transform.position = item.transform.position;
                flyingItem.name = "Flying Rainbow";
                flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                GameObject prefab = Resources.Load(Configure.CookieRainbow()) as GameObject;

                if (prefab != null)
                {
                    spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
        // rock candy
        else if (item.IsRockCandy())
        {
            for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
            {
                if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.ROCK_CANDY
                    && targetLeftList[i] > 0
                )
                {
                    targetLeftList[i]--;
                    flyingItem = new GameObject();
                    order = i;
                    break;
                }
            }

            if (flyingItem != null)
            {
                flyingItem.transform.position = item.transform.position;
                flyingItem.name = "Flying Rock Candy";
                flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                GameObject prefab = Resources.Load(Configure.RockCandyGeneric()) as GameObject;

                if (prefab != null)
                {
                    spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
        else if (item.type == ITEM_TYPE.CHERRY)
        {
            for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
            {
                if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.CHERRY
                    && targetLeftList[i] > 0
                )
                {
                    targetLeftList[i]--;
                    flyingItem = new GameObject();
                    order = i;
                    break;
                }
            }

            if (flyingItem != null)
            {
                flyingItem.transform.position = item.transform.position;
                flyingItem.name = "Flying Cherry";
                flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                GameObject prefab = Resources.Load(Configure.Cherry()) as GameObject;

                if (prefab != null)
                {
                    spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
        else if (item.type == ITEM_TYPE.APPLEBOX)
        {
            if (item.applebox != null && item.applebox.appleNum > 0) 
            {
                for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
                {
                    if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.APPLEBOX
                        && targetLeftList[i] > 0
                    )
                    {
                        targetLeftList[i]--;
                        item.applebox.appleNum--;
                        flyingItem = new GameObject();
                        order = i;
                        break;
                    }
                }

                if (flyingItem != null)
                {
                    flyingItem.transform.position = item.transform.position;
                    flyingItem.name = "Flying Apple";
                    flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                    SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                    GameObject prefab = Resources.Load(Configure.Apple()) as GameObject;

                    if (prefab != null)
                    {
                        spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                    }
                }
            }
        }

        if (flyingItem != null)
        {
            StartCoroutine(CollectItemAnim(flyingItem, order));
        }
    }

    public void CollectWaffle(Waffle waffle)
    {
        GameObject flyingItem = null;
        var order = 0;

        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.WAFFLE
                && targetLeftList[i] > 0
            )
            {
                targetLeftList[i]--;
                flyingItem = new GameObject();
                order = i;
                break;
            }
        }

        if (flyingItem != null)
        {
            flyingItem.transform.position = waffle.transform.position;
            flyingItem.name = "Flying Waffle";
            flyingItem.layer = LayerMask.NameToLayer("On Top UI");
            flyingItem.transform.localScale = new Vector3(0.75f, 0.75f, 0);

            SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

            GameObject prefab = Resources.Load(Configure.Waffle1()) as GameObject; ;

            spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            StartCoroutine(CollectItemAnim(flyingItem, order));
        }
    }

    public void CollectCage(Cage cage)
    {
        GameObject flyingItem = null;

        var order = 0;

        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.CAGE
                && targetLeftList[i] > 0
            )
            {
                targetLeftList[i]--;
                flyingItem = new GameObject();
                order = i;
                break;
            }
        }

        if (flyingItem != null)
        {
            flyingItem.transform.position = cage.transform.position;
            flyingItem.name = "Flying Cage";
            flyingItem.layer = LayerMask.NameToLayer("On Top UI");
            flyingItem.transform.localScale = new Vector3(0.75f, 0.75f, 0);

            SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

            GameObject prefab = Resources.Load(Configure.Cage1()) as GameObject; ;

            spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            StartCoroutine(CollectItemAnim(flyingItem, order));
        }
    }

    // if the collectible
    bool CollectCollectible()
    {
        bool hasCollectible = false;
        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.COLLECTIBLE)
            {
                hasCollectible = true;
                break;
            }
        }
        if (!hasCollectible)
        {
            return false;
        }

        var items = GetListItems();

        foreach (var item in items)
        {
            bool collectable = false;

            // check each item in last row and in column which can collect and item is collectible
            if (item != null &&
                (item.node.i == LevelLoader.instance.row - 1) &&
                LevelLoader.instance.collectibleCollectColumnMarkers.Contains(item.node.j) &&
                item.IsCollectible())
            {
                collectable = true;
            }
            
            // collectible marker by node
            if (item != null && 
                LevelLoader.instance.collectibleCollectNodeMarkers.Contains(NodeOrder(item.node.i, item.node.j)) &&
                item.IsCollectible())
            {
                collectable = true;
            }
            
            if (collectable)
            {
                Debug.Log("收集到cake"+item.node.name+item.type.ToString());
                GameObject flyingItem = null;
                var order = 0;

                for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
                {
                    if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.COLLECTIBLE
                        && LevelLoader.instance.targetList[i].color == item.color
                        && targetLeftList[i] > 0
                    )
                    {
                        targetLeftList[i]--;
                        flyingItem = new GameObject();
                        order = i;
                        break;
                    }
                }

                if (flyingItem != null)
                {
                    flyingItem.transform.position = item.transform.position;
                    flyingItem.name = "Flying Collectible";
                    flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                    SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                    GameObject prefab = null;

                    switch (item.color)
                    {
                        case 1:
                            prefab = Resources.Load(Configure.Collectible1()) as GameObject;
                            break;
                        case 2:
                            prefab = Resources.Load(Configure.Collectible2()) as GameObject;
                            break;
                        case 3:
                            prefab = Resources.Load(Configure.Collectible3()) as GameObject;
                            break;
                        case 4:
                            prefab = Resources.Load(Configure.Collectible4()) as GameObject;
                            break;
                        case 5:
                            prefab = Resources.Load(Configure.Collectible5()) as GameObject;
                            break;
                        case 6:
                            prefab = Resources.Load(Configure.Collectible6()) as GameObject;
                            break;
                        case 7:
                            prefab = Resources.Load(Configure.Collectible7()) as GameObject;
                            break;
                        case 8:
                            prefab = Resources.Load(Configure.Collectible7()) as GameObject;
                            break;
                        case 9:
                            prefab = Resources.Load(Configure.Collectible9()) as GameObject;
                            break;
                        case 10:
                            prefab = Resources.Load(Configure.Collectible10()) as GameObject;
                            break;
                        case 11:
                            prefab = Resources.Load(Configure.Collectible11()) as GameObject;
                            break;
                        case 12:
                            prefab = Resources.Load(Configure.Collectible12()) as GameObject;
                            break;
                        case 13:
                            prefab = Resources.Load(Configure.Collectible13()) as GameObject;
                            break;
                        case 14:
                            prefab = Resources.Load(Configure.Collectible14()) as GameObject;
                            break;
                        case 15:
                            prefab = Resources.Load(Configure.Collectible15()) as GameObject;
                            break;
                        case 16:
                            prefab = Resources.Load(Configure.Collectible16()) as GameObject;
                            break;
                        case 17:
                            prefab = Resources.Load(Configure.Collectible17()) as GameObject;
                            break;
                        case 18:
                            prefab = Resources.Load(Configure.Collectible18()) as GameObject;
                            break;
                        case 19:
                            prefab = Resources.Load(Configure.Collectible19()) as GameObject;
                            break;
                        case 20:
                            prefab = Resources.Load(Configure.Collectible20()) as GameObject;
                            break;
                    }

                    if (prefab != null)
                    {
                        spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                    }

                    StartCoroutine(CollectItemAnim(flyingItem, order));

                    item.Destroy(true);

                    return true;
                }
            }
        }

        return false;
    }

    // item fly to target
    public IEnumerator CollectItemAnim(GameObject item, int order)
    {
        yield return new WaitForFixedUpdate();

        GameObject target = null;

        target = UITarget.TargetCellList[order].gameObject;

        flyingItems++;

        AnimationCurve curveX = new AnimationCurve(new Keyframe(0, item.transform.localPosition.x), new Keyframe(0.4f, target.transform.position.x));
        AnimationCurve curveY = new AnimationCurve(new Keyframe(0, item.transform.localPosition.y), new Keyframe(0.4f, target.transform.position.y));
        curveX.AddKey(0.2f, item.transform.localPosition.x + UnityEngine.Random.Range(-2f, 2f));
        curveY.AddKey(0.2f, item.transform.localPosition.y + UnityEngine.Random.Range(-1f, 0f));

        float startTime = Time.time;
        float speed = 2.0f + flyingItems * 0.25f;
        float distCovered = 0;
        while (distCovered < 0.4f)
        {
            distCovered = (Time.time - startTime) / speed;
            item.transform.localPosition = new Vector3(curveX.Evaluate(distCovered), curveY.Evaluate(distCovered), 0);

            yield return new WaitForFixedUpdate();
        }

        AudioManager.instance.CollectTargetAudio();

        UITarget.UpdateTargetAmount(order);

        Destroy(item);
        
        flyingItems--;
    }

    #endregion

    #region Popup

    void TargetPopup()
    {
        StartCoroutine(StartTargetPopup());
    }

    IEnumerator StartTargetPopup()
    {
        state = GAME_STATE.OPENING_POPUP;

        yield return new WaitForSeconds(0.5f);

        AudioManager.instance.PopupTargetAudio();

        WindowManager.instance.Show<TargetPopupWindow>();

        yield return new WaitForSeconds(1.0f);

        var popup = GameObject.Find("TargetPopup(Clone)");

        if (popup)
        {
            popup.GetComponent<Popup>().Close();
        }

        yield return new WaitForSeconds(0.5f);

        state = GAME_STATE.WAITING_USER_SWAP;

        if (Help.instance.help == false)
        {
            StartCoroutine(CheckHint());
        }
        else
        {
            Help.instance.Show();
        }

        // Plus 5 moves popup
//        if (Configure.instance.beginFiveMoves == true)
//        {
//            StartCoroutine(Plus5MovesPopup());
//        }
    }

    IEnumerator Plus5MovesPopup()
    {
        Configure.instance.beginFiveMoves = false;

        WindowManager.instance.Show<Plus5MovesPopupWindow>();

        yield return new WaitForSeconds(1.0f);

        var popup = GameObject.Find("Plus5MovesPopup(Clone)");

        if (popup)
        {
            popup.GetComponent<Popup>().Close();
        }
    }

    void ShowInspiringPopup()
    {
        //print("Excellent!, Amazing!, Great!, Nice!");

        var encouraging = Random.Range(0, 3);

        switch (encouraging)
        {
            case 0:
                WindowManager.instance.Show<AmazingPopupWindow>();
                AudioManager.instance.amazingAudio();
                break;
            case 1:
                WindowManager.instance.Show<ExcellentPopupWindow>();
                AudioManager.instance.exellentAudio();
                break;
            case 2:
                WindowManager.instance.Show<GreatPopupWindow>();
                AudioManager.instance.greatAudio();
                break;            
        }
    }

    #endregion

    #region Complete

    bool IsLevelCompleted()
    {
        for (int i = 0; i < targetLeftList.Count; i++)
        {
            if (targetLeftList[i] != 0)
            {
                return false;
            }
        }
        return true;
    }

    // auto play the left moves when target is reached
    IEnumerator PreWinAutoPlay()
    {
        HideHint();

        // reset drop time
        dropTime = 1;

        state = GAME_STATE.OPENING_POPUP;

        yield return new WaitForSeconds(0.5f);

        WindowManager.instance.Show<CompletedPopupWindow>();

        AudioManager.instance.PopupCompletedAudio();

        yield return new WaitForSeconds(1.0f);

        if (GameObject.Find("CompletedPopup(Clone)"))
        {
            GameObject.Find("CompletedPopup(Clone)").GetComponent<Popup>().Close();
        }

        yield return new WaitForSeconds(0.5f);

        state = GAME_STATE.PRE_WIN_AUTO_PLAYING;


        DestroyAllObstacles();

        var items = GetRandomItems(moveLeft);

        foreach (var item in items)
        {
            item.SetRandomNextType();
            item.nextSound = false;

            DecreaseMoveLeft(true);

            var prefab = Instantiate(Resources.Load(Configure.StarGold())) as GameObject;
            prefab.transform.position = UITop.GetComponent<UITop>().movesText.gameObject.transform.position;

            var startPosition = prefab.transform.position;
            var endPosition = item.gameObject.transform.position;
            var bending = new Vector3(1, 1, 0);
            var timeToTravel = 0.2f;
            var timeStamp = Time.time;

            while (Time.time < timeStamp + timeToTravel) {
                var currentPos = Vector3.Lerp(startPosition, endPosition, (Time.time - timeStamp)/timeToTravel);
         
                currentPos.x += bending.x * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/timeToTravel) * Mathf.PI);
                currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/timeToTravel) * Mathf.PI);
                currentPos.z += bending.z * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/timeToTravel) * Mathf.PI);

                prefab.transform.position = currentPos;
         
                yield return null;
            }

            Destroy(prefab);

            item.Destroy();

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        //GameObject.Find("Canvas").transform.Find("PiggyBank").gameObject.SetActive(true);

        while (GetAllSpecialItems().Count > 0)
        {
            while (GetAllSpecialItems().Count > 0)
            {
                var specials = GetAllSpecialItems();

                var item = specials[UnityEngine.Random.Range(0, specials.Count)];

                //WinGoldReward(item);

                item.Destroy();

                while (destroyingItems > 0 || playingAnimation > 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForEndOfFrame();

                Drop();

                while (droppingItems > 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForEndOfFrame();
            }
            state = GAME_STATE.PRE_WIN_AUTO_PLAYING;

            yield return StartCoroutine(DestroyMatches());
        }

        while (destroyingItems > 0 || playingAnimation > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForEndOfFrame();

        while (droppingItems > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(0.5f);

        state = GAME_STATE.OPENING_POPUP;

        // SaveLevelInfo();

        //GameObject.Find("Canvas").transform.Find("PiggyBank").gameObject.SetActive(false);

        AudioManager.instance.PopupWinAudio();

        WindowManager.instance.Show<WinPopupWindow>();
    }

    public void WinGoldReward(Item item)
    {
        //var config = DefaultConfig.getInstance().GetConfigByType<setting>().GetDictionaryByID("moviesgold");
        SettingConfig config = GameMainManager.Instance.configManager.settingConfig;
        //int maxgold = Int32.Parse(config["maxgold"]);
        int maxgold = config.maxgold;
        if (winGold > maxgold)
        {
            winGold = maxgold;
            return;
        }

        if (item.IsPlaneBreaker(item.type))
        {
            //winGold += Int32.Parse(config["planebreaker"]);
            winGold += config.planebreaker;
            //Debug.Log("xxxxxxxxxxxxx: " + winGold.ToString());
            getWinGold(item, winGold);
        }
        else if (item.IsColumnBreaker(item.type))
        {
            //winGold += Int32.Parse(config["columnbreaker"]);
            winGold += config.columnbreaker;
            getWinGold(item, winGold);
        }
        else if (item.IsRowBreaker(item.type))
        {
            //winGold += Int32.Parse(config["rowbreaker"]);
            winGold += config.rowbreaker;
            getWinGold(item, winGold);
        }
        else if (item.IsBombBreaker(item.type))
        {
           // winGold += Int32.Parse(config["bombbreaker"]);
            winGold += config.bombbreaker;
            getWinGold(item, winGold);
        }
        else if (item.type == ITEM_TYPE.COOKIE_RAINBOW)
        {
            //winGold += Int32.Parse(config["rainbow"]);
            winGold += config.rainbow;
            getWinGold(item, winGold);
        }
        if (winGold > maxgold)
        {
            winGold = maxgold;
        }

        //todo:金币弹出动画
    }

    private void getWinGold(Item item, int gold) {
		return;
        var cloneGold = Instantiate(item.gameObject, GameObject.Find("Board").transform);
        cloneGold.transform.position = item.transform.position;
        cloneGold.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Cookie/UI/Map/goldsp", typeof(Sprite)) as Sprite;
        cloneGold.GetComponent<SpriteRenderer>().sortingOrder = 90;
        cloneGold.transform.localScale = new Vector3(1.0f, 1.0f, 1);
        cloneGold.gameObject.SetActive(true);

		Vector3 v1 = new Vector3 (item.transform.position.x, item.transform.position.y, 0);
		Vector3 v2 = GameObject.Find ("Canvas").transform.Find ("PiggyBank").transform.position;
		Vector3[] path = {v1, new Vector3(v1.x + 1, v1.y + 1, 0), new Vector3(v1.x + 2, v1.y + 2, 0), new Vector3(v1.x + 3, v1.y + 3, 0), v2};
		cloneGold.transform.DOPath(path, 1.0f).SetEase(Ease.InQuad).OnComplete(() => 
			{
				Destroy(cloneGold);
				GameObject.FindObjectOfType<Canvas>().transform.Find("PiggyBank").transform.Find("Text").GetComponent<Text>().text = gold.ToString();
			}
		);
    }

    private void DestroyAllObstacles()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var node = GetNode(i, j);
                if (node != null)
                {
                    if (node.ice != null)
                    {
                        Destroy(node.ice.gameObject);
                        node.ice = null;
                    }
                    if (node.jelly != null)
                    {
                        Destroy(node.jelly.gameObject);
                        node.jelly = null;
                    }
                    if (node.cage != null)
                    {
                        Destroy(node.cage.gameObject);
                        node.cage = null;
                    }
                    if (node.packagebox != null)
                    {
                        Destroy(node.packagebox.gameObject);
                        node.packagebox = null;
                    }
                }
            }
        }
    }



    List<Item> GetRandomItems(int number)
    {
        var avaiableItems = new List<Item>();
        var returnItems = new List<Item>();

        foreach (var item in GetListItems())
        {
            if (item != null)
            {
                if (item.node != null)
                {
                    if (item.IsCookie())
                    {
                        avaiableItems.Add(item);
                    }
                }
            }
        }

        while (returnItems.Count < number && avaiableItems.Count > 0)
        {
            var item = avaiableItems[Random.Range(0, avaiableItems.Count)];

            returnItems.Add(item);

            avaiableItems.Remove(item);
        }

        return returnItems;
    }

    List<Item> GetAllSpecialItems()
    {
        var specials = new List<Item>();

        foreach (var item in GetListItems())
        {
            if (item != null)
            {
                if (item.type == ITEM_TYPE.COOKIE_RAINBOW || item.IsColumnBreaker(item.type) || item.IsRowBreaker(item.type) || item.IsBombBreaker(item.type) || item.IsPlaneBreaker(item.type))
                {
                    specials.Add(item);
                }
            }
        }

        return specials;
    }

    public void SaveLevelInfo()
    {
        // level star
        if (score < LevelLoader.instance.score1Star)
        {
            star = 0;
        }
        else if (LevelLoader.instance.score1Star <= score && score < LevelLoader.instance.score2Star)
        {
            star = 1;
        }
        else if (LevelLoader.instance.score2Star <= score && score < LevelLoader.instance.score3Star)
        {
            star = 2;
        }
        else if (score >= LevelLoader.instance.score2Star)
        {
            star = 3;
        }

        // score and star
        GameData.instance.SaveLevelStatistics(LevelLoader.instance.level, score, star);

        // open next level
        int openedLevel = GameData.instance.GetOpendedLevel();

        if (LevelLoader.instance.level == openedLevel)
        {
            if (openedLevel < Configure.instance.maxLevel)
            {
                GameData.instance.SaveOpendedLevel(openedLevel + 1);
            }            
        }

        // add bonus coin
        int coin = GameData.instance.GetPlayerCoin();

        if (star == 1)
        {
            GameData.instance.SavePlayerCoin(coin + Configure.instance.bonus1Star);
        }
        else if (star == 2)
        {
            GameData.instance.SavePlayerCoin(coin + Configure.instance.bonus2Star);
        }
        else if (star == 3)
        {
            GameData.instance.SavePlayerCoin(coin + Configure.instance.bonus3Star);
        }
    }

    #endregion

    #region Ads

    public void ShowAds()
    {
        StartCoroutine(ShowPopupAds());
    }

    IEnumerator ShowPopupAds()
    {
        yield return new WaitForSeconds(0.1f);

        // TODO: check if allow to show ads
        var allowShowAds = true;

        if (allowShowAds)
        {
            // TODO
        }
    }

    #endregion

    #region Hint

    public void Hint()
    {
        StartCoroutine(CheckHint());
    }

    public IEnumerator CheckHint()
    {
        //Debug.Log("CheckHint()");

        // prevent multiple call this function only last call is triggered
        checkHintCall++;

        if (checkHintCall > 1)
        {
            checkHintCall--;

            //Debug.Log("Cancel checking hint because of multiple hint call");

            yield break;
        }

        if (Configure.instance.showHint == false)
        {
            yield break;
        }

        if (moveLeft <= 0)
        {
            yield break;
        }

        // put delay here user also need to wait when the is no matches
        //yield return new WaitForSeconds(Configure.instance.hintDelay);

        // need to call hide hint here in case items destroy after no matches generate
        HideHint();

        while (state != GAME_STATE.WAITING_USER_SWAP)
        {
            //Debug.Log("Wait for checking hint because of game state");

            yield return new WaitForSeconds(0.1f);
        }

        while (lockSwap)
        {
            //Debug.Log("Wait for checking hint because of lock swap");

            yield return new WaitForSeconds(0.1f);
        }

        //Debug.Log("Start checking hint");

        // check for rainbow item / breaker / color
        if (GetHintByRainbowItem() || GetHintByBreaker() || GetHintByColor())
        {
            StartCoroutine(ShowHint());

            checkHintCall--;

            yield break;
        }
        // if reach this code that mean there is no matches
        else
        {
            // prevent multiple call
            if (!GameObject.Find("NoMatchesdPopup(Clone)"))
            {
                state = GAME_STATE.NO_MATCHES_REGENERATING;

                lockSwap = true;

                AudioManager.instance.PopupNoMatchesAudio();

                WindowManager.instance.Show<NoMatchesdPopupWindow>();

                yield return new WaitForSeconds(1.0f);

                if (GameObject.Find("NoMatchesdPopup(Clone)"))
                {
                    GameObject.Find("NoMatchesdPopup(Clone)").GetComponent<Popup>().Close();
                }

                yield return new WaitForSeconds(0.5f);

                var position = Camera.main.aspect * Camera.main.orthographicSize * 2;

                // hide board
                iTween.MoveTo(gameObject, iTween.Hash(
                    "x", -position,
                    "easeType", iTween.EaseType.easeOutBack,
                    "time", 0.5
                ));

                yield return new WaitForSeconds(0.5f);

                NoMoveRegenerate();

                while (GetHintByColor() == false)
                {
                    NoMoveRegenerate();

                    yield return new WaitForEndOfFrame();
                }

                // show board
                iTween.MoveTo(gameObject, iTween.Hash(
                    "x", 0,
                    "easeType", iTween.EaseType.easeOutBack,
                    "time", 0.5
                ));

                yield return new WaitForSeconds(0.5f);

                state = GAME_STATE.WAITING_USER_SWAP;

                FindMatches();
            }

            checkHintCall--;
        }
    }

    public IEnumerator ShowHint()
    {
        //Debug.Log("ShowHint()");

        showHintCall++;

        // prevent multiple call
        if (showHintCall > 1)
        {
            //Debug.Log("Cancel showing hint because of multiple call");

            showHintCall--;

            yield break;
        }

        if (Configure.instance.showHint == false)
        {
            yield break;
        }
        
        yield return new WaitForSeconds(Configure.instance.hintDelay);

        while (state != GAME_STATE.WAITING_USER_SWAP)
        {
            yield return new WaitForSeconds(0.1f);
        }

        while (lockSwap)
        {
            yield return new WaitForSeconds(0.1f);
        }

        //Debug.Log("Start showing hint");

        // make sure only items in hint list run animation
        foreach (var item in GetListItems())
        {
            if (item != null)
            {
                if (!hintItems.Contains(item))
                {
                    iTween.StopByName(item.gameObject, "HintAnimation");
                    item.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
            }
        }

        foreach (var item in hintItems)
        {
            if (item != null)
            {
                //Debug.Log("Show hint: " + item.node.name);

                iTween.ShakeRotation(item.gameObject, iTween.Hash(
                    "name", "HintAnimation",
                    "amount", new Vector3(0f, 0f, 50f),
                    "easetype", iTween.EaseType.easeOutBack,
                    //"looptype", iTween.LoopType.pingPong,
                    "oncomplete", "OnCompleteShowHint",
                    "oncompletetarget", gameObject,
                    "oncompleteparams", new Hashtable() { { "item", item } },
                    //"delay", 2f,
                    "time", 1f
                ));
            }
        }

        // only wait if hint items run animation
        // if there is no item that mean the hint list is clean in clear hint function
        if (hintItems.Count > 0)
        {
            yield return new WaitForSeconds(1.5f);
        }
        
        showHintCall--;

        StartCoroutine(CheckHint());
    }

    // when shake a item it is not in good place we need to reset it
    public void OnCompleteShowHint(Hashtable args)
    {
        var item = (Item)args["item"];

        //Debug.Log("On Complete Shake Item: " + item.node.name);

        iTween.RotateTo(item.gameObject, iTween.Hash(
            "rotation", Vector3.zero,
            "time", 0.2f
        ));
    }

    public void HideHint()
    {
        //Debug.Log("Hide Hint");

        foreach (var item in hintItems)
        {
            if (item != null)
            {
                //print("Hide item in node: " + item.node.name);

                iTween.StopByName(item.gameObject, "HintAnimation");

                iTween.RotateTo(item.gameObject, iTween.Hash(
                    "rotation", Vector3.zero,
                    "time", 0.2f
                ));
            }
        }

        // clear hint list to make sure show hint function do not runs animation
        hintItems.Clear();
    }

    List<int> Shuffle(List<int> list)
    {
        System.Random rng = new System.Random();

        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    void CheckHintNode(Node node, int color, SWAP_DIRECTION direction)
    {
        if (node != null)
        {
            if (node.item != null && node.item.color == color)
            {
                if (direction == SWAP_DIRECTION.TOP 
                    ||direction == SWAP_DIRECTION.RIGHT
                    || direction == SWAP_DIRECTION.BOTTOM
                    || direction == SWAP_DIRECTION.LEFT
                    )
                {
                    if (node.item.Exchangeable(direction) && node.item.Matchable())
                    {
                        hintItems.Add(node.item);
                    }
                }
                else
                {
                    if (node.item.Matchable())
                    {
                        hintItems.Add(node.item);
                    }
                }
            }
        }
    }

    void NoMoveRegenerate()
    {
        //print("No moves generate");

        foreach (var item in GetListItems())
        {
            if (item != null)
            {
                if (item.Exchangeable(SWAP_DIRECTION.NONE) && item.IsCookie())
                {
                    item.color = LevelLoader.instance.RandomColor();

                    item.ChangeSpriteAndType(item.color);
                }
            }
        }
    }

    bool GetHintByColor()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        foreach (int color in Shuffle(LevelLoader.instance.usingColors))
        {
            for (int j = 0; j < column; j++)
            {
                for (int i = 0; i < row; i++)
                {
                    Node node = GetNode(i, j);

                    if (node != null)
                    {
                        if (node.item == null  || !(node.item.Exchangeable(SWAP_DIRECTION.NONE)))
                        {
                            continue;
                        }

                        // current node is x

                        //   o
                        // o x o 
                        //   o o
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.BOTTOM);
                        CheckHintNode(GetNode(i , j - 1), color, SWAP_DIRECTION.RIGHT);
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i + 1, j + 1), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count >= 4)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        //   o
                        // o x o 
                        // o o
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.BOTTOM);
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.LEFT);
                        CheckHintNode(GetNode(i, j - 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i + 1, j - 1), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count >= 4)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        //   o o
                        // o x o 
                        //   o  
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.TOP);
                        CheckHintNode(GetNode(i, j - 1), color, SWAP_DIRECTION.RIGHT);
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i - 1, j + 1), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count >= 4)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }


                        //  o o
                        //  o x o 
                        //    o  
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.TOP);
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.LEFT);
                        CheckHintNode(GetNode(i, j - 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i - 1, j - 1), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count >= 4)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }



                        // o-o-x
                        //	   o
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.TOP);
                        CheckHintNode(GetNode(i, j - 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i, j - 2), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        //     o
                        // o-o x
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.BOTTOM);
                        CheckHintNode(GetNode(i, j - 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i, j - 2), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        // x o o
                        // o
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.TOP);
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i, j + 2), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        // o
                        // x o o
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.BOTTOM);
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i, j + 2), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        // o
                        // o
                        // x o
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.LEFT);
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i - 2, j), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        // x o
                        // o
                        // o
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.LEFT);
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i + 2, j), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        //	 o
                        //   o
                        // o x
                        CheckHintNode(GetNode(i, j - 1), color, SWAP_DIRECTION.RIGHT);
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i - 2, j), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        // o x
                        //   o
                        //   o
                        CheckHintNode(GetNode(i, j - 1), color, SWAP_DIRECTION.RIGHT);
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i + 2, j), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        // o-x-o-o
                        CheckHintNode(GetNode(i, j - 1), color, SWAP_DIRECTION.RIGHT);
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i, j + 2), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        // o-o-x-o
                        CheckHintNode(GetNode(i, j + 1), color, SWAP_DIRECTION.LEFT);
                        CheckHintNode(GetNode(i, j - 1), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i, j - 2), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        // o
                        // x
                        // o
                        // o
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.BOTTOM);
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i + 2, j), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        // o
                        // o
                        // x
                        // o
                        CheckHintNode(GetNode(i + 1, j), color, SWAP_DIRECTION.TOP);
                        CheckHintNode(GetNode(i - 1, j), color, SWAP_DIRECTION.NONE);
                        CheckHintNode(GetNode(i - 2, j), color, SWAP_DIRECTION.NONE);
                        if (hintItems.Count == 3)
                        {
                            return true;
                        }
                        else
                        {
                            hintItems.Clear();
                        }

                        //   o
                        // o x o
                        //   o
                        int h = 0;
                        int v = 0;
                        Node neighbor = null;

                        neighbor = node.LeftNeighbor();
                        if (neighbor != null)
                        {
                            if (neighbor.item != null && neighbor.item.Matchable() && neighbor.item.color == color)
                            {
                                hintItems.Add(neighbor.item);

                                h++;
                            }
                        }

                        neighbor = node.RightNeighbor();
                        if (neighbor != null)
                        {
                            if (neighbor.item != null && neighbor.item.Matchable() && neighbor.item.color == color)
                            {
                                hintItems.Add(neighbor.item);

                                h++;
                            }
                        }

                        neighbor = node.TopNeighbor();
                        if (neighbor != null)
                        {
                            if (neighbor.item != null && neighbor.item.Matchable() && neighbor.item.color == color)
                            {
                                hintItems.Add(neighbor.item);

                                v++;
                            }
                        }

                        neighbor = node.BottomNeighbor();
                        if (neighbor != null)
                        {
                            if (neighbor.item != null && neighbor.item.Matchable() && neighbor.item.color == color)
                            {
                                hintItems.Add(neighbor.item);

                                v++;
                            }
                        }

                        if (hintItems.Count == 3)
                        {
                            if (v > h && hintItems[0].node.item != null)
                            {
                                if (hintItems[0].node == node.LeftNeighbor() && hintItems[0].node.item.Exchangeable(SWAP_DIRECTION.RIGHT))
                                {
                                    return true;
                                }
                                else if (hintItems[0].node == node.RightNeighbor() &&
                                         hintItems[0].node.item.Exchangeable(SWAP_DIRECTION.LEFT))
                                {
                                    return true;
                                }
                                else
                                {
                                    hintItems.Clear();
                                }
                            }
                            else if (v < h && hintItems[2].node.item != null)
                            {
                                if (hintItems[2].node == node.TopNeighbor() && hintItems[2].node.item.Exchangeable(SWAP_DIRECTION.BOTTOM))
                                {
                                    return true;
                                }
                                else if (hintItems[2].node == node.BottomNeighbor() && hintItems[2].node.item.Exchangeable(SWAP_DIRECTION.TOP))
                                {
                                    return true;
                                }
                                else
                                {
                                    hintItems.Clear();
                                }
                            }
                            else
                            {
                                hintItems.Clear();
                            }
                        }
                        else if (hintItems.Count == 4)
                        {
                            if (hintItems[0].node.item.Exchangeable(SWAP_DIRECTION.RIGHT))
                            {
                                hintItems.RemoveAt(1);

                                return true;
                            }
                            else if (hintItems[1].node.item.Exchangeable(SWAP_DIRECTION.LEFT))
                            {
                                hintItems.RemoveAt(0);

                                return true;
                            }
                            else if (hintItems[2].node.item.Exchangeable(SWAP_DIRECTION.BOTTOM))
                            {
                                hintItems.RemoveAt(3);

                                return true;
                            }
                            else if (hintItems[3].node.item.Exchangeable(SWAP_DIRECTION.TOP))
                            {
                                hintItems.RemoveAt(2);

                                return true;
                            }
                            else
                            {
                                hintItems.Clear();
                            }
                        }
                        else
                        {
                            hintItems.Clear();
                        }
                    }
                } // end for row
            }
        } // end foreach color

        return false;
    }

    bool GetHintByRainbowItem()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Node node = GetNode(i, j);
                if (node != null)
                {
                    if (node.item == null)
                    {
                        continue;
                    }

                    if (node.item.type == ITEM_TYPE.COOKIE_RAINBOW)
                    {
                        Node neighbor = null;

                        neighbor = node.LeftNeighbor();
                        if (neighbor != null)
                        {
                            if (neighbor.item != null && neighbor.item.Exchangeable(SWAP_DIRECTION.RIGHT))
                            {
                                hintItems.Add(node.item);

                                return true;
                            }
                        }

                        neighbor = node.RightNeighbor();
                        if (neighbor != null)
                        {
                            if (neighbor.item != null && neighbor.item.Exchangeable(SWAP_DIRECTION.LEFT))
                            {
                                hintItems.Add(node.item);

                                return true;
                            }
                        }

                        neighbor = node.TopNeighbor();
                        if (neighbor != null)
                        {
                            if (neighbor.item != null && neighbor.item.Exchangeable(SWAP_DIRECTION.BOTTOM))
                            {
                                hintItems.Add(node.item);

                                return true;
                            }
                        }

                        neighbor = node.BottomNeighbor();
                        if (neighbor != null)
                        {
                            if (neighbor.item != null && neighbor.item.Exchangeable(SWAP_DIRECTION.TOP))
                            {
                                hintItems.Add(node.item);

                                return true;
                            }
                        }
                    } // end if item is rainbow
                }
            }
        }

        return false;
    }

    bool GetHintByBreaker()
    {
        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Node node = GetNode(i, j);
                if (node != null)
                {
                    if (node.item == null || !(node.item.Exchangeable(SWAP_DIRECTION.NONE)))
                    {
                        continue;
                    }

                    if (node.item.IsBreaker(node.item.type))
                    {
                        hintItems.Add(node.item);

                        return true;
                    } // end if
                }
            }
        }

        return false;
    }

    #endregion

    #region Gingerbread

    bool GenerateGingerbread()
    {
        if (IsGingerbreadTarget() == false)
        {
            return false;
        }

        if (skipGenerateGingerbread)
        {
            return false;
        }

        // calculate the total gingerbread need to generate
        var needGenerate = 0;

        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.GINGERBREAD)
            {
                needGenerate += targetLeftList[i];
            }
        }

        if (needGenerate <= 0)
        {
            return false;
        }

        // check gingerbread on board
        var amount = GingerbreadOnBoard().Count;

        if (amount >= LevelLoader.instance.maxGingerbread)
        {
            return false;
        }

        // prevent multiple call
        if (generatingGingerbread)
        {
            return false;
        }

        // skip generate randomly
        if (Random.Range(0, 2) == 0 && skipGingerbreadCount < 2)
        {
            skipGingerbreadCount++;
            return false;
        }
        skipGingerbreadCount = 0;

        generatingGingerbread = true;

        // get node to generate gingerbread
        var row = LevelLoader.instance.row - 1;
        var column = LevelLoader.instance.gingerbreadMarkers[Random.Range(0, LevelLoader.instance.gingerbreadMarkers.Count)];

        var node = GetNode(row, column);

        //print(node.name);

        if (node != null && node.item != null)
        {
            node.item.ChangeToGingerbread(LevelLoader.instance.RandomGingerbread());
            return true;
        }

        return false;
    }

    bool IsGingerbreadTarget()
    {

        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.GINGERBREAD)
            {
                return true;
            }
        }
        return false;
    }

    List<Item> GingerbreadOnBoard()
    {
        var list = new List<Item>();

        var items = GetListItems();

        foreach (var item in items)
        {
            if (item != null && item.IsGingerbread())
            {
                list.Add(item);
            }
        }

        return list;
    }

    bool MoveGingerbread()
    {
        if (IsGingerbreadTarget() == false)
        {
            return false;
        }

        // prevent multiple call
        if (movingGingerbread)
        {
            return false;
        }
        movingGingerbread = true;

        var isMoved = false;

        //print("Move gingerbread");

        foreach (var gingerbread in GingerbreadOnBoard())
        {
            if (gingerbread != null)
            {
                var upper = GetUpperItem(gingerbread.node);

                if (upper != null && upper.node != null && upper.IsGingerbread() == false && gingerbread.node.cage == null && gingerbread.node.ice == null)
                {
                    var gingerbreadPosition = NodeLocalPosition(upper.node.i, upper.node.j);
                    var upperItemPosition = NodeLocalPosition(gingerbread.node.i, gingerbread.node.j);

                    gingerbread.neighborNode = upper.node;
                    gingerbread.swapItem = upper;

                    touchedItem = gingerbread;
                    swappedItem = upper;

                    gingerbread.SwapItem();

                    gingerbread.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

                    // animation
                    iTween.MoveTo(gingerbread.gameObject, iTween.Hash(
                        "position", gingerbreadPosition,
                        "easetype", iTween.EaseType.linear,
                        "time", Configure.instance.swapTime
                    ));

                    iTween.MoveTo(upper.gameObject, iTween.Hash(
                        "position", upperItemPosition,
                        "easetype", iTween.EaseType.linear,
                        "time", Configure.instance.swapTime
                    ));
                }
                else if (upper == null || upper.node == null)
                {
                    AudioManager.instance.GingerbreadExplodeAudio();

                    gingerbread.color = LevelLoader.instance.RandomColor();

                    gingerbread.ChangeSpriteAndType(gingerbread.color);

                    // after changing a gingerbread to a cookie. skip generate one turn on generate call right after this function
                    skipGenerateGingerbread = true;
                }

                isMoved = true;
            }
        }

        return isMoved;
    }

    public Item GetUpperItem(Node node)
    {
        var top = node.TopNeighbor();

        if (top == null)
        {
            return null;
        }
        else 
        {
            if (top.tile.type == TILE_TYPE.NONE || top.tile.type == TILE_TYPE.PASS_THROUGH)
            {
                return GetUpperItem(top);
            }
            else if (top.item != null && top.item.Movable())
            {
                return top.item;
            }
            else
            {
                return node.item;
            }
        }
    }

    #endregion

    #region Booster

    void DestroyBoosterItems(Item boosterItem)
    {
        if (boosterItem == null)
        {
            return;
        }

        if (boosterItem.Destroyable() && booster != BOOSTER_TYPE.OVEN_BREAKER)
        {
            if (booster == BOOSTER_TYPE.RAINBOW_BREAKER && boosterItem.IsCookie() == false)
            {
                return;
            }

            lockSwap = true;

            switch (booster)
            {
                case BOOSTER_TYPE.SINGLE_BREAKER:
                    DestroySingleBooster(boosterItem);
                    break;
                case BOOSTER_TYPE.ROW_BREAKER:
                    StartCoroutine(DestroyRowBooster(boosterItem));
                    break;
                case BOOSTER_TYPE.COLUMN_BREAKER:
                    StartCoroutine(DestroyColumnBooster(boosterItem));
                    break;
                case BOOSTER_TYPE.RAINBOW_BREAKER:
                    StartCoroutine(DestroyRainbowBooster(boosterItem));
                    break;
            }

            Booster.instance.BoosterComplete();

            // hide help object
            if (LevelLoader.instance.level == 7 && Help.instance.step == 2)
            {
                Help.instance.Hide();
            }
            if (LevelLoader.instance.level == 12 && Help.instance.step == 2)
            {
                Help.instance.Hide();
            }
            if (LevelLoader.instance.level == 15 && Help.instance.step == 2)
            {
                Help.instance.Hide();
            }
            if (LevelLoader.instance.level == 18 && Help.instance.step == 2)
            {
                Help.instance.Hide();
            }
        }

        if (boosterItem.Movable() && booster == BOOSTER_TYPE.OVEN_BREAKER)
        {
            StartCoroutine(DestroyOvenBooster(boosterItem));
        }
    }

    void DestroySingleBooster(Item boosterItem)
    {
        HideHint();

        AudioManager.instance.SingleBoosterAudio();

        boosterItem.Destroy();

        FindMatches();
    }

    IEnumerator DestroyRowBooster(Item boosterItem)
    {
        HideHint();

        AudioManager.instance.RowBoosterAudio();

        // animation

        // destroy a row
        var items = new List<Item>();

        items = RowItems(boosterItem.node.i);

        foreach (var item in items)
        {
            // this item maybe destroyed in other call
            if (item != null)
            {
                item.Destroy();
            }

            yield return new WaitForSeconds(0.1f);
        }

        FindMatches();
    }

    IEnumerator DestroyColumnBooster(Item boosterItem)
    {
        HideHint();

        AudioManager.instance.ColumnBoosterAudio();

        // animation

        // destroy a row
        var items = new List<Item>();

        items = ColumnItems(boosterItem.node.j);

        foreach (var item in items)
        {
            // this item maybe destroyed in other call
            if (item != null)
            {
                item.Destroy();
            }

            yield return new WaitForSeconds(0.1f);
        }

        FindMatches();
    }

    IEnumerator DestroyRainbowBooster(Item boosterItem)
    {
        HideHint();

        AudioManager.instance.RainbowBoosterAudio();

        boosterItem.DestroyItemsSameColor(boosterItem.color);

        yield return new WaitForFixedUpdate();
    }

    IEnumerator DestroyOvenBooster(Item boosterItem)
    {
        HideHint();

        if (ovenTouchItem == null)
        {
            ovenTouchItem = boosterItem;

            // add active
            ovenTouchItem.node.AddOvenBoosterActive();

            AudioManager.instance.ButtonClickAudio();
        }
        else
        {
            // the same item
            if (ovenTouchItem.node.OrderOnBoard() == boosterItem.node.OrderOnBoard())
            {
                // remove active
                ovenTouchItem.node.RemoveOvenBoosterActive();
                
                ovenTouchItem = null;

                AudioManager.instance.ButtonClickAudio();
            }
            // swap
            else
            {
                lockSwap = true;

                // hide help object
                if (LevelLoader.instance.level == 25 && Help.instance.step == 2)
                {
                    Help.instance.Hide();
                }

                boosterItem.node.AddOvenBoosterActive();

                AudioManager.instance.OvenBoosterAudio();

                AudioManager.instance.ButtonClickAudio();

                // animation
                iTween.MoveTo(ovenTouchItem.gameObject, iTween.Hash(
                    "position", boosterItem.gameObject.transform.position,                    
                    "easetype", iTween.EaseType.linear,
                    "time", Configure.instance.swapTime
                ));

                iTween.MoveTo(boosterItem.gameObject, iTween.Hash(
                    "position", ovenTouchItem.gameObject.transform.position,
                    "easetype", iTween.EaseType.linear,
                    "time", Configure.instance.swapTime
                ));

                yield return new WaitForSeconds(Configure.instance.swapTime);

                ovenTouchItem.node.RemoveOvenBoosterActive();
                boosterItem.node.RemoveOvenBoosterActive();

                var ovenTouchNode = ovenTouchItem.node;
                var boosterItemNode = boosterItem.node;

                // swap item
                ovenTouchNode.item = boosterItem;
                boosterItemNode.item = ovenTouchItem;

                // swap node
                ovenTouchItem.node = boosterItemNode;
                boosterItem.node = ovenTouchNode;

                // swap on hierarchy
                ovenTouchItem.gameObject.transform.SetParent(boosterItemNode.gameObject.transform);
                boosterItem.gameObject.transform.SetParent(ovenTouchNode.gameObject.transform);

                yield return new WaitForEndOfFrame();

                ovenTouchItem = null;

                Booster.instance.BoosterComplete();

                yield return new WaitForSeconds(0.1f);

                FindMatches();
            }
        }

        yield return new WaitForFixedUpdate();
    }

    #endregion 

    #region Collectible

    List<ITEM_TYPE> CheckGenerateCollectible()
    {
        bool hasCollectible = false;
        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.COLLECTIBLE)
            {
                hasCollectible = true;
                break;
            }
        }
        if (!hasCollectible)
        {
            return null;
        }

        var collectibles = new List<ITEM_TYPE>();

        if (CollectibleOnBoard() >= LevelLoader.instance.collectibleMaxOnBoard)
        {
            return null;
        }

        for (int j = 0; j < LevelLoader.instance.targetList.Count; j++)
        {
            TARGET_TYPE targetType = TARGET_TYPE.NONE;
            int targetColor = 0;
            int collectibleOnBoard = 0;
            int targetLeft = 0;

            targetType = LevelLoader.instance.targetList[j].Type;
            targetColor = LevelLoader.instance.targetList[j].color;
            collectibleOnBoard = CollectibleOnBoard(LevelLoader.instance.targetList[j].color);
            targetLeft = targetLeftList[j];

            if (targetType == TARGET_TYPE.COLLECTIBLE && collectibleOnBoard < targetLeft)
            {
                for (int k = 0; k < targetLeft - collectibleOnBoard; k++)
                {
                    collectibles.Add(ColorToCollectible(targetColor));
                }
            }
        }
        return collectibles;
    }

    ITEM_TYPE ColorToCollectible(int color)
    {
        switch (color)
        {
            case 1:
                return ITEM_TYPE.COLLECTIBLE_1;
            case 2:
                return ITEM_TYPE.COLLECTIBLE_2;
            case 3:
                return ITEM_TYPE.COLLECTIBLE_3;
            case 4:
                return ITEM_TYPE.COLLECTIBLE_4;
            case 5:
                return ITEM_TYPE.COLLECTIBLE_5;
            case 6:
                return ITEM_TYPE.COLLECTIBLE_6;
            case 7:
                return ITEM_TYPE.COLLECTIBLE_7;
            case 8:
                return ITEM_TYPE.COLLECTIBLE_8;
            case 9:
                return ITEM_TYPE.COLLECTIBLE_9;
            case 10:
                return ITEM_TYPE.COLLECTIBLE_10;
            case 11:
                return ITEM_TYPE.COLLECTIBLE_11;
            case 12:
                return ITEM_TYPE.COLLECTIBLE_12;
            case 13:
                return ITEM_TYPE.COLLECTIBLE_13;
            case 14:
                return ITEM_TYPE.COLLECTIBLE_14;
            case 15:
                return ITEM_TYPE.COLLECTIBLE_15;
            case 16:
                return ITEM_TYPE.COLLECTIBLE_16;
            case 17:
                return ITEM_TYPE.COLLECTIBLE_17;
            case 18:
                return ITEM_TYPE.COLLECTIBLE_18;
            case 19:
                return ITEM_TYPE.COLLECTIBLE_19;
            case 20:
                return ITEM_TYPE.COLLECTIBLE_20;
            default:
                return ITEM_TYPE.NONE;
        }
    }

    int CollectibleOnBoard(int color = 0)
    {
        int amount = 0;

        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var node = GetNode(i, j);

                if (node != null && node.item != null && node.item.IsCollectible())
                {
                    if (color == 0)
                    {
                        amount++;
                    }
                    else
                    {
                        if (node.item.color == color)
                        {
                            amount++;
                        }
                    }
                }
            }
        }

        return amount;
    }

    #endregion

    #region Marshmallow

    bool CheckGenerateMarshmallow()
    {

        bool hasMarshmallow = false;
        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.MARSHMALLOW)
            {
                hasMarshmallow = true;
                break;
            }
        }
        if (!hasMarshmallow)
        {
            return false;
        }

        var needGenerate = 0;

        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.MARSHMALLOW)
            {
                needGenerate += targetLeftList[i];
            }
        }

        if (needGenerate + LevelLoader.instance.marshmallowMoreThanTarget <= MarshmallowOnBoard())
        {
            return false;
        }

        return true;
    }

    int MarshmallowOnBoard()
    {
        int amount = 0;

        var row = LevelLoader.instance.row;
        var column = LevelLoader.instance.column;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var node = GetNode(i, j);

                if (node != null && node.item != null && node.item.IsMarshmallow())
                {
                    amount++;
                }
            }
        }

        return amount;
    }

    #endregion

    #region Itemclick

    public void ClickChoseItem(Item item)
    {
        var positiontmp = item.GetMousePosition();
        var swapItem = item;

        if (clickedItem != null)
        {
            //双击特殊块
            if (clickedItem == item && clickedItem.IsBreaker(clickedItem.type))
            {
                CancelChoseItem();

                if (!item.CheckHelpSwapable(SWAP_DIRECTION.SELFCLICK))
                {
                    return;
                }

                needIncreaseBubble = true;

                movingGingerbread = false;
                generatingGingerbread = false;
                skipGenerateGingerbread = false;

                lockSwap = true;

                HideHint();

                dropTime = 1;

                // hide help if need
                Help.instance.Hide();

                DecreaseMoveLeft();

                item.Destroy();

                FindMatches();
                return;
            }
            //点击周围块
            else if ((clickedItem.node.TopNeighbor() && item.node == clickedItem.node.TopNeighbor())
                || (clickedItem.node.LeftNeighbor() && item.node == clickedItem.node.LeftNeighbor())
                || (clickedItem.node.RightNeighbor() && item.node == clickedItem.node.RightNeighbor())
                || (clickedItem.node.BottomNeighbor() && item.node == clickedItem.node.BottomNeighbor()))
            {
                swapItem = clickedItem;
                positiontmp = clickedItem.gameObject.transform.position;
            }
            //点击其他块
            else
            {
                CancelChoseItem();

                if (item.type != ITEM_TYPE.BLANK)
                {
                    clickedItem = item;

                    CreateChoseSprite(item);
                }

            }
            //check
        }
        else
        {
            if (item.type != ITEM_TYPE.BLANK)
            {
                clickedItem = item;

                CreateChoseSprite(item);
            }

        }
        if (swapItem.type != ITEM_TYPE.BLANK)
        {
            swapItem.drag = true;
            swapItem.mousePostion = positiontmp;
            swapItem.deltaPosition = Vector3.zero;

            movingGingerbread = false;
            generatingGingerbread = false;
            skipGenerateGingerbread = false;
        }


    }

    public void CancelChoseItem()
    {
        if (clickedItem != null)
        {
            //function()

            if (clickedItem.transform.parent.Find("kuang"))
            {
                GameObject.Destroy(clickedItem.gameObject.transform.parent.Find("kuang").gameObject);
            }
            clickedItem = null;
        }
    }

    private void CreateChoseSprite(Item item)
    {
        item.CookieGeneralEffect();
        GameObject kuang = new GameObject();
        kuang.transform.parent = item.node.transform;
        kuang.name = "kuang";
        kuang.transform.localPosition = item.transform.localPosition;
        kuang.transform.localScale = item.transform.localScale;
        kuang.AddComponent<SpriteRenderer>();
        kuang.GetComponent<SpriteRenderer>().sortingLayerName = "Effect";

        var spr = new Sprite();
        spr = Resources.Load(Configure.ChoseIcon(),spr.GetType()) as Sprite;
        kuang.GetComponent<SpriteRenderer>().sprite = spr;
        //float loopTime = 0.5f;
        //kuang.transform.DOScale(new Vector3(1.1f, 1.1f, 1.0f), loopTime).SetLoops(-1, LoopType.Yoyo);
        //kuang.GetComponent<SpriteRenderer>().material.DOFade(0.4f, loopTime).SetLoops(-1, LoopType.Yoyo);
    }

    #endregion

    #region bubble

    public void IncreaseBubble()
    {
        List<Item> prepareToChange = new List<Item>();
        for (int i = 0; i < LevelLoader.instance.row; i++)
        {
            for (int j = 0; j < LevelLoader.instance.column; j++)
            {
                var order = NodeOrder(i, j);
                var item = nodes[order].item;
                if (item != null && item.type == ITEM_TYPE.ROCK_CANDY)
                {
                    var top = item.node.TopNeighbor();
                    var bottom = item.node.BottomNeighbor();
                    var left = item.node.LeftNeighbor();
                    var right = item.node.RightNeighbor();
                    if (top != null && top.item != null && top.item.CanChangeToBubble())
                    {
                        prepareToChange.Add(top.item);
                    }
                    if (bottom!= null && bottom.item != null && bottom.item.CanChangeToBubble())
                    {
                        prepareToChange.Add(bottom.item);
                    }
                    if (left != null && left.item!=null && left.item.CanChangeToBubble())
                    {
                        prepareToChange.Add(left.item);
                    }
                    if (right!=null && right.item !=null && right.item.CanChangeToBubble())
                    {
                        prepareToChange.Add(right.item);
                    }
                }
            }
        }
        if (prepareToChange.Count == 0)
        {
            return;
        }

        var rnd = Random.Range(0, prepareToChange.Count);

        ChangeToBubble(prepareToChange[rnd]);


    }


    public void ChangeToBubble(Item item)
    {
        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            if (LevelLoader.instance.targetList[i].Type == TARGET_TYPE.ROCK_CANDY
                && targetLeftList[i] > 0
            )
            {
                targetLeftList[i]++;

                UITarget.UpdateTargetAmount(i);
                break;
            }
        }

        item.node.GenerateItem(ITEM_TYPE.ROCK_CANDY);
        Destroy(item.gameObject);
    }

    #endregion

    #region Change


    // change all items to column-breaker/row-breaker/bomb-breaker/x-breaker when swap a rainbow with a breaker
    public void ChangeItemsType(int color, ITEM_TYPE changeToType, bool isgrass)
    {
        StartCoroutine(TryToChangeType(color, changeToType,isgrass));
    }

    IEnumerator TryToChangeType(int color, ITEM_TYPE changeToType, bool isgrass)
    {
        List<Item> items = GetListItems();

        foreach (var item in items)
        {
            if (item != null)
            {
                if (item.color == color && item.IsCookie() && item.Matchable())
                {
                    GameObject explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RainbowExplosion()) as GameObject);

                    if (explosion != null) explosion.transform.position = item.transform.position;

                    if (item.node != null && item.node.cage != null)
                    {
                        // destroy and check collect cage
                        item.node.CageExplode();
                        yield return new WaitForSeconds(0.1f);
                        continue;
                    }
                    if (item.node != null && item.node.ice != null)
                    {
                        // destroy and check collect ice
                        item.node.IceExplode();
                        yield return new WaitForSeconds(0.1f);
                        continue;
                    }
                    if (item.node != null && item.node.jelly != null)
                    {
                        // destroy and check collect jelly
                        if (item.node.JellyExplode())
                        {
                            yield return new WaitForSeconds(0.1f);
                            continue;
                        }
                    }
                    if (item.node != null && item.node.packagebox != null)
                    {
                        // destroy packagebox
                        item.node.PackageBoxExplode();
                        yield return new WaitForSeconds(0.1f);
                        continue;
                    }



                    if (item.IsColumnBreaker(changeToType) || item.IsRowBreaker(changeToType))
                    {
                        if (item.IsCookie() && item.node.CanChangeType())
                        {
                            CollectItem(item);
                            item.ChangeToColRowBreaker();
                        }
                    }
                    else if (item.IsBombBreaker(changeToType))
                    {
                        if (item.IsCookie() && item.node.CanChangeType())
                        {
                            CollectItem(item);
                            item.ChangeToBombBreaker();
                        }
                    }
                    else if (item.IsPlaneBreaker(changeToType))
                    {
                        if (item.IsCookie() && item.node.CanChangeType())
                        {
                            CollectItem(item);
                            item.ChangeToPlaneBreaker();
                        }
                    }
                    changingList.Add(item);

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        DestroyChangingList(isgrass);
    }

    #endregion

    #region MoveLeft

    public void DecreaseMoveLeft(bool effect = false)
    {
        moveLeft--;
        allstep++;
        UITop.DecreaseMoves(effect);

        if (isFirstMove)
        {
            isFirstMove = false;
            //NetManager.instance.eliminateLevelStart();
            qy.GameMainManager.Instance.playerModel.StartLevel();
            //todo：扣除开始道具
            //LevelLoader.instance.beginItemList
			int j = LevelLoader.instance.beginItemList.Count;
			for (int i = 0; i < j; i++) {
				string itemId = LevelLoader.instance.beginItemList [i];
				//NetManager.instance.userToolsToServer (itemId, "1");
                qy.GameMainManager.Instance.playerModel.UseProp(itemId,1);
            }
        }
    }

    #endregion
}
