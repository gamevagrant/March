//#define NETWORK_LINE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerGlobal
{
#if NETWORK_LINE
    public const string Adr = "xxx.xxx.xxx.xxx";  //外网ip
#else
    public static string Adr = "10.1.15.234";  //内网ip
    public static int Port = 14120;
#endif

  //  public static string loginUrl_inner = "http://10.1.33.20:8080";
    public static string loginUrl = "http://123.206.90.153:9933";
    //public static string loginUrl = "http://10.1.33.220:8080";
    public static string test = "http://203.195.148.41/homeserver/qzone";

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
    public static string MAKE_POINT_ELIMINATEGUIDE = "eliminateGuide.makepoint";
}



