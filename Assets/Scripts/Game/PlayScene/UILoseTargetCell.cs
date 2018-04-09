using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoseTargetCell : MonoBehaviour
{

    public Image Image;
    public GameObject TargetCross;
	public Text TargetNum;

	public void Init(TARGET_TYPE type, int num, int color = 0)
    {
        Image.gameObject.SetActive(true);
        TargetCross.gameObject.SetActive(true);
        //TargetNum.text = num.ToString();

        GameObject prefab = null;

        if (type == TARGET_TYPE.COOKIE)
        {
            switch (color)
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
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }

            Image.rectTransform.localScale = new Vector3(1, 1, 0);
        }
        // 3 - marshmallow
        else if (type == TARGET_TYPE.MARSHMALLOW)
        {
            prefab = Resources.Load(Configure.Marshmallow()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }

            Image.rectTransform.localScale = new Vector3(1, 1, 0);
        }
        // 4 -waffle
        else if (type == TARGET_TYPE.WAFFLE)
        {
            prefab = Resources.Load(Configure.Waffle1()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }

            Image.rectTransform.localScale = new Vector3(0.75f, 0.75f, 0);
        }
        // 5 - collectible
        else if (type == TARGET_TYPE.COLLECTIBLE)
        {
            switch (color)
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
                    prefab = Resources.Load(Configure.Collectible8()) as GameObject;
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
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }

            Image.rectTransform.localScale = new Vector3(1, 1, 0);
        }
        // 6 - col_row_breaker
        else if (type == TARGET_TYPE.COLUMN_ROW_BREAKER)
        {

            prefab = Resources.Load(Configure.ColumnRowBreaker()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        // 7 - bomb_breaker
        else if (type == TARGET_TYPE.BOMB_BREAKER)
        {
            prefab = Resources.Load(Configure.GenericBombBreaker()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }

        }
        // 8 - x_breaker
        else if (type == TARGET_TYPE.X_BREAKER)
        {
            prefab = Resources.Load(Configure.GenericXBreaker()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }

        }
        // 9 - cage
        else if (type == TARGET_TYPE.CAGE)
        {
            prefab = Resources.Load(Configure.Cage1()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }

            Image.rectTransform.localScale = new Vector3(0.75f, 0.75f, 0);
        }
        // 10 - rainbow
        else if (type == TARGET_TYPE.RAINBOW)
        {
            prefab = Resources.Load(Configure.CookieRainbow()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        // 11 - gingerbread
        else if (type == TARGET_TYPE.GINGERBREAD)
        {
            prefab = Resources.Load(Configure.GingerbreadGeneric()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        // 12 - chocolate
        else if (type == TARGET_TYPE.CHOCOLATE)
        {
            prefab = Resources.Load(Configure.Chocolate1()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        // 13 - rock candy
        else if (type == TARGET_TYPE.ROCK_CANDY)
        {
            prefab = Resources.Load(Configure.RockCandyGeneric()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        // 14 - grass
        else if (type == TARGET_TYPE.GRASS)
        {
            prefab = Resources.Load(Configure.GrassPrefab()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        //15
        else if (type == TARGET_TYPE.CHERRY)
        {
            prefab = Resources.Load(Configure.Cherry()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        //16
        else if (type == TARGET_TYPE.PACKAGEBOX)
        {
            prefab = Resources.Load(Configure.PackageBox1()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        //17
        else if (type == TARGET_TYPE.APPLEBOX)
        {
            prefab = Resources.Load(Configure.Apple()) as GameObject;

            if (prefab != null)
            {
                Image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        else
        {
            Image.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
