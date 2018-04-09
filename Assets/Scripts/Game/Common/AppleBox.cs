using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBox : MonoBehaviour
{
    public Item item;
    public Node node;
    public Board board;
    public int beAbleToDestroy;
    public APPLEBOX_TYPE status;

    public int appleNum;

    public void TryToDestroyApple(Item sourceItem)
    {
        if (status == APPLEBOX_TYPE.STATUS_8)
        {
            var obj = Resources.Load(Configure.Applebox7Main()) as GameObject;
            if (obj != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            }
            status = APPLEBOX_TYPE.STATUS_7;
        }
        else if (status == APPLEBOX_TYPE.STATUS_7)
        {
            var obj = Resources.Load(Configure.Applebox6Main()) as GameObject;
            if (obj != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            }
            status = APPLEBOX_TYPE.STATUS_6;
        }
        else if (status == APPLEBOX_TYPE.STATUS_6)
        {
            var obj = Resources.Load(Configure.Applebox5Main()) as GameObject;
            if (obj != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            }
            status = APPLEBOX_TYPE.STATUS_5;
        }
        else if (status == APPLEBOX_TYPE.STATUS_5)
        {
            var obj = Resources.Load(Configure.Applebox4Main()) as GameObject;
            if (obj != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            }
            status = APPLEBOX_TYPE.STATUS_4;
        }
        else if (status == APPLEBOX_TYPE.STATUS_4)
        {
            var obj = Resources.Load(Configure.Applebox3Main()) as GameObject;
            if (obj != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            }
            status = APPLEBOX_TYPE.STATUS_3;
        }
        else if (status == APPLEBOX_TYPE.STATUS_3)
        {
            var obj = Resources.Load(Configure.Applebox2Main()) as GameObject;
            if (obj != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            }
            status = APPLEBOX_TYPE.STATUS_2;
        }
        else if (status == APPLEBOX_TYPE.STATUS_2)
        {
            var obj = Resources.Load(Configure.Applebox1Main()) as GameObject;
            if (obj != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            }
            status = APPLEBOX_TYPE.STATUS_1;
        }
        else if (status == APPLEBOX_TYPE.STATUS_1)
        {
            var obj = Resources.Load(Configure.Applebox0Main()) as GameObject;
            if (obj != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            }
            status = APPLEBOX_TYPE.STATUS_0;
        }
        StartCoroutine(sourceItem.ResetDestroying());
        sourceItem.board.destroyingItems--;
    }

    public void TryToDestroyBox()
    {
        if (status == APPLEBOX_TYPE.STATUS_0)
        {
            Destroy(node.RightNeighbor().item.gameObject);
            node.RightNeighbor().item = null;
            node.RightNeighbor().GenerateItem(ITEM_TYPE.BLANK);

            Destroy(node.BottomNeighbor().item.gameObject);
            node.BottomNeighbor().item = null;
            node.BottomNeighbor().GenerateItem(ITEM_TYPE.BLANK);

            Destroy(node.BottomRightNeighbor().item.gameObject);
            node.BottomRightNeighbor().item = null;
            node.BottomRightNeighbor().GenerateItem(ITEM_TYPE.BLANK);

            node.item = null;
            node.GenerateItem(ITEM_TYPE.BLANK);
            node.board.appleBoxes.Remove(this);
            Destroy(gameObject);
        }
    }

}
