package cn.sharesdk.unity3d;

import android.app.Application;

public class ShareSDKApplication extends Application {

	@Override
	public void onCreate() {
		super.onCreate();
		ShareSDKUtils.prepare(this.getApplicationContext());
	}

}
