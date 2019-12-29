using UnityEngine;

namespace playpod.client.sdk.unity.Share
{
    /**
 *
 *<div style='width: 100%;text-align: right'> انواع پیام های رد درخواست</div>
 * */
    public class MatchRequestRejectTypes : MonoBehaviour
    {
        /**
     *
     *<div style='width: 100%;text-align: right'> عدم پذیرش درخواست توسط کاربر</div>
     * */
        public const int UserNotAccept = 3;

        /**
         *
         *<div style='width: 100%;text-align: right'>  عدم نصب بودن برنامه </div>
         * */
        public const int AppNotInstalled = 4;

        /**
         *
         *<div style='width: 100%;text-align: right'>تداخل ورژن برنامه نصب شده </div>
         * */
        public const int UserVersionConflict = 5;

        /**
         *
         *<div style='width: 100%;text-align: right'> مشغول بودن کاربر به بازی </div>
         * */
        public const int UserIsBusy = 6;

        /**
         *
         *<div style='width: 100%;text-align: right'>دریافت متن پیام </div>
         *
         * @param type نوع پیام
         * @param  langType  نوع زبان
         * @return متن پیام
         * */
        public static string GetMessage(int type, LanguageTypes langType)
        {
            var message = "";

            switch (type)
            {
                case UserNotAccept:
                    if (langType.Equals(LanguageTypes.En))
                    {
                        message = "user not accept your request";
                    }
                    else
                    {
                        message = "با درخواست شما موافقت نشده است";
                    }

                    break;
                case AppNotInstalled:
                    if (langType.Equals(LanguageTypes.En))
                    {
                        message = "app not installed.";
                    }
                    else
                    {
                        message = "کاربر برنامه را نصب نکرده است";
                    }

                    break;

                case UserVersionConflict:
                    if (langType.Equals(LanguageTypes.En))
                    {
                        message = "user app version not update";
                    }
                    else
                    {
                        message = "نسخه بازی کاربر مورد نظر به روز نمی باشد.";
                    }

                    break;

                case UserIsBusy:
                    if (langType.Equals(LanguageTypes.En))
                    {
                        message = "user is busy.";
                    }
                    else
                    {
                        message = "کاربر مورد نظر مشغول انجام بازی می باشد.";
                    }

                    break;
            }

            return message;
        }
    }
}