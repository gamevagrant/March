using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
    public TILE_TYPE type;
    public Node node;

    float borderPosition = 0.55f;

    public void SetBorder()
    {
        SetBorderTop();

        SetBorderBottom();

        SetBorderLeft();

        SetBorderRight();
    }

    void SetBorderTop()
    {
        // if this tile is none/pass then do not set border
        if (NoTile()) return;

        var name = "";

        Node top = node.TopNeighbor();

        // this node is able to set top border
        if (!TileNode(top))
        {
            //print("Node: " + node.name);

            name += "top_";

            // left
            Node topLeft = node.TopLeftNeighbor();
            if (TileNode(topLeft))
            {
                name += "left_bevel_";
            }
            else
            {
                Node left = node.LeftNeighbor();
                if (TileNode(left))
                {
                    name += "top_";
                }
                else
                {
                    name += "left_corner_";
                }
            }

            // right
            Node topRight = node.TopRightNeighbor();
            if (TileNode(topRight))
            {
                name += "right_bevel";
            }
            else
            {
                Node right = node.RightNeighbor();
                if (TileNode(right))
                {
                    name += "top";
                }
                else
                {
                    name += "right_corner";
                }
            }
        }

        if (name != "")
        {
            var border = Instantiate(Resources.Load(Configure.TileBorderTop() + name)) as GameObject;
            border.name = name;
            border.transform.SetParent(gameObject.transform);
            border.transform.localPosition = new Vector3(0, borderPosition, 0);
            //border.transform.position = gameObject.transform.position + new Vector3(0, borderPosition, 0);
           // border.transform.localScale = Vector3.one;
        }
    }

    void SetBorderBottom()
    {
        if (NoTile()) return;

        var name = "";
        Node bottom = node.BottomNeighbor();

        if (!TileNode(bottom))
        {
            name += "bottom_";

            Node bottomLeft = node.BottomLeftNeighbor();
            if (TileNode(bottomLeft))
            {
                name += "left_bevel_";
            }
            else
            {
                Node left = node.LeftNeighbor();
                if (TileNode(left))
                {
                    name += "bottom_";
                }
                else
                {
                    name += "left_corner_";
                }
            }

            Node bottomRight = node.BottomRightNeighbor();
            if (TileNode(bottomRight))
            {
                name += "right_bevel";
            }
            else
            {
                Node right = node.RightNeighbor();
                if (TileNode(right))
                {
                    name += "bottom";
                }
                else
                {
                    name += "right_corner";
                }
            }
        }

        if (name != "")
        {
            var border = Instantiate(Resources.Load(Configure.TileBorderBottom() + name)) as GameObject;
            border.name = name;
            border.transform.SetParent(gameObject.transform);
            border.transform.localPosition = new Vector3(0, -borderPosition, 0);
            //border.transform.position = gameObject.transform.position + new Vector3(0, -borderPosition, 0);
            //border.transform.localScale = Vector3.one;
        }
    }

    void SetBorderLeft()
    {
        if (NoTile()) return;

        var name = "";

        Node left = node.LeftNeighbor();

        if (!TileNode(left))
        {
            name += "left_";

            // top
            Node topLeft = node.TopLeftNeighbor();
            if (TileNode(topLeft))
            {
                name += "top_bevel_";
            }
            else
            {
                Node top = node.TopNeighbor();
                if (TileNode(top))
                {
                    name += "left_";
                }
                else
                {
                    name += "top_corner_";
                }
            }

            // bottom
            Node bottomLeft = node.BottomLeftNeighbor();
            if (TileNode(bottomLeft))
            {
                name += "bottom_bevel";
            }
            else
            {
                Node bottom = node.BottomNeighbor();
                if (TileNode(bottom))
                {
                    name += "left";
                }
                else
                {
                    name += "bottom_corner";
                }
            }
        }

        if (name != "")
        {
            var border = Instantiate(Resources.Load(Configure.TileBorderLeft() + name)) as GameObject;
            border.name = name;
            border.transform.SetParent(gameObject.transform);
            border.transform.localPosition = new Vector3( -borderPosition, 0, 0);
            //border.transform.position = gameObject.transform.position + new Vector3(-borderPosition, 0, 0);
            //border.transform.localScale = Vector3.one;
        }
    }

    void SetBorderRight()
    {
        if (NoTile()) return;

        var name = "";

        Node right = node.RightNeighbor();

        if (!TileNode(right))
        {
            name += "right_";

            // top
            Node topRight = node.TopRightNeighbor();
            if (TileNode(topRight))
            {
                name += "top_bevel_";
            }
            else
            {
                Node top = node.TopNeighbor();
                if (TileNode(top))
                {
                    name += "right_";
                }
                else
                {
                    name += "top_corner_";
                }
            }

            // bottom
            Node bottomRight = node.BottomRightNeighbor();
            if (TileNode(bottomRight))
            {
                name += "bottom_bevel";
            }
            else
            {
                Node bottom = node.BottomNeighbor();
                if (TileNode(bottom))
                {
                    name += "right";
                }
                else
                {
                    name += "bottom_corner";
                }
            }
        }

        if (name != "")
        {
            var border = Instantiate(Resources.Load(Configure.TileBorderRight() + name)) as GameObject;
            border.name = name;
            border.transform.SetParent(gameObject.transform);
            border.transform.localPosition = new Vector3(borderPosition, 0, 0);
            //border.transform.position = gameObject.transform.position + new Vector3(borderPosition, 0, 0);
            //border.transform.localScale = Vector3.one;
        }
    }

    public bool NoTile()
    {
        if (type == TILE_TYPE.NONE || type == TILE_TYPE.PASS_THROUGH)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TileNode(Node check)
    {
        if (check != null)
        {
            if (check.tile.type == TILE_TYPE.LIGHT_TILE || check.tile.type == TILE_TYPE.DARD_TILE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
