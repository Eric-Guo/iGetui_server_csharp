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

namespace com.igetui.api.openservice
{
    public interface ITemplate
    {
        Transparent getTransparent();

        String getTransmissionContent();

        String getPushType();
    }
}
