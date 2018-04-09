using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Common
{
    public class TimeUtil:Singleton<TimeUtil>
    {
        public  DateTime DtBaseZone ;
        private  DateTime DtLocalTime;

        private static DateTime startDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

        public double ServerDeltaTime;
        public override void Init()
        {
            base.Init();
            DtBaseZone = new DateTime(1970, 1, 1);
            DtLocalTime = TimeZone.CurrentTimeZone.ToLocalTime(DtBaseZone);
        }
        /// <summary>
        /// 时间戳转date time
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public  DateTime ConvertUnixToDateTime(ulong unixTimeStamp)
        {
            DateTime dt = DtLocalTime.AddMilliseconds(unixTimeStamp);
            return dt;
        }
        /// <summary>
        /// date time 转时间戳 注意修正serverdelta
        /// </summary>
        /// <returns></returns>
        public long ConvertDateTimeToUnix()
        {
            long timeStamp = (long)(DateTime.Now.AddMilliseconds(ServerDeltaTime) - DtLocalTime).TotalMilliseconds; // 相差秒数
            return timeStamp;
        }
        /// <summary>
        /// ms转换hour，min，s 
        /// </summary>
        /// <returns></returns>
        public  String formatLongToTimeStr(uint _time)
        {
            String str = "";
            uint hour = 0;
            uint minute = 0;
            uint second = 0;
            second = _time / 1000;

            if (second > 60)
            {
                minute = second / 60;
                second = second % 60;
            }
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }
            return (hour.ToString() + "小时" + minute.ToString() + "分钟"
                + second.ToString() + "秒");
        }

        public ulong DateTimeToUnixTimestamp()
        {
          /*  var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);*/
            System.TimeSpan ts = System.DateTime.Now - startDateTime;
            return (ulong)ts.TotalMilliseconds;

        }


    }
}
