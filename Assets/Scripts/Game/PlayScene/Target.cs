using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Target
{
    public TARGET_TYPE Type;
    public int Amount;
    public int color;

    public void DataToTargetType(int data)
    {
        switch (data)
        {
            case 1:
                Type =  TARGET_TYPE.SCORE;
                break;
            case 2:
                Type = TARGET_TYPE.COOKIE;
                break;
            case 3:
                Type = TARGET_TYPE.MARSHMALLOW;
                break;
            case 4:
                Type = TARGET_TYPE.WAFFLE;
                break;
            case 5:
                Type = TARGET_TYPE.COLLECTIBLE;
                break;
            case 6:
                Type = TARGET_TYPE.COLUMN_ROW_BREAKER;
                break;
            case 7:
                Type = TARGET_TYPE.BOMB_BREAKER;
                break;
            case 8:
                Type = TARGET_TYPE.X_BREAKER;
                break;
            case 9:
                Type = TARGET_TYPE.CAGE;
                break;
            case 10:
                Type = TARGET_TYPE.RAINBOW;
                break;
            case 11:
                Type = TARGET_TYPE.GINGERBREAD;
                break;
            case 12:
                Type = TARGET_TYPE.CHOCOLATE;
                break;
            case 13:
                Type = TARGET_TYPE.ROCK_CANDY;
                break;
            case 14:
                Type = TARGET_TYPE.GRASS;
                break;
            case 15:
                Type = TARGET_TYPE.CHERRY;
                break;
            case 16:
                Type = TARGET_TYPE.PACKAGEBOX;
                break;
            case 17:
                Type = TARGET_TYPE.APPLEBOX;
                break;
            default:
                    Type = TARGET_TYPE.NONE;
                    break;
        }
    }
}

