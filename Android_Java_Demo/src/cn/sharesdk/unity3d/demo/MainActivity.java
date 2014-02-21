package cn.sharesdk.unity3d.demo;

import cn.sharesdk.unity3d.ShareSDKUtils;

import com.unity3d.player.UnityPlayerActivity;
import android.os.Bundle;

public class MainActivity extends UnityPlayerActivity {
	
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		ShareSDKUtils.prepare();
	}
	
}
