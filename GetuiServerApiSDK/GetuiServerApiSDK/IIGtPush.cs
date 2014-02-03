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
    public interface IIGtPush
    {
        /**
	 * 与服务其建立连接
	 * 
	 * @return true -- 连接成功 false -- 连接失败
	 * @throws IOException
	 *         出现任何连接异常
	 */
	Boolean connect();

	/**
	 * 关闭连接
	 * 
	 * @throws IOException
	 */
	void close();

	/**
	 * 推送一条消息到某个客户端
	 * 
	 * @param message
	 *        消息
	 * @param target
	 *        目标用户
	 * @return 推送结果
	 */
	String pushMessageToSingle(SingleMessage message, Target target);

	/**
	 * 推送消息到一组用户，该方法先从listProvider的实现类中获得一批用户。 然后发送到个推上，每推送完一批就再次从listProvider中获得一批用户。 直到，listProvider不再返回任何用户。
	 * 同时每发送一次，就会触发IPushEventListener的事件可以实现该接口获得发送的结果信息
	 * 
	 * @param message
	 *        推送消息
	 * @param listProvider
	 *        列表提供类
	 * @param listener
	 *        推送结果监听类
	 */
	void pushMessageToList(ListMessage message, IListProvider listProvider, IPushEventListener listener);

	/**
	 * 批量推送前需要通过这个接口向服务其申请一个“ContentID”
	 * 
	 * @param message
	 * @return
	 */
	String getContentId(ListMessage message);
	
	/**
	 * 根据contentId取消上传的消息体
	 * 
	 * @param contentId
	 *        {@link IIGtPush.cancleContentId(String contentId)}返回是否成功
	 * @return
	 */
	Boolean cancleContentId(String contentId);

	/**
	 * 通过{@link IIGtPush.getContentId(ListMessage message)}接口 获得“ContentID”后，通过这个方法实现批量推送。
	 * 
	 * @param contentId
	 *        {@link IIGtPush.getContentId(ListMessage message)}接口返回的ID
	 * @param targetList
	 *        目标用户列表
	 * @return
	 */
	String pushMessageToList(String contentId, List<Target> targetList);

	/**
	 * 推送消息到条件限定的用户，限定条件由AppMessage中的参数控制， 如果没有任何限定条件，将会此App的对所有用户进行推送
	 * 
	 * @param message
	 *        推送消息
	 * @return
	 */
	String pushMessageToApp(AppMessage message);
    }
}
