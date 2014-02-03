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
    public class TransmissionTemplate : ITemplate
    {
        private String transmissionType;
        private String transmissionContent;
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

        public String TransmissionType
        {
            get { return transmissionType; }
            set { transmissionType = value; }
        }

        public String TransmissionContent
        {
            get { return transmissionContent; }
            set { transmissionContent = value; }
        }

        protected List<ActionChain> getActionChain()
        {
            List<ActionChain> actionChains = new List<ActionChain>();
            // 设置actionChain
            ActionChain actionChain1 = ActionChain.CreateBuilder()
                .SetActionId(1)
                .SetType(ActionChain.Types.Type.Goto)
                .SetNext(10030)
                .Build();
            //
            AppStartUp appStartUp = AppStartUp.CreateBuilder()
                    .SetAndroid("")
                    .SetSymbia("")
                    .SetIos("")
                    .Build();
            //启动app
            ActionChain actionChain2 = ActionChain.CreateBuilder()
                    .SetActionId(10030)
                    .SetType(ActionChain.Types.Type.startapp)
                    .SetAppid("")
                    .SetAutostart(("1".Equals(transmissionType)) ? true : false)
                    .SetAppstartupid(appStartUp)
                    .SetFailedAction(100)
                    .SetNext(100)
                    .Build();
            //结束
            ActionChain actionChain3 = ActionChain.CreateBuilder()
                    .SetActionId(100)
                    .SetType(ActionChain.Types.Type.eoa)
                    .Build();

            actionChains.Add(actionChain1);
            actionChains.Add(actionChain2);
            actionChains.Add(actionChain3);

            return actionChains;
        }

        public String getTransmissionContent()
        {

            return transmissionContent;
        }

        public String getPushType()
        {
            return "TransmissionMsg";
        }
    }
}
