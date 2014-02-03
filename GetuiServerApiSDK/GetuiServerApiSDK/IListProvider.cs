using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.igetui.api.openservice.igetui;

/*
 * Create Date: 2013-04-20
 * Create By: johny.zheng
 * Version: V1.0.0
 * Update Date: 2013-06-08
 * Company: mrtech
 * dev.igetui.com
 */

namespace com.igetui.api.openservice
{
    public interface IListProvider
    {
        List<Target> getClientList(int page);
    }
}
