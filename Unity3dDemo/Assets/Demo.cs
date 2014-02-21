using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;

public class Demo : MonoBehaviour {
	
	public GUISkin demoSkin;

	// Use this for initialization
	void Start ()
	{	
		ShareSDK.setCallbackObjectName("Main Camera");
		ShareSDK.open ("api20");
		
		Hashtable sinaWeiboConf = new Hashtable();
		sinaWeiboConf.Add("app_key", "568898243");
		sinaWeiboConf.Add("app_secret", "38a4f8204cc784f81f9f0daaf31e02e3");
		sinaWeiboConf.Add("redirect_uri", "http://www.sharesdk.cn");
		
		ShareSDK.setPlatformConfig (PlatformType.SinaWeibo, sinaWeiboConf);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI ()
	{
		GUI.skin = demoSkin;
		
		float scale = 1.0f;
		
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			scale = Screen.width / 320;
		}
		
		float btnWidth = 200 * scale;
		float btnHeight = 45 * scale;
		float btnTop = 20 * scale;
		GUI.skin.button.fontSize = Convert.ToInt32(16 * scale);
		
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Authorize"))
		{

			AuthResultEvent evt = new AuthResultEvent(AuthResultHandler);
			ShareSDK.authorize(PlatformType.SinaWeibo, evt);
		}
		
		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Get User Info"))
		{
			GetUserInfoResultEvent evt = new GetUserInfoResultEvent(GetUserInfoResultHandler);
			ShareSDK.getUserInfo(PlatformType.SinaWeibo, evt);
		}
		
		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Show Share Menu"))
		{
			Hashtable content = new Hashtable();
			content["content"] = "this is a test string.";
			content["image"] = "http://img.baidu.com/img/image/zhenrenmeinv0207.jpg";
			content["title"] = "test title";
			content["description"] = "test description";
			content["url"] = "http://sharesdk.cn";
			content["type"] = Convert.ToString((int)ContentType.News);
			content["siteUrl"] = "http://sharesdk.cn";
			content["site"] = "ShareSDK";
			content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
			
			ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
			ShareSDK.showShareMenu (null, content, 100, 100, MenuArrowDirection.Up, evt);
		}
		
		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Show Share View"))
		{
			Hashtable content = new Hashtable();
			content["content"] = "this is a test string.";
			content["image"] = "http://img.baidu.com/img/image/zhenrenmeinv0207.jpg";
			content["title"] = "test title";
			content["description"] = "test description";
			content["url"] = "http://sharesdk.cn";
			content["type"] = Convert.ToString((int)ContentType.News);
			content["siteUrl"] = "http://sharesdk.cn";
			content["site"] = "ShareSDK";
			content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
			
			ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
			ShareSDK.showShareView (PlatformType.Any, content, evt);
		}
		
		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Show Content"))
		{
			Hashtable content = new Hashtable();
			content["content"] = "this is a test string.";
			content["image"] = "http://img.baidu.com/img/image/zhenrenmeinv0207.jpg";
			content["title"] = "test title";
			content["description"] = "test description";
			content["url"] = "http://sharesdk.cn";
			content["type"] = Convert.ToString((int)ContentType.News);
			content["siteUrl"] = "http://sharesdk.cn";
			content["site"] = "ShareSDK";
			content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
			
			ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
			ShareSDK.shareContent (PlatformType.SinaWeibo, content, evt);
		}
	}
	
	void AuthResultHandler(ResponseState state, PlatformType type, Hashtable error)
	{
		if (state == ResponseState.Success)
		{
			print ("success !");
		}
		else if (state == ResponseState.Fail)
		{
			print ("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
		}
	}
	
	void GetUserInfoResultHandler (ResponseState state, PlatformType type, Hashtable user, Hashtable error)
	{
		if (state == ResponseState.Success)
		{
			print ("get user result :");
			print (MiniJSON.jsonEncode(user));
		}
		else if (state == ResponseState.Fail)
		{
			print ("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
		}
	}
	
	void ShareResultHandler (ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
	{
		if (state == ResponseState.Success)
		{
			print ("share result :");
			print (MiniJSON.jsonEncode(shareInfo));
		}
		else if (state == ResponseState.Fail)
		{
			print ("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
		}
	}
}
