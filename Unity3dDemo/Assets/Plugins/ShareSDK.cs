using UnityEngine;
using System.Collections;
using System;  
using System.Collections.Generic;  
using System.IO;  
using System.Text; 

namespace cn.sharesdk.unity3d
{
	/// <summary>
	/// Platform type.
	/// </summary>
	public enum PlatformType
	{
		SinaWeibo = 1,			//Sina Weibo         
		TencentWeibo = 2,		//Tencent Weibo     
		SohuWeibo = 3,			//Sohu Weibo         
		NetEaseWeibo = 4,		//NetEase Weibo         
		DouBan = 5,				//Dou Ban           
		QZone = 6, 				//QZone           
		Renren = 7,				//Ren Ren           
		Kaixin = 8,				//Kai Xin          
		Pengyou = 9,			//Friends          
		Facebook = 10,			//Facebook         
		Twitter = 11,			//Twitter         
		Evernote = 12,			//Evernote        
		Foursquare = 13,		//Foursquare      
		GooglePlus = 14,		//Google+       
		Instagram = 15,			//Instagram      
		LinkedIn = 16,			//LinkedIn       
		Tumblr = 17,			//Tumblr         
		Mail = 18, 				//Mail          
		SMS = 19,				//SMS           
		Print = 20, 			//Print       
		Copy = 21,				//Copy             
		WeChatSession = 22,		//WeChat Session    
		WeChatTimeline = 23,	//WeChat Timeline   
		QQ = 24,				//QQ              
		Instapaper = 25,		//Instapaper       
		Pocket = 26,			//Pocket           
		YouDaoNote = 27, 		//You Dao Note     
		SohuKan = 28, 			//Sohu Sui Shen Kan         
		Pinterest = 30, 		//Pinterest     
		Flickr = 34,			//Flickr          
		Dropbox = 35,			//Dropbox          
		VKontakte = 36,			//VKontakte       
		WeChatFav = 37,			//WeChat Favorited        
		YiXinSession = 38, 		//YiXin Session   
		YiXinTimeline = 39,		//YiXin Timeline   
		YiXinFav = 40,			//YiXin Favorited  
		MingDao = 41,          	//明道
		Line = 42,             	//Line
		WhatsApp = 43,         	//Whats App
		Any = 99 				//Any Platform 
	}
	
	/// <summary>
	/// Response state.
	/// </summary>
	public enum ResponseState
	{
		Begin = 0,				//Begin
		Success = 1,			//Success
		Fail = 2,				//Failure
		Cancel = 3				//Cancel
	}
	
	/// <summary>
	/// Menu arrow direction.
	/// </summary>
	public enum MenuArrowDirection
	{
		Up = 1 << 0,						//Up
		Down = 1 << 1,						//Down
		Left = 1 << 2,						//Left
		Right = 1 << 3,						//Right
		Any =  Up | Down | Left | Right,	//Any
		Unknown = int.MaxValue				//Unkonwn
	}
	
	/// <summary>
	/// Content type.
	/// </summary>
    public enum ContentType
    {
		Text = 0, 				//Text
		Image = 1,				//Image 
		News = 2,				//News 
		Music = 3,				//Music 
		Video = 4,				//Video 
		App = 5, 				//App
		NonGif = 6,				//Non Gif Image 
		Gif = 7,				//Gif Image 
		File = 8				//File
    };

	/// <summary>
	/// Inherited value.
	/// </summary>
	public class InheritedValue
	{
		public static readonly InheritedValue VALUE = new InheritedValue();
	}
	
	/// <summary>
	/// Auth result event.
	/// </summary>
	public delegate void AuthResultEvent (ResponseState state, PlatformType type, Hashtable error);
	
	/// <summary>
	/// Get user info result event.
	/// </summary>
	public delegate void GetUserInfoResultEvent (ResponseState state, PlatformType type, Hashtable userInfo, Hashtable error);
	
	/// <summary>
	/// Share result event.
	/// </summary>
	public delegate void ShareResultEvent (ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end);

	/// <summary>
	/// Share result event.
	/// </summary>
	public delegate void GetFriendsResultEvent (ResponseState state, PlatformType type, ArrayList users,  Hashtable error);

	/// <summary>
	/// Share result event.
	/// </summary>
	public delegate void GetCredentialResultEvent (ResponseState state, PlatformType type, Hashtable credential,  Hashtable error);

	/// <summary>
	/// ShareSDK.
	/// </summary>
	public class ShareSDK : MonoBehaviour 
	{

		private static readonly string INHERITED_VALUE = "{inherited}";

		/// <summary>
		/// callback the specified data.
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		private void _callback (string data)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.callback (data);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				AndroidUtils.getInstance().onActionCallback(data);
#endif
			}
		}
		
		/// <summary>
		/// Sets the name of the callback object.
		/// </summary>
		/// <param name='objectName'>
		/// Object name.
		/// </param>
		public static void setCallbackObjectName (string objectName)
		{
			if (objectName == null)
			{
				objectName = "Main Camera";
			}
			
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.setCallbackObjectName(objectName);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				AndroidUtils.getInstance().setGameObject(objectName);
#endif
			}
		}
		
		/// <summary>
		/// Initialize ShareSDK
		/// </summary>
		/// <param name='appKey'>
		/// App key.
		/// </param>
		public static void open (string appKey)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.open (appKey);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				//AndroidUtils.getInstance().initSDK();
				// if you don't add ShareSDK.xml in your assets folder, use the following line
				AndroidUtils.getInstance().initSDK(appKey);
#endif
			}
		}
		
		/// <summary>
		/// Close this instance.
		/// </summary>
		public static void close ()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.close ();
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				AndroidUtils.getInstance().stopSDK();
#endif
			}
		}
		
		/// <summary>
		/// Sets the platform config.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='configInfo'>
		/// Config info.
		/// </param>
		public static void setPlatformConfig (PlatformType type, Hashtable configInfo)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.setPlatformConfig (type, configInfo);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				// if you don't add ShareSDK.xml in your assets folder, use the following line
				AndroidUtils.getInstance().setPlatformConfig((int)type, configInfo);
#endif
			}
		}
		
		/// <summary>
		/// Authorize the specified type, observer and resultHandler.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='observer'>
		/// Observer.
		/// </param>
		/// <param name='resultHandler'>
		/// Result handler.
		/// </param>
		public static void authorize (PlatformType type, AuthResultEvent resultHandler)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.authorize (type, resultHandler);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				AndroidUtils.getInstance().authorize((int) type, resultHandler);
#endif
			}
		}
		
		/// <summary>
		/// Cancel authorized
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		public static void cancelAuthorie (PlatformType type)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.cancelAuthorie (type);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				AndroidUtils.getInstance().removeAccount((int) type);
#endif
			}
		}
		
		/// <summary>
		/// Has authorized
		/// </summary>
		/// <returns>
		/// true has authorized, otherwise not authorized.
		/// </returns>
		/// <param name='type'>
		/// Type.
		/// </param>
		public static bool hasAuthorized (PlatformType type)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				return ios.ShareSDK.hasAuthorized (type);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				return AndroidUtils.getInstance().isValid((int) type);
#endif
			}
			
			return false;
		}
		
		/// <summary>
		/// Gets the user info.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public static void getUserInfo (PlatformType type, GetUserInfoResultEvent resultHandler)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.getUserInfo (type, resultHandler);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				AndroidUtils.getInstance().showUser((int) type, resultHandler);
#endif
			}
		}
	
		/// <summary>
		/// Shares the content.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='content'>
		/// Content.
		/// </param>
		/// <param name='resultHandler'>
		/// Callback.
		/// </param>
		public static void shareContent (PlatformType type, Hashtable content, ShareResultEvent resultHandler)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.shareContent (type, content, resultHandler);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				AndroidUtils.getInstance().share((int) type, content, resultHandler);
#endif
			}
		}
		
		/// <summary>
		/// share content to multiple platform
		/// </summary>
		/// <param name='types'>
		/// Types.
		/// </param>
		/// <param name='content'>
		/// Content.
		/// </param>
		/// <param name='resultHandler'>
		/// Callback.
		/// </param>
		public static void oneKeyShareContent (PlatformType[] types, Hashtable content, ShareResultEvent resultHandler)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.oneKeyShareContent (types, content, resultHandler);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				foreach (PlatformType type in types) 
				{
					AndroidUtils.getInstance().share((int) type, content, resultHandler);
				}
#endif
			}
		}
		
		/// <summary>
		/// Shows the share menu.
		/// </summary>
		/// <param name='types'>
		/// Types.
		/// </param>
		/// <param name='content'>
		/// Content.
		/// </param>
		/// <param name='x'>
		/// X. only for iPad
		/// </param>
		/// <param name='y'>
		/// Y. only for iPad
		/// </param>
		/// <param name='direction'>
		/// Direction. only for iPad
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public static void showShareMenu (PlatformType[] types, Hashtable content, int x, int y, MenuArrowDirection direction, ShareResultEvent resultHandler)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.showShareMenu (types, content, x, y, direction, resultHandler);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				// The android platform doesn't have this method, you can modify the PlatformGridView class of OnekeyShare in java layer to achieve this function
				AndroidUtils.getInstance().onekeyShare(content, resultHandler);
#endif
			}
		}
		
		/// <summary>
		/// Shows the share view.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='content'>
		/// Content.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public static void showShareView (PlatformType type, Hashtable content, ShareResultEvent resultHandler)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
#if UNITY_IPHONE
				ios.ShareSDK.showShareView (type, content, resultHandler);
#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
#if UNITY_ANDROID
				AndroidUtils.getInstance().onekeyShare(content, resultHandler);
				// if you want to skip the PlatformGridView, use the following line
				// AndroidUtils.getInstance().onekeyShare((int) type, content, resultHandler);
#endif
			}
		}

		/// <summary>
		/// Shows the share view.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='page'>
		/// Content.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public static void getFriends (PlatformType type, Hashtable page, GetFriendsResultEvent resultHandler)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				#if UNITY_IPHONE
				ios.ShareSDK.getFriends (type, page, resultHandler);
				#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				#if UNITY_ANDROID
				#endif
			}
		}

		/// <summary>
		/// Shows the share view.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='content'>
		/// Content.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public static void getCredential (PlatformType type, GetCredentialResultEvent resultHandler)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				#if UNITY_IPHONE
				ios.ShareSDK.getCredential (type, resultHandler);
				#endif
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				#if UNITY_ANDROID
				#endif
			}
		}

		/// <summary>
		/// Customs the content of the sina weibo share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		/// <param name="location">Location.</param>
		public static void customSinaWeiboShareContent (Hashtable content, object message, object image, object location)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (location != null)
				{
					if (location is InheritedValue)
					{
						unit.Add("location", INHERITED_VALUE);
					}
					else
					{
						unit.Add("location", location);
					}
				}

				content.Add(PlatformType.SinaWeibo, unit);
			}
		}

		/// <summary>
		/// Customs the content of the tencent weibo share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		/// <param name="location">Location.</param>
		public static void customTencentWeiboShareContent (Hashtable content, object message, object image, object location)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (location != null)
				{
					if (location is InheritedValue)
					{
						unit.Add("location", INHERITED_VALUE);
					}
					else
					{
						unit.Add("location", location);
					}
				}
				content.Add(PlatformType.TencentWeibo, unit);
			}
		}

		/// <summary>
		/// Customs the content of the sohu weibo share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		public static void customSohuWeiboShareContent (Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				content.Add(PlatformType.SohuWeibo, unit);
			}
		}

		/// <summary>
		/// Customs the content of the net ease weibo share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		public static void customNetEaseWeiboShareContent (Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				content.Add(PlatformType.NetEaseWeibo, unit);
			}
		}

		/// <summary>
		/// Customs the content of the dou ban share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		public static void customDouBanShareContent (Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				content.Add(PlatformType.DouBan, unit);
			}
		}

		/// <summary>
		/// Customs the content of the Q zone share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="title">Title.</param>
		/// <param name="url">URL.</param>
		/// <param name="site">Site.</param>
		/// <param name="fromUrl">From URL.</param>
		/// <param name="comment">Comment.</param>
		/// <param name="summary">Summary.</param>
		/// <param name="image">Image.</param>
		/// <param name="type">Type.</param>
		/// <param name="playUrl">Play URL.</param>
		/// <param name="nswb">Nswb.</param>
		public static void customQZoneShareContent (Hashtable content, object title, object url, object site, object fromUrl, object comment, object summary, object image, object type, object playUrl, object nswb)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (site != null)
				{
					if (site is InheritedValue)
					{
						unit.Add("site", INHERITED_VALUE);
					}
					else
					{
						unit.Add("site", site);
					}
				}
				if (fromUrl != null)
				{
					if (fromUrl is InheritedValue)
					{
						unit.Add("fromUrl", INHERITED_VALUE);
					}
					else
					{
						unit.Add("fromUrl", fromUrl);
					}
				}
				if (comment != null)
				{
					if (fromUrl is InheritedValue)
					{
						unit.Add("comment", INHERITED_VALUE);
					}
					else
					{
						unit.Add("comment", comment);
					}
				}
				if (summary != null)
				{
					if (fromUrl is InheritedValue)
					{
						unit.Add("summary", INHERITED_VALUE);
					}
					else
					{
						unit.Add("summary", summary);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (type != null)
				{
					if (type is InheritedValue)
					{
						unit.Add("type", INHERITED_VALUE);
					}
					else
					{
						unit.Add("type", type);
					}
				}
				if (playUrl != null)
				{
					if (type is InheritedValue)
					{
						unit.Add("playUrl", INHERITED_VALUE);
					}
					else
					{
						unit.Add("playUrl", playUrl);
					}
				}
				if (nswb != null)
				{
					if (type is InheritedValue)
					{
						unit.Add("nswb", INHERITED_VALUE);
					}
					else
					{
						unit.Add("nswb", nswb);
					}
				}
				content.Add(PlatformType.QZone, unit);
			}
		}

		/// <summary>
		/// Customs the content of the renren share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="url">URL.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		/// <param name="caption">Caption.</param>
		public static void customRenrenShareContent (Hashtable content, object name, object description, object url, object message, object image, object caption)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (name != null)
				{
					if (name is InheritedValue)
					{
						unit.Add("name", INHERITED_VALUE);
					}
					else
					{
						unit.Add("name", name);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						unit.Add("description", INHERITED_VALUE);
					}
					else
					{
						unit.Add("description", description);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (caption != null)
				{
					if (caption is InheritedValue)
					{
						unit.Add("caption", INHERITED_VALUE);
					}
					else
					{
						unit.Add("caption", caption);
					}
				}
				content.Add(PlatformType.Renren, unit);
			}
		}

		/// <summary>
		/// Customs the content of the kaixin share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		public static void customKaixinShareContent (Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				content.Add(PlatformType.Kaixin, unit);
			}
		}

		/// <summary>
		/// Customs the content of the facebook share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		public static void customFacebookShareContent (Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				content.Add(PlatformType.Facebook, unit);
			}
		}

		/// <summary>
		/// Customs the content of the twitter share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		/// <param name="location">Location.</param>
		public static void customTwitterShareContent (Hashtable content, object message, object image, object location)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (location != null)
				{
					if (location is InheritedValue)
					{
						unit.Add("location", INHERITED_VALUE);
					}
					else
					{
						unit.Add("location", location);
					}
				}
				content.Add(PlatformType.Twitter, unit);
			}
		}

		/// <summary>
		/// Customs the content of the evernote share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="title">Title.</param>
		/// <param name="resource">Resource.</param>
		/// <param name="notebookGuid">Notebook GUID.</param>
		/// <param name="tagsGuid">Tags GUID.</param>
		public static void customEvernoteShareContent (Hashtable content, object message, object title, object resource, object notebookGuid, object tagsGuid)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (resource != null)
				{
					if (resource is InheritedValue)
					{
						unit.Add("resource", INHERITED_VALUE);
					}
					else
					{
						unit.Add("resource", resource);
					}
				}
				if (notebookGuid != null)
				{
					if (notebookGuid is InheritedValue)
					{
						unit.Add("notebookGuid", INHERITED_VALUE);
					}
					else
					{
						unit.Add("notebookGuid", notebookGuid);
					}
				}
				if (tagsGuid != null)
				{
					if (tagsGuid is InheritedValue)
					{
						unit.Add("tagsGuid", INHERITED_VALUE);
					}
					else
					{
						unit.Add("tagsGuid", tagsGuid);
					}
				}
				content.Add(PlatformType.Evernote, unit);
			}
		}

		/// <summary>
		/// Customs the content of the google plus share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		/// <param name="url">URL.</param>
		/// <param name="deepLinkId">Deep link identifier.</param>
		/// <param name="title">Title.</param>
		/// <param name="description">Description.</param>
		/// <param name="thumbnail">Thumbnail.</param>
		public static void customGooglePlusShareContent (Hashtable content, object message, object image, object url, object deepLinkId, object title, object description, object thumbnail)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (deepLinkId != null)
				{
					if (deepLinkId is InheritedValue)
					{
						unit.Add("deepLinkId", INHERITED_VALUE);
					}
					else
					{
						unit.Add("deepLinkId", deepLinkId);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						unit.Add("description", INHERITED_VALUE);
					}
					else
					{
						unit.Add("description", description);
					}
				}
				if (thumbnail != null)
				{
					if (thumbnail is InheritedValue)
					{
						unit.Add("thumbnail", INHERITED_VALUE);
					}
					else
					{
						unit.Add("thumbnail", thumbnail);
					}
				}
				content.Add(PlatformType.GooglePlus, unit);
			}
		}

		/// <summary>
		/// Customs the content of the instagram share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		public static void customInstagramShareContent (Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				content.Add(PlatformType.Instagram, unit);
			}
		}

		/// <summary>
		/// Customs the content of the linked in share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="comment">Comment.</param>
		/// <param name="title">Title.</param>
		/// <param name="description">Description.</param>
		/// <param name="url">URL.</param>
		/// <param name="image">Image.</param>
		/// <param name="visibility">Visibility.</param>
		public static void customLinkedInShareContent (Hashtable content, object comment, object title, object description, object url, object image, object visibility)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (comment != null)
				{
					if (comment is InheritedValue)
					{
						unit.Add("comment", INHERITED_VALUE);
					}
					else
					{
						unit.Add("comment", comment);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						unit.Add("description", INHERITED_VALUE);
					}
					else
					{
						unit.Add("description", description);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (visibility != null)
				{
					if (visibility is InheritedValue)
					{
						unit.Add("visibility", INHERITED_VALUE);
					}
					else
					{
						unit.Add("visibility", visibility);
					}
				}
				content.Add(PlatformType.LinkedIn, unit);
			}
		}

		/// <summary>
		/// Customs the content of the thumblr share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="text">Text.</param>
		/// <param name="title">Title.</param>
		/// <param name="image">Image.</param>
		/// <param name="url">URL.</param>
		/// <param name="blogName">Blog name.</param>
		public static void customTumblrShareContent (Hashtable content, object text, object title, object image, object url, object blogName)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (text != null)
				{
					if (text is InheritedValue)
					{
						unit.Add("text", INHERITED_VALUE);
					}
					else
					{
						unit.Add("text", text);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (blogName != null)
				{
					if (blogName is InheritedValue)
					{
						unit.Add("blogName", INHERITED_VALUE);
					}
					else
					{
						unit.Add("blogName", blogName);
					}
				}
				content.Add(PlatformType.Tumblr, unit);
			}
		}

		/// <summary>
		/// Customs the content of the mail share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="subject">Subject.</param>
		/// <param name="message">Message.</param>
		/// <param name="isHTML">Is HTM.</param>
		/// <param name="attachments">Attachments.</param>
		/// <param name="to">To.</param>
		/// <param name="cc">Cc.</param>
		/// <param name="bcc">Bcc.</param>
		public static void customMailShareContent (Hashtable content, object subject, object message, object isHTML, object attachments, object to, object cc, object bcc)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (subject != null)
				{
					if (subject is InheritedValue)
					{
						unit.Add("subject", INHERITED_VALUE);
					}
					else
					{
						unit.Add("subject", subject);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (isHTML != null)
				{
					if (isHTML is InheritedValue)
					{
						unit.Add("isHTML", INHERITED_VALUE);
					}
					else
					{
						unit.Add("isHTML", isHTML);
					}
				}
				if (attachments != null)
				{
					if (attachments is InheritedValue)
					{
						unit.Add("attachments", INHERITED_VALUE);
					}
					else
					{
						unit.Add("attachments", attachments);
					}
				}
				if (to != null)
				{
					if (to is InheritedValue)
					{
						unit.Add("to", INHERITED_VALUE);
					}
					else
					{
						unit.Add("to", to);
					}
				}
				if (cc != null)
				{
					if (cc is InheritedValue)
					{
						unit.Add("cc", INHERITED_VALUE);
					}
					else
					{
						unit.Add("cc", cc);
					}
				}
				if (bcc != null)
				{
					if (bcc is InheritedValue)
					{
						unit.Add("bcc", INHERITED_VALUE);
					}
					else
					{
						unit.Add("bcc", bcc);
					}
				}

				content.Add(PlatformType.Mail, unit);
			}
		}

		/// <summary>
		/// Customs the content of the print share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		public static void customPrintShareContent (Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				
				content.Add(PlatformType.Print, unit);
			}
		}

		/// <summary>
		/// Customs the content of the SMS share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		public static void customSMSShareContent (Hashtable content, object message)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}

				content.Add(PlatformType.SMS, unit);
			}
		}

		/// <summary>
		/// Customs the content of the copy share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		public static void customCopyShareContent (Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}

				content.Add(PlatformType.Copy, unit);
			}
		}

		/// <summary>
		/// Customs the content of the we chat session share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="type">Type.</param>
		/// <param name="message">Message.</param>
		/// <param name="title">Title.</param>
		/// <param name="url">URL.</param>
		/// <param name="thumbImage">ThumbImage.</param>
		/// <param name="image">Image.</param>
		/// <param name="musicFileUrl">Music file URL.</param>
		/// <param name="extInfo">Ext info.</param>
		/// <param name="fileData">File data.</param>
		/// <param name="emoticonData">Emoticon data.</param>
		public static void customWeChatSessionShareContent (Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData, object emoticonData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						unit.Add("type", INHERITED_VALUE);
					}
					else
					{
						unit.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						unit.Add("thumbImage", INHERITED_VALUE);
					}
					else
					{
						unit.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						unit.Add("musicFileUrl", INHERITED_VALUE);
					}
					else
					{
						unit.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						unit.Add("extInfo", INHERITED_VALUE);
					}
					else
					{
						unit.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						unit.Add("fileData", INHERITED_VALUE);
					}
					else
					{
						unit.Add("fileData", fileData);
					}
				}
				if (emoticonData != null)
				{
					if (emoticonData is InheritedValue)
					{
						unit.Add("emoticonData", INHERITED_VALUE);
					}
					else
					{
						unit.Add("emoticonData", emoticonData);
					}
				}
				
				content.Add(PlatformType.WeChatSession, unit);
			}
		}

		/// <summary>
		/// Customs the content of the we chat timeline share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="type">Type.</param>
		/// <param name="message">Message.</param>
		/// <param name="title">Title.</param>
		/// <param name="url">URL.</param>
		/// <param name="thumbImage">Thumb image.</param>
		/// <param name="image">Image.</param>
		/// <param name="musicFileUrl">Music file URL.</param>
		/// <param name="extInfo">Ext info.</param>
		/// <param name="fileData">File data.</param>
		/// <param name="emoticonData">Emoticon data.</param>
		public static void customWeChatTimelineShareContent (Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData, object emoticonData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						unit.Add("type", INHERITED_VALUE);
					}
					else
					{
						unit.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						unit.Add("thumbImage", INHERITED_VALUE);
					}
					else
					{
						unit.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						unit.Add("musicFileUrl", INHERITED_VALUE);
					}
					else
					{
						unit.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						unit.Add("extInfo", INHERITED_VALUE);
					}
					else
					{
						unit.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						unit.Add("fileData", INHERITED_VALUE);
					}
					else
					{
						unit.Add("fileData", fileData);
					}
				}
				if (emoticonData != null)
				{
					if (emoticonData is InheritedValue)
					{
						unit.Add("emoticonData", INHERITED_VALUE);
					}
					else
					{
						unit.Add("emoticonData", emoticonData);
					}
				}
				
				content.Add(PlatformType.WeChatTimeline, unit);
			}

		}

		/// <summary>
		/// Customs the content of the QQ share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="type">Type.</param>
		/// <param name="message">Message.</param>
		/// <param name="title">Title.</param>
		/// <param name="url">URL.</param>
		/// <param name="image">Image.</param>
		public static void customQQShareContent (Hashtable content, object type, object message, object title, object url, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						unit.Add("type", INHERITED_VALUE);
					}
					else
					{
						unit.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				
				content.Add(PlatformType.QQ, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the instapaper share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="url">URL.</param>
		/// <param name="title">Title.</param>
		/// <param name="description">Description.</param>
		public static void customInstapaperShareContent (Hashtable content, object url, object title, object description)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						unit.Add("description", INHERITED_VALUE);
					}
					else
					{
						unit.Add("description", description);
					}
				}
				
				content.Add(PlatformType.Instapaper, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the pocket share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="url">URL.</param>
		/// <param name="title">Title.</param>
		/// <param name="tags">Tags.</param>
		/// <param name="tweetId">Tweet identifier.</param>
		public static void customPocketShareContent (Hashtable content, object url, object title, object tags, object tweetId)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (tags != null)
				{
					if (tags is InheritedValue)
					{
						unit.Add("tags", INHERITED_VALUE);
					}
					else
					{
						unit.Add("tags", tags);
					}
				}
				if (tweetId != null)
				{
					if (tweetId is InheritedValue)
					{
						unit.Add("tweetId", INHERITED_VALUE);
					}
					else
					{
						unit.Add("tweetId", tweetId);
					}
				}
				
				content.Add(PlatformType.Pocket, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the you DAO note share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="title">Title.</param>
		/// <param name="author">Author.</param>
		/// <param name="source">Source.</param>
		/// <param name="attachments">Attachments.</param>
		public static void customYouDaoNoteShareContent (Hashtable content, object message, object title, object author, object source, object attachments)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (author != null)
				{
					if (author is InheritedValue)
					{
						unit.Add("author", INHERITED_VALUE);
					}
					else
					{
						unit.Add("author", author);
					}
				}
				if (source != null)
				{
					if (source is InheritedValue)
					{
						unit.Add("source", INHERITED_VALUE);
					}
					else
					{
						unit.Add("source", source);
					}
				}
				if (attachments != null)
				{
					if (attachments is InheritedValue)
					{
						unit.Add("attachments", INHERITED_VALUE);
					}
					else
					{
						unit.Add("attachments", attachments);
					}
				}
				
				content.Add(PlatformType.YouDaoNote, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the sohu kan share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="url">URL.</param>
		public static void customSohuKanShareContent (Hashtable content, object url)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				
				content.Add(PlatformType.SohuKan, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the pinterest share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="image">Image.</param>
		/// <param name="url">URL.</param>
		/// <param name="description">Description.</param>
		public static void customPinterestShareContent (Hashtable content, object image, object url, object description)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						unit.Add("description", INHERITED_VALUE);
					}
					else
					{
						unit.Add("description", description);
					}
				}
				
				content.Add(PlatformType.Pinterest, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the flickr share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="photo">Photo.</param>
		/// <param name="title">Title.</param>
		/// <param name="description">Description.</param>
		/// <param name="tags">Tags.</param>
		/// <param name="isPublic">Is public.</param>
		/// <param name="isFriend">Is friend.</param>
		/// <param name="isFamily">Is family.</param>
		/// <param name="safetyLevel">Safety level.</param>
		/// <param name="contentType">Content type.</param>
		/// <param name="hidden">Hidden.</param>
		public static void customFlickrShareContent (Hashtable content, object photo, object title, object description, object tags, object isPublic, object isFriend, object isFamily, object safetyLevel, object contentType, object hidden)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (photo != null)
				{
					if (photo is InheritedValue)
					{
						unit.Add("photo", INHERITED_VALUE);
					}
					else
					{
						unit.Add("photo", photo);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						unit.Add("description", INHERITED_VALUE);
					}
					else
					{
						unit.Add("description", description);
					}
				}
				if (tags != null)
				{
					if (tags is InheritedValue)
					{
						unit.Add("tags", INHERITED_VALUE);
					}
					else
					{
						unit.Add("tags", tags);
					}
				}
				if (isPublic != null)
				{
					if (isPublic is InheritedValue)
					{
						unit.Add("isPublic", INHERITED_VALUE);
					}
					else
					{
						unit.Add("isPublic", isPublic);
					}
				}
				if (isFriend != null)
				{
					if (isFriend is InheritedValue)
					{
						unit.Add("isFriend", INHERITED_VALUE);
					}
					else
					{
						unit.Add("isFriend", isFriend);
					}
				}
				if (isFamily != null)
				{
					if (isFamily is InheritedValue)
					{
						unit.Add("isFamily", INHERITED_VALUE);
					}
					else
					{
						unit.Add("isFamily", isFamily);
					}
				}
				if (safetyLevel != null)
				{
					if (safetyLevel is InheritedValue)
					{
						unit.Add("safetyLevel", INHERITED_VALUE);
					}
					else
					{
						unit.Add("safetyLevel", safetyLevel);
					}
				}
				if (contentType != null)
				{
					if (contentType is InheritedValue)
					{
						unit.Add("contentType", INHERITED_VALUE);
					}
					else
					{
						unit.Add("contentType", contentType);
					}
				}
				if (hidden != null)
				{
					if (hidden is InheritedValue)
					{
						unit.Add("hidden", INHERITED_VALUE);
					}
					else
					{
						unit.Add("hidden", hidden);
					}
				}
				
				content.Add(PlatformType.Flickr, unit);
			}
		}

		/// <summary>
		/// Customs the content of the dropbox share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="file">File.</param>
		public static void customDropboxShareContent (Hashtable content, object file)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (file != null)
				{
					if (file is InheritedValue)
					{
						unit.Add("file", INHERITED_VALUE);
					}
					else
					{
						unit.Add("file", file);
					}
				}
				
				content.Add(PlatformType.Dropbox, unit);
			}
		}

		/// <summary>
		/// Customs the content of the VK share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="attachments">Attachments.</param>
		/// <param name="url">URL.</param>
		/// <param name="groupId">Group identifier.</param>
		/// <param name="friendsOnly">Friends only.</param>
		/// <param name="location">Location.</param>
		public static void customVKShareContent (Hashtable content, object message, object attachments, object url, object groupId, object friendsOnly, object location)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (attachments != null)
				{
					if (attachments is InheritedValue)
					{
						unit.Add("attachments", INHERITED_VALUE);
					}
					else
					{
						unit.Add("attachments", attachments);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (groupId != null)
				{
					if (groupId is InheritedValue)
					{
						unit.Add("groupId", INHERITED_VALUE);
					}
					else
					{
						unit.Add("groupId", groupId);
					}
				}
				if (friendsOnly != null)
				{
					if (friendsOnly is InheritedValue)
					{
						unit.Add("friendsOnly", INHERITED_VALUE);
					}
					else
					{
						unit.Add("friendsOnly", friendsOnly);
					}
				}
				if (location != null)
				{
					if (location is InheritedValue)
					{
						unit.Add("location", INHERITED_VALUE);
					}
					else
					{
						unit.Add("location", location);
					}
				}
				
				content.Add(PlatformType.VKontakte, unit);
			}
		}

		/// <summary>
		/// Customs the content of the we chat fav share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="type">Type.</param>
		/// <param name="message">Message.</param>
		/// <param name="title">Title.</param>
		/// <param name="url">URL.</param>
		/// <param name="thumbImage">Thumb image.</param>
		/// <param name="image">Image.</param>
		/// <param name="musicFileUrl">Music file URL.</param>
		/// <param name="extInfo">Ext info.</param>
		/// <param name="fileData">File data.</param>
		/// <param name="emoticonData">Emoticon data.</param>
		public static void customWeChatFavShareContent (Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData, object emoticonData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						unit.Add("type", INHERITED_VALUE);
					}
					else
					{
						unit.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						unit.Add("thumbImage", INHERITED_VALUE);
					}
					else
					{
						unit.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						unit.Add("musicFileUrl", INHERITED_VALUE);
					}
					else
					{
						unit.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						unit.Add("extInfo", INHERITED_VALUE);
					}
					else
					{
						unit.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						unit.Add("fileData", INHERITED_VALUE);
					}
					else
					{
						unit.Add("fileData", fileData);
					}
				}
				if (emoticonData != null)
				{
					if (emoticonData is InheritedValue)
					{
						unit.Add("emoticonData", INHERITED_VALUE);
					}
					else
					{
						unit.Add("emoticonData", emoticonData);
					}
				}
				
				content.Add(PlatformType.WeChatFav, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the yi xin session share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="type">Type.</param>
		/// <param name="message">Message.</param>
		/// <param name="title">Title.</param>
		/// <param name="url">URL.</param>
		/// <param name="thumbImage">Thumb image.</param>
		/// <param name="image">Image.</param>
		/// <param name="musicFileUrl">Music file URL.</param>
		/// <param name="extInfo">Ext info.</param>
		/// <param name="fileData">File data.</param>
		/// <param name="emoticonData">Emoticon data.</param>
		public static void customYiXinSessionShareContent (Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						unit.Add("type", INHERITED_VALUE);
					}
					else
					{
						unit.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						unit.Add("thumbImage", INHERITED_VALUE);
					}
					else
					{
						unit.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						unit.Add("musicFileUrl", INHERITED_VALUE);
					}
					else
					{
						unit.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						unit.Add("extInfo", INHERITED_VALUE);
					}
					else
					{
						unit.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						unit.Add("fileData", INHERITED_VALUE);
					}
					else
					{
						unit.Add("fileData", fileData);
					}
				}
				
				content.Add(PlatformType.YiXinSession, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the yi xin timeline share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="type">Type.</param>
		/// <param name="message">Message.</param>
		/// <param name="title">Title.</param>
		/// <param name="url">URL.</param>
		/// <param name="thumbImage">Thumb image.</param>
		/// <param name="image">Image.</param>
		/// <param name="musicFileUrl">Music file URL.</param>
		/// <param name="extInfo">Ext info.</param>
		/// <param name="fileData">File data.</param>
		public static void customYiXinTimelineShareContent (Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						unit.Add("type", INHERITED_VALUE);
					}
					else
					{
						unit.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						unit.Add("thumbImage", INHERITED_VALUE);
					}
					else
					{
						unit.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						unit.Add("musicFileUrl", INHERITED_VALUE);
					}
					else
					{
						unit.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						unit.Add("extInfo", INHERITED_VALUE);
					}
					else
					{
						unit.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						unit.Add("fileData", INHERITED_VALUE);
					}
					else
					{
						unit.Add("fileData", fileData);
					}
				}
				
				content.Add(PlatformType.YiXinTimeline, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the ming DAO share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		/// <param name="title">Title.</param>
		/// <param name="url">URL.</param>
		public static void customMingDaoShareContent (Hashtable content, object message, object image, object title, object url)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						unit.Add("title", INHERITED_VALUE);
					}
					else
					{
						unit.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						unit.Add("url", INHERITED_VALUE);
					}
					else
					{
						unit.Add("url", url);
					}
				}
				content.Add(PlatformType.MingDao, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the line share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		public static void customLineShareContent (Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				content.Add(PlatformType.Line, unit);
			}
			
		}

		/// <summary>
		/// Customs the content of the whats app share.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="message">Message.</param>
		/// <param name="image">Image.</param>
		/// <param name="music">Music.</param>
		/// <param name="video">Video.</param>
		public static void customWhatsAppShareContent (Hashtable content, object message, object image, object music, object video)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable unit = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						unit.Add("message", INHERITED_VALUE);
					}
					else
					{
						unit.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						unit.Add("image", INHERITED_VALUE);
					}
					else
					{
						unit.Add("image", image);
					}
				}
				if (music != null)
				{
					if (music is InheritedValue)
					{
						unit.Add("music", INHERITED_VALUE);
					}
					else
					{
						unit.Add("music", music);
					}
				}
				if (video != null)
				{
					if (video is InheritedValue)
					{
						unit.Add("video", INHERITED_VALUE);
					}
					else
					{
						unit.Add("video", video);
					}
				}
				content.Add(PlatformType.WhatsApp, unit);
			}
			
		}
	}
}