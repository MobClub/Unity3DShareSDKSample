package cn.sharesdk.unity3d.demo;

import com.unity3d.player.UnityPlayerActivity;
import android.os.Bundle;

public class MainActivity extends UnityPlayerActivity {
	
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		//当使用ShareSDKApplication后，这里就不需要重复调用了
		//ShareSDKUtils.prepare();
	}
	
}
