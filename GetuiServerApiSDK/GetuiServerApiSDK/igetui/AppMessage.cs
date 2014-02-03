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
    public class AppMessage : Message
    {
        private List<String> appIdList;
        private List<String> phoneTypeList = new List<String>();
        private List<String> provinceList = new List<String>();

        public List<String> AppIdList
        {
            get { return appIdList; }
            set { appIdList = value; }
        }

        public List<String> PhoneTypeList
        {
            get { return phoneTypeList; }
            set { phoneTypeList = value; }
        }

        public List<String> ProvinceList
        {
            get { return provinceList; }
            set { provinceList = value; }
        }
    }
}
