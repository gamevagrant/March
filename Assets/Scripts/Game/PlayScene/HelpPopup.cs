using UnityEngine;
using UnityEngine.UI;

public class HelpPopup : MonoBehaviour
{
    public void AnimateHand(Image arrow)
    {
        if (arrow != null)
        {
            Vector2 direction = Vector2.zero;
            float looptime = 1.5f;
            if (LevelLoader.instance.level == 1)
            {
                if (Help.instance.step == 1)
                {
                    direction = Vector2.up;
                }
                else if (Help.instance.step == 2)
                {
                    direction = Vector2.right;
                }
            }
            else if (LevelLoader.instance.level == 2)
            {
                if (Help.instance.step == 1)
                {
                    direction = Vector2.down;
                }
                else if (Help.instance.step == 2)
                {
                    direction = Vector2.right;
                }
                else if (Help.instance.step == 3)
                {
                    direction = Vector2.left;
                }
                else if (Help.instance.step == 4)
                {
                    looptime = 0.5f;
                    direction = new Vector2(-0.1f, -0.1f);
                }
            }
            else if (LevelLoader.instance.level == 3)
            {
                if (Help.instance.step == 1)
                {
                    direction = Vector2.left;
                }
                else if (Help.instance.step == 2)
                {
                    direction = Vector2.left;
                }
                else if (Help.instance.step == 3)
                {
                    direction = Vector2.left;
                }
                else if (Help.instance.step == 4)
                {
                    looptime = 0.5f;
                    direction = new Vector2(-0.1f, -0.1f);
                }
            }
            else if (LevelLoader.instance.level == 4)
            {
                if (Help.instance.step == 1)
                {
                    direction = Vector2.left;
                }
                else if (Help.instance.step == 2)
                {
                    direction = Vector2.right;
                }
                else if (Help.instance.step == 3)
                {
                    direction = Vector2.left;
                }
                else if (Help.instance.step == 4)
                {
                    looptime = 0.5f;
                    direction = new Vector2(-0.1f, -0.1f);
                }
            }
            else if (LevelLoader.instance.level == 5)
            {
                if (Help.instance.step == 1)
                {
                    direction = Vector2.down;
                }
                else if (Help.instance.step == 2)
                {
                    direction = Vector2.right;
                }
            }
            else if (LevelLoader.instance.level == 6)
            {
                if (Help.instance.step == 1)
                {
                    direction = Vector2.right;
                }
            }
            else if (LevelLoader.instance.level == 7)
            {
                if (Help.instance.step == 1)
                {
                    direction = Vector2.left;
                }
                else if (Help.instance.step == 2)
                {
                    direction = Vector2.left;
                }
                else if (Help.instance.step == 3)
                {
                    direction = Vector2.left;
                }
                else if (Help.instance.step == 4)
                {
                    direction = Vector2.down;
                }
            }
            else if (LevelLoader.instance.level == 8)
            {
                if (Help.instance.step == 1)
                {
                    direction = Vector2.left;
                }
            }
            else if (LevelLoader.instance.level == 9)
            {
                looptime = 0.5f;
                direction = new Vector2(-0.1f, -0.1f);
            }
            else if (LevelLoader.instance.level == 12)
            {
                if (Help.instance.step == 1)
                {
                    direction = Vector2.down;
                }
            }

            iTween.MoveBy(arrow.gameObject, iTween.Hash(
                "x", direction.x,
                "y", direction.y,
                "looptype", iTween.LoopType.loop,
                "time", looptime
            ));
        }
    }

    #region Next

    public void NextButtonDown()
    {
        Configure.instance.touchIsSwallowed = true;
    }

    public void NextButtonUp()
    {
        Configure.instance.touchIsSwallowed = false;
        if (LevelLoader.instance.level == 6)
        {
            if (Help.instance.step == 2)
            {
                SkipButtonUp();
            }
        }
        else if (LevelLoader.instance.level == 12)
        {
            if (Help.instance.step == 1)
            {
                SkipButtonUp();
            }
        }
        else if (LevelLoader.instance.level == 18)
        {
            if (Help.instance.step == 1)
            {
                SkipButtonUp();
            }
        }

        Help.instance.step = 0;
        Help.instance.current = null;
        // hide old step
        gameObject.SetActive(false);
    }

    #endregion Next

    #region Skip

    public void SkipButtonDown()
    {
        Configure.instance.touchIsSwallowed = true;
    }

    public void SkipButtonUp()
    {
        Configure.instance.touchIsSwallowed = false;

        Help.instance.step = 0;

        Help.instance.SelfDisactive();
    }

    #endregion Skip

    #region Mask

    public void MaskDown()
    {
        Configure.instance.touchIsSwallowed = true;
    }

    public void MaskUp()
    {
        Configure.instance.touchIsSwallowed = false;
    }

    #endregion
}
