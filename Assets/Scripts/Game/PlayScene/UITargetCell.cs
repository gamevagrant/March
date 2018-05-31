using UnityEngine;
using UnityEngine.UI;

public class UITargetCell : MonoBehaviour
{
    public Image Image;
    public Text Amount;
    public GameObject TargetTick;

    public void Init(TARGET_TYPE type, int num, int color)
    {
        Image.gameObject.SetActive(true);
        Amount.gameObject.SetActive(true);
        TargetTick.gameObject.SetActive(false);

        Amount.text = num.ToString();

        GameObject prefab;

        if (type == TARGET_TYPE.COOKIE)
        {
            prefab = Resources.Load(string.Format("{0}/cookie_{1}", Configure.ItemsPath, color)) as GameObject;
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
            prefab = Resources.Load(string.Format("{0}/collectible_{1}", Configure.ItemsPath, color)) as GameObject;
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
            prefab = Resources.Load(Configure.GrassPrefab) as GameObject;

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
