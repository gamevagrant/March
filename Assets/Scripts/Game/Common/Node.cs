using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour 
{
    [Header("Variables")]
    public Board board;
    public Tile tile;
    public Waffle waffle;
    public Item item;
    public Cage cage;
    public Jelly jelly;
    public PackageBox packagebox;
    public Ice ice;
    public Grass grass;
    public Baffle baffleright;
    public Baffle bafflebottom;
    public GameObject ovenActive;
    [Header("")]
    public int i; // row of node
    public int j; // column of node

    #region Neighbor

    public Node LeftNeighbor()
    {
        return board.GetNode(i, j - 1);
    }

    public Node RightNeighbor()
    {
        return board.GetNode(i, j + 1);
    }

    public Node TopNeighbor()
    {
        return board.GetNode(i - 1, j);
    }

    public Node BottomNeighbor()
    {
        return board.GetNode(i + 1, j);
    }

    public Node TopLeftNeighbor()
    {
        return board.GetNode(i - 1, j - 1);
    }

    public Node TopRightNeighbor()
    {
        return board.GetNode(i - 1, j + 1);
    }

    public Node BottomLeftNeighbor()
    {
        return board.GetNode(i + 1, j - 1);
    }

    public Node BottomRightNeighbor()
    {
        return board.GetNode(i + 1, j + 1);
    }

    #endregion

    #region Item

    // Some how the function does not return a object. 
    // It always return a null pointer.
    public Item GenerateItem(ITEM_TYPE type)
    {
        Item item = null;

        switch (type)
        {
            case ITEM_TYPE.COOKIE_RAMDOM:
                GenerateRandomCookie();
                break;

            case ITEM_TYPE.BLANK:

            case ITEM_TYPE.COOKIE_RAINBOW:

            case ITEM_TYPE.COOKIE_1:

            case ITEM_TYPE.COOKIE_2:

            case ITEM_TYPE.COOKIE_3:

            case ITEM_TYPE.COOKIE_4:

            case ITEM_TYPE.COOKIE_5:

            case ITEM_TYPE.COOKIE_6:

            case ITEM_TYPE.COOKIE_COLUMN_BREAKER:
            case ITEM_TYPE.COOKIE_ROW_BREAKER:
            case ITEM_TYPE.COOKIE_BOMB_BREAKER:
            case ITEM_TYPE.COOKIE_PLANE_BREAKER:

            case ITEM_TYPE.MARSHMALLOW:

            case ITEM_TYPE.CHERRY:

                InstantiateItem(type);
                break;

            case ITEM_TYPE.GINGERBREAD_RANDOM:
                GenerateRandomGingerbread();
                break;

            case ITEM_TYPE.GINGERBREAD_1:
            case ITEM_TYPE.GINGERBREAD_2:
            case ITEM_TYPE.GINGERBREAD_3:
            case ITEM_TYPE.GINGERBREAD_4:
            case ITEM_TYPE.GINGERBREAD_5:
            case ITEM_TYPE.GINGERBREAD_6:

            case ITEM_TYPE.CHOCOLATE_1_LAYER:
            case ITEM_TYPE.CHOCOLATE_2_LAYER:
            case ITEM_TYPE.CHOCOLATE_3_LAYER:
            case ITEM_TYPE.CHOCOLATE_4_LAYER:
            case ITEM_TYPE.CHOCOLATE_5_LAYER:
            case ITEM_TYPE.CHOCOLATE_6_LAYER:
                InstantiateItem(type);
                break;

            case ITEM_TYPE.ROCK_CANDY_RANDOM:
                InstantiateItem(ITEM_TYPE.ROCK_CANDY);
                break;

            case ITEM_TYPE.ROCK_CANDY:

            case ITEM_TYPE.COLLECTIBLE_1:
            case ITEM_TYPE.COLLECTIBLE_2:
            case ITEM_TYPE.COLLECTIBLE_3:
            case ITEM_TYPE.COLLECTIBLE_4:
            case ITEM_TYPE.COLLECTIBLE_5:
            case ITEM_TYPE.COLLECTIBLE_6:
            case ITEM_TYPE.COLLECTIBLE_7:
            case ITEM_TYPE.COLLECTIBLE_8:
            case ITEM_TYPE.COLLECTIBLE_9:
            case ITEM_TYPE.COLLECTIBLE_10:
            case ITEM_TYPE.COLLECTIBLE_11:
            case ITEM_TYPE.COLLECTIBLE_12:
            case ITEM_TYPE.COLLECTIBLE_13:
            case ITEM_TYPE.COLLECTIBLE_14:
            case ITEM_TYPE.COLLECTIBLE_15:
            case ITEM_TYPE.COLLECTIBLE_16:
            case ITEM_TYPE.COLLECTIBLE_17:
            case ITEM_TYPE.COLLECTIBLE_18:
            case ITEM_TYPE.COLLECTIBLE_19:
            case ITEM_TYPE.COLLECTIBLE_20:

                InstantiateItem(type);
                break;
            case ITEM_TYPE.APPLEBOX:
                InstantiateAppleBox(type);
                break;

        }

        return item;
    }

    Item GenerateRandomCookie()
    {
        var type = LevelLoader.instance.RandomCookie();

        return InstantiateItem(type);
    }

    Item GenerateRandomGingerbread()
    {
        var type = LevelLoader.instance.RandomCookie();

        switch (type)
        {
            case ITEM_TYPE.COOKIE_1:
                InstantiateItem(ITEM_TYPE.GINGERBREAD_1);
                break;

            case ITEM_TYPE.COOKIE_2:
                InstantiateItem(ITEM_TYPE.GINGERBREAD_2);
                break;

            case ITEM_TYPE.COOKIE_3:
                InstantiateItem(ITEM_TYPE.GINGERBREAD_3);
                break;

            case ITEM_TYPE.COOKIE_4:
                InstantiateItem(ITEM_TYPE.GINGERBREAD_4);
                break;

            case ITEM_TYPE.COOKIE_5:
                InstantiateItem(ITEM_TYPE.GINGERBREAD_5);
                break;

            case ITEM_TYPE.COOKIE_6:
                InstantiateItem(ITEM_TYPE.GINGERBREAD_6);
                break;
        }

        return null;
    }

    Item InstantiateItem(ITEM_TYPE type)
    {
        GameObject piece = null;
        var color = 0;
        int beabletodestroy = 1;
        switch (type)
        {
            case ITEM_TYPE.BLANK:
                piece = Instantiate(Resources.Load(Configure.Blank())) as GameObject;
                break;

            case ITEM_TYPE.COOKIE_RAINBOW:
                piece = Instantiate(Resources.Load(Configure.CookieRainbow())) as GameObject;
                break;

            case ITEM_TYPE.COOKIE_1:
                color = 1;
                piece = Instantiate(Resources.Load(Configure.Cookie1())) as GameObject;
                break;
           
            case ITEM_TYPE.COOKIE_2:
                color = 2;
                piece = Instantiate(Resources.Load(Configure.Cookie2())) as GameObject;
                break;

            case ITEM_TYPE.COOKIE_3:
                color = 3;
                piece = Instantiate(Resources.Load(Configure.Cookie3())) as GameObject;
                break;

            case ITEM_TYPE.COOKIE_4:
                color = 4;
                piece = Instantiate(Resources.Load(Configure.Cookie4())) as GameObject;
                break;

            case ITEM_TYPE.COOKIE_5:
                color = 5;
                piece = Instantiate(Resources.Load(Configure.Cookie5())) as GameObject;
                break;

            case ITEM_TYPE.COOKIE_6:
                color = 6;
                piece = Instantiate(Resources.Load(Configure.Cookie6())) as GameObject;
                break;


            case ITEM_TYPE.COOKIE_COLUMN_BREAKER:
                piece = Instantiate(Resources.Load(Configure.Cookie1ColumnBreaker())) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_ROW_BREAKER:
                piece = Instantiate(Resources.Load(Configure.Cookie1RowBreaker())) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_BOMB_BREAKER:
                piece = Instantiate(Resources.Load(Configure.Cookie1BombBreaker())) as GameObject;
                break;
            case ITEM_TYPE.COOKIE_PLANE_BREAKER:
                piece = Instantiate(Resources.Load(Configure.PlaneBreaker())) as GameObject;
                break;


            case ITEM_TYPE.MARSHMALLOW:
                piece = Instantiate(Resources.Load(Configure.Marshmallow())) as GameObject;
                break;

            case ITEM_TYPE.CHERRY:
                beabletodestroy = 0;
                piece = Instantiate(Resources.Load(Configure.Cherry())) as GameObject;
                break;

            case ITEM_TYPE.GINGERBREAD_1:
                color = 1;
                piece = Instantiate(Resources.Load(Configure.Gingerbread1())) as GameObject;
                break;
            case ITEM_TYPE.GINGERBREAD_2:
                color = 2;
                piece = Instantiate(Resources.Load(Configure.Gingerbread2())) as GameObject;
                break;
            case ITEM_TYPE.GINGERBREAD_3:
                color = 3;
                piece = Instantiate(Resources.Load(Configure.Gingerbread3())) as GameObject;
                break;
            case ITEM_TYPE.GINGERBREAD_4:
                color = 4;
                piece = Instantiate(Resources.Load(Configure.Gingerbread4())) as GameObject;
                break;
            case ITEM_TYPE.GINGERBREAD_5:
                color = 5;
                piece = Instantiate(Resources.Load(Configure.Gingerbread5())) as GameObject;
                break;
            case ITEM_TYPE.GINGERBREAD_6:
                color = 6;
                piece = Instantiate(Resources.Load(Configure.Gingerbread6())) as GameObject;
                break;

            case ITEM_TYPE.CHOCOLATE_1_LAYER:
                piece = Instantiate(Resources.Load(Configure.Chocolate1())) as GameObject;
                break;
            case ITEM_TYPE.CHOCOLATE_2_LAYER:
                piece = Instantiate(Resources.Load(Configure.Chocolate2())) as GameObject;
                break;
            case ITEM_TYPE.CHOCOLATE_3_LAYER:
                piece = Instantiate(Resources.Load(Configure.Chocolate3())) as GameObject;
                break;
            case ITEM_TYPE.CHOCOLATE_4_LAYER:
                piece = Instantiate(Resources.Load(Configure.Chocolate4())) as GameObject;
                break;
            case ITEM_TYPE.CHOCOLATE_5_LAYER:
                piece = Instantiate(Resources.Load(Configure.Chocolate5())) as GameObject;
                break;
            case ITEM_TYPE.CHOCOLATE_6_LAYER:
                piece = Instantiate(Resources.Load(Configure.Chocolate6())) as GameObject;
                break;

            case ITEM_TYPE.ROCK_CANDY:
                color = 0;
                piece = Instantiate(Resources.Load(Configure.RockCandy1())) as GameObject;
                break;

            case ITEM_TYPE.COLLECTIBLE_1:
                piece = Instantiate(Resources.Load(Configure.Collectible1())) as GameObject;
                color = 1;
                break;

            case ITEM_TYPE.COLLECTIBLE_2:
                piece = Instantiate(Resources.Load(Configure.Collectible2())) as GameObject;
                color = 2;
                break;

            case ITEM_TYPE.COLLECTIBLE_3:
                piece = Instantiate(Resources.Load(Configure.Collectible3())) as GameObject;
                color = 3;
                break;

            case ITEM_TYPE.COLLECTIBLE_4:
                piece = Instantiate(Resources.Load(Configure.Collectible4())) as GameObject;
                color = 4;
                break;

            case ITEM_TYPE.COLLECTIBLE_5:
                piece = Instantiate(Resources.Load(Configure.Collectible5())) as GameObject;
                color = 5;
                break;

            case ITEM_TYPE.COLLECTIBLE_6:
                piece = Instantiate(Resources.Load(Configure.Collectible6())) as GameObject;
                color = 6;
                break;

            case ITEM_TYPE.COLLECTIBLE_7:
                piece = Instantiate(Resources.Load(Configure.Collectible7())) as GameObject;
                color = 7;
                break;

            case ITEM_TYPE.COLLECTIBLE_8:
                piece = Instantiate(Resources.Load(Configure.Collectible8())) as GameObject;
                color = 8;
                break;

            case ITEM_TYPE.COLLECTIBLE_9:
                piece = Instantiate(Resources.Load(Configure.Collectible9())) as GameObject;
                color = 9;
                break;

            case ITEM_TYPE.COLLECTIBLE_10:
                piece = Instantiate(Resources.Load(Configure.Collectible10())) as GameObject;
                color = 10;
                break;

            case ITEM_TYPE.COLLECTIBLE_11:
                piece = Instantiate(Resources.Load(Configure.Collectible11())) as GameObject;
                color = 11;
                break;

            case ITEM_TYPE.COLLECTIBLE_12:
                piece = Instantiate(Resources.Load(Configure.Collectible12())) as GameObject;
                color = 12;
                break;

            case ITEM_TYPE.COLLECTIBLE_13:
                piece = Instantiate(Resources.Load(Configure.Collectible13())) as GameObject;
                color = 13;
                break;

            case ITEM_TYPE.COLLECTIBLE_14:
                piece = Instantiate(Resources.Load(Configure.Collectible14())) as GameObject;
                color = 14;
                break;

            case ITEM_TYPE.COLLECTIBLE_15:
                piece = Instantiate(Resources.Load(Configure.Collectible15())) as GameObject;
                color = 15;
                break;

            case ITEM_TYPE.COLLECTIBLE_16:
                piece = Instantiate(Resources.Load(Configure.Collectible16())) as GameObject;
                color = 16;
                break;

            case ITEM_TYPE.COLLECTIBLE_17:
                piece = Instantiate(Resources.Load(Configure.Collectible17())) as GameObject;
                color = 17;
                break;

            case ITEM_TYPE.COLLECTIBLE_18:
                piece = Instantiate(Resources.Load(Configure.Collectible18())) as GameObject;
                color = 18;
                break;

            case ITEM_TYPE.COLLECTIBLE_19:
                piece = Instantiate(Resources.Load(Configure.Collectible19())) as GameObject;
                color = 19;
                break;

            case ITEM_TYPE.COLLECTIBLE_20:
                piece = Instantiate(Resources.Load(Configure.Collectible20())) as GameObject;
                color = 20;
                break;
        }

        if (piece != null)
        {
            piece.transform.SetParent(gameObject.transform);
            piece.name = "Item";
            piece.transform.localPosition = board.NodeLocalPosition(i, j);
            piece.GetComponent<Item>().node = this;
            piece.GetComponent<Item>().board = this.board;
            piece.GetComponent<Item>().type = type;
            piece.GetComponent<Item>().color = color;
            piece.GetComponent<Item>().beAbleToDestroy = beabletodestroy;

            this.item = piece.GetComponent<Item>();
            
            return piece.GetComponent<Item>();
        }
        else
        {
            return null;
        }
    }

    void InstantiateAppleBox(ITEM_TYPE type)
    {
        if (RightNeighbor() == null || BottomNeighbor() == null || BottomRightNeighbor() == null)
        {
            Debug.Log("苹果箱位置摆放错误！");
        }

        GameObject applebox_1 = null;
        GameObject applebox_2 = null;
        GameObject applebox_3 = null;
        GameObject applebox_4 = null;
        int color = 0;

        switch (type)
        {
            case ITEM_TYPE.APPLEBOX:
                applebox_1 = Instantiate(Resources.Load(Configure.Applebox8Main())) as GameObject;
                applebox_2 = Instantiate(Resources.Load(Configure.AppleboxOther())) as GameObject;
                applebox_3 = Instantiate(Resources.Load(Configure.AppleboxOther())) as GameObject;
                applebox_4 = Instantiate(Resources.Load(Configure.AppleboxOther())) as GameObject;
                break;
        }

        if (applebox_1 != null)
        {
            applebox_1.transform.SetParent(gameObject.transform);
            applebox_1.name = "Item";
            applebox_1.transform.localPosition = board.NodeLocalPosition(i, j);
            applebox_1.GetComponent<Item>().node = this;
            applebox_1.GetComponent<Item>().board = this.board;
            applebox_1.GetComponent<Item>().type = type;
            applebox_1.GetComponent<Item>().color = color;
            applebox_1.GetComponent<Item>().applebox = applebox_1.GetComponent<AppleBox>();
            this.item = applebox_1.GetComponent<Item>();
            applebox_1.GetComponent<AppleBox>().item = applebox_1.GetComponent<Item>();
            applebox_1.GetComponent<AppleBox>().node = this;
            applebox_1.GetComponent<AppleBox>().board = board;
            applebox_1.GetComponent<AppleBox>().appleNum = 8;
            applebox_1.GetComponent<AppleBox>().beAbleToDestroy = 8;
            applebox_1.GetComponent<AppleBox>().status = APPLEBOX_TYPE.STATUS_8;

            applebox_2.transform.SetParent(RightNeighbor().gameObject.transform);
            applebox_2.name = "Item";
            applebox_2.transform.localPosition = board.NodeLocalPosition(i, j + 1);
            applebox_2.GetComponent<Item>().node = RightNeighbor();
            applebox_2.GetComponent<Item>().board = RightNeighbor().board;
            applebox_2.GetComponent<Item>().type = type;
            applebox_2.GetComponent<Item>().color = color;
            applebox_2.GetComponent<Item>().applebox = applebox_1.GetComponent<AppleBox>();
            RightNeighbor().item = applebox_2.GetComponent<Item>();

            applebox_3.transform.SetParent(BottomNeighbor().gameObject.transform);
            applebox_3.name = "Item";
            applebox_3.transform.localPosition = board.NodeLocalPosition(i + 1, j);
            applebox_3.GetComponent<Item>().node = BottomNeighbor();
            applebox_3.GetComponent<Item>().board = BottomNeighbor().board;
            applebox_3.GetComponent<Item>().type = type;
            applebox_3.GetComponent<Item>().color = color;
            applebox_3.GetComponent<Item>().applebox = applebox_1.GetComponent<AppleBox>();
            BottomNeighbor().item = applebox_3.GetComponent<Item>();

            applebox_4.transform.SetParent(BottomRightNeighbor().gameObject.transform);
            applebox_4.name = "Item";
            applebox_4.transform.localPosition = board.NodeLocalPosition(i, j + 1);
            applebox_4.GetComponent<Item>().node = BottomRightNeighbor();
            applebox_4.GetComponent<Item>().board = BottomRightNeighbor().board;
            applebox_4.GetComponent<Item>().type = type;
            applebox_4.GetComponent<Item>().color = color;
            applebox_4.GetComponent<Item>().applebox = applebox_1.GetComponent<AppleBox>();
            BottomRightNeighbor().item = applebox_4.GetComponent<Item>();

            board.appleBoxes.Add(applebox_1.GetComponent<AppleBox>());

        }
    }

    #endregion

    #region Match

    public List<Item> FindSquareMatches()
    {
        var list = new List<Item>();
        if (item == null || !item.Matchable())
        {
            return null;
        }
        if (IsSameColorItem(TopNeighbor()) &&IsSameColorItem(TopRightNeighbor()) && IsSameColorItem(RightNeighbor()))
        {
            list.Add(item);
            list.Add(TopNeighbor().item);
            list.Add(TopRightNeighbor().item);
            list.Add(RightNeighbor().item);
        }
        else if (IsSameColorItem(TopNeighbor()) && IsSameColorItem(TopLeftNeighbor()) && IsSameColorItem(LeftNeighbor()))
        {
            list.Add(item);
            list.Add(TopNeighbor().item);
            list.Add(TopLeftNeighbor().item);
            list.Add(LeftNeighbor().item);
        }
        else if (IsSameColorItem(RightNeighbor()) && IsSameColorItem(BottomRightNeighbor()) && IsSameColorItem(BottomNeighbor()))
        {
            list.Add(item);
            list.Add(RightNeighbor().item);
            list.Add(BottomRightNeighbor().item);
            list.Add(BottomNeighbor().item);
        }
        else if (IsSameColorItem(LeftNeighbor()) && IsSameColorItem(BottomLeftNeighbor()) && IsSameColorItem(BottomNeighbor()))
        {
            list.Add(item);
            list.Add(LeftNeighbor().item);
            list.Add(BottomLeftNeighbor().item);
            list.Add(BottomNeighbor().item);
        }

        return list;
    }

    private bool IsSameColorItem(Node checkNode)
    {
        if (checkNode == null || checkNode.item == null)
        {
            return false;
        }
        if (checkNode.item.Matchable() && checkNode.item.color == item.color)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // find matches at a node
    public List<Item> FindMatches(FIND_DIRECTION direction = FIND_DIRECTION.NONE, int matches = 3)
    {
        var list = new List<Item>();
        var countedNodes = new Dictionary<int, Item>();

        if (item == null || !item.Matchable())
        {
            return null;
        }

        if (direction != FIND_DIRECTION.COLUMN)
        {
            countedNodes = FindMoreMatches(item.color, countedNodes, FIND_DIRECTION.ROW);
        }

        if (countedNodes.Count < matches)
        {
            countedNodes.Clear();
        }

        if (direction != FIND_DIRECTION.ROW)
        {
            countedNodes = FindMoreMatches(item.color, countedNodes, FIND_DIRECTION.COLUMN);
        }

        if (countedNodes.Count < matches)
        {
            countedNodes.Clear();
        }

        foreach (KeyValuePair<int, Item> entry in countedNodes)
        {
            list.Add(entry.Value);
        }

        return list;
    }

    // helper function to find matches
    Dictionary<int, Item> FindMoreMatches(int color, Dictionary<int, Item> countedNodes, FIND_DIRECTION direction)
    {
        if (item == null || item.destroying)
        {
            return countedNodes;
        }

        if (item.color == color && !countedNodes.ContainsValue(item) && item.Matchable() && item.node != null)
        {
            countedNodes.Add(item.node.OrderOnBoard(), item);

            if (direction == FIND_DIRECTION.ROW)
            {
                if (LeftNeighbor() != null)
                {
                    countedNodes = LeftNeighbor().FindMoreMatches(color, countedNodes, FIND_DIRECTION.ROW);
                }

                if (RightNeighbor() != null)
                {
                    countedNodes = RightNeighbor().FindMoreMatches(color, countedNodes, FIND_DIRECTION.ROW);
                }
            }
            else if (direction == FIND_DIRECTION.COLUMN)
            {
                if (TopNeighbor() != null)
                {
                    countedNodes = TopNeighbor().FindMoreMatches(color, countedNodes, FIND_DIRECTION.COLUMN);
                }

                if (BottomNeighbor() != null)
                {
                    countedNodes = BottomNeighbor().FindMoreMatches(color, countedNodes, FIND_DIRECTION.COLUMN);
                }
            }
        }

        return countedNodes;
    }

    #endregion

    #region Utility

    // return the order base on i and j
    public int OrderOnBoard()
    {
        return (i * LevelLoader.instance.column + j);
    }

    #endregion

    #region Type

    public bool CanStoreItem()
    {
        if (tile != null)
        {
            if (tile.type == TILE_TYPE.DARD_TILE || tile.type == TILE_TYPE.LIGHT_TILE)
            {
                return true;
            }
        }

        return false;
    }

    public bool CanGoThrough()
    {
        if (tile == null || tile.type == TILE_TYPE.NONE)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CanGenerateNewItem()
    {
        if (CanStoreItem() == true && CanDropIn() && !HasTopBaffle())
        {
            for (int row = i - 1; row >= 0; row--)
            {
                Node upNode = board.GetNode(row, j);

                if (upNode != null)
                {
                    if (upNode.CanGoThrough() == false)
                    {
                        return false;
                    }
                    else
                    {
                        if (upNode.item != null)
                        {
                            if (upNode.item.type != ITEM_TYPE.BLANK && upNode.item.Droppable() == false || !upNode.CanDropIn() || upNode.HasTopBaffle())
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanChangeType()
    {
        if (cage != null
            || ice != null
            || jelly != null
            || packagebox != null
        )
        {
            return false;
        }
        return true;
    }

    public bool CanDropIn()
    {
        if (cage != null
            || ice != null
            || jelly != null
            || packagebox != null
        )
        {
            return false;
        }
        return true;
    }

    public bool HasTopBaffle()
    {
        if (TopNeighbor() != null && TopNeighbor().bafflebottom != null)
        {
            return true;
        }
        return false;
    }

    public bool isNodeBlank()
    {
        if (ice != null || jelly != null || packagebox != null || cage != null)
        {
            return false;
        }

        if (item != null && item.type == ITEM_TYPE.BLANK)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Node

    // get source node of an empty node
    public Node GetSourceNode()
    {
        Node source = null;

        // top
        Node top = board.GetNode(i - 1, j);
        if (top != null && top.bafflebottom == null)
        {
            if (top.tile!= null && top.tile.type == TILE_TYPE.PASS_THROUGH || top.item != null && top.item.type == ITEM_TYPE.BLANK && top.CanGoThrough() && top.CanDropIn())
            {
                    source = top.GetSourceNode();
            }
        }

        if (source != null)
        {
            return source;
        }

        // top left
        Node left = board.GetNode(i - 1, j - 1);
        if (left != null && left.bafflebottom == null)
        {
            if (this.LeftNeighbor() == null || (this.LeftNeighbor()!= null && this.LeftNeighbor().baffleright == null))
            {
                if (left.tile != null && left.tile.type == TILE_TYPE.PASS_THROUGH || left.item != null && left.item.type == ITEM_TYPE.BLANK && left.CanGoThrough() && left.CanDropIn())
                {
                        source = left.GetSourceNode();
                }
                else
                {
                    if (left.item != null && left.item.type != ITEM_TYPE.BLANK && left.item.Movable())
                    {
                        source = left;
                    }
                }
            }
        }

        if (source != null)
        {
            return source;
        }

        // top right
        Node right = board.GetNode(i - 1, j + 1);
        if (right != null && right.bafflebottom == null)
        {
            if (this.baffleright == null)
            {
                if (right.tile != null && right.tile.type == TILE_TYPE.PASS_THROUGH || right.item != null && right.item.type == ITEM_TYPE.BLANK && right.CanGoThrough() &&
                    right.CanDropIn())
                {
                        source = right.GetSourceNode();
                }
                else
                {
                    if (right.item != null && right.item.type != ITEM_TYPE.BLANK && right.item.Movable())
                    {
                        source = right;
                    }
                }
            }
        }

        return source;
    }

    // get move path from an empty node to source node
    public List<Vector3> GetMovePath()
    {
        List<Vector3> path = new List<Vector3>();

        path.Add(board.NodeLocalPosition(i, j)+board.transform.position);

        // top
        Node top = board.GetNode(i - 1, j);
        if (top != null && top.bafflebottom == null)
        {
            if (top.item != null && top.item.type == ITEM_TYPE.BLANK && top.CanGoThrough() && top.CanDropIn())
            {
                if (top.GetSourceNode() != null)
                {
                    path.AddRange(top.GetMovePath());
                    return path;
                }
            }
        }

        // left
        Node left = board.GetNode(i - 1, j - 1);
        if (left != null && left.bafflebottom == null)
        {
            if (this.LeftNeighbor() == null || (this.LeftNeighbor() != null && this.LeftNeighbor().baffleright == null))
            {
                if (left.item != null && left.item.type == ITEM_TYPE.BLANK && left.CanGoThrough() && left.CanDropIn())
                {
                    if (left.GetSourceNode() != null)
                    {
                        path.AddRange(left.GetMovePath());
                        return path;
                    }
                }
                else
                {
                    if (left.item != null && left.item.type != ITEM_TYPE.BLANK && left.item.Movable())
                    {
                        return path;
                    }
                }
            }
        }

        // right
        Node right = board.GetNode(i - 1, j + 1);
        if (right != null && right.bafflebottom == null)
        {
            if (this.baffleright == null)
            {
                if (right.item != null && right.item.type == ITEM_TYPE.BLANK && right.CanGoThrough() && right.CanDropIn())
                {
                    if (right.GetSourceNode() != null)
                    {
                        path.AddRange(right.GetMovePath());
                        return path;
                    }
                }
                else
                {
                    if (right.item != null && right.item.type != ITEM_TYPE.BLANK && right.item.Movable())
                    {
                        return path;
                    }
                }
            }
        }

        return path;
    }

    #endregion

    #region Waffle

    public void WaffleExplode()
    {
        if (waffle != null && item != null & (item.IsCookie() == true || item.IsBreaker(item.type) || item.type == ITEM_TYPE.COOKIE_RAINBOW))
        {
            AudioManager.instance.WaffleExplodeAudio();

            board.CollectWaffle(waffle);

            GameObject prefab = null;

            if (waffle.type == WAFFLE_TYPE.WAFFLE_3)
            {
                prefab = Resources.Load(Configure.Waffle2()) as GameObject;

                waffle.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

                waffle.type = WAFFLE_TYPE.WAFFLE_2;
            }
            else if (waffle.type == WAFFLE_TYPE.WAFFLE_2)
            {
                prefab = Resources.Load(Configure.Waffle1()) as GameObject;

                waffle.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

                waffle.type = WAFFLE_TYPE.WAFFLE_1;
            }
            else if (waffle.type == WAFFLE_TYPE.WAFFLE_1)
            {
                Destroy(waffle.gameObject);

                waffle = null;
            }
        }
    }

    #endregion

    #region Cage

    public void CageExplode()
    {
        if (cage == null)
        {
            return;
        }

        GameObject explosion = null;

//        if (item != null)
//        {
//            switch (item.GetCookie(item.type))
//            {
//                case ITEM_TYPE.COOKIE_1:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BlueCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_2:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.GreenCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_3:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.OrangeCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_4:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.PurpleCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_5:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RedCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_6:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.YellowCookieExplosion()) as GameObject);
//                    break;
//            }
//        }

        board.CollectCage(cage);

        if (explosion != null) explosion.transform.position = item.transform.position;

        AudioManager.instance.CageExplodeAudio();

        if (cage.type == CAGE_TYPE.CAGE_1)
        {
            Destroy(cage.gameObject);

            cage = null;
        }
        else if (cage.type == CAGE_TYPE.CAGE_2)
        {

            var prefab = Resources.Load(Configure.Cage1()) as GameObject;

            cage.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            cage.type = CAGE_TYPE.CAGE_1;
        }

        StartCoroutine(item.ResetDestroying());

    }

    #endregion

    #region Ice


    public void IceExplode()
    {
        if (ice == null)
        {
            return;
        }

        GameObject explosion = null;

//        if (item != null)
//        {
//            switch (item.GetCookie(item.type))
//            {
//                case ITEM_TYPE.COOKIE_1:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BlueCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_2:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.GreenCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_3:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.OrangeCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_4:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.PurpleCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_5:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RedCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_6:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.YellowCookieExplosion()) as GameObject);
//                    break;
//            }
//        }

//        board.CollectCage(cage);

        if (explosion != null) explosion.transform.position = item.transform.position;

        //todo:待修改
        AudioManager.instance.CageExplodeAudio();

        if (ice.type == ICE_TYPE.ICE_1)
        {
            Destroy(ice.gameObject);

            ice = null;
        }
        else if (ice.type == ICE_TYPE.ICE_2)
        {

            var prefab = Resources.Load(Configure.Ice1()) as GameObject;

            ice.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            ice.type =ICE_TYPE.ICE_1;
        }

        StartCoroutine(item.ResetDestroying());

    }


    #endregion

    #region Jelly

    public bool JellyExplode()
    {
        if (jelly == null)
        {
            return false;
        }

        GameObject explosion = null;

//        if (item != null)
//        {
//            switch (item.GetCookie(item.type))
//            {
//                case ITEM_TYPE.COOKIE_1:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BlueCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_2:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.GreenCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_3:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.OrangeCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_4:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.PurpleCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_5:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RedCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_6:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.YellowCookieExplosion()) as GameObject);
//                    break;
//            }
//        }

       // board.CollectJelly(jelly);
		explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.Jelly1Effects()) as GameObject);
        if (explosion != null) explosion.transform.position = item.transform.position;

        AudioManager.instance.CageExplodeAudio();

        if (jelly.type == JELLY_TYPE.JELLY_1)
        {
            Destroy(jelly.gameObject);

            jelly = null;

            if (item != null && item.type == ITEM_TYPE.CHERRY)
            {
                return false;
            }
        }
        else if (jelly.type == JELLY_TYPE.JELLY_2)
        {

            var prefab = Resources.Load(Configure.Jelly1()) as GameObject;

            jelly.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            jelly.type = JELLY_TYPE.JELLY_1;
        }
        else if (jelly.type == JELLY_TYPE.JELLY_3)
        {

            var prefab = Resources.Load(Configure.Jelly2()) as GameObject;

            jelly.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            jelly.type = JELLY_TYPE.JELLY_2;
        }
        StartCoroutine(item.ResetDestroying());
        return true;
    }


    #endregion

    #region PackageBox

    public void PackageBoxExplode()
    {
        if (packagebox == null)
        {
            return;
        }

//        GameObject explosion = null;

//        if (item != null)
//        {
//            switch (item.GetCookie(item.type))
//            {
//                case ITEM_TYPE.COOKIE_1:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.BlueCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_2:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.GreenCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_3:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.OrangeCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_4:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.PurpleCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_5:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.RedCookieExplosion()) as GameObject);
//                    break;
//                case ITEM_TYPE.COOKIE_6:
//                    explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.YellowCookieExplosion()) as GameObject);
//                    break;
//            }
//        }

        // board.CollectJelly(jelly);

//        if (explosion != null) explosion.transform.position = item.transform.position;

        AudioManager.instance.CageExplodeAudio();//todo:音效 修改

        if (packagebox.type == PACKAGEBOX_TYPE.PACKAGEBOX_1)
        {

			GameObject explosion = null;
			explosion = CFX_SpawnSystem.GetNextObject(Resources.Load(Configure.PackageBox1Effects()) as GameObject);
			if (explosion != null) explosion.transform.position = item.transform.position;


            GameObject flyingItem = null;
            var order = 0;

            for (int k = 0; k < LevelLoader.instance.targetList.Count; k++)
            {
                if (LevelLoader.instance.targetList[k].Type == TARGET_TYPE.PACKAGEBOX
                    && board.targetLeftList[k] > 0
                )
                {
                    board.targetLeftList[k]--;
                    flyingItem = new GameObject();
                    order = k;
                    break;
                }
            }

            if (flyingItem != null)
            {
                flyingItem.transform.position = item.transform.position;
                flyingItem.name = "Flying PackageBox";
                flyingItem.layer = LayerMask.NameToLayer("On Top UI");

                SpriteRenderer spriteRenderer = flyingItem.AddComponent<SpriteRenderer>();

                GameObject prefab = Resources.Load(Configure.PackageBox1()) as GameObject;

                if (prefab != null)
                {
                    spriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                }

                StartCoroutine(board.CollectItemAnim(flyingItem, order));
            }


            Destroy(packagebox.gameObject);

            packagebox = null;

        }
        else if (packagebox.type == PACKAGEBOX_TYPE.PACKAGEBOX_2)
        {

            var prefab = Resources.Load(Configure.PackageBox1()) as GameObject;

            packagebox.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            packagebox.type = PACKAGEBOX_TYPE.PACKAGEBOX_1;
        }
        else if (packagebox.type == PACKAGEBOX_TYPE.PACKAGEBOX_3)
        {

            var prefab = Resources.Load(Configure.PackageBox2()) as GameObject;

            packagebox.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            packagebox.type = PACKAGEBOX_TYPE.PACKAGEBOX_2;
        }
        else if (packagebox.type == PACKAGEBOX_TYPE.PACKAGEBOX_4)
        {

            var prefab = Resources.Load(Configure.PackageBox3()) as GameObject;

            packagebox.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            packagebox.type = PACKAGEBOX_TYPE.PACKAGEBOX_3;
        }
        else if (packagebox.type == PACKAGEBOX_TYPE.PACKAGEBOX_5)
        {

            var prefab = Resources.Load(Configure.PackageBox4()) as GameObject;

            packagebox.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            packagebox.type = PACKAGEBOX_TYPE.PACKAGEBOX_4;
        }
        else if (packagebox.type == PACKAGEBOX_TYPE.PACKAGEBOX_6)
        {

            var prefab = Resources.Load(Configure.PackageBox5()) as GameObject;

            packagebox.gameObject.GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            packagebox.type = PACKAGEBOX_TYPE.PACKAGEBOX_5;
        }

        StartCoroutine(item.ResetDestroying());

    }

    #endregion


    #region Booster

    public void AddOvenBoosterActive()
    {
        ovenActive = Instantiate(Resources.Load(Configure.BoosterActive())) as GameObject;

        ovenActive.transform.localPosition = board.NodeLocalPosition(i, j);
    }

    public void RemoveOvenBoosterActive()
    {
        Destroy(ovenActive);

        ovenActive = null;
    }

    #endregion


    #region Grass

    public void ChangeToGrass()
    {
        if (tile == null 
            || (tile.type != TILE_TYPE.DARD_TILE && tile.type != TILE_TYPE.LIGHT_TILE)
            ||(cage != null && cage.type == CAGE_TYPE.CAGE_2)
            || (jelly != null && jelly.type != JELLY_TYPE.JELLY_1)
            )
        {
            return;
        }

        if (grass == null)
        {
            var grassPrefab = Instantiate(Resources.Load(Configure.GrassPrefab())) as GameObject;

            if (grassPrefab)
            {
                grassPrefab.transform.SetParent(gameObject.transform);
                grassPrefab.name = "Grass";
                grassPrefab.transform.localPosition = board.NodeLocalPosition(i,j);
                grassPrefab.GetComponent<Grass>().type = 0;
                grassPrefab.GetComponent<Grass>().node = this;
                //if (tile.GetComponent<SpriteRenderer>()) tile.GetComponent<SpriteRenderer>().enabled = false;

                grass = grassPrefab.GetComponent<Grass>();

                int order = -1;

                for (int k = 0; k < LevelLoader.instance.targetList.Count; k++)
                {
                    if (LevelLoader.instance.targetList[k].Type == TARGET_TYPE.GRASS
                        && board.targetLeftList[k] > 0
                    )
                    {
                        board.targetLeftList[k]--;
                        order = k;
                        break;
                    }
                }
                if (order != -1)
                {
                    board.UITarget.UpdateTargetAmount(order);
                }

            }
        }
    }

    public bool IsGrass(bool special = false)
    {
        if (special == true)
        {
            return true;
        }
        if (grass != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Destroy


    #endregion
}
