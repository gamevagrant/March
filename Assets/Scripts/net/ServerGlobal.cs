using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerGlobal
{
    // public static string loginUrl = "http://10.1.9.247:9933"; //测试服务器
#if RELEASE_FLAG
    public static string loginUrl = "http://169.44.81.206:9933";//线上海外服务器
#else
    public static string loginUrl = "http://123.206.90.153:9933"; //线上国内服务器
#endif

   //cmd 
    public  static string LOGIN_CMD = "user.login";
	public  static string BIND_CMD = "user.bind";
	public  static string UNBIND_CMD = "bind.cancel";
    public  static string LEVEL_UP_CMD = "level.up";
    public  static string LEVEL_FAIL_CMD = "level.fail";
    public  static string SAVE_STORY_CMD = "story.unlock";
    public  static string ITEM_DEL_CMD = "item.del";
    public  static string ITEM_BUY_CMD = "item.buy";
    public  static string LEVEL_START_CMD = "level.start";
    public  static string LEVEL_END = "level.end";
    public  static string HEART_BUY = "heart.buy";
    public  static string LEVEL_FIVEMORE = "level.fivemore";
    public 	static string CHANGE_NAME = "user.modify.nickName";
	public 	static string SAVE_OFF_LINE = "offline.save";
	public  static string SAVE_DAY_INFO = "sevenDay.info";
	public 	static string SAVE_DAY_AWARD = "sevenDay.award";
    public  static string MAKE_POINT_ELIMINATEGUIDE = "eliminateGuide.makepoint";
    public  static string MAKE_POINT_CLICK = "buttonClick.makepoint";
}



