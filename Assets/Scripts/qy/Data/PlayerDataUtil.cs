public class PlayerDataUtil
{
    public static int GetEraserCount()
    {
        return qy.GameMainManager.Instance.playerData.GetPropItem("200006").count;
    }

    public static bool HasEraser()
    {
        return GetEraserCount() > 0;
    }
}
