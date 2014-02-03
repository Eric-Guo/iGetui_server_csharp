using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Create Date: 2013-04-20
 * Create By: johny.zheng
 * Version: V1.0.0
 * Update Date: 2013-06-08
 * Company: mrtech
 * dev.igetui.com
 */

namespace com.igetui.api.openservice.igetui
{
    public class Message : IPacket
    {
        private Boolean isOffline;
        public Boolean IsOffline
        {
            get { return isOffline; }
            set { isOffline = value; }
        }
        /*
         * 过多久该消息离线失效（单位秒） 支持1-72小时*3600秒，默认1小时
         */
        private long offlineExpireTime;
        public long OfflineExpireTime
        {
            get { return offlineExpireTime; }
            set { offlineExpireTime = value; }
        }

        private ITemplate data;
        public ITemplate Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
