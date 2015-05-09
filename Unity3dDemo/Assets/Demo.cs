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

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			ShareSDK.close();
			Application.Quit();
		}
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
			content["url"] = "http://www.mob.com";
			content["type"] = Convert.ToString((int)ContentType.News);
			content["siteUrl"] = "http://www.mob.com";
			content["site"] = "ShareSDK";
			content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
			//开启，调用客户端授权
			content["disableSSOWhenAuthorize"] = false;
			content["shareTheme"] = "classic";//ShareTheme has only two value which are skyblue and classic
			
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
			content["url"] = "http://www.mob.com";
			content["type"] = Convert.ToString((int)ContentType.News);
			content["siteUrl"] = "http://www.mob.com";
			content["site"] = "ShareSDK";
			content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
			//关闭，调用客户端授权
			content["disableSSOWhenAuthorize"] = true;
			content["shareTheme"] = "skyblue";//ShareTheme has only two value which are skyblue and classic

			ShareSDK.customSinaWeiboShareContent(content, "sina weibo test string", InheritedValue.VALUE, null);

			ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
			ShareSDK.showShareView (PlatformType.Any, content, evt);
		}
		
		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Share Content"))
		{
			Hashtable content = new Hashtable();
			content["content"] = "this is a test string.";
			content["image"] = "http://img.baidu.com/img/image/zhenrenmeinv0207.jpg";
			content["title"] = "test title";
			content["description"] = "test description";
			content["url"] = "http://www.mob.com";
			content["type"] = Convert.ToString((int)ContentType.News);
			content["siteUrl"] = "http://www.mob.com";
			content["site"] = "ShareSDK";
			content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
			
			ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
			ShareSDK.shareContent (PlatformType.SinaWeibo, content, evt);
		}


		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Get Friends SinaWeibo "))
		{
			GetFriendsResultEvent evt = new GetFriendsResultEvent(GetFriendsResultHandler);
			ShareSDK.getFriends (PlatformType.SinaWeibo, null, evt);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Get Token SinaWeibo "))
		{
			GetCredentialResultEvent evt = new GetCredentialResultEvent(GetTokenResultHandler);
			ShareSDK.getCredential (PlatformType.SinaWeibo, evt);
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
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
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
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
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
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void GetFriendsResultHandler (ResponseState state, PlatformType type, ArrayList friends, Hashtable error)
	{
		if (state == ResponseState.Success)
		{
			print ("share result :");
			print (MiniJSON.jsonEncode(friends));
		}
		else if (state == ResponseState.Fail)
		{
			print ("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void GetTokenResultHandler (ResponseState state, PlatformType type, Hashtable credential, Hashtable error)
	{
		if (state == ResponseState.Success)
		{
			print ("share result :");
			print (MiniJSON.jsonEncode(credential));
		}
		else if (state == ResponseState.Fail)
		{
			print ("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
}
