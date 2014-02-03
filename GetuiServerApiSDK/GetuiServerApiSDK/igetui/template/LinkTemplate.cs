using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.gexin.rp.sdk.dto;

/*
 * Create Date: 2013-04-20
 * Create By: johny.zheng
 * Version: V1.0.0
 * Update Date: 2013-06-08
 * Company: mrtech
 * dev.igetui.com
 */

namespace com.igetui.api.openservice.igetui.template
{
    public class LinkTemplate : ITemplate
    {

        private static String text;
        private static String title;
        private static String logo;
        private static String logoURL;
        private static String url;
        private Boolean isRing = true;
        private Boolean isVibrate = true;
        private Boolean isClearable = true;
        private String appId;
        private String appKey;
        private PushInfo pushInfo;

        public Transparent getTransparent()
        {
            //
            Transparent transparent = Transparent.CreateBuilder()
                    .SetId("")
                    .SetMessageId("")
                    .SetTaskId("")
                    .SetAction("pushmessage")
                    .AddRangeActionChain(getActionChain())
                    .SetPushInfo(getPushInfo())
                    .SetAppId(appId)
                    .SetAppKey(appKey).Build();
            //
            return transparent;
        }

        public PushInfo getPushInfo()
        {
            if (pushInfo == null)
            {

                pushInfo = PushInfo.CreateBuilder()
                        .SetActionKey("")
                        .SetBadge("")
                        .SetMessage("")
                        .SetSound("")
                        .Build();
            }

            return pushInfo;
        }

        public void setPushInfo(String actionLocKey, int badge, String message, String sound, String payload,
            String locKey, String locArgs, String launchImage)
        {
            PushInfo.Builder builder = PushInfo.CreateBuilder().SetActionLocKey(actionLocKey).SetBadge(Convert.ToString(badge)).SetMessage(message);

            if (sound != null)
            {
                builder.SetSound(sound);
            }
            if (payload != null)
            {
                builder.SetPayload(payload);
            }
            if (locKey != null)
            {
                builder.SetLocKey(locKey);
            }
            if (locArgs != null)
            {
                builder.SetLocArgs(locArgs);
            }
            if (launchImage != null)
            {
                builder.SetLaunchImage(launchImage);
            }
            pushInfo = builder.Build();
        }

        public Boolean IsRing
        {
            get { return isRing; }
            set { isRing = value; }
        }

        public Boolean IsVibrate
        {
            get { return isVibrate; }
            set { isVibrate = value; }
        }

        public Boolean IsClearable
        {
            get { return isClearable; }
            set { isClearable = value; }
        }

        public String AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        public String AppKey
        {
            get { return appKey; }
            set { appKey = value; }
        }

        public String Text
        {
            get { return text; }
            set { text = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public String Logo
        {
            get { return logo; }
            set { logo = value; }
        }

        public String LogoURL
        {
            get { return logoURL; }
            set { logoURL = value; }
        }

        public String Url
        {
            get { return url; }
            set { url = value; }
        }

        protected List<ActionChain> getActionChain()
        {
            List<ActionChain> actionChains = new List<ActionChain>();
            // 设置actionChain
            ActionChain actionChain1 = ActionChain.CreateBuilder()
                    .SetActionId(1)
                    .SetType(ActionChain.Types.Type.Goto)
                    .SetNext(10000).Build();
            // 通知
            ActionChain actionChain2 = ActionChain.CreateBuilder()
                    .SetActionId(10000)
                    .SetType(ActionChain.Types.Type.notification)
                    .SetTitle(title)
                    .SetText(text)
                    .SetLogo(logo)
                    .SetLogoURL(logoURL)
                    .SetRing(isRing)
                    .SetClearable(isClearable)
                    .SetBuzz(isVibrate)
                    .SetNext(10010).Build();
            // goto
            ActionChain actionChain3 = ActionChain.CreateBuilder()
                    .SetActionId(10010)
                    .SetType(ActionChain.Types.Type.Goto)
                    .SetNext(10030).Build();
            // 启动web
            ActionChain actionChain4 = ActionChain.CreateBuilder()
                    .SetActionId(10030)
                    .SetType(ActionChain.Types.Type.startweb)
                    .SetUrl(url)
                    .SetNext(100).Build();
            // 结束
            ActionChain actionChain5 = ActionChain.CreateBuilder()
                    .SetActionId(100)
                    .SetType(ActionChain.Types.Type.eoa)
                    .Build();

            actionChains.Add(actionChain1);
            actionChains.Add(actionChain2);
            actionChains.Add(actionChain3);
            actionChains.Add(actionChain4);
            actionChains.Add(actionChain5);

            return actionChains;
        }

        public String getTransmissionContent()
        {

            return "";
        }

        public String getPushType()
        {
            return "NotifyMsg";
        }
    }
}
