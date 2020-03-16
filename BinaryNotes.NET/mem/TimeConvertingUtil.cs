using System;

namespace mmwl.AMQP.ASN1Mappers
{
    public class TimeConvertingUtil
    {
        public static DateTime GetDateTime( String utcTime )
        {
            if( utcTime.Length != 12 )
            {
                throw new FormatException("Invalid time format. Correct format is yyMMddHHmmss. This value is:" + utcTime);
            }

            var year = Int32.Parse(utcTime.Substring(0, 2)) + 2000;
            var month = Int32.Parse(utcTime.Substring(2, 2));
            var day = Int32.Parse(utcTime.Substring(4, 2));
            var hour = Int32.Parse(utcTime.Substring(6, 2));
            var minutes = Int32.Parse(utcTime.Substring(8, 2));
            var seconds = Int32.Parse(utcTime.Substring(10, 2));

            return new DateTime( year, month, day, hour, minutes,seconds );
        }

        public static DateTime GetDateTimeWithOptimalSeconds(String utcTime)
        {
            if (utcTime.Length != 12 && utcTime.Length != 10)
            {
                throw new FormatException("Invalid time format. Correct format is yyMMddHHmmss. This value is:" + utcTime);
            }

            var year = Int32.Parse(utcTime.Substring(0, 2)) + 2000;
            var month = Int32.Parse(utcTime.Substring(2, 2));
            var day = Int32.Parse(utcTime.Substring(4, 2));
            var hour = Int32.Parse(utcTime.Substring(6, 2));
            var minutes = Int32.Parse(utcTime.Substring(8, 2));
            var seconds = (utcTime.Length == 12) ? Int32.Parse(utcTime.Substring(10, 2)) : 0;

            return new DateTime(year, month, day, hour, minutes, seconds);
        }

        public static DateTime GetLocalDateTime(String utcTime)
        {
            return GetDateTime(utcTime).ToLocalTime();
        }

        public static String GetStringDate( DateTime datetime )
        {
            return datetime.ToString("yyMMddHHmmss");
        }

        public static String GetStringDateWithoutSeconds(DateTime datetime)
        {
            return datetime.ToString("yyMMddHHmm");
        }

        public static string DateTimeToString(DateTime utc)
        {
            return utc.ToString("yyMMddHHmmss");
        }


    }
}
