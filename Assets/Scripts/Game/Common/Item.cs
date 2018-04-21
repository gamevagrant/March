using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;

public class Item : MonoBehaviour 
{
    [Header("Parent")]
    public Board board;
    public Node node;

    [Header("Variables")]
    public int color;
    public ITEM_TYPE type;
    public ITEM_TYPE next = ITEM_TYPE.NONE;    
    public BREAKER_EFFECT effect = BREAKER_EFFECT.NORMAL;

    [Header("Check")]
    public bool drag;
    public bool nextSound = true;
    public bool destroying;
    public bool dropping;
    public bool changing;

    [SerializeField]
    private int _beabletodestroy;
    public int beAbleToDestroy
    {
        get
        {
            if (type == ITEM_TYPE.APPLEBOX)
            {
                return applebox.beAbleToDestroy;
            }
            else
            {
                return _beabletodestroy;
            }
        }
        set
        {
            if (type == ITEM_TYPE.APPLEBOX)
            {
                applebox.beAbleToDestroy = value;
            }
            else
            {
                _beabletodestroy = value;
            }
        }
    }


    //public int 
    public Vector3 mousePostion = Vector3.zero;
    public Vector3 deltaPosition = Vector3.zero;
    public Vector3 swapDirection = Vector3.zero;

    [Header("Swap")]
    public Node neighborNode;
    public Item swapItem;

    [Header("Drop")]
    public List<Vector3> dropPath;
    private SpriteRenderer m_spriteRenderer;

    private ITEM_TYPE planeTargetType = ITEM_TYPE.NONE;

    public AppleBox applebox;

    //effect config param
    private Vector3 COOKIESCALE = new Vector3(1.8f,1.8f,1.8f);

    void Update()
    {
        //todo: 修改为拖拽时判断 去掉每帧检测
        // if a item is dragged
        if (drag)
        {
            deltaPosition = mousePostion - GetMousePosition();
            if (swapDirection == Vector3.zero)
            {
                SwapDirection(deltaPosition);
            }
        }
        if (m_spriteRenderer == null)
        {
            var renderer = GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                m_spriteRenderer = renderer;
                m_spriteRenderer.sortingLayerName="Item";
            }
        }
    }

    #region Type

    public bool Movable()
    {
        if (
            type == ITEM_TYPE.CHOCOLATE_1_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_2_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_3_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_4_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_5_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_6_LAYER ||
            type == ITEM_TYPE.ROCK_CANDY ||
            type == ITEM_TYPE.APPLEBOX ||
            type == ITEM_TYPE.BLANK
            )
        {
            return false;
        }

        // cage
        if (node.cage != null)
        {
            if (node.cage.type == CAGE_TYPE.CAGE_1 || node.cage.type == CAGE_TYPE.CAGE_2)
            {
                return false;
            }
        }

        if (node.jelly != null)
        {
            if (node.jelly.type == JELLY_TYPE.JELLY_1 || node.jelly.type == JELLY_TYPE.JELLY_2 ||
                node.jelly.type == JELLY_TYPE.JELLY_3)
            {
                return false;
            }
        }

        if (node.packagebox != null)
        {
            if (node.packagebox.type != PACKAGEBOX_TYPE.NONE)
            {
                return false;
            }
        }

        return true;
    }

    public bool Droppable()
    {
        if (
            type == ITEM_TYPE.CHOCOLATE_1_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_2_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_3_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_4_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_5_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_6_LAYER ||
            type == ITEM_TYPE.ROCK_CANDY ||
            type == ITEM_TYPE.APPLEBOX ||
            type == ITEM_TYPE.BLANK
        )
        {
            return false;
        }

        // cage
        if (node.cage != null)
        {
            if (node.cage.type == CAGE_TYPE.CAGE_1 || node.cage.type == CAGE_TYPE.CAGE_2)
            {
                return false;
            }
        }

        if (node.jelly != null)
        {
            if (node.jelly.type == JELLY_TYPE.JELLY_1 || node.jelly.type == JELLY_TYPE.JELLY_2 ||
                node.jelly.type == JELLY_TYPE.JELLY_3)
            {
                return false;
            }
        }

        if (node.packagebox != null)
        {
            if (node.packagebox.type != PACKAGEBOX_TYPE.NONE)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 检查是否可交换 0 不检查方向 1上 2右 3下 4左
    /// </summary>
    /// <param name="derection"></param>
    /// <returns></returns>
    public bool Exchangeable(SWAP_DIRECTION direction)
    {
        if (
            type == ITEM_TYPE.CHOCOLATE_1_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_2_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_3_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_4_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_5_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_6_LAYER ||
            type == ITEM_TYPE.APPLEBOX ||
            type == ITEM_TYPE.ROCK_CANDY
        )
        {
            return false;
        }

        // cage
        if (node.cage != null)
        {
            if (node.cage.type == CAGE_TYPE.CAGE_1 || node.cage.type == CAGE_TYPE.CAGE_2)
            {
                return false;
            }
        }

        if (node.jelly != null)
        {
            if (node.jelly.type == JELLY_TYPE.JELLY_1 || node.jelly.type == JELLY_TYPE.JELLY_2 ||
                node.jelly.type == JELLY_TYPE.JELLY_3)
            {
                return false;
            }
        }

        if (node.ice != null)
        {
            if (node.ice.type == ICE_TYPE.ICE_1 || node.ice.type == ICE_TYPE.ICE_2)
            {
                return false;
            }
        }

        if (node.packagebox != null)
        {
            if (node.packagebox.type != PACKAGEBOX_TYPE.NONE)
            {
                return false;
            }
        }
        if (direction == SWAP_DIRECTION.NONE)
        {
            
        }
        else if (direction == SWAP_DIRECTION.TOP)
        {
            if (node.TopNeighbor() != null && node.TopNeighbor().bafflebottom != null)
            {
                return false;
            }
        }
        else if (direction == SWAP_DIRECTION.RIGHT)
        {
            if (node.baffleright!= null)
            {
                return false;
            }
        }
        else if (direction == SWAP_DIRECTION.BOTTOM)
        {
            if(node.bafflebottom != null)
            {
                return false;
            }
        }
        else if (direction == SWAP_DIRECTION.LEFT)
        {
            if (node.LeftNeighbor() != null && node.LeftNeighbor().baffleright != null)
            {
                return false;
            }
        }
        else
        {
            Debug.Log("方向参数错误");
        }

        return true;
    }


    public bool Matchable()
    {
        if (type == ITEM_TYPE.BLANK ||
            type == ITEM_TYPE.CHOCOLATE_1_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_2_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_3_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_4_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_5_LAYER ||
            type == ITEM_TYPE.CHOCOLATE_6_LAYER ||
            type == ITEM_TYPE.ROCK_CANDY ||
            type == ITEM_TYPE.MARSHMALLOW ||
            type == ITEM_TYPE.COOKIE_RAINBOW ||

            type == ITEM_TYPE.COOKIE_COLUMN_BREAKER ||
            type == ITEM_TYPE.COOKIE_ROW_BREAKER ||
            type == ITEM_TYPE.COOKIE_BOMB_BREAKER ||
            type == ITEM_TYPE.COOKIE_PLANE_BREAKER ||

            type == ITEM_TYPE.COLLECTIBLE_1 ||
            type == ITEM_TYPE.COLLECTIBLE_2 ||
            type == ITEM_TYPE.COLLECTIBLE_3 ||
            type == ITEM_TYPE.COLLECTIBLE_4 ||
            type == ITEM_TYPE.COLLECTIBLE_5 ||
            type == ITEM_TYPE.COLLECTIBLE_6 ||
            type == ITEM_TYPE.COLLECTIBLE_7 ||
            type == ITEM_TYPE.COLLECTIBLE_8 ||
            type == ITEM_TYPE.COLLECTIBLE_9 ||

            type == ITEM_TYPE.APPLEBOX
            
            )
        {
            return false;
        }

        if (node.jelly != null)
        {
            if (node.jelly.type == JELLY_TYPE.JELLY_1 || node.jelly.type == JELLY_TYPE.JELLY_2 ||
                node.jelly.type == JELLY_TYPE.JELLY_3)
            {
                return false;
            }
        }

        if (node.packagebox != null)
        {
            if (node.packagebox.type != PACKAGEBOX_TYPE.NONE)
            {
                return false;
            }
        }

        return true;
    }

    public bool Destroyable()
    {
        if (type == ITEM_TYPE.COLLECTIBLE_1 ||
            type == ITEM_TYPE.COLLECTIBLE_2 ||
            type == ITEM_TYPE.COLLECTIBLE_3 ||
            type == ITEM_TYPE.COLLECTIBLE_4 ||
            type == ITEM_TYPE.COLLECTIBLE_5 ||
            type == ITEM_TYPE.COLLECTIBLE_6 ||
            type == ITEM_TYPE.COLLECTIBLE_7 ||
            type == ITEM_TYPE.COLLECTIBLE_8 ||
            type == ITEM_TYPE.COLLECTIBLE_9 || 
            type == ITEM_TYPE.COLLECTIBLE_10 ||
            type == ITEM_TYPE.COLLECTIBLE_11 ||
            type == ITEM_TYPE.COLLECTIBLE_12 ||
            type == ITEM_TYPE.COLLECTIBLE_13 ||
            type == ITEM_TYPE.COLLECTIBLE_14 ||
            type == ITEM_TYPE.COLLECTIBLE_15 ||
            type == ITEM_TYPE.COLLECTIBLE_16 ||
            type == ITEM_TYPE.COLLECTIBLE_17 ||
            type == ITEM_TYPE.COLLECTIBLE_18 ||
            type == ITEM_TYPE.COLLECTIBLE_19 ||
            type == ITEM_TYPE.COLLECTIBLE_20)
        {
            return false;
        }

        return true;
    }

    public bool CanChangeToBubble()
    {
        if (Movable()
            && (IsCookie() ||IsBreaker(type) ||type == ITEM_TYPE.COOKIE_RAINBOW)
        )
        {
            return true;
        }
        return false;
    }

    public bool IsCookie()
    {
        if (type == ITEM_TYPE.COOKIE_1 ||
               type == ITEM_TYPE.COOKIE_2 ||
               type == ITEM_TYPE.COOKIE_3 ||
               type == ITEM_TYPE.COOKIE_4 ||
               type == ITEM_TYPE.COOKIE_5 ||
               type == ITEM_TYPE.COOKIE_6)
        {
            return true;
        }

        return false;

    }

    public bool IsBlank()
    {
        if (type == ITEM_TYPE.BLANK)
        {
            return true;
        }

        return false;
    }

    public bool IsCollectible()
    {
        if (type == ITEM_TYPE.COLLECTIBLE_1 ||
            type == ITEM_TYPE.COLLECTIBLE_2 ||
            type == ITEM_TYPE.COLLECTIBLE_3 ||
            type == ITEM_TYPE.COLLECTIBLE_4 ||
            type == ITEM_TYPE.COLLECTIBLE_5 ||
            type == ITEM_TYPE.COLLECTIBLE_6 ||
            type == ITEM_TYPE.COLLECTIBLE_7 ||
            type == ITEM_TYPE.COLLECTIBLE_8 ||
            type == ITEM_TYPE.COLLECTIBLE_9 ||
            type == ITEM_TYPE.COLLECTIBLE_10 ||
            type == ITEM_TYPE.COLLECTIBLE_11 ||
            type == ITEM_TYPE.COLLECTIBLE_12 ||
            type == ITEM_TYPE.COLLECTIBLE_13 ||
            type == ITEM_TYPE.COLLECTIBLE_14 ||
            type == ITEM_TYPE.COLLECTIBLE_15 ||
            type == ITEM_TYPE.COLLECTIBLE_16 ||
            type == ITEM_TYPE.COLLECTIBLE_17 ||
            type == ITEM_TYPE.COLLECTIBLE_18 ||
            type == ITEM_TYPE.COLLECTIBLE_19 ||
            type == ITEM_TYPE.COLLECTIBLE_20)
        {
            return true;
        }

        return false;
    }

    public bool IsGingerbread()
    {
        if (type == ITEM_TYPE.GINGERBREAD_1 ||
               type == ITEM_TYPE.GINGERBREAD_2 ||
               type == ITEM_TYPE.GINGERBREAD_3 ||
               type == ITEM_TYPE.GINGERBREAD_4 ||
               type == ITEM_TYPE.GINGERBREAD_5 ||
               type == ITEM_TYPE.GINGERBREAD_6)
        {
            return true;
        }

        return false;
    }

    public bool IsMarshmallow()
    {
        if (type == ITEM_TYPE.MARSHMALLOW)
        {
            return true;
        }
        return false;
    }

    public bool IsAppleBox()
    {
        if (type == ITEM_TYPE.APPLEBOX)
        {
            return true;
        }
        return false;
    }

	public bool IsPackageBox()
	{
		if (type == ITEM_TYPE.APPLEBOX)
		{
			return true;
		}
		return false;
	}

    public bool IsChocolate()
    {
        if (type == ITEM_TYPE.CHOCOLATE_1_LAYER ||
               type == ITEM_TYPE.CHOCOLATE_2_LAYER ||
               type == ITEM_TYPE.CHOCOLATE_3_LAYER ||
               type == ITEM_TYPE.CHOCOLATE_4_LAYER ||
               type == ITEM_TYPE.CHOCOLATE_5_LAYER ||
               type == ITEM_TYPE.CHOCOLATE_6_LAYER)
        {
            return true;
        }

        return false;
    }

    public bool IsRockCandy()
    {
        if (type == ITEM_TYPE.ROCK_CANDY)
        {
            return true;
        }

        return false;
    }

    public bool IsCherry()
    {
        if (type == ITEM_TYPE.CHERRY)
        {
            return true;
        }

        return false;
    }

    public ITEM_TYPE OriginCookieType()
    {
        var order = board.NodeOrder(node.i, node.j);

        return LevelLoader.instance.itemLayerData[order];
    }

    // check if breaker is row or column
    ITEM_TYPE GetColRowBreaker(ITEM_TYPE check, Vector3 direction)
    {
        //print("direction: " + direction);

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //print("row");

            switch (check)
            {
                case ITEM_TYPE.COOKIE_1:
                case ITEM_TYPE.COOKIE_2:
                case ITEM_TYPE.COOKIE_3:
                case ITEM_TYPE.COOKIE_4:
                case ITEM_TYPE.COOKIE_5:
                case ITEM_TYPE.COOKIE_6:
                    return ITEM_TYPE.COOKIE_ROW_BREAKER;
                default:
                    return ITEM_TYPE.NONE;
            }
        }
        else
        {
            //print("colmn");

            switch (check)
            {
                case ITEM_TYPE.COOKIE_1:
                case ITEM_TYPE.COOKIE_2:
                case ITEM_TYPE.COOKIE_3:
                case ITEM_TYPE.COOKIE_4:
                case ITEM_TYPE.COOKIE_5:
                case ITEM_TYPE.COOKIE_6:
                    return ITEM_TYPE.COOKIE_COLUMN_BREAKER;
                default:
                    return ITEM_TYPE.NONE;
            }
        }
    }

    public bool IsBombBreaker(ITEM_TYPE check)
    {
        if (check == ITEM_TYPE.COOKIE_BOMB_BREAKER
            )
        {
            return true;
        }

        return false;
    }


    public bool IsColumnBreaker(ITEM_TYPE check)
    {
        if (check == ITEM_TYPE.COOKIE_COLUMN_BREAKER
            )
        {
            return true;
        }

        return false;
    }

    public bool IsRowBreaker(ITEM_TYPE check)
    {
        if (check == ITEM_TYPE.COOKIE_ROW_BREAKER
            )
        {
            return true;
        }

        return false;
    }

    public bool IsPlaneBreaker(ITEM_TYPE check)
    {
        if (check == ITEM_TYPE.COOKIE_PLANE_BREAKER
        )
        {
            return true;
        }

        return false;
    }

    public bool IsBreaker(ITEM_TYPE check)
    {
        if (IsBombBreaker(check) || IsColumnBreaker(check) || IsRowBreaker(check) || IsPlaneBreaker(check))
        {
            return true;
        }

        return false;
    }

    public ITEM_TYPE GetBombBreaker(ITEM_TYPE check)
    {
        switch (check)
        {
            case ITEM_TYPE.COOKIE_1:
            case ITEM_TYPE.COOKIE_2:
            case ITEM_TYPE.COOKIE_3:
            case ITEM_TYPE.COOKIE_4:
            case ITEM_TYPE.COOKIE_5:
            case ITEM_TYPE.COOKIE_6:
                return ITEM_TYPE.COOKIE_BOMB_BREAKER;
            default:
                return ITEM_TYPE.NONE;
        }
    }

    public ITEM_TYPE GetColumnBreaker(ITEM_TYPE check)
    {
        switch (check)
        {
            case ITEM_TYPE.COOKIE_1:
            case ITEM_TYPE.COOKIE_2:
            case ITEM_TYPE.COOKIE_3:
            case ITEM_TYPE.COOKIE_4:
            case ITEM_TYPE.COOKIE_5:
            case ITEM_TYPE.COOKIE_6:
                return ITEM_TYPE.COOKIE_COLUMN_BREAKER;
            default:
                return ITEM_TYPE.NONE;
        }
    }

    public ITEM_TYPE GetRowBreaker(ITEM_TYPE check)
    {
        switch (check)
        {
            case ITEM_TYPE.COOKIE_1:
            case ITEM_TYPE.COOKIE_2:
            case ITEM_TYPE.COOKIE_3:
            case ITEM_TYPE.COOKIE_4:
            case ITEM_TYPE.COOKIE_5:
            case ITEM_TYPE.COOKIE_6:
                return ITEM_TYPE.COOKIE_ROW_BREAKER;
            default:
                return ITEM_TYPE.NONE;
        }
    }

    public ITEM_TYPE GetPlaneBreaker(ITEM_TYPE check)
    {
        switch (check)
        {
            case ITEM_TYPE.COOKIE_1:
            case ITEM_TYPE.COOKIE_2:
            case ITEM_TYPE.COOKIE_3:
            case ITEM_TYPE.COOKIE_4:
            case ITEM_TYPE.COOKIE_5:
            case ITEM_TYPE.COOKIE_6:
                return ITEM_TYPE.COOKIE_PLANE_BREAKER;
            default:
                return ITEM_TYPE.NONE;
        }
    }

    public ITEM_TYPE GetCookie(ITEM_TYPE check)
    {
        switch (check)
        {
            case ITEM_TYPE.COOKIE_1:
                return ITEM_TYPE.COOKIE_1;
            case ITEM_TYPE.COOKIE_2:
                return ITEM_TYPE.COOKIE_2;
            case ITEM_TYPE.COOKIE_3:
                return ITEM_TYPE.COOKIE_3;
            case ITEM_TYPE.COOKIE_4:
                return ITEM_TYPE.COOKIE_4;
            case ITEM_TYPE.COOKIE_5:
                return ITEM_TYPE.COOKIE_5;
            case ITEM_TYPE.COOKIE_6:
                return ITEM_TYPE.COOKIE_6;
            default:
                return ITEM_TYPE.NONE;
        }
    }

    #endregion

    #region Swap
    // helper function to know the direction of swap
    public Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // calculate the direction
    void SwapDirection(Vector3 delta)
    {
        deltaPosition = delta;

        var direction = SWAP_DIRECTION.NONE;

        if (Vector3.Magnitude(deltaPosition) > 0.85f)
        {
            if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y) && deltaPosition.x > 0) swapDirection.x = 1;
            else if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y) && deltaPosition.x < 0) swapDirection.x = -1;
            else if (Mathf.Abs(deltaPosition.x) < Mathf.Abs(deltaPosition.y) && deltaPosition.y > 0) swapDirection.y = 1;
            else if (Mathf.Abs(deltaPosition.x) < Mathf.Abs(deltaPosition.y) && deltaPosition.y < 0) swapDirection.y = -1;

            bool hasbaffle = false;

            if (swapDirection.x > 0)
            {
                //Debug.Log("Left");
                direction = SWAP_DIRECTION.LEFT;

                if (node != null)
                {
                    if (node.LeftNeighbor() != null)
                    {
                        if (node.LeftNeighbor().item != null)
                        {
                            if (node.LeftNeighbor().item.Exchangeable(SWAP_DIRECTION.RIGHT))
                            {
                                neighborNode = node.LeftNeighbor();
                                if (neighborNode.baffleright != null)
                                {
                                    hasbaffle = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (swapDirection.x < 0)
            {
                //Debug.Log("Right");
                direction = SWAP_DIRECTION.RIGHT;

                if (node != null)
                {
                    if (node.RightNeighbor() != null)
                    {
                        if (node.RightNeighbor().item != null)
                        {
                            if (node.RightNeighbor().item.Exchangeable(SWAP_DIRECTION.LEFT))
                            {
                                neighborNode = node.RightNeighbor();
                                if(node.baffleright != null)
                                {
                                    hasbaffle = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (swapDirection.y > 0)
            {
                //Debug.Log("Bottom");
                direction = SWAP_DIRECTION.BOTTOM;

                if (node != null)
                {
                    if (node.BottomNeighbor() != null)
                    {
                        if (node.BottomNeighbor().item != null)
                        {
                            if (node.BottomNeighbor().item.Exchangeable(SWAP_DIRECTION.TOP))
                            {
                                neighborNode = node.BottomNeighbor();
                                if (node.bafflebottom != null)
                                {
                                    hasbaffle = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (swapDirection.y < 0)
            {
                //Debug.Log("Top");
                direction = SWAP_DIRECTION.TOP;

                if (node != null)
                {
                    if (node.TopNeighbor() != null)
                    {
                        if (node.TopNeighbor().item != null)
                        {
                            if (node.TopNeighbor().item.Exchangeable(SWAP_DIRECTION.BOTTOM))
                            {
                                neighborNode = node.TopNeighbor();
                                if (neighborNode.bafflebottom != null)
                                {
                                    hasbaffle = true;
                                }
                            }
                        }
                    }
                }
            }

            if (neighborNode != null && neighborNode.item != null && CheckHelpSwapable(direction) && !hasbaffle)
            {
                swapItem = neighborNode.item;

                board.touchedItem = this;
                board.swappedItem = swapItem;

                board.CancelChoseItem();
                Swap();
            }
            else
            {
                if (hasbaffle)
                {
                    
                }
                // if no neighbor item we need to reset to be able to swap again
                Reset();
            }
        }
    }

    // swap animation
    public void Swap(bool forced = false)
    {
        if (swapDirection != Vector3.zero && neighborNode != null)
        {
            CookieGeneralEffect();
            swapItem.CookieGeneralEffect();

            iTween.MoveTo(gameObject, iTween.Hash(
                "position", swapItem.transform.position,
                "onstart", "OnStartSwap",
                "oncomplete", "OnCompleteSwap",
                "oncompleteparams", new Hashtable() { { "forced", forced } },
                "easetype", iTween.EaseType.linear,
                "time", Configure.instance.swapTime
            ));

            iTween.MoveTo(swapItem.gameObject, iTween.Hash(
                "position", transform.position,
                "easetype", iTween.EaseType.linear,
                "time", Configure.instance.swapTime
            ));
        }
    }

    public void OnStartSwap()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

        AudioManager.instance.SwapAudio();

        board.lockSwap = true;

        board.HideHint();

        board.dropTime = 1;

        // hide help if need
        Help.instance.Hide();
    }

    public void OnCompleteSwap(Hashtable args)
    {
        var forced = (bool)args["forced"];

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

        SwapItem();
        
        // after swap this.node = neighbor
        var matchesHere = (node.FindMatches() != null)? node.FindMatches().Count : 0;
        var matchesAtNeighbor = (swapItem.node.FindMatches() != null)? swapItem.node.FindMatches().Count : 0;
        var squareMatchesHere = (node.FindSquareMatches() != null) ? node.FindSquareMatches().Count : 0;
        var squareMatchesAtNeighbor = (swapItem.node.FindSquareMatches() != null) ? swapItem.node.FindSquareMatches().Count : 0;
        var special = false;

        //print("matches here:" + matchesHere);
        //print("matches at neighbor" + matchesAtNeighbor);

        // one of items is rainbow and other is cookie
        if (type == ITEM_TYPE.COOKIE_RAINBOW && (swapItem.IsCookie() || IsBreaker(swapItem.type) || swapItem.type == ITEM_TYPE.COOKIE_RAINBOW))
        {
            special = true;
        }


        if (swapItem.type == ITEM_TYPE.COOKIE_RAINBOW && (IsCookie() || IsBreaker(type) || type == ITEM_TYPE.COOKIE_RAINBOW))
        {
            special = true;
        }

        if (IsBreaker(type) && (swapItem.IsCookie() || IsBreaker(swapItem.type) || swapItem.type == ITEM_TYPE.COOKIE_RAINBOW || swapItem.IsMarshmallow() || swapItem.IsCollectible() || swapItem.IsBlank()))
        {
            special = true;
        }


        if (IsBreaker(swapItem.type) && (IsCookie() || IsBreaker(type) || type == ITEM_TYPE.COOKIE_RAINBOW|| IsMarshmallow() || IsCollectible()))
        {
            special = true;
        }

        if (matchesHere <= 0 && matchesAtNeighbor <= 0 && squareMatchesHere <= 0 && squareMatchesAtNeighbor <= 0 && special == false && Configure.instance.checkSwap && forced == false)
        {
            // swap back
            iTween.MoveTo(gameObject, iTween.Hash(
                "position", swapItem.transform.position,
                "onstart", "OnStartSwapBack",
                "oncomplete", "OnCompleteSwapBack",
                "easetype", iTween.EaseType.linear,
                "time", Configure.instance.swapTime
            ));
            
            iTween.MoveTo(swapItem.gameObject, iTween.Hash(
                "position", transform.position,
                "easetype", iTween.EaseType.linear,
                "time", Configure.instance.swapTime
                ));
        }
        else
        {

            //泡沫增长
            board.needIncreaseBubble = true;

            // do not reduce move when forced swap
            if (forced == false)
            {
                board.DecreaseMoveLeft();
            }

            if (special)
            {
                //如果交换前的位置有草地 特殊块会使交换后的地块变为草地
                if ((IsBreaker(type)||type == ITEM_TYPE.COOKIE_RAINBOW) && swapItem.node.IsGrass())
                {
                    node.ChangeToGrass();
                }
                if ((swapItem.IsBreaker(swapItem.type) || swapItem.type == ITEM_TYPE.COOKIE_RAINBOW) && node.IsGrass())
                {
                    swapItem.node.ChangeToGrass();
                }


                RainbowDestroy(this, swapItem);

                PlaneSpecialDestroy(this, swapItem);

                TwoColRowBreakerDestroy(this, swapItem);

                TwoBombBreakerDestroy(this, swapItem);

                ColRowBreakerAndBombBreakerDestroy(this, swapItem);

                if ((swapItem.IsCookie()||swapItem.IsMarshmallow() || swapItem.IsCollectible() || swapItem.IsBlank()) &&
                    (type == ITEM_TYPE.COOKIE_ROW_BREAKER
                    || type == ITEM_TYPE.COOKIE_COLUMN_BREAKER
                    || type == ITEM_TYPE.COOKIE_BOMB_BREAKER
                    )
                )
                {
                    this.Destroy();
                    this.beAbleToDestroy--;

                    board.FindMatches();
                }

                if ((IsCookie() || IsMarshmallow() || IsCollectible()) && 
                    (swapItem.type == ITEM_TYPE.COOKIE_ROW_BREAKER
                     || swapItem.type == ITEM_TYPE.COOKIE_COLUMN_BREAKER
                     || swapItem.type == ITEM_TYPE.COOKIE_BOMB_BREAKER
                     )
                )
                {
                    swapItem.Destroy();
                    //swapItem.beAbleToDestroy--;

                    board.FindMatches();

                }

                }
            else
            {
                //todo:根据配置决定优先级
                //飞机生成的优先级最低在最上面
                if (squareMatchesHere == 4)
                {
                    next = ITEM_TYPE.COOKIE_PLANE_BREAKER;
                }

                if (squareMatchesAtNeighbor == 4)
                {
                    swapItem.next = ITEM_TYPE.COOKIE_PLANE_BREAKER;
                }

                if (matchesHere == 4)
                {
                    next = GetColRowBreaker(this.type, transform.position - swapItem.transform.position);
                }
                else if (matchesHere >= 5)
                {
                    next = ITEM_TYPE.COOKIE_RAINBOW;
                }
                if (matchesAtNeighbor == 4)
                {
                    swapItem.next = GetColRowBreaker(swapItem.type, transform.position - swapItem.transform.position);
                }
                else if (matchesAtNeighbor >= 5)
                {
                    swapItem.next = ITEM_TYPE.COOKIE_RAINBOW;
                }


                // find the matches to destroy (destroy match 3/4/5)
                // this function will not destroy special match such as rainbow swap with breaker etc.
                board.FindMatches();
            }

            // we reset here because the item will be destroy soon (the board is still lock)
            Reset();
        }
    }

    public void OnStartSwapBack()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

        AudioManager.instance.SwapBackAudio();

        // hide in case the help crash
        if (Help.instance.gameObject.activeSelf)
        {
            Help.instance.HideOnSwapBack();
        }        
    }

    public void OnCompleteSwapBack()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

        SwapItemBack();

        // fix swap back wrong position cause by iTween
        transform.position = board.NodeLocalPosition(node.i, node.j) + board.transform.position;

        Reset();

        board.lockSwap = false;

        StartCoroutine(board.ShowHint());
    }

    public void SwapItem()
    {
        Node thisNode = node;

        thisNode.item = swapItem;
        neighborNode.item = this;

        this.node = neighborNode;
        swapItem.node = thisNode;

        this.gameObject.transform.SetParent(neighborNode.gameObject.transform);
        swapItem.gameObject.transform.SetParent(thisNode.gameObject.transform);
    }

    void SwapItemBack()
    {
        Node swapNode = swapItem.node;

        this.node.item = swapItem;
        swapNode.item = this;

        this.node = swapItem.node;
        swapItem.node = neighborNode;

        this.gameObject.transform.SetParent(swapNode.gameObject.transform);
        swapItem.gameObject.transform.SetParent(neighborNode.gameObject.transform);
    }

    // reset info after a swap
    public void Reset()
    {
        drag = false;

        swapDirection = Vector3.zero;

        neighborNode = null;

        swapItem = null;

        board.CancelChoseItem();
    }

    public bool CheckHelpSwapable(SWAP_DIRECTION direction)
    {
        if (!Help.instance.gameObject.activeSelf)
        {
            return true;
        }
		if (LevelLoader.instance.level == 1) {
			if (Help.instance.step == 1) {
				if (node.OrderOnBoard () == 25 && direction == SWAP_DIRECTION.BOTTOM) {
					return true;
				} else if (node.OrderOnBoard () == 32 && direction == SWAP_DIRECTION.TOP) {
					return true;
				} else {
					return false;
				}
			}
			if (Help.instance.step == 2) {
				if (node.OrderOnBoard () == 13 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else if (node.OrderOnBoard () == 12 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else {
					return false;
				}
			}

		} else if (LevelLoader.instance.level == 2) {
			if (Help.instance.step == 1) {
				if (node.OrderOnBoard () == 13 && direction == SWAP_DIRECTION.BOTTOM) {
					return true;
				} else if (node.OrderOnBoard () == 21 && direction == SWAP_DIRECTION.TOP) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 2) {
				if (node.OrderOnBoard () == 21 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else if (node.OrderOnBoard () == 22 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 3) {
				if (node.OrderOnBoard () == 41 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else if (node.OrderOnBoard () == 42 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 4) {
				if (node.OrderOnBoard () == 49 && direction == SWAP_DIRECTION.SELFCLICK) {
					return true;
				} else {
					return false;
				}
			}
		} else if (LevelLoader.instance.level == 3) {
			if (Help.instance.step == 1) {
				if (node.OrderOnBoard () == 32 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else if (node.OrderOnBoard () == 31 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 2) {
				if (node.OrderOnBoard () == 31 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else if (node.OrderOnBoard () == 30 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 3) {
				if (node.OrderOnBoard () == 25 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else if (node.OrderOnBoard () == 24 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 4) {
				if (node.OrderOnBoard () == 24 && direction == SWAP_DIRECTION.SELFCLICK) {
					return true;
				} else {
					return false;
				}
			}
		} else if (LevelLoader.instance.level == 4) {
			if (Help.instance.step == 1) {
				if (node.OrderOnBoard () == 26 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else if (node.OrderOnBoard () == 25 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 2) {
				if (node.OrderOnBoard () == 26 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else if (node.OrderOnBoard () == 25 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 3) {
				if (node.OrderOnBoard () == 66 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else if (node.OrderOnBoard () == 65 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 4) {
				if (node.OrderOnBoard () == 65 && direction == SWAP_DIRECTION.SELFCLICK) {
					return true;
				} else {
					return false;
				}
			}
		} else if (LevelLoader.instance.level == 5) {
			if (Help.instance.step == 1) {
				if (node.OrderOnBoard () == 40 && direction == SWAP_DIRECTION.BOTTOM) {
					return true;
				} else if (node.OrderOnBoard () == 49 && direction == SWAP_DIRECTION.TOP) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 2) {
				if (node.OrderOnBoard () == 49 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else if (node.OrderOnBoard () == 50 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else {
					return false;
				}
			}
		} else if (LevelLoader.instance.level == 6) {
			if (Help.instance.step == 1) {
				if (node.OrderOnBoard () == 37 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else if (node.OrderOnBoard () == 38 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else {
					return false;
				}
			}
		} else if (LevelLoader.instance.level == 7) {
			if (Help.instance.step == 1) {
				if (node.OrderOnBoard () == 1 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else if (node.OrderOnBoard () == 2 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 2) {
				if (node.OrderOnBoard () == 34 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else if (node.OrderOnBoard () == 35 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 3) {
				if (node.OrderOnBoard () == 67 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else if (node.OrderOnBoard () == 68 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else {
					return false;
				}
			} else if (Help.instance.step == 4) {
				if (node.OrderOnBoard () == 67 && direction == SWAP_DIRECTION.BOTTOM) {
					return true;
				} else if (node.OrderOnBoard () == 78 && direction == SWAP_DIRECTION.TOP) {
					return true;
				} else {
					return false;
				}
			}
		} else if (LevelLoader.instance.level == 8) {
			if (Help.instance.step == 1) {
				if (node.OrderOnBoard () == 13 && direction == SWAP_DIRECTION.RIGHT) {
					return true;
				} else if (node.OrderOnBoard () == 14 && direction == SWAP_DIRECTION.LEFT) {
					return true;
				} else {
					return false;
				}
			}
		}
        else if (LevelLoader.instance.level == 12)
        {
            if (Help.instance.step == 1)
            {
                if (node.OrderOnBoard() == 13 && direction == SWAP_DIRECTION.BOTTOM)
                {
                    return true;
                }
                else if (node.OrderOnBoard() == 22 && direction == SWAP_DIRECTION.TOP)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else if (LevelLoader.instance.level == 18)
        {
            if (Help.instance.step == 1)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region ColorAndAppear

    // after the board is generate we need to alter the color to make sure there is no "pre-matches" on the board
    public void GenerateColor(int except)
    {
        var colors = new List<int>();

        var usingColors = LevelLoader.instance.usingColors;

        for (int i = 0; i < usingColors.Count; i++)
        {
            int color = usingColors[i];

            bool generatable = true;
            Node neighbor = null;

            neighbor = node.TopNeighbor();
            if (neighbor != null)
            {
                if (neighbor.item != null)
                {
                    if (neighbor.item.color == color)
                    {
                        generatable = false;
                    }
                }
            }

            neighbor = node.LeftNeighbor();
            if (neighbor != null)
            {
                if (neighbor.item != null)
                {
                    if (neighbor.item.color == color)
                    {
                        generatable = false;
                    }
                }
            }

            neighbor = node.RightNeighbor();
            if (neighbor != null)
            {
                if (neighbor.item != null)
                {
                    if (neighbor.item.color == color)
                    {
                        generatable = false;
                    }
                }
            }

            if (generatable && color != except)
            {
                colors.Add(color);
            }
        } // end for

        // by default index is a random color
        int index = usingColors[Random.Range(0, usingColors.Count)];

        // if there is generatable colors then change index
        if (colors.Count > 0)
        {
            index = colors[Random.Range(0, colors.Count)];
        }

        // if the random in colors list is a except color then change the index
        if (index == except)
        {
            index = (index++) % usingColors.Count;
        }

        this.color = index;

        ChangeSpriteAndType(index);
    }

    public void ChangeSpriteAndType(int itemColor)
    {
        GameObject prefab = null;

        switch (itemColor)
        {
            case 1:
                prefab = Resources.Load(Configure.Cookie1()) as GameObject;
                type = ITEM_TYPE.COOKIE_1;
                break;
            case 2:
                prefab = Resources.Load(Configure.Cookie2()) as GameObject;
                type = ITEM_TYPE.COOKIE_2;
                break;
            case 3:
                prefab = Resources.Load(Configure.Cookie3()) as GameObject;
                type = ITEM_TYPE.COOKIE_3;
                break;
            case 4:
                prefab = Resources.Load(Configure.Cookie4()) as GameObject;
                type = ITEM_TYPE.COOKIE_4;
                break;
            case 5:
                prefab = Resources.Load(Configure.Cookie5()) as GameObject;
                type = ITEM_TYPE.COOKIE_5;
                break;
            case 6:
                prefab = Resources.Load(Configure.Cookie6()) as GameObject;
                type = ITEM_TYPE.COOKIE_6;
                break;
        }

        if (prefab != null)
        {
            GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void ChangeToRainbow()
    {
        var prefab = Resources.Load(Configure.CookieRainbow()) as GameObject;
        
        type = ITEM_TYPE.COOKIE_RAINBOW;
        
        color = 0;
        
        GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
    }

    public void ChangeToGingerbread(ITEM_TYPE check)
    {
        if (node.item.IsGingerbread())
        {
            return;
        }

        var upper = board.GetUpperItem(this.node);

        if (upper != null && upper.IsGingerbread())
        {
            return;
        }

        AudioManager.instance.GingerbreadAudio();

        GameObject explosion = null;

        switch (node.item.type)
        {
            case ITEM_TYPE.COOKIE_1:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BlueCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_2:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.GreenCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_3:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.OrangeCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_4:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.PurpleCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_5:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RedCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_6:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.YellowCookieExplosion()) as GameObject);
                break;
        }

        if (explosion != null) explosion.transform.position = node.item.transform.position + Vector3.back * 2;

        GameObject prefab = null;

        switch (check)
        {
            case ITEM_TYPE.GINGERBREAD_1:
                prefab = Resources.Load(Configure.Gingerbread1()) as GameObject;
                check = ITEM_TYPE.GINGERBREAD_1;
                color = 1;
                break;
            case ITEM_TYPE.GINGERBREAD_2:
                prefab = Resources.Load(Configure.Gingerbread2()) as GameObject;
                check = ITEM_TYPE.GINGERBREAD_2;
                color = 2;
                break;
            case ITEM_TYPE.GINGERBREAD_3:
                prefab = Resources.Load(Configure.Gingerbread3()) as GameObject;
                check = ITEM_TYPE.GINGERBREAD_3;
                color = 3;
                break;
            case ITEM_TYPE.GINGERBREAD_4:
                prefab = Resources.Load(Configure.Gingerbread4()) as GameObject;
                check = ITEM_TYPE.GINGERBREAD_4;
                color = 4;
                break;
            case ITEM_TYPE.GINGERBREAD_5:
                prefab = Resources.Load(Configure.Gingerbread5()) as GameObject;
                check = ITEM_TYPE.GINGERBREAD_5;
                color = 5;
                break;
            case ITEM_TYPE.GINGERBREAD_6:
                prefab = Resources.Load(Configure.Gingerbread6()) as GameObject;
                check = ITEM_TYPE.GINGERBREAD_6;
                color = 6;
                break;
        }

        if (prefab != null)
        {
            type = check;
            effect = BREAKER_EFFECT.NORMAL;

            GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void ChangeToBombBreaker()
    {
        GameObject prefab = null;

        prefab = Resources.Load(Configure.Cookie1BombBreaker()) as GameObject;
        type = ITEM_TYPE.COOKIE_BOMB_BREAKER;

        if (prefab != null)
        {
            GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void ChangeToColRowBreaker()
    {
        GameObject prefab = null;

        if (Random.Range(0, 2) == 0)
        {
            prefab = Resources.Load(Configure.Cookie1ColumnBreaker()) as GameObject;
            type = ITEM_TYPE.COOKIE_COLUMN_BREAKER;
        }
        else
        {
            prefab = Resources.Load(Configure.Cookie1RowBreaker()) as GameObject;
            type = ITEM_TYPE.COOKIE_ROW_BREAKER;
        }

        if (prefab != null)
        {
            GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void ChangeToColBreaker()
    {
        GameObject prefab = null;
        prefab = Resources.Load(Configure.Cookie1ColumnBreaker()) as GameObject;
        type = ITEM_TYPE.COOKIE_COLUMN_BREAKER;

        if (prefab != null)
        {
            GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void ChangeToRowBreaker()
    {
        GameObject prefab = null;
        prefab = Resources.Load(Configure.Cookie1RowBreaker()) as GameObject;
        type = ITEM_TYPE.COOKIE_ROW_BREAKER;

        if (prefab != null)
        {
            GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void ChangeToPlaneBreaker()
    {
        GameObject prefab = null;

        prefab = Resources.Load(Configure.PlaneBreaker()) as GameObject;
        type = ITEM_TYPE.COOKIE_PLANE_BREAKER;

        if (prefab != null)
        {
            GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void SetRandomNextType()
    {
        var random = Random.Range(0, 4);

        if (random == 0)
        {
            next = ITEM_TYPE.COOKIE_COLUMN_BREAKER;
        }
        else if (random == 1)
        {
            next = ITEM_TYPE.COOKIE_ROW_BREAKER;
        }
        else if (random == 2)
        {
            next = ITEM_TYPE.COOKIE_BOMB_BREAKER;
        }
        else if (random == 3)
        {
            next = ITEM_TYPE.COOKIE_PLANE_BREAKER;
        }
    }

    #endregion

    #region Destroy

    public void Destroy(bool forced = false, bool isPlaneChangeExplode = false)
    {
        if (Destroyable() == false && forced == false)
        {
            return;
        }

        // prevent multiple calls
        if (destroying) return;
        else destroying = true;

        beAbleToDestroy--;

        if (board.state == GAME_STATE.PRE_WIN_AUTO_PLAYING)
        {
            board.WinGoldReward(this);
        }

        if (!isPlaneChangeExplode && node != null && node.cage != null)
        {
            // destroy and check collect cage
            node.CageExplode();
            return;
        }
        if (!isPlaneChangeExplode && node != null && node.ice != null)
        {
            // destroy and check collect ice
            node.IceExplode();
            return;
        }
        if (!isPlaneChangeExplode && node != null && node.jelly != null)
        {
            // destroy and check collect jelly
            if (node.JellyExplode())
            {
                return;
            }
        }

        if (!isPlaneChangeExplode && node != null && node.packagebox != null)
        {
            // destroy packagebox
            node.PackageBoxExplode();
            return;
        }

        board.destroyingItems++;

        // destroy animation
        iTween.ScaleTo(gameObject, iTween.Hash(
            "scale", new Vector3(1.8f,1.8f,1f),
            "onstart", "OnStartDestroy",
            "oncomplete", "OnCompleteDestroy",
            "oncompleteparams", new Hashtable() { { "isPlaneChangeExplode",isPlaneChangeExplode } },
            "easetype", iTween.EaseType.linear,
            "time", Configure.instance.destroyTime
        ));
    }

    public void OnStartDestroy()
    {
        // destroy and check collect waffle
        if (node != null) node.WaffleExplode();

        // check collect
        board.CollectItem(this);

        // destroy neighbor marshmallow/chocolate/rock candy/jelly
        if (type != ITEM_TYPE.BLANK)
        {
            board.DestroyNeighborItems(this);
        }
        // explosion effect
        if (effect == BREAKER_EFFECT.BOMB_ROWCOL_BREAKER)
        {
            BombColRowBreakerExplosion();
        }
        else if (effect == BREAKER_EFFECT.CROSS)
        {
            CrossBreakerExplosion();
        }
        else if (effect == BREAKER_EFFECT.BOMB_X_BREAKER)
        {
            BombXBreakerExplosion();
        }
        else if (effect == BREAKER_EFFECT.CROSS_X_BREAKER)
        {
            CrossXBreakerExplosion();
        }
        else if (effect == BREAKER_EFFECT.BIG_BOMB_BREAKER)
        {
            BigBombBreakerExplosion();
        }
        else if (effect == BREAKER_EFFECT.BIG_PLANE_BREAKER)
        {
            BigPlaneBreakerExplosion();
        }
        else if (effect == BREAKER_EFFECT.PLANE_CHANGE_BREAKER)
        {
            PlaneChangeBreakerExplosion();
        }
        else if (effect == BREAKER_EFFECT.NORMAL)
        {
			if (IsCookie ())
            {
				CookieExplosion ();
			}
            else if (IsGingerbread ())
            {
				GingerbreadExplosion ();
			}
            else if (IsMarshmallow ())
            {
				MarshmallowExplosion ();
			}
            else if (IsChocolate ())
            {
				ChocolateExplosion ();
			}
            else if (IsRockCandy ())
            {
				RockCandyExplosion ();
			}
            else if (IsCollectible ())
            {
				CollectibleExplosion ();
			}
            else if (IsBombBreaker (type))
            {
				BombBreakerExplosion ();
			}
            else if (type == ITEM_TYPE.COOKIE_RAINBOW)
            {
				RainbowExplosion ();
			}
            else if (IsColumnBreaker (type))
            {
				ColumnBreakerExplosion ();
			}
            else if (IsRowBreaker (type))
            {
				RowBreakerExplosion ();
			}
            else if (IsPlaneBreaker (type))
            {
				PlaneBreakerExplosion ();
			}
            else if (IsAppleBox ())
            {
				AppleBoxExplosion ();
			}
        }
    }

    public void OnCompleteDestroy( Hashtable param)
    {
        bool isPlaneChangeExplode = (bool) param["isPlaneChangeExplode"];

        if (board.state == GAME_STATE.PRE_WIN_AUTO_PLAYING)
        {
            board.score += Configure.instance.finishedScoreItem * board.dropTime;
        }
        else
        {
            board.score += Configure.instance.scoreItem * board.dropTime;
        }

        board.UITop.UpdateProgressBar(board.score);

        board.UITop.UpdateScoreAmount(board.score);


        if (next != ITEM_TYPE.NONE)
        {
            //print("next type: " + next);

            if (IsBombBreaker(next))
            {
                if (nextSound) AudioManager.instance.BombBreakerAudio();
            }
            else if (IsRowBreaker(next) || IsColumnBreaker(next))
            {
                if (nextSound) AudioManager.instance.ColRowBreakerAudio();
            }
            else if (next == ITEM_TYPE.COOKIE_RAINBOW)
            {
                if (nextSound) AudioManager.instance.RainbowAudio();
            }

            // generate a item at position of the node
            node.GenerateItem(next);
        }
        else if (type == ITEM_TYPE.CHOCOLATE_2_LAYER)
        {
            // generate a new chocolate
            node.GenerateItem(ITEM_TYPE.CHOCOLATE_1_LAYER);

            // set position
            board.GetNode(node.i, node.j).item.gameObject.transform.localPosition = board.NodeLocalPosition(node.i, node.j); ;
        }
        else if (type == ITEM_TYPE.CHOCOLATE_3_LAYER)
        {
            node.GenerateItem(ITEM_TYPE.CHOCOLATE_2_LAYER);

            board.GetNode(node.i, node.j).item.gameObject.transform.localPosition = board.NodeLocalPosition(node.i, node.j); ;
        }
        else if (type == ITEM_TYPE.CHOCOLATE_4_LAYER)
        {
            node.GenerateItem(ITEM_TYPE.CHOCOLATE_3_LAYER);

            board.GetNode(node.i, node.j).item.gameObject.transform.localPosition = board.NodeLocalPosition(node.i, node.j);
        }
        else if (type == ITEM_TYPE.APPLEBOX)
        {
            applebox.TryToDestroyApple(this);
            return;
        }
        else if(!isPlaneChangeExplode)
        {
            node.GenerateItem(ITEM_TYPE.BLANK);
        }

        if (destroying)
        {
            board.destroyingItems--;

            // there is a case when a item is dropping and it is destroyed by other call
            if (dropping) board.droppingItems--;


            GameObject.Destroy(gameObject);
        }
    }

    public IEnumerator ResetDestroying()
    {
        yield return new WaitForSeconds(Configure.instance.destroyTime);

        destroying = false;
    }

    #endregion

    #region Explosion

    void CookieExplosion()
    {
        AudioManager.instance.CookieCrushAudio();

        GameObject explosion = null;

        switch (type)
        {
            case ITEM_TYPE.COOKIE_1:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BlueCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_2:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.GreenCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_3:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.OrangeCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_4:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.PurpleCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_5:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RedCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.COOKIE_6:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.YellowCookieExplosion()) as GameObject);
                break;
        }

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }
    }

    void GingerbreadExplosion()
    {
        AudioManager.instance.GingerbreadExplodeAudio();

        GameObject explosion = null;

        switch (type)
        {
            case ITEM_TYPE.GINGERBREAD_1:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BlueCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.GINGERBREAD_2:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.GreenCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.GINGERBREAD_3:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.OrangeCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.GINGERBREAD_4:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.PurpleCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.GINGERBREAD_5:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RedCookieExplosion()) as GameObject);
                break;
            case ITEM_TYPE.GINGERBREAD_6:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.YellowCookieExplosion()) as GameObject);
                break;
        }

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }
    }

    void MarshmallowExplosion()
    {
        AudioManager.instance.MarshmallowExplodeAudio();

        GameObject explosion = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.MarshmallowExplosion()) as GameObject);

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }
    }

    void AppleBoxExplosion()
    {
        AudioManager.instance.MarshmallowExplodeAudio();

        GameObject explosion = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.MarshmallowExplosion()) as GameObject);

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }
    }

    public void ChocolateExplosion()
    {
        AudioManager.instance.ChocolateExplodeAudio();

        GameObject explosion = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ChocolateExplosion()) as GameObject);

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }
    }

    public void RockCandyExplosion()
    {
        AudioManager.instance.RockCandyExplodeAudio();

        GameObject explosion = null;

        switch (type)
        {
            case ITEM_TYPE.ROCK_CANDY:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BlueCookieExplosion()) as GameObject);
                break;
        }

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }
    }

    void CollectibleExplosion()
    {
        AudioManager.instance.CollectibleExplodeAudio();
    }

    void BombBreakerExplosion()
    {
        AudioManager.instance.BombExplodeAudio();

        BombBreakerDestroy(2);

        GameObject explosion = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BreakerExplosion1()) as GameObject);

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
            //explosion.transform.position = new Vector3(explosion.transform.position.x, explosion.transform.position.y, -12f);
        }
    }

    void RainbowExplosion()
    {
        AudioManager.instance.RainbowExplodeAudio();

        DestroyItemsSameColor(LevelLoader.instance.RandomColor());


        GameObject explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RainbowExplosion()) as GameObject);

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }
    }

    void XBreakerExplosion()
    {
        AudioManager.instance.ColRowBreakerExplodeAudio();

        XBreakerDestroy();

        GameObject explosion = null;
        GameObject animation = null;
        GameObject cross = null;

        switch (GetCookie(type))
        {
            case ITEM_TYPE.COOKIE_1:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker1()) as GameObject);
                animation = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation1()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_2:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker2()) as GameObject);
                animation = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation2()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_3:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker3()) as GameObject);
                animation = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation3()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_4:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker4()) as GameObject);
                animation = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation4()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_5:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker5()) as GameObject);
                animation = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation5()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_6:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker6()) as GameObject);
                animation = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation6()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
        }

        if (animation != null)
        {
            cross = Instantiate(animation);
            animation.transform.Rotate(Vector3.back, 45);
            animation.transform.position = new Vector3(animation.transform.position.x, animation.transform.position.y, -12f);
        }

        if (cross != null)
        {
            cross.transform.Rotate(Vector3.back, -45);
            cross.transform.position = new Vector3(cross.transform.position.x, cross.transform.position.y, -12f);
        }

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }

        GameObject.Destroy(animation, 1f);
    }

    void ColumnBreakerExplosion()
    {
        AudioManager.instance.ColRowBreakerExplodeAudio();

        ColumnDestroy();

        GameObject explosion = null;
        GameObject animation = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker1()) as GameObject);
        animation = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation1()) as GameObject, transform.position, Quaternion.identity) as GameObject;

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }

        if (animation != null)
        {
            animation.transform.position = transform.position + Vector3.back;
        }

        GameObject.Destroy(animation, 1f);
    }

    void BombColRowBreakerExplosion()
    {
        AudioManager.instance.ColRowBreakerExplodeAudio();
        bool isSpecialForGrass = false;
        if (node.IsGrass())
        {
            isSpecialForGrass = true;
        }
        ColumnDestroy(node.j - 1, isSpecialForGrass);
        ColumnDestroy(node.j, isSpecialForGrass);
        ColumnDestroy(node.j + 1, isSpecialForGrass);

        RowDestroy(node.i - 1, isSpecialForGrass);
        RowDestroy(node.i, isSpecialForGrass);
        RowDestroy(node.i + 1, isSpecialForGrass);

        GameObject explosion = null;
        GameObject animation = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker1()) as GameObject);
        animation = Instantiate(Resources.Load(Configure.BigColumnBreakerAnimation1()) as GameObject, transform.position, Quaternion.identity) as GameObject;


        if (explosion != null)
        {
            explosion.transform.position = transform.position + transform.position + Vector3.back * 2;
        }

        if (animation != null)
        {
            animation.transform.position = transform.position + Vector3.back;
        }

        GameObject.Destroy(animation, 1f);
    }

    void CrossBreakerExplosion()
    {
        AudioManager.instance.ColRowBreakerExplodeAudio();

        ColumnDestroy();
        RowDestroy();

        GameObject explosion = null;
        GameObject columnEffect = null;
        GameObject rowEffect = null;

        switch (GetCookie(type))
        {
            case ITEM_TYPE.COOKIE_1:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker1()) as GameObject);
                columnEffect = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation1()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_2:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker2()) as GameObject);
                columnEffect = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation2()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_3:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker3()) as GameObject);
                columnEffect = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation3()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_4:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker4()) as GameObject);
                columnEffect = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation4()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_5:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker5()) as GameObject);
                columnEffect = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation5()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_6:
                explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker6()) as GameObject);
                columnEffect = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation6()) as GameObject, transform.position, Quaternion.identity) as GameObject;
                break;
        }

        if (columnEffect != null)
        {
            rowEffect = Instantiate(columnEffect as GameObject, transform.position, Quaternion.identity) as GameObject;
            columnEffect.transform.position = new Vector3(columnEffect.transform.position.x, columnEffect.transform.position.y, -12f);
        }

        if (rowEffect != null)
        {
            rowEffect.transform.Rotate(Vector3.back, 90);
            rowEffect.transform.position = new Vector3(rowEffect.transform.position.x, rowEffect.transform.position.y, -12f);
        }

        if (explosion != null)
        {
            explosion.transform.position = transform.position + transform.position + Vector3.back * 2;
        }

        GameObject.Destroy(rowEffect, 1f);

        GameObject.Destroy(columnEffect, 1f);
    }

    void BigBombBreakerExplosion()
    {
        AudioManager.instance.BombExplodeAudio();

        BombBreakerDestroy(3);

        GameObject explosion = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BreakerExplosion1()) as GameObject);

        if (explosion != null)
        {
            explosion.transform.position = transform.position + transform.position + Vector3.back * 2;
           // explosion.transform.position = new Vector3(explosion.transform.position.x, explosion.transform.position.y, -12f);
        }
    }

    void BigPlaneBreakerExplosion()
    {
        AudioManager.instance.BombExplodeAudio();

        PlaneDestroy(3);

        GameObject explosion = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BreakerExplosion1()) as GameObject);

        if (explosion != null)
        {
            explosion.transform.position = transform.position + transform.position + Vector3.back * 2;
            //explosion.transform.position = new Vector3(explosion.transform.position.x, explosion.transform.position.y, -12f);
        }
    }

    void PlaneChangeBreakerExplosion()
    {
        AudioManager.instance.BombExplodeAudio();

        PlaneDestroy();

        GameObject explosion = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BreakerExplosion1()) as GameObject);

        if (explosion != null)
        {
            explosion.transform.position = transform.position + transform.position + Vector3.back * 2;
            //explosion.transform.position = new Vector3(explosion.transform.position.x, explosion.transform.position.y, -12f);
        }
    }

    void RowBreakerExplosion()
    {
        AudioManager.instance.ColRowBreakerExplodeAudio();

        RowDestroy();

        GameObject explosion = null;
        GameObject animation = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker1()) as GameObject);
        animation = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation1()) as GameObject, transform.position, Quaternion.identity) as GameObject;

        if (animation != null)
        {
            animation.transform.Rotate(Vector3.back, 90);
            animation.transform.position = transform.position + Vector3.back;
        }

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
            explosion.transform.SetParent(node.transform);
        }

        GameObject.Destroy(animation, 1f);
    }

    void PlaneBreakerExplosion()
    {
        AudioManager.instance.ColRowBreakerExplodeAudio();
        PlaneDestroy();
        GameObject explosion = null;
      //  GameObject animation = null;

        explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.ColRowBreaker1()) as GameObject);
       // animation = Instantiate(Resources.Load(Configure.ColumnBreakerAnimation1()) as GameObject, transform.position, Quaternion.identity) as GameObject;

       /* if (animation != null)
        {
            animation.transform.Rotate(Vector3.back, 90);
            animation.transform.position = new Vector3(animation.transform.position.x, animation.transform.position.y, -12f);
        }*/

        if (explosion != null)
        {
            explosion.transform.position = transform.position + Vector3.back * 2;
        }

       // GameObject.Destroy(animation, 1f);
    }

    void BombXBreakerExplosion()
    {
        BombBreakerExplosion();

        XBreakerExplosion();
    }

    void CrossXBreakerExplosion()
    {
        CrossBreakerExplosion();

        XBreakerExplosion();
    }

    #endregion

    #region SpecialDestroy

    void BombBreakerDestroy(int range)
    {
        List<Item> items = board.ItemAround(node,range);

        var isgrass = false;
        if (node.IsGrass())
        {
            isgrass = true;
        }


        foreach (var item in items)
        {
            if (item != null)
            {
                if (isgrass)
                {
                    item.node.ChangeToGrass();
                }

                if (item.node.bafflebottom != null)
                {
                    item.node.bafflebottom.DestroyBaffle();
                }
                if(item.node.baffleright != null)
                {
                    item.node.baffleright.DestroyBaffle();
                }
                item.Destroy();
            }
        }
    }

    void XBreakerDestroy()
    {
        List<Item> items = board.XCrossItems(node);

        foreach (var item in items)
        {
            if (item != null)
            {
                item.Destroy();
            }
        }
    }

    // destroy all items with the same color (when this color swap with a rainbow)
    public void DestroyItemsSameColor(int color)
    {
        List<Item> items = board.GetListItems();

        var isgrass = false;
        if (node.IsGrass())
        {
            isgrass = true;
        }

        foreach (Item item in items)
        {
            if (item != null)
            {
                if (item.color == color && item.Matchable())
                {
                    board.sameColorList.Add(item);
                }
            }
        }

        board.DestroySameColorList(isgrass);
    }

    // Rainbow swap with other item
    public void RainbowDestroy(Item thisItem, Item otherItem)
    {
        if (thisItem.Destroyable() == false || otherItem.Destroyable() == false)
        {
            return;
        }
        var isgrass = false;
        if (node.IsGrass())
        {
            isgrass = true;
        }


        if (thisItem.type == ITEM_TYPE.COOKIE_RAINBOW)
        {
            //Debug.Log("touched item is rainbow");

            if (otherItem.IsCookie())
            {
                thisItem.DestroyItemsSameColor(otherItem.color);
                thisItem.type = ITEM_TYPE.NONE;

                thisItem.Destroy();
            }
            else if (otherItem.IsBombBreaker(otherItem.type) || otherItem.IsRowBreaker(otherItem.type) || otherItem.IsColumnBreaker(otherItem.type) || otherItem.IsPlaneBreaker(otherItem.type))
            {
                //board.
                var mostColor = board.GetMostColor();
                board.ChangeItemsType(mostColor, otherItem.type, thisItem.node.IsGrass());

                thisItem.type = ITEM_TYPE.NONE;
                otherItem.type = ITEM_TYPE.NONE;

                thisItem.Destroy();
                otherItem.Destroy();
            }
            else if (otherItem.type == ITEM_TYPE.COOKIE_RAINBOW)
            {
                board.DoubleRainbowDestroy(isgrass);

                thisItem.type = ITEM_TYPE.NONE;
                otherItem.type = ITEM_TYPE.NONE;

                thisItem.Destroy();
                otherItem.Destroy();
            }
        }
        else if (otherItem.type == ITEM_TYPE.COOKIE_RAINBOW)
        {
            //Debug.Log("swap item is rainbow");

            if (thisItem.IsCookie())
            {
                otherItem.DestroyItemsSameColor(thisItem.color);
                otherItem.type = ITEM_TYPE.NONE;

                otherItem.Destroy();
            }
            else if (thisItem.IsBombBreaker(thisItem.type) || thisItem.IsRowBreaker(thisItem.type) || thisItem.IsColumnBreaker(thisItem.type) || otherItem.IsPlaneBreaker(thisItem.type))
            {
                var mostColor = board.GetMostColor();

                board.ChangeItemsType(mostColor, thisItem.type, otherItem.node.IsGrass());

                thisItem.type = ITEM_TYPE.NONE;
                otherItem.type = ITEM_TYPE.NONE;

                otherItem.Destroy();
                thisItem.Destroy();
            }
            else if (thisItem.type == ITEM_TYPE.COOKIE_RAINBOW)
            {
                //                thisItem.type = ITEM_TYPE.NONE;
                //                otherItem.type = ITEM_TYPE.NONE;
                board.DoubleRainbowDestroy(isgrass);

                thisItem.type = ITEM_TYPE.NONE;
                otherItem.type = ITEM_TYPE.NONE;

                thisItem.Destroy();
                otherItem.Destroy();
            }
        }
    }

    void ColumnDestroy(int col = -1, bool isspecialforgrass = false)
    {
        var nodes = new List<Node>();

        if (col == -1)
        {
            nodes = board.ColumnNodes(node.j);
        }
        else
        {
            nodes = board.ColumnNodes(col);
        }

        var upNodes = new List<Node>();

        var downNodes = new List<Node>();

        for (int i = node.i; i >= 0; i--)
        {
            upNodes.Add(nodes[i]);
        }
        var isGrass = false;
        var findMarshmallow = false;
        foreach (var node in upNodes)
        {
            if (findMarshmallow)
            {
                break;
            }
            if (node != null)
            {
                if (node.IsGrass(isspecialforgrass))
                {
                    isGrass = true;
                }
                if (isGrass)
                {
                    node.ChangeToGrass();
                }
                if (node.bafflebottom != null)
                {
                    node.bafflebottom.DestroyBaffle();
                }
                if (node.item != null)
                {
                    if (node.item.type == ITEM_TYPE.MARSHMALLOW)
                    {
                        findMarshmallow = true;
                    }
                    node.item.Destroy();
                }
            }
        }

        for (int i = node.i; i < nodes.Count; i++)
        {
            downNodes.Add(nodes[i]);
        }
        isGrass = false;
        findMarshmallow = false;
        foreach (var node in downNodes)
        {
            if (findMarshmallow)
            {
                break;
            }
            if (node != null)
            {
                if (node.IsGrass(isspecialforgrass))
                {
                    isGrass = true;
                }
                if (isGrass)
                {
                    node.ChangeToGrass();
                }

                if (node.TopNeighbor() != null && node.TopNeighbor().bafflebottom != null)
                {
                    node.TopNeighbor().bafflebottom.DestroyBaffle();
                }

                if (node.item != null)
                {
                    if (node.item.type == ITEM_TYPE.MARSHMALLOW)
                    {
                        findMarshmallow = true;
                    }
                    node.item.Destroy();
                }
            }
        }
    }

    public void RowDestroy(int row = -1,bool isspecialforgrass = false)
    {
        var nodes = new List<Node>();

        if (row == -1)
        {
            nodes = board.RowNodes(node.i);
        }
        else
        {
            nodes = board.RowNodes(row);
        }

        var leftNodes = new List<Node>();
        var rightNodes = new List<Node>();

        for (int i = node.j; i >= 0; i--)
        {
            leftNodes.Add(nodes[i]);
        }
        var isGrass = false;
        var findMarshmallow = false;
        foreach (var node in leftNodes)
        {
            if (findMarshmallow)
            {
                break;
            }
            if (node != null)
            {
                if (node.IsGrass(isspecialforgrass))
                {
                    isGrass = true;
                }
                if (isGrass)
                {
                    node.ChangeToGrass();
                }

                if (node.baffleright != null)
                {
                    node.baffleright.DestroyBaffle();
                }

                if (node.item != null)
                {
                    if (node.item.type == ITEM_TYPE.MARSHMALLOW)
                    {
                        findMarshmallow = true;
                    }
                    node.item.Destroy();
                }

            }
        }

        for (int i = node.j; i < nodes.Count; i++)
        {
            rightNodes.Add(nodes[i]);
        }
        isGrass = false;
        findMarshmallow = false;
        foreach (var node in rightNodes)
        {

            if (findMarshmallow)
            {
                break;
            }

            if (node != null)
            {
                if (node.IsGrass(isspecialforgrass))
                {
                    isGrass = true;
                }
                if (isGrass)
                {
                    node.ChangeToGrass();
                }

                if (node.LeftNeighbor() != null && node.LeftNeighbor().baffleright != null)
                {
                    node.LeftNeighbor().baffleright.DestroyBaffle();
                }

                if (node.item != null)
                {
                    if (node.item.type == ITEM_TYPE.MARSHMALLOW)
                    {
                        findMarshmallow = true;
                    }

                    node.item.Destroy();
                }
            }
        }
    }

    public void PlaneSpecialDestroy(Item thisItem, Item otherItem)
    {


//        if (thisItem.Destroyable() == false || otherItem.Destroyable() == false)
//        {
//            return;
//        }

        if (thisItem.type == ITEM_TYPE.COOKIE_PLANE_BREAKER)
        {

            if (otherItem.IsCookie())
            {
                thisItem.Destroy();
                board.FindMatches();
            }
            else if (otherItem.IsBombBreaker(otherItem.type) || otherItem.IsRowBreaker(otherItem.type) || otherItem.IsColumnBreaker(otherItem.type))
            {
                //board.
                planeTargetType = otherItem.type;
                thisItem.effect = BREAKER_EFFECT.PLANE_CHANGE_BREAKER;

                otherItem.type = ITEM_TYPE.NONE;

                thisItem.Destroy();
                board.FindMatches();
            }
            else if (otherItem.IsPlaneBreaker(otherItem.type))
            {
                thisItem.effect = BREAKER_EFFECT.BIG_PLANE_BREAKER;
                otherItem.type = ITEM_TYPE.NONE;

                var isgrass = false;
                if (otherItem.node.IsGrass())
                {
                    isgrass = true;
                }

                if (thisItem.node.TopLeftNeighbor() != null && thisItem.node.TopLeftNeighbor().item != null)
                {
                    thisItem.node.TopLeftNeighbor().item.Destroy();
                }

                if (thisItem.node.TopRightNeighbor() != null && thisItem.node.TopRightNeighbor().item != null)
                {
                    thisItem.node.TopRightNeighbor().item.Destroy();
                }

                if (thisItem.node.BottomLeftNeighbor() != null && thisItem.node.BottomLeftNeighbor().item != null)
                {
                    thisItem.node.BottomLeftNeighbor().item.Destroy();
                }

                if (thisItem.node.BottomRightNeighbor() != null && thisItem.node.BottomRightNeighbor().item != null)
                {
                    thisItem.node.BottomRightNeighbor().item.Destroy();
                }


                otherItem.Destroy();
                thisItem.Destroy();
                board.FindMatches();
            }
            else 
            {
                thisItem.Destroy();
                board.FindMatches();
            }
        }
        else if (otherItem.type == ITEM_TYPE.COOKIE_PLANE_BREAKER)
        {
            //Debug.Log("swap item is rainbow");

            if (thisItem.IsCookie())
            {
                otherItem.Destroy();
                board.FindMatches();
            }
            else if (thisItem.IsBombBreaker(thisItem.type) || thisItem.IsRowBreaker(thisItem.type) || thisItem.IsColumnBreaker(thisItem.type))
            {
                planeTargetType = thisItem.type;
                thisItem.effect = BREAKER_EFFECT.PLANE_CHANGE_BREAKER;

                otherItem.type = ITEM_TYPE.NONE;

                thisItem.Destroy();
                board.FindMatches();
            }
            else if (thisItem.IsPlaneBreaker(thisItem.type))
            {
                thisItem.effect = BREAKER_EFFECT.BIG_PLANE_BREAKER;

                otherItem.type = ITEM_TYPE.NONE;


                var isgrass = false;
                if (otherItem.node.IsGrass())
                {
                    isgrass = true;
                }

                if (thisItem.node.TopLeftNeighbor() != null && thisItem.node.TopLeftNeighbor().item != null)
                {
                    thisItem.node.TopLeftNeighbor().item.Destroy();
                }

                if (thisItem.node.TopRightNeighbor() != null && thisItem.node.TopRightNeighbor().item != null)
                {
                    thisItem.node.TopRightNeighbor().item.Destroy();
                }

                if (thisItem.node.BottomLeftNeighbor() != null && thisItem.node.BottomLeftNeighbor().item != null)
                {
                    thisItem.node.BottomLeftNeighbor().item.Destroy();
                }

                if (thisItem.node.BottomRightNeighbor() != null && thisItem.node.BottomRightNeighbor().item != null)
                {
                    thisItem.node.BottomRightNeighbor().item.Destroy();
                }


                otherItem.Destroy();



                thisItem.Destroy();
                board.FindMatches();

            }
            else
            {
                otherItem.Destroy();
                board.FindMatches();
            }
        }
    }


    public void PlaneDestroy(int planeNum = 1)
    {
        //开始时使用了生成飞机增加
        planeNum += board.planePlusNum;

        var isgrass = false;
        if (node.IsGrass())
        {
            isgrass = true;
        }

        var items = board.ItemAround(node, 1);
        foreach (var item in items)
        {
            // this item maybe destroyed in other call
            if (item != null)
            {
                if (isgrass)
                {
                    item.node.ChangeToGrass();
                }
                //item.beAbleToDestroy--;
                item.Destroy();
                //board.FindMatches();
            }
        }

        if (node.TopNeighbor() != null && node.TopNeighbor().bafflebottom != null)
        {
            node.TopNeighbor().bafflebottom.DestroyBaffle();
        }
        if (node.LeftNeighbor() != null && node.LeftNeighbor().baffleright != null)
        {
            node.LeftNeighbor().baffleright.DestroyBaffle();
        }
        if (node.bafflebottom != null)
        {
            node.bafflebottom.DestroyBaffle();
        }
        if (node.baffleright != null)
        {
            node.baffleright.DestroyBaffle();
        }


        var rdmItems = ChosePlaneTargets(planeNum);

        if (rdmItems.Count < planeNum)
        {
            if (board.state == GAME_STATE.WAITING_USER_SWAP)
            {
               // board.FindMatches();
            }
            return;
        }

        List<GameObject> gameObjectPlane = new List<GameObject>();

        for (int i = 0; i < planeNum; i++)
        {
            rdmItems[i].beAbleToDestroy--;

            gameObjectPlane.Add(Instantiate(Resources.Load(Configure.PlaneBreaker())) as GameObject);
            gameObjectPlane[i].transform.position = gameObject.transform.position;//+ Vector3.back;
            gameObjectPlane[i].GetComponent<Item>().planeTargetType = planeTargetType;
            gameObjectPlane[i].GetComponent<Item>().node = rdmItems[i].node;
            gameObjectPlane[i].GetComponent<Item>().board = board;

            board.playingAnimation++;

            //var angle = Vector3.Angle(gameObjectPlane[i].transform.position, rdmItems[i].transform.position);
            var vect = rdmItems[i].transform.position - gameObjectPlane[i].transform.position;
            float angle = 0;
            if (Vector3.Cross(new Vector3(-1, 1.732f, 0), vect).z > 0)
            {
                angle = Vector3.Angle(new Vector3(-1, 1.732f, 0), vect);
            }
            else
            {
                angle = 360 - Vector3.Angle(new Vector3(-1, 1.732f, 0), vect);
            }

            Debug.Log("angle " + angle);

            iTween.RotateTo(gameObjectPlane[i], iTween.Hash(
                "rotation", new Vector3(0, 0, angle),
                "easeType", "linear",
                "time", 0.1f,
                "loolType", "none",
                "islocal", true
            ));

            iTween.MoveTo(gameObjectPlane[i], iTween.Hash(
                "position", rdmItems[i].transform.position,
               // "looktarget", rdmItems[i].transform.position,
                "oncomplete", "onPlaneComplete",
                //"orienttopath", true,
                "oncompleteparams", new Hashtable() { { "targetItem", rdmItems[i] },{"isGrass", isgrass} },
                "easetype", iTween.EaseType.linear,
                "time", 0.5f
            ));



        }



    }

    private void onPlaneComplete(Hashtable param1)
    {

        var item = (Item)param1["targetItem"];

        var isgrass = (bool)param1["isGrass"];

        if (isgrass)
        {
            node.ChangeToGrass();
        }

        if (planeTargetType != ITEM_TYPE.NONE)
        {
            ChangeToSpecialType(planeTargetType,isgrass);
        }
        else
        {
            //item.Destroy();
            board.DestroyPlaneTargetList(item, isgrass);
            //            board.FindMatches();
            GameObject.Destroy(gameObject);
        }
        board.playingAnimation--;

    }

    private List<Item> ChosePlaneTargets(int planeNum)
    {
        //todo:修改结构 提高效率
        List<Item> rdmItems = new List<Item>();

        var targetNumList = new List<int>(board.targetLeftList);

        for (int i = 0; i < planeNum; i++)
        {
            for (int j = 0; j < targetNumList.Count; j++)
            {
                if (targetNumList[j] > 0)
                {
                    FindPlaneTarget(LevelLoader.instance.targetList[j].Type, LevelLoader.instance.targetList[j].color, rdmItems);
                    targetNumList[j]--;
                }
            }
        }
        if(rdmItems.Count < planeNum)
        {
            for(int i = rdmItems.Count; i < planeNum; i++)
            {
                FindPlaneGeneraltarget(rdmItems);
            }
        }
        return rdmItems;
    }

    private void FindPlaneTarget(TARGET_TYPE targetType, int targetColor, List<Item> targetItems)
    {
        List<Item> allTargetItems = new List<Item>();

        for (int i = 0; i < LevelLoader.instance.row; i++)
        {
            for (int j = 0; j < LevelLoader.instance.column; j++)
            {
                Node node = board.GetNode(i, j);

                if (node != null && node.item != null && node.item.beAbleToDestroy > 0 && !allTargetItems.Contains(node.item) && !targetItems.Contains(node.item))
                {
                    Item item = node.item;

                    if (targetType == TARGET_TYPE.COOKIE)
                    {
                        if (item.IsCookie() && item.color == targetColor)
                        {
                            allTargetItems.Add(item);
                        }
                    }
                    else if (targetType == TARGET_TYPE.MARSHMALLOW)
                    {
                        if (item.IsMarshmallow())
                        {
                            allTargetItems.Add(item);
                        }
                    }
                    else if (targetType == TARGET_TYPE.COLLECTIBLE)
                    {
                        if (item.IsCollectible() && item.color == targetColor)
                        {
                            List<Item> obstacle = new List<Item>();
                            List<Item> targettmp = new List<Item>();

                            for (int k = i+1; k < LevelLoader.instance.row; k++)
                            {
                                if (board.GetNode(k, j) != null && board.GetNode(k, j).item != null && !board.GetNode(k, j).CanDropIn())
                                {
                                    obstacle.Add(board.GetNode(k, j).item);
                                }
                                else if (board.GetNode(k, j) != null && board.GetNode(k, j).item != null && !board.GetNode(k, j).isNodeBlank())
                                {
                                    targettmp.Add(board.GetNode(k, j).item);
                                }
                            }
                            if (obstacle.Count > 0)
                            {
                                allTargetItems.AddRange(obstacle);
                            }
                            else if (targettmp.Count > 0)
                            {
                                allTargetItems.AddRange(targettmp);
                            }
                        }
                    }
                    else if (targetType == TARGET_TYPE.CAGE)
                    {
                        if (node.cage != null)
                        {
                            allTargetItems.Add(item);
                        }
                    }
                    else if (targetType == TARGET_TYPE.ROCK_CANDY)
                    {
                        if (item.IsRockCandy())
                        {
                            allTargetItems.Add(item);
                        }
                    }
                    else if (targetType == TARGET_TYPE.GRASS)
                    {
                        if (node.grass == null)
                        {
                            allTargetItems.Add(item);
                        }
                    }
                    else if (targetType == TARGET_TYPE.CHERRY)
                    {
                        if (item.IsCherry())
                        {
                            allTargetItems.Add(item);
                        }
                    }
                    else if (targetType == TARGET_TYPE.PACKAGEBOX)
                    {
                        if (node.packagebox != null)
                        {
                            allTargetItems.Add(item);
                        }
                    }
                    else if (targetType == TARGET_TYPE.APPLEBOX)
                    {
                        if (node.item.IsAppleBox())
                        {
                            allTargetItems.Add(item);
                        }
                    }
                }
            }
        }

        if (allTargetItems.Count > 0)
        {
            targetItems.Add(allTargetItems[Random.Range(0, allTargetItems.Count)]);
        }

    }

    private void FindPlaneGeneraltarget(List<Item> targetItems)
    {
        List<Item> allSpecialTargetItems = new List<Item>();
        List<Item> allCookieTargetItems = new List<Item>();
        for (int i = 0; i < LevelLoader.instance.row; i++)
        {
            for (int j = 0; j < LevelLoader.instance.column; j++)
            {
                var node = board.GetNode(i, j);
                if (node != null && node.item != null && !targetItems.Contains(node.item) && node.item.beAbleToDestroy > 0)
                {
                    Item item = node.item;
                    if (item != this && item.IsBreaker(item.type) && !allSpecialTargetItems.Contains(item))
                    {
                        allSpecialTargetItems.Add(item);
                    }
                    if (item.IsCookie() && !allCookieTargetItems.Contains(item))
                    {
                        allCookieTargetItems.Add(item);
                    }
                }
            }
        }
        if (allSpecialTargetItems.Count > 0)
        {
            targetItems.Add(allSpecialTargetItems[Random.Range(0, allSpecialTargetItems.Count)]);
        }
        else if(allCookieTargetItems.Count > 0)
        {
            targetItems.Add(allCookieTargetItems[Random.Range(0, allCookieTargetItems.Count)]);
        }

    }

    void TwoColRowBreakerDestroy(Item thisItem, Item otherItem)
    {
        if (thisItem == null || otherItem == null)
        {
            return;
        }

        if ((IsRowBreaker(thisItem.type) || IsColumnBreaker(thisItem.type)) && (IsRowBreaker(otherItem.type) || IsColumnBreaker(otherItem.type)))
        {
            thisItem.effect = BREAKER_EFFECT.CROSS;            
            otherItem.effect = BREAKER_EFFECT.NONE;

            thisItem.Destroy();
            otherItem.Destroy();

            board.FindMatches();
        }
    }

    void TwoBombBreakerDestroy(Item thisItem, Item otherItem)
    {
        if (thisItem == null || otherItem == null)
        {
            return;
        }

        if (IsBombBreaker(thisItem.type) && IsBombBreaker(otherItem.type))
        {
            thisItem.effect = BREAKER_EFFECT.BIG_BOMB_BREAKER;

            otherItem.type = ITEM_TYPE.NONE;

            thisItem.Destroy();
            otherItem.Destroy();

            board.FindMatches();
        }
    }

    void ColRowBreakerAndBombBreakerDestroy(Item thisItem, Item otherItem)
    {
        if (thisItem == null || otherItem == null)
        {
            return;
        }

        if (
            (IsRowBreaker(thisItem.type) || IsColumnBreaker(thisItem.type)) && IsBombBreaker(otherItem.type)
            ||(IsRowBreaker(otherItem.type) || IsColumnBreaker(otherItem.type)) && IsBombBreaker(thisItem.type)
            )
        {
            thisItem.effect = BREAKER_EFFECT.BOMB_ROWCOL_BREAKER;

            otherItem.type = otherItem.GetCookie(otherItem.type);

            thisItem.ChangeToBig();
        }
    }


    #endregion

    #region Change

    void ChangeToSpecialType(ITEM_TYPE changeToType,bool isgrass = false)
    {
        //                GameObject explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RainbowExplosion()) as GameObject);
        //
        //                if (explosion != null) explosion.transform.position = item.transform.position + Vector3.back;

        if (IsColumnBreaker(changeToType))
        {
            ChangeToColBreaker();
        }
        else if (IsRowBreaker(changeToType))
        {
            ChangeToRowBreaker();
        }
        else if (IsBombBreaker(changeToType))
        {
            ChangeToBombBreaker();
        }
        else if (IsPlaneBreaker(changeToType))
        {
            ChangeToPlaneBreaker();
        }
        beAbleToDestroy++;
        Destroy(false,true);
        board.FindMatches();
//        board.changingList.Add(this);
//
//        board.DestroyChangingList(isgrass);
    }

    void ChangeToBig()
    {
        // prevent multiple calls
        if (changing) return;
        else changing = true;

        this.GetComponent<SpriteRenderer>().sortingLayerName = "Effect";

        iTween.ScaleTo(gameObject, iTween.Hash(
            "scale", new Vector3(2.5f, 2.5f, 0),
            "oncomplete", "CompleteChangeToBig",
            "easetype", iTween.EaseType.linear,
            "time", Configure.instance.changingTime
            ));
    }

    void CompleteChangeToBig()
    {
        this.Destroy();

        board.FindMatches();
    }

    #endregion

    #region Drop

    public void Drop()
    {
        // prevent multiple calls
        if (dropping) return;
        else dropping = true;

        if (dropPath.Count > 1)
        {
            board.droppingItems++;

            var dist = (transform.position.y - dropPath[0].y);
            
            //print("dist: " + dist);

            var time = (transform.position.y - dropPath[dropPath.Count - 1].y) / board.NodeSize();

            // fix iTween interesting problems http://vanstrydonck.com/working-with-itween-paths/
            while (dist > 0.1f)
            {
                dist -= board.NodeSize();
                dropPath.Insert(0, dropPath[0] + new Vector3(0, board.NodeSize(), 0));
            }

            if (node.ice != null)
            {
                iTween.MoveTo(node.ice.gameObject, iTween.Hash(
                    "path", dropPath.ToArray(),
                    "movetopath", false,
                    "onstart", "OnStartDrop",
                    "oncomplete", "OnCompleteDrop",
                    "easetype", iTween.EaseType.linear,
                    "time", Configure.instance.dropTime * time
                ));
            }

            iTween.MoveTo(gameObject, iTween.Hash(
                "path", dropPath.ToArray(),
                "movetopath", false,
                "onstart", "OnStartDrop",
                "oncomplete", "OnCompleteDrop",
                "easetype", iTween.EaseType.linear,
                "time", Configure.instance.dropTime * time
            ));
        }
        else
        {
             Vector3 target = board.NodeLocalPosition(node.i, node.j)+ board.transform.position;

            if (Mathf.Abs(transform.position.x - target.x) > 0.1f || Mathf.Abs(transform.position.y - target.y) > 0.1f)
            {
                board.droppingItems++;

                var time = (transform.position.y - target.y) / board.NodeSize();

                //print("target: " + target);

                 //Debug.Log("Node i:"+node.i+" j:"+node.j+"Target"+target);

                if(node.ice != null)
                {
                    iTween.MoveTo(node.ice.gameObject, iTween.Hash(
                        "position", target,
                        "onstart", "OnStartDrop",
                        "oncomplete", "OnCompleteDrop",
                        "easetype", iTween.EaseType.linear,
                        "time", Configure.instance.dropTime * time
                    ));
                }

                iTween.MoveTo(gameObject, iTween.Hash(
                    "position", target,
                    "onstart", "OnStartDrop",
                    "oncomplete", "OnCompleteDrop",
                    "easetype", iTween.EaseType.linear,
                    "time", Configure.instance.dropTime * time
                ));
            }
            else
            {
                dropping = false;
            }
        }
    }

    public bool isNeedDrop()
    {
        if (dropPath.Count > 1)
        {
            return true;
        }
        else
        {
            Vector3 target = board.NodeLocalPosition(node.i, node.j) + board.transform.position;

            if (Mathf.Abs(transform.position.x - target.x) > 0.1f || Mathf.Abs(transform.position.y - target.y) > 0.1f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public void OnStartDrop()
    {
        
    }

    public void OnCompleteDrop()
    {
        if (dropping)
        {

           // Debug.Log("Node i:" + node.i + " j:" + node.j + " Position:" + transform.position);

            AudioManager.instance.DropAudio();

            // reset
            dropPath.Clear();

            board.droppingItems--;

            // reset
            dropping = false;

            CookieGeneralEffect();
            if (node.BottomNeighbor() != null && node.BottomNeighbor().item != null)
            {
                node.BottomNeighbor().item.CookieGeneralEffect();
            }
        }
    }

    #endregion

    #region AnimationEffect

    public void CookieGeneralEffect()
    {
        if (IsCookie())
        {
            Sequence m_sequence = DOTween.Sequence();

            var tmp = Resources.Load(Configure.Cookie1()) as GameObject;
            Vector3 scale = COOKIESCALE;

            m_sequence.Append(transform.DOScale(new Vector3(scale.x + 0.15f, scale.y - 0.2f, scale.z), 0.15f))
                .Append(transform.DOScale(new Vector3(scale.x - 0.10f, scale.y + 0.03f, scale.z), 0.15f))
                .Append(transform.DOScale(new Vector3(scale.x + 0.05f, scale.y, scale.z), 0.024f))
                .Append(transform.DOScale(new Vector3(scale.x, scale.y, scale.z), 0.2f));
        }
    }

    #endregion

}
