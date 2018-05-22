using UnityEngine;
using UnityEngine.UI;

public class GuideActionHandler : MonoBehaviour
{
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
