//
//  ShareSDKUnity3DBridge.m
//  Unity-iPhone
//
//  Created by 冯 鸿杰 on 14-2-14.
//
//

#import "ShareSDKUnity3DBridge.h"
#import <ShareSDK/ShareSDK.h>
#import <SHareSDK/ShareSDK+Utils.h>
#import <AGCommon/CMRegexKitLite.h>
#import <AGCommon/UIDevice+Common.h>

#define INHERITED_VALUE_STR @"{inherited}"

#define __SHARESDK_WECHAT__
#define __SHARESDK_YIXIN__
#define __SHARESDK_PINTEREST__
#define __SHARESDK_GOOGLEPLUS__
#define __SHARESDK_QQ__
#define __SHARESDK_RENREN__
#define __SHARESDK_TENCENT_WEIBO__

#ifdef __SHARESDK_WECHAT__
#import "WXApi.h"
#endif

#ifdef __SHARESDK_YIXIN__
#import "YXApi.h"
#endif

#ifdef __SHARESDK_PINTEREST__
#import <Pinterest/Pinterest.h>
#endif

#ifdef __SHARESDK_GOOGLEPLUS__
#import <GoogleOpenSource/GoogleOpenSource.h>
#import <GooglePlus/GooglePlus.h>
#endif

#ifdef __SHARESDK_QQ__
#import <TencentOpenAPI/TencentOAuth.h>
#import <TencentOpenAPI/QQApiInterface.h>
#endif

#ifdef __SHARESDK_RENREN__
#import <RennSDK/RennSDK.h>
#endif

#ifdef __SHARESDK_TENCENT_WEIBO__
#import "WeiboApi.h"
#endif

static UIView *_refView = nil;

#if defined (__cplusplus)
extern "C" {
#endif
    
    /**
     *	@brief	初始化ShareSDK
     *
     *	@param 	appKey 	应用Key
     */
    extern void __iosShareSDKOpen(void *appKey);

    /**
     *	@brief	初始化社交平台
     *
     *	@param 	platType 	平台类型
     *	@param 	contigInfo 	配置信息
     */
    extern void __iosShareSDKSetPlatformConfig(int platType, void *configInfo);

    /**
     *	@brief	用户授权
     *
     *	@param 	platType 	平台类型
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKAuthorize (int platType, void *observer);

    /**
     *	@brief	取消用户授权
     *
     *	@param 	platType 	平台类型
     */
    extern void __iosShareSDKCancelAuthorize (int platType);

    /**
     *	@brief	判断用户是否授权
     *
     *	@param 	platType 	平台类型
     *
     *	@return	YES 表示已经授权，NO 表示尚未授权
     */
    extern bool __iosShareSDKHasAuthorized (int platType);
    
    /**
     *	@brief	获取用户信息
     *
     *	@param 	platType 	平台类型
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKGetUserInfo (int platType, void *observer);
    
    /**
     *	@brief	分享内容
     *
     *	@param 	platType 	平台类型
     *	@param 	content 	分享内容
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKShare (int platType, void *content, void *observer);

    /**
     *	@brief	一键分享内容
     *
     *	@param 	platTypes 	平台类型列表
     *	@param 	content 	分享内容
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKOneKeyShare (void *platTypes, void *content, void *observer);
    
    /**
     *	@brief	显示分享菜单
     *
     *	@param 	platTypes 	平台类型列表
     *	@param 	content 	分享内容
     *	@param 	x 	弹出菜单的箭头的横坐标，仅用于iPad
     *	@param 	y 	弹出菜单的箭头的纵坐标，仅用于iPad
     *	@param 	direction 	菜单箭头方向，仅用于iPad
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKShowShareMenu (void *platTypes, void *content, int x, int y, int direction, void *observer);

    /**
     *	@brief	显示分享编辑界面
     *
     *	@param 	platType 	平台类型
     *	@param 	content 	分享内容
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKShowShareView (int platType, void *content, void *observer);
    
    /**
     *	@brief	获取授权用户好友列表
     *
     *	@param 	platType 	平台类型
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKGetFriendsList (int platType, void *page, void *observer);
    
    /**
     *	@brief	获取授权信息
     *
     *	@param 	platType 	平台类型
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKGetCredential (int platType, void *observer);
    
#if defined (__cplusplus)
}
#endif

#if defined (__cplusplus)
extern "C" {
#endif
    
    /**
     *  解析字符串字段
     *
     *  @param value 字段值
     *
     *  @return 字符串
     */
    NSString* __parseStringField(id value)
    {
        if ([value isKindOfClass:[NSString class]])
        {
            if ([value isEqualToString:INHERITED_VALUE_STR])
            {
                return INHERIT_VALUE;
            }
            else
            {
                return value;
            }
        }
        
        return nil;
    }
    
    /**
     *  解析数值字段
     *
     *  @param value 字段值
     *
     *  @return 数值
     */
    NSNumber* __parseNumberField(id value)
    {
        if ([value isKindOfClass:[NSString class]])
        {
            if ([value isEqualToString:INHERITED_VALUE_STR])
            {
                return INHERIT_VALUE;
            }
        }
        else if ([value isKindOfClass:[NSNumber class]])
        {
            return value;
        }
        
        return nil;
    }
    
    /**
     *  解析图片字段
     *
     *  @param value 字段值
     *
     *  @return 图片附件
     */
    id<ISSCAttachment> __parseImageField(id value)
    {
        if ([value isKindOfClass:[NSString class]])
        {
            if ([value isEqualToString:INHERITED_VALUE_STR])
            {
                return INHERIT_VALUE;
            }
            
            if ([value isMatchedByRegex:@"\\w://.*"])
            {
                return [ShareSDK imageWithUrl:value];
            }
            else
            {
                return [ShareSDK imageWithPath:value];
            }
        }
        else if ([value isKindOfClass:[NSDictionary class]])
        {
            NSString *path = [value objectForKey:@"path"];
            NSString *ext = [value objectForKey:@"type"];
            
            NSString *imagePath = [[NSBundle mainBundle] pathForResource:path ofType:ext];
            if (imagePath)
            {
                return [ShareSDK imageWithPath:imagePath];
            }
        }
        
        return nil;
    }
    
    /**
     *  解析资源列表数据
     *
     *  @param value 字段值
     *
     *  @return 资源数组
     */
    NSArray* __parseResourcesField(id value)
    {
        if ([value isKindOfClass:[NSString class]])
        {
            if ([value isEqualToString:INHERITED_VALUE_STR])
            {
                return INHERIT_VALUE;
            }
        }
        else if ([value isKindOfClass:[NSArray class]])
        {
            NSMutableArray *resources = [NSMutableArray array];
            for (int i = 0; i < [value count]; i++)
            {
                id<ISSCAttachment> attach = __parseImageField (value [i]);
                if (attach)
                {
                    [resources addObject:attach];
                }
            }
            
            return resources;
        }
        
        return nil;
    }
    
    /**
     *  解析字符串数组
     *
     *  @param value 字段值
     *
     *  @return 字符串数组
     */
    NSArray* __parseStringsField(id value)
    {
        if ([value isKindOfClass:[NSString class]])
        {
            if ([value isEqualToString:INHERITED_VALUE_STR])
            {
                return INHERIT_VALUE;
            }
        }
        else if ([value isKindOfClass:[NSArray class]])
        {
            NSMutableArray *strings = [NSMutableArray array];
            for (int i = 0; i < [value count]; i++)
            {
                NSString *str = __parseStringField (value [i]);
                if (str)
                {
                    [strings addObject:str];
                }
            }
            
            return strings;
        }
        
        return nil;
    }
    
    /**
     *  解析地理位置信息
     *
     *  @param value 字段值
     *
     *  @return 地理位置信息
     */
    SSCLocationCoordinate2D* __parseLocationField(id value)
    {
        if ([value isKindOfClass:[NSString class]])
        {
            if ([value isEqualToString:INHERITED_VALUE_STR])
            {
                return INHERIT_VALUE;
            }
        }
        else if ([value isKindOfClass:[NSDictionary class]])
        {
            double lat = 0;
            double lng = 0;
            
            id subValue = [value objectForKey:@"lat"];
            if ([subValue isKindOfClass:[NSNumber class]])
            {
                lat = [subValue floatValue];
            }
            subValue = [value objectForKey:@"lng"];
            if ([subValue isKindOfClass:[NSNumber class]])
            {
                lng = [subValue floatValue];
            }
            
            return [SSCLocationCoordinate2D locationCoordinate2DWithLatitude:lat
                                                                   longitude:lng];
        }
        
        return nil;
    }
    
    id<ISSContent> __getContentObjectWithString(NSString *data)
    {
        NSDictionary *contentDict = [ShareSDK jsonObjectWithString:data];
        
        NSLog(@"contentDict = %@", contentDict);
        
        NSString *message = nil;
        id<ISSCAttachment> image = nil;
        NSString *title = nil;
        NSString *url = nil;
        NSString *desc = nil;
        SSPublishContentMediaType type = SSPublishContentMediaTypeText;
        
        if (contentDict)
        {
            if ([[contentDict objectForKey:@"content"] isKindOfClass:[NSString class]])
            {
                message = [contentDict objectForKey:@"content"];
            }
            
            id value = [contentDict objectForKey:@"image"];
            if ([value isKindOfClass:[NSString class]])
            {
                NSString *imagePath = value;
                if ([imagePath isMatchedByRegex:@"\\w://.*"])
                {
                    image = [ShareSDK imageWithUrl:value];
                }
                else
                {
                    image = [ShareSDK imageWithPath:value];
                }
            }
            else if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *path = [value objectForKey:@"path"];
                NSString *ext = [value objectForKey:@"type"];
                
                NSString *imagePath = [[NSBundle mainBundle] pathForResource:path ofType:ext];
                if (imagePath)
                {
                    image = [ShareSDK imageWithPath:imagePath];
                }
            }
            
            if ([[contentDict objectForKey:@"title"] isKindOfClass:[NSString class]])
            {
                title = [contentDict objectForKey:@"title"];
            }
            if ([[contentDict objectForKey:@"url"] isKindOfClass:[NSString class]])
            {
                url = [contentDict objectForKey:@"url"];
            }
            if ([[contentDict objectForKey:@"description"] isKindOfClass:[NSString class]])
            {
                desc = [contentDict objectForKey:@"description"];
            }
            if ([[contentDict objectForKey:@"type"] isKindOfClass:[NSString class]])
            {
                type = (SSPublishContentMediaType)[[contentDict objectForKey:@"type"] integerValue];
            }
        }
        
        id<ISSContent> contentObj = [ShareSDK content:message
                                       defaultContent:nil
                                                image:image
                                                title:title
                                                  url:url
                                          description:desc
                                            mediaType:type];
        
        if (contentDict)
        {
            NSString *site = nil;
            NSString *siteUrl = nil;
            
            if ([[contentDict objectForKey:@"site"] isKindOfClass:[NSString class]])
            {
                site = [contentDict objectForKey:@"site"];
            }
            
            if ([[contentDict objectForKey:@"siteUrl"] isKindOfClass:[NSString class]])
            {
                siteUrl = [contentDict objectForKey:@"siteUrl"];
            }
            
            if (site || siteUrl)
            {
                if ([ShareSDK getClientWithType:ShareTypeQQSpace])
                {
                    [contentObj addQQSpaceUnitWithTitle:INHERIT_VALUE
                                                    url:INHERIT_VALUE
                                                   site:site
                                                fromUrl:siteUrl
                                                comment:INHERIT_VALUE
                                                summary:INHERIT_VALUE
                                                  image:INHERIT_VALUE
                                                   type:INHERIT_VALUE
                                                playUrl:INHERIT_VALUE
                                                   nswb:INHERIT_VALUE];
                }
            }
            
            
            NSString *extInfo = nil;
            NSString *musicUrl = nil;
            
            if ([[contentDict objectForKey:@"extInfo"] isKindOfClass:[NSString class]])
            {
                extInfo = [contentDict objectForKey:@"extInfo"];
            }
            
            if ([[contentDict objectForKey:@"musicUrl"] isKindOfClass:[NSString class]])
            {
                musicUrl = [contentDict objectForKey:@"musicUrl"];
            }
            
            if (extInfo || musicUrl)
            {
                if ([ShareSDK getClientWithType:ShareTypeWeixiSession])
                {
                    [contentObj addWeixinSessionUnitWithType:INHERIT_VALUE
                                                     content:INHERIT_VALUE
                                                       title:INHERIT_VALUE
                                                         url:INHERIT_VALUE
                                                       image:INHERIT_VALUE
                                                musicFileUrl:musicUrl
                                                     extInfo:extInfo
                                                    fileData:INHERIT_VALUE
                                                emoticonData:INHERIT_VALUE];
                }
                
                if ([ShareSDK getClientWithType:ShareTypeWeixiTimeline])
                {
                    [contentObj addWeixinTimelineUnitWithType:INHERIT_VALUE
                                                      content:INHERIT_VALUE
                                                        title:INHERIT_VALUE
                                                          url:INHERIT_VALUE
                                                        image:INHERIT_VALUE
                                                 musicFileUrl:musicUrl
                                                      extInfo:extInfo
                                                     fileData:INHERIT_VALUE
                                                 emoticonData:INHERIT_VALUE];
                }
                
                if ([ShareSDK getClientWithType:ShareTypeWeixiFav])
                {
                    [contentObj addWeixinFavUnitWithType:INHERIT_VALUE
                                                 content:INHERIT_VALUE
                                                   title:INHERIT_VALUE
                                                     url:INHERIT_VALUE
                                              thumbImage:INHERIT_VALUE
                                                   image:INHERIT_VALUE
                                            musicFileUrl:musicUrl
                                                 extInfo:extInfo
                                                fileData:INHERIT_VALUE
                                            emoticonData:INHERIT_VALUE];
                }
            }
        }
        
        //平台自定义内容
        if (contentDict)
        {
            //新浪微博
            id value = [contentDict objectForKey:@"SinaWeibo"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                SSCLocationCoordinate2D *location = __parseLocationField([value objectForKey:@"location"]);

                if ([ShareSDK getClientWithType:ShareTypeSinaWeibo])
                {
                    [contentObj addSinaWeiboUnitWithContent:message
                                                      image:image
                                         locationCoordinate:location];
                }
            }
            
            //腾讯微博
            value  = [contentDict objectForKey:@"TencentWeibo"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                SSCLocationCoordinate2D *location = __parseLocationField([value objectForKey:@"location"]);
                
                if ([ShareSDK getClientWithType:ShareTypeTencentWeibo])
                {
                    [contentObj addTencentWeiboUnitWithContent:message
                                                         image:image
                                            locationCoordinate:location];
                }
            }
            
            //搜狐微博
            value = [contentDict objectForKey:@"SohuWeibo"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                
                if ([ShareSDK getClientWithType:ShareTypeSohuWeibo])
                {
                    [contentObj addSohuWeiboUnitWithContent:message
                                                      image:image];
                }
            }
            
            //网易微博
            value = [contentDict objectForKey:@"NetEaseWeibo"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                
                if ([ShareSDK getClientWithType:ShareType163Weibo])
                {
                    [contentObj add163WeiboUnitWithContent:message image:image];
                }
            }
            
            //豆瓣
            value = [contentDict objectForKey:@"DouBan"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                
                if ([ShareSDK getClientWithType:ShareTypeDouBan])
                {
                    [contentObj addDouBanWithContent:message image:image];
                }
            }
            
            //QQ空间
            value = [contentDict objectForKey:@"QZone"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                NSString *site = __parseStringField([value objectForKey:@"site"]);
                NSString *fromUrl = __parseStringField([value objectForKey:@"fromUrl"]);
                NSString *comment = __parseStringField([value objectForKey:@"comment"]);
                NSString *summary = __parseStringField([value objectForKey:@"summary"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                NSNumber *type = __parseNumberField([value objectForKey:@"type"]);
                NSString *playUrl = __parseStringField([value objectForKey:@"playUrl"]);
                NSNumber *nswb = __parseNumberField([value objectForKey:@"nswb"]);

                if ([ShareSDK getClientWithType:ShareTypeQQSpace])
                {
                    [contentObj addQQSpaceUnitWithTitle:title
                                                    url:url
                                                   site:site
                                                fromUrl:fromUrl
                                                comment:comment
                                                summary:summary
                                                  image:image
                                                   type:type
                                                playUrl:playUrl
                                                   nswb:nswb];
                }
            }
            
            //人人网
            value = [contentDict objectForKey:@"Renren"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                
                NSString *name = __parseStringField([value objectForKey:@"name"]);
                NSString *description = __parseStringField([value objectForKey:@"description"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                NSString *caption = __parseStringField([value objectForKey:@"caption"]);
                
                if ([ShareSDK getClientWithType:ShareTypeRenren])
                {
                    [contentObj addRenRenUnitWithName:name
                                          description:description
                                                  url:url
                                              message:message
                                                image:image
                                              caption:caption];
                }
            }
            
            //开心网
            value = [contentDict objectForKey:@"Kaixin"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);

                if ([ShareSDK getClientWithType:ShareTypeKaixin])
                {
                    [contentObj addKaiXinUnitWithContent:message
                                                   image:image];
                }
            }
            
            //Facebook
            value = [contentDict objectForKey:@"Facebook"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                
                if ([ShareSDK getClientWithType:ShareTypeFacebook])
                {
                    [contentObj addFacebookWithContent:message
                                                 image:image];
                }
            }
            
            //Twitter
            value = [contentDict objectForKey:@"Twitter"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                SSCLocationCoordinate2D *location = __parseLocationField([value objectForKey:@"location"]);
                
                if ([ShareSDK getClientWithType:ShareTypeTwitter])
                {
                    [contentObj addTwitterWithContent:message
                                                image:image
                                   locationCoordinate:location];
                }
            }
            
            //Evernote
            value = [contentDict objectForKey:@"Evernote"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSArray *resources = __parseResourcesField([value objectForKey:@"resources"]);
                NSString *notebookGuid = __parseStringField([value objectForKey:@"notebookGuid"]);
                NSString *tagsGuid = __parseStringField([value objectForKey:@"tagsGuid"]);
                
                if ([ShareSDK getClientWithType:ShareTypeEvernote])
                {
                    [contentObj addEvernoteUnitWithContent:message
                                                     title:title
                                                 resources:resources
                                              notebookGuid:notebookGuid
                                                  tagsGuid:tagsGuid];
                }
            }
            
            //GooglePlus
            value = [contentDict objectForKey:@"GooglePlus"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField([value objectForKey:@"image"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                NSString *deepLinkId = __parseStringField([value objectForKey:@"deepLinkId"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSArray *resources = __parseResourcesField([value objectForKey:@"resources"]);
                NSString *description = __parseStringField([value objectForKey:@"description"]);
                NSString *thumbnail = __parseStringField([value objectForKey:@"thumbnail"]);
                
                if ([ShareSDK getClientWithType:ShareTypeGooglePlus])
                {
                    [contentObj addGooglePlusUnitWithText:message
                                                    image:image
                                                      url:url
                                               deepLinkId:deepLinkId
                                                    title:title
                                              description:description
                                                thumbnail:thumbnail];
                }
            }
            
            //Instagram
            value = [contentDict objectForKey:@"Instagram"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                
                if ([ShareSDK getClientWithType:ShareTypeInstagram])
                {
                    [contentObj addInstagramUnitWithTitle:message
                                                    image:image];
                }
            }
            
            //LinkedIn
            value = [contentDict objectForKey:@"LinkedIn"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *comment = __parseStringField([value objectForKey:@"comment"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *description = __parseStringField([value objectForKey:@"description"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                NSString *visibility = __parseStringField([value objectForKey:@"visibility"]);

                if ([ShareSDK getClientWithType:ShareTypeLinkedIn])
                {
                    [contentObj addLinkedInUnitWithComment:comment
                                                     title:title
                                               description:description
                                                       url:url
                                                     image:image
                                                visibility:visibility];
                }
            }
            
            //Tumblr
            value = [contentDict objectForKey:@"Tumblr"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *text = __parseStringField([value objectForKey:@"text"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *blogName = __parseStringField([value objectForKey:@"blogName"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                
                if ([ShareSDK getClientWithType:ShareTypeTumblr])
                {
                    [contentObj addTumblrUnitWithText:text
                                                title:title
                                                image:image
                                                  url:url
                                             blogName:blogName];
                }
            }
            
            //邮件
            value = [contentDict objectForKey:@"Mail"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *subject = __parseStringField([value objectForKey:@"subject"]);
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSNumber *isHTML = __parseNumberField([value objectForKey:@"isHTML"]);
                NSArray *attachments = __parseResourcesField([value objectForKey:@"attachments"]);
                NSArray *to = __parseStringsField([value objectForKey:@"to"]);
                NSArray *cc = __parseStringsField([value objectForKey:@"cc"]);
                NSArray *bcc = __parseStringsField([value objectForKey:@"bcc"]);

                if ([ShareSDK getClientWithType:ShareTypeMail])
                {
                    [contentObj addMailUnitWithSubject:subject
                                               content:message
                                                isHTML:isHTML
                                           attachments:attachments
                                                    to:to
                                                    cc:cc
                                                   bcc:bcc];
                }
            }
            
            //短信
            value = [contentDict objectForKey:@"SMS"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                
                if ([ShareSDK getClientWithType:ShareTypeSMS])
                {
                    [contentObj addSMSUnitWithContent:message];
                }
            }
            
            //打印
            value = [contentDict objectForKey:@"Print"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);

                if ([ShareSDK getClientWithType:ShareTypeAirPrint])
                {
                    [contentObj addAirPrintWithContent:message
                                                 image:image];
                }
            }
            
            //拷贝
            value = [contentDict objectForKey:@"Copy"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                
                if ([ShareSDK getClientWithType:ShareTypeCopy])
                {
                    [contentObj addCopyUnitWithContent:message
                                                 image:image];
                }
            }
            
            //微信好友
            value = [contentDict objectForKey:@"WeChatSession"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSNumber *type = __parseNumberField([value objectForKey:@"type"]);
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                id<ISSCAttachment> thumbImage = __parseImageField ([value objectForKey:@"thumbImage"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                NSString *musicFileUrl = __parseStringField([value objectForKey:@"musicFileUrl"]);
                NSString *extInfo = __parseStringField([value objectForKey:@"extInfo"]);
                
                NSString *fileDataStr = __parseStringField([value objectForKey:@"fileData"]);
                NSData *fileData = nil;
                if (fileDataStr)
                {
                    fileData = [NSData dataWithContentsOfFile:fileDataStr];
                }
                
                NSString *emoticonDataStr = __parseStringField([value objectForKey:@"emoticonData"]);
                NSData *emoticonData = nil;
                if (emoticonDataStr)
                {
                    emoticonData = [NSData dataWithContentsOfFile:emoticonDataStr];
                }
                
                if ([ShareSDK getClientWithType:ShareTypeWeixiSession])
                {
                    [contentObj addWeixinSessionUnitWithType:type
                                                     content:message
                                                       title:title
                                                         url:url
                                                  thumbImage:thumbImage
                                                       image:image
                                                musicFileUrl:musicFileUrl
                                                     extInfo:extInfo
                                                    fileData:fileData
                                                emoticonData:emoticonData];
                }
            }
            
            //微信朋友圈
            value = [contentDict objectForKey:@"WeChatTimeline"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSNumber *type = __parseNumberField([value objectForKey:@"type"]);
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                id<ISSCAttachment> thumbImage = __parseImageField ([value objectForKey:@"thumbImage"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                NSString *musicFileUrl = __parseStringField([value objectForKey:@"musicFileUrl"]);
                NSString *extInfo = __parseStringField([value objectForKey:@"extInfo"]);
                
                NSString *fileDataStr = __parseStringField([value objectForKey:@"fileData"]);
                NSData *fileData = nil;
                if (fileDataStr)
                {
                    fileData = [NSData dataWithContentsOfFile:fileDataStr];
                }
                
                NSString *emoticonDataStr = __parseStringField([value objectForKey:@"emoticonData"]);
                NSData *emoticonData = nil;
                if (emoticonDataStr)
                {
                    emoticonData = [NSData dataWithContentsOfFile:emoticonDataStr];
                }
                
                if ([ShareSDK getClientWithType:ShareTypeWeixiTimeline])
                {
                    [contentObj addWeixinTimelineUnitWithType:type
                                                      content:message
                                                        title:title
                                                          url:url
                                                   thumbImage:thumbImage
                                                        image:image
                                                 musicFileUrl:musicFileUrl
                                                      extInfo:extInfo
                                                     fileData:fileData
                                                 emoticonData:emoticonData];
                }
            }
            
            //QQ
            value = [contentDict objectForKey:@"QQ"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSNumber *type = __parseNumberField([value objectForKey:@"type"]);
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);

                if ([ShareSDK getClientWithType:ShareTypeQQ])
                {
                    [contentObj addQQUnitWithType:type
                                          content:message
                                            title:title
                                              url:url
                                            image:image];
                }
            }
            
            //Instapaper
            value = [contentDict objectForKey:@"Instapaper"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *description = __parseStringField([value objectForKey:@"description"]);
                
                if ([ShareSDK getClientWithType:ShareTypeInstapaper])
                {
                    [contentObj addInstapaperContentWithUrl:url
                                                      title:title
                                                description:description];
                }
            }
            
            //Pocket
            value = [contentDict objectForKey:@"Pocket"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *tags = __parseStringField([value objectForKey:@"tags"]);
                NSString *tweetId = __parseStringField([value objectForKey:@"tweetId"]);
                
                if ([ShareSDK getClientWithType:ShareTypeInstapaper])
                {
                    [contentObj addPocketUnitWithUrl:url
                                               title:title
                                                tags:tags
                                             tweetId:tweetId];
                }
            }
            
            //有道云笔记
            value = [contentDict objectForKey:@"YouDaoNote"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *author = __parseStringField([value objectForKey:@"author"]);
                NSString *source = __parseStringField([value objectForKey:@"source"]);
                NSArray *attachments = __parseResourcesField([value objectForKey:@"attachments"]);
                
                if ([ShareSDK getClientWithType:ShareTypeYouDaoNote])
                {
                    [contentObj addYouDaoNoteUnitWithContent:message
                                                       title:title
                                                      author:author
                                                      source:source
                                                 attachments:attachments];
                }
            }
            
            //搜狐随身看
            value = [contentDict objectForKey:@"SohuKan"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                
                if ([ShareSDK getClientWithType:ShareTypeSohuKan])
                {
                    [contentObj addSohuKanUnitWithUrl:url];
                }
            }
            
            //Pinterest
            value = [contentDict objectForKey:@"Pinterest"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                id<ISSCAttachment> image = __parseImageField([value objectForKey:@"image"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                NSString *description = __parseStringField([value objectForKey:@"description"]);
                
                if ([ShareSDK getClientWithType:ShareTypePinterest])
                {
                    [contentObj addPinterestUnitWithImage:image
                                                      url:url
                                              description:description];
                }
            }
            
            //Flickr
            value = [contentDict objectForKey:@"Flickr"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                id<ISSCAttachment> photo = __parseImageField([value objectForKey:@"photo"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *description = __parseStringField([value objectForKey:@"description"]);
                NSString *tags = __parseStringField([value objectForKey:@"tags"]);
                
                NSNumber *isPublic = __parseNumberField([value objectForKey:@"isPublic"]);
                NSNumber *isFriend = __parseNumberField([value objectForKey:@"isFriend"]);
                NSNumber *isFamily = __parseNumberField([value objectForKey:@"isFamily"]);
                NSNumber *safetyLevel = __parseNumberField([value objectForKey:@"safetyLevel"]);
                NSNumber *contentType = __parseNumberField([value objectForKey:@"contentType"]);
                NSNumber *hidden = __parseNumberField([value objectForKey:@"hidden"]);
                
                if ([ShareSDK getClientWithType:ShareTypeFlickr])
                {
                    [contentObj addFlickrUnitWithPhoto:photo
                                                 title:title
                                           description:description
                                                  tags:tags
                                              isPublic:isPublic
                                              isFriend:isFriend
                                              isFamily:isFamily
                                           safetyLevel:safetyLevel
                                           contentType:contentType
                                                hidden:hidden];
                }
            }
            
            //Dropbox
            value = [contentDict objectForKey:@"Dropbox"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                id<ISSCAttachment> file = __parseImageField([value objectForKey:@"file"]);
                
                if ([ShareSDK getClientWithType:ShareTypeDropbox])
                {
                    [contentObj addDropboxUnitWithFile:file];
                }
            }
            
            //VKontakte
            value = [contentDict objectForKey:@"VKontakte"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSArray *attachments = __parseResourcesField([value objectForKey:@"attachments"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                NSString *groupId = __parseStringField([value objectForKey:@"groupId"]);
                NSNumber *friendsOnly = __parseNumberField([value objectForKey:@"friendsOnly"]);
                SSCLocationCoordinate2D *location = __parseLocationField([value objectForKey:@"location"]);
                
                if ([ShareSDK getClientWithType:ShareTypeVKontakte])
                {
                    [contentObj addVKontakteUnitWithMessage:message
                                                attachments:attachments
                                                        url:url
                                                    groupId:groupId
                                                friendsOnly:friendsOnly
                                         locationCoordinate:location];
                }
            }
            
            //微信收藏
            value = [contentDict objectForKey:@"WeChatFav"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSNumber *type = __parseNumberField([value objectForKey:@"type"]);
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                id<ISSCAttachment> thumbImage = __parseImageField ([value objectForKey:@"thumbImage"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                NSString *musicFileUrl = __parseStringField([value objectForKey:@"musicFileUrl"]);
                NSString *extInfo = __parseStringField([value objectForKey:@"extInfo"]);
                
                NSString *fileDataStr = __parseStringField([value objectForKey:@"fileData"]);
                NSData *fileData = nil;
                if (fileDataStr)
                {
                    fileData = [NSData dataWithContentsOfFile:fileDataStr];
                }
                
                NSString *emoticonDataStr = __parseStringField([value objectForKey:@"emoticonData"]);
                NSData *emoticonData = nil;
                if (emoticonDataStr)
                {
                    emoticonData = [NSData dataWithContentsOfFile:emoticonDataStr];
                }
                
                if ([ShareSDK getClientWithType:ShareTypeWeixiFav])
                {
                    [contentObj addWeixinFavUnitWithType:type
                                                 content:message
                                                   title:title
                                                     url:url
                                              thumbImage:thumbImage
                                                   image:image
                                            musicFileUrl:musicFileUrl
                                                 extInfo:extInfo
                                                fileData:fileData
                                            emoticonData:emoticonData];
                }
            }
            
            //易信好友
            value = [contentDict objectForKey:@"YiXinSession"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSNumber *type = __parseNumberField([value objectForKey:@"type"]);
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                id<ISSCAttachment> thumbImage = __parseImageField ([value objectForKey:@"thumbImage"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                NSString *musicFileUrl = __parseStringField([value objectForKey:@"musicFileUrl"]);
                NSString *extInfo = __parseStringField([value objectForKey:@"extInfo"]);
                
                NSString *fileDataStr = __parseStringField([value objectForKey:@"fileData"]);
                NSData *fileData = nil;
                if (fileDataStr)
                {
                    fileData = [NSData dataWithContentsOfFile:fileDataStr];
                }
                
                if ([ShareSDK getClientWithType:ShareTypeYiXinSession])
                {
                    [contentObj addYiXinSessionUnitWithType:type
                                                    content:message
                                                      title:title
                                                        url:url
                                                 thumbImage:thumbImage
                                                      image:image
                                               musicFileUrl:musicFileUrl
                                                    extInfo:extInfo
                                                   fileData:fileData];
                }
            }
            
            //易信朋友圈
            value = [contentDict objectForKey:@"YiXinTimeline"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSNumber *type = __parseNumberField([value objectForKey:@"type"]);
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                id<ISSCAttachment> thumbImage = __parseImageField ([value objectForKey:@"thumbImage"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                NSString *musicFileUrl = __parseStringField([value objectForKey:@"musicFileUrl"]);
                NSString *extInfo = __parseStringField([value objectForKey:@"extInfo"]);
                
                NSString *fileDataStr = __parseStringField([value objectForKey:@"fileData"]);
                NSData *fileData = nil;
                if (fileDataStr)
                {
                    fileData = [NSData dataWithContentsOfFile:fileDataStr];
                }
                
                if ([ShareSDK getClientWithType:ShareTypeYiXinTimeline])
                {
                    [contentObj addYiXinTimelineUnitWithType:type
                                                     content:message
                                                       title:title
                                                         url:url
                                                  thumbImage:thumbImage
                                                       image:image
                                                musicFileUrl:musicFileUrl
                                                     extInfo:extInfo
                                                    fileData:fileData];
                }
            }
            
            //明道
            value = [contentDict objectForKey:@"MingDao"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                NSString *title = __parseStringField([value objectForKey:@"title"]);
                NSString *url = __parseStringField([value objectForKey:@"url"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                
                if ([ShareSDK getClientWithType:ShareTypeMingDao])
                {
                    [contentObj addMingDaoUnitWithContent:message
                                                    image:image
                                                    title:title
                                                      url:url];
                }
            }
            
            //Line
            value = [contentDict objectForKey:@"Line"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                
                if ([ShareSDK getClientWithType:ShareTypeLine])
                {
                    [contentObj addLineUnitWithContent:message
                                                 image:image];
                }
            }
            
            //WhatsApp
            value = [contentDict objectForKey:@"WhatsApp"];
            if ([value isKindOfClass:[NSDictionary class]])
            {
                NSString *message = __parseStringField([value objectForKey:@"message"]);
                id<ISSCAttachment> image = __parseImageField ([value objectForKey:@"image"]);
                id<ISSCAttachment> music = __parseImageField ([value objectForKey:@"music"]);
                id<ISSCAttachment> video = __parseImageField ([value objectForKey:@"video"]);
                
                if ([ShareSDK getClientWithType:ShareTypeWhatsApp])
                {
                    [contentObj addWhatsAppUnitWithContent:message
                                                     image:image
                                                     music:music
                                                     video:video];
                }
            }
        }
        //平台自定义内容 结束
        
        return contentObj;
    }
    
    
    void __iosShareSDKOpen(void *appKey)
    {
        NSString *appKeyStr = [NSString stringWithCString:appKey encoding:NSUTF8StringEncoding];
        [ShareSDK registerApp:appKeyStr];
        
#ifdef __SHARESDK_WECHAT__
        [ShareSDK importWeChatClass:[WXApi class]];
#endif
        
#ifdef __SHARESDK_YIXIN__
        [ShareSDK importYiXinClass:[YXApi class]];
#endif
        
#ifdef __SHARESDK_PINTEREST__
        [ShareSDK importPinterestClass:[Pinterest class]];
#endif
        
#ifdef __SHARESDK_GOOGLEPLUS__
        [ShareSDK importGooglePlusClass:[GPPSignIn class] shareClass:[GPPShare class]];
#endif
        
#ifdef __SHARESDK_QQ__
        [ShareSDK importQQClass:[QQApiInterface class] tencentOAuthCls:[TencentOAuth class]];
#endif
        
#ifdef __SHARESDK_RENREN__
        [ShareSDK importRenRenClass:[RennClient class]];
#endif
      
#ifdef __SHARESDK_TENCENT_WEIBO__
        [ShareSDK importTencentWeiboClass:[WeiboApi class]];
#endif
    }
    
    void __iosShareSDKSetPlatformConfig(int platType, void *configInfo)
    {
        NSString *configInfoStr = nil;
        if (configInfo)
        {
            configInfoStr = [NSString stringWithCString:configInfo encoding:NSUTF8StringEncoding];
        }
        NSMutableDictionary *configInfoDict = [NSMutableDictionary dictionaryWithDictionary:[ShareSDK jsonObjectWithString:configInfoStr]];
        
        switch (platType)
        {
            case ShareTypeWeixiSession:
            case ShareTypeYiXinSession:
                [configInfoDict setObject:[NSNumber numberWithInt:0] forKey:@"scene"];
                break;
            case ShareTypeWeixiTimeline:
            case ShareTypeYiXinTimeline:
                [configInfoDict setObject:[NSNumber numberWithInt:1] forKey:@"scene"];
                break;
            case ShareTypeWeixiFav:
                [configInfoDict setObject:[NSNumber numberWithInt:2] forKey:@"scene"];
                break;
            default:
                break;
        }
        
        [ShareSDK connectPlatformWithType:platType
                                 platform:nil
                                  appInfo:configInfoDict];
    }
    
    void __iosShareSDKAuthorize (int platType, void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [ShareSDK authWithType:platType
                       options:nil
                        result:^(SSAuthState state, id<ICMErrorInfo> error) {
                            
                            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                            [resultDict setObject:[NSNumber numberWithInteger:1] forKey:@"action"];
                            [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"state"];
                            [resultDict setObject:[NSNumber numberWithInteger:platType] forKey:@"type"];
                            
                            if (state == SSResponseStateFail && error)
                            {
                                NSMutableDictionary *errorDict = [NSMutableDictionary dictionary];
                                [errorDict setObject:[NSNumber numberWithInteger:[error errorCode]] forKey:@"error_code"];
                                if ([error errorDescription])
                                {
                                    [errorDict setObject:[error errorDescription] forKey:@"error_msg"];
                                }
                                [resultDict setObject:errorDict forKey:@"error"];
                            }
                            
                            NSString *resultStr = [ShareSDK jsonStringWithObject:resultDict];
                            UnitySendMessage([observerStr UTF8String], "_callback", [resultStr UTF8String]);
                        }];
    }
    
    void __iosShareSDKCancelAuthorize (int platType)
    {
        [ShareSDK cancelAuthWithType:platType];
    }
    
    bool __iosShareSDKHasAuthorized (int platType)
    {
        return [ShareSDK hasAuthorizedWithType:platType];
    }
    
    void __iosShareSDKGetUserInfo (int platType, void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [ShareSDK getUserInfoWithType:platType
                          authOptions:nil
                               result:^(BOOL result, id<ISSPlatformUser> userInfo, id<ICMErrorInfo> error) {
                                   
                                   NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                                   [resultDict setObject:[NSNumber numberWithInteger:2] forKey:@"action"];
                                   [resultDict setObject:[NSNumber numberWithInteger:result ? SSResponseStateSuccess : SSResponseStateFail] forKey:@"state"];
                                   [resultDict setObject:[NSNumber numberWithInteger:platType] forKey:@"type"];
                                   
                                   if (!result && error)
                                   {
                                       NSMutableDictionary *errorDict = [NSMutableDictionary dictionary];
                                       [errorDict setObject:[NSNumber numberWithInteger:[error errorCode]] forKey:@"error_code"];
                                       if ([error errorDescription])
                                       {
                                           [errorDict setObject:[error errorDescription] forKey:@"error_msg"];
                                       }
                                       [resultDict setObject:errorDict forKey:@"error"];
                                   }
                                   else if ([userInfo sourceData])
                                   {
                                       [resultDict setObject:[userInfo sourceData] forKey:@"user"];
                                   }
                                   
                                   NSString *resultStr = [ShareSDK jsonStringWithObject:resultDict];
                                   UnitySendMessage([observerStr UTF8String], "_callback", [resultStr UTF8String]);
                                   
                               }];
    }
    
    void __iosShareSDKShare (int platType, void *content, void *observer)
    {
        NSString *observerStr = nil;
        id<ISSContent> contentObj = nil;
        
        observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        
        if (content)
        {
            NSString *contentStr = [NSString stringWithCString:content encoding:NSUTF8StringEncoding];
            contentObj = __getContentObjectWithString(contentStr);
        }
        
        [ShareSDK shareContent:contentObj
                          type:platType
                   authOptions:nil
                  shareOptions:nil
                 statusBarTips:NO
                        result:^(ShareType type, SSResponseState state, id<ISSPlatformShareInfo> statusInfo, id<ICMErrorInfo> error, BOOL end) {
                            
                            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                            [resultDict setObject:[NSNumber numberWithInteger:3] forKey:@"action"];
                            [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"state"];
                            [resultDict setObject:[NSNumber numberWithInteger:platType] forKey:@"type"];
                            [resultDict setObject:[NSNumber numberWithBool:end] forKey:@"end"];
                            
                            if (state == SSResponseStateFail && error)
                            {
                                NSMutableDictionary *errorDict = [NSMutableDictionary dictionary];
                                [errorDict setObject:[NSNumber numberWithInteger:[error errorCode]] forKey:@"error_code"];
                                if ([error errorDescription])
                                {
                                    [errorDict setObject:[error errorDescription] forKey:@"error_msg"];
                                }
                                [resultDict setObject:errorDict forKey:@"error"];
                            }
                            else if ([statusInfo sourceData])
                            {
                                if (type == ShareTypeRenren)
                                {
                                    [resultDict setObject:@{@"postid" : [statusInfo sourceData]}
                                                   forKey:@"share_info"];
                                }
                                else
                                {
                                    [resultDict setObject:[statusInfo sourceData]
                                                   forKey:@"share_info"];
                                }
                            }
                            
                            NSString *resultStr = [ShareSDK jsonStringWithObject:resultDict];
                            UnitySendMessage([observerStr UTF8String], "_callback", [resultStr UTF8String]);
                        }];
    }
    
    void __iosShareSDKOneKeyShare (void *platTypes, void *content, void *observer)
    {
        NSArray *platTypesArr = nil;
        NSString *observerStr = nil;
        id<ISSContent> contentObj = nil;
        
        observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        
        if (platTypes)
        {
            NSString *platTypesStr = [NSString stringWithCString:platTypes encoding:NSUTF8StringEncoding];
            platTypesArr = [ShareSDK jsonObjectWithString:platTypesStr];
        }
        
        if (content)
        {
            NSString *contentStr = [NSString stringWithCString:content encoding:NSUTF8StringEncoding];
            contentObj = __getContentObjectWithString(contentStr);
        }
        
        [ShareSDK oneKeyShareContent:contentObj
                           shareList:platTypesArr
                         authOptions:nil
                        shareOptions:nil
                       statusBarTips:NO
                              result:^(ShareType type, SSResponseState state, id<ISSPlatformShareInfo> statusInfo, id<ICMErrorInfo> error, BOOL end) {
                                  
                                  NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                                  [resultDict setObject:[NSNumber numberWithInteger:3] forKey:@"action"];
                                  [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"state"];
                                  [resultDict setObject:[NSNumber numberWithInteger:type] forKey:@"type"];
                                  [resultDict setObject:[NSNumber numberWithBool:end] forKey:@"end"];
                                  
                                  if (state == SSResponseStateFail && error)
                                  {
                                      NSMutableDictionary *errorDict = [NSMutableDictionary dictionary];
                                      [errorDict setObject:[NSNumber numberWithInteger:[error errorCode]] forKey:@"error_code"];
                                      if ([error errorDescription])
                                      {
                                          [errorDict setObject:[error errorDescription] forKey:@"error_msg"];
                                      }
                                      [resultDict setObject:errorDict forKey:@"error"];
                                  }
                                  else if ([statusInfo sourceData])
                                  {
                                      if (type == ShareTypeRenren)
                                      {
                                          [resultDict setObject:@{@"postid" : [statusInfo sourceData]}
                                                         forKey:@"share_info"];
                                      }
                                      else
                                      {
                                          [resultDict setObject:[statusInfo sourceData]
                                                         forKey:@"share_info"];
                                      }
                                  }
                                  
                                  NSString *resultStr = [ShareSDK jsonStringWithObject:resultDict];
                                  UnitySendMessage([observerStr UTF8String], "_callback", [resultStr UTF8String]);
                                  
                              }];
    }
    
    void __iosShareSDKShowShareMenu (void *platTypes, void *content, int x, int y, int direction, void *observer)
    {
        NSArray *platTypesArr = nil;
        NSString *observerStr = nil;
        id<ISSContent> contentObj = nil;
        
        observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];

        if (platTypes)
        {
            NSString *platTypesStr = [NSString stringWithCString:platTypes encoding:NSUTF8StringEncoding];
            platTypesArr = [ShareSDK jsonObjectWithString:platTypesStr];
        }
        
        if (content)
        {
            NSString *contentStr = [NSString stringWithCString:content encoding:NSUTF8StringEncoding];
            contentObj = __getContentObjectWithString(contentStr);
        }
        
        id<ISSContainer> container = nil;
        if ([UIDevice currentDevice].isPad)
        {
            if (!_refView)
            {
                _refView = [[UIView alloc] initWithFrame:CGRectMake(x, y, 10, 10)];
            }
            
             [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_refView];
            
            container = [ShareSDK container];
            [container setIPadContainerWithView:_refView arrowDirect:direction];
        }
        
        [ShareSDK showShareActionSheet:container
                             shareList:platTypesArr
                               content:contentObj
                         statusBarTips:NO
                           authOptions:nil
                          shareOptions:nil
                                result:^(ShareType type, SSResponseState state, id<ISSPlatformShareInfo> statusInfo, id<ICMErrorInfo> error, BOOL end) {
                                    
                                    NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                                    [resultDict setObject:[NSNumber numberWithInteger:3] forKey:@"action"];
                                    [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"state"];
                                    [resultDict setObject:[NSNumber numberWithInteger:type] forKey:@"type"];
                                    [resultDict setObject:[NSNumber numberWithBool:end] forKey:@"end"];
                                    
                                    if (state == SSResponseStateFail && error)
                                    {
                                        NSMutableDictionary *errorDict = [NSMutableDictionary dictionary];
                                        [errorDict setObject:[NSNumber numberWithInteger:[error errorCode]] forKey:@"error_code"];
                                        if ([error errorDescription])
                                        {
                                            [errorDict setObject:[error errorDescription] forKey:@"error_msg"];
                                        }
                                        [resultDict setObject:errorDict forKey:@"error"];
                                    }
                                    else if ([statusInfo sourceData])
                                    {
                                        if (type == ShareTypeRenren)
                                        {
                                            [resultDict setObject:@{@"postid" : [statusInfo sourceData]}
                                                           forKey:@"share_info"];
                                        }
                                        else
                                        {
                                            [resultDict setObject:[statusInfo sourceData]
                                                           forKey:@"share_info"];
                                        }
                                    }
                                    
                                    NSString *resultStr = [ShareSDK jsonStringWithObject:resultDict];
                                    
                                    NSLog (@"callback = %@", resultStr);
                                    
                                    UnitySendMessage([observerStr UTF8String], "_callback", [resultStr UTF8String]);
                                    
                                    if (_refView)
                                    {
                                        //移除视图
                                        [_refView removeFromSuperview];
                                    }
                                }];
    }
    
    void __iosShareSDKShowShareView (int platType, void *content, void *observer)
    {
        NSString *observerStr = nil;
        id<ISSContent> contentObj = nil;
        
        observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        
        if (content)
        {
            NSString *contentStr = [NSString stringWithCString:content encoding:NSUTF8StringEncoding];
            contentObj = __getContentObjectWithString(contentStr);
        }
        
        [ShareSDK showShareViewWithType:platType
                              container:nil
                                content:contentObj
                          statusBarTips:NO
                            authOptions:nil
                           shareOptions:nil
                                 result:^(ShareType type, SSResponseState state, id<ISSPlatformShareInfo> statusInfo, id<ICMErrorInfo> error, BOOL end) {
                                     
                                     NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                                     [resultDict setObject:[NSNumber numberWithInteger:3] forKey:@"action"];
                                     [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"state"];
                                     [resultDict setObject:[NSNumber numberWithInteger:type] forKey:@"type"];
                                     [resultDict setObject:[NSNumber numberWithBool:end] forKey:@"end"];
                                     
                                     if (state == SSResponseStateFail && error)
                                     {
                                         NSMutableDictionary *errorDict = [NSMutableDictionary dictionary];
                                         [errorDict setObject:[NSNumber numberWithInteger:[error errorCode]] forKey:@"error_code"];
                                         if ([error errorDescription])
                                         {
                                             [errorDict setObject:[error errorDescription] forKey:@"error_msg"];
                                         }
                                         [resultDict setObject:errorDict forKey:@"error"];
                                     }
                                     else if ([statusInfo sourceData])
                                     {
                                         if (type == ShareTypeRenren)
                                         {
                                             [resultDict setObject:@{@"postid" : [statusInfo sourceData]}
                                                            forKey:@"share_info"];
                                         }
                                         else
                                         {
                                             [resultDict setObject:[statusInfo sourceData]
                                                            forKey:@"share_info"];
                                         }
                                     }
                                     
                                     NSString *resultStr = [ShareSDK jsonStringWithObject:resultDict];
                                     UnitySendMessage([observerStr UTF8String], "_callback", [resultStr UTF8String]);
                                     
                                 }];
    }
    
    
    void __iosShareSDKGetFriendsList (int platType, void *page, void *observer)
    {
        ShareType shareType = (ShareType)platType;
        
        NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        
        id<ISSPage> pageInfo = nil;
        if (page)
        {
            NSString *pageStr = [NSString stringWithCString:page encoding:NSUTF8StringEncoding];
            NSDictionary *pageDict = [ShareSDK jsonObjectWithString:pageStr];
            
            if (shareType == ShareTypeTwitter)
            {
                NSInteger cursor = -1;
                id value = [pageDict objectForKey:@"cursor"];
                if ([value isKindOfClass:[NSNumber class]])
                {
                    cursor = [value integerValue];
                }
                pageInfo = [ShareSDK pageWithCursor:cursor];
            }
            else
            {
                NSInteger pageNo = 1;
                NSInteger pageSize = 0;
                
                id value = [pageDict objectForKey:@"pageNo"];
                if ([value isKindOfClass:[NSNumber class]])
                {
                    pageNo = [value integerValue];
                }
                value = [pageDict objectForKey:@"pageSize"];
                if ([value isKindOfClass:[NSNumber class]])
                {
                    pageSize = [value integerValue];
                }
                
                pageInfo = [ShareSDK pageWithPageNo:pageNo pageSize:pageSize];
            }
        }
        else
        {
            if (shareType == ShareTypeTwitter)
            {
                pageInfo = [ShareSDK pageWithCursor:-1];
            }
            else
            {
                pageInfo = [ShareSDK pageWithPageNo:(NSInteger)1 pageSize:0];
            }
        }
        
        
        
        [ShareSDK getFriendsWithType:shareType
                                page:pageInfo
                         authOptions:nil
                              result:^(SSResponseState state, NSArray *users, long long curr, long long prev, long long next, BOOL hasNext, NSDictionary *extInfo, id<ICMErrorInfo> error) {
                                  
                                  NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                                  [resultDict setObject:[NSNumber numberWithInteger:4] forKey:@"action"];
                                  [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"state"];
                                  [resultDict setObject:[NSNumber numberWithInteger:shareType] forKey:@"type"];
                                  
                                  if (state == SSResponseStateSuccess)
                                  {
                                      NSMutableArray *friends = [NSMutableArray array];
                                      for (int i = 0; i < [users count]; i++)
                                      {
                                          id<ISSPlatformUser> userInfo = [users objectAtIndex:i];
                                          
                                          [friends addObject:[userInfo sourceData]];
                                      }
                                      [resultDict setObject:friends forKey:@"users"];
                                  }
                                  else if (state == SSResponseStateFail)
                                  {
                                      if (error)
                                      {
                                          NSMutableDictionary *errorDict = [NSMutableDictionary dictionary];
                                          [errorDict setObject:[NSNumber numberWithInteger:[error errorCode]] forKey:@"error_code"];
                                          if ([error errorDescription])
                                          {
                                              [errorDict setObject:[error errorDescription] forKey:@"error_msg"];
                                          }
                                          [resultDict setObject:errorDict forKey:@"error"];
                                      }
                                  }
                                  
                                  NSString *resultStr = [ShareSDK jsonStringWithObject:resultDict];
                                  UnitySendMessage([observerStr UTF8String], "_callback", [resultStr UTF8String]);
                              }];
        

    }
    
    
    void __iosShareSDKGetCredential (int platType, void *observer)
    {
        ShareType shareType = (ShareType)platType;
        
        NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        
        id<ISSPlatformCredential> credential = [ShareSDK getCredentialWithType:shareType];
        NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
        [resultDict setObject:[NSNumber numberWithInteger:5] forKey:@"action"];
        [resultDict setObject:[NSNumber numberWithInteger:SSResponseStateSuccess] forKey:@"state"];
        [resultDict setObject:[NSNumber numberWithInteger:shareType] forKey:@"type"];
        
        if (credential)
        {
            NSMutableDictionary *credDict = [NSMutableDictionary dictionaryWithDictionary:[credential extInfo]];
            if ([credential uid])
            {
                [credDict setObject:[credential uid] forKey:@"uid"];
            }
            if ([credential token])
            {
                [credDict setObject:[credential token] forKey:@"token"];
            }
            if ([credential secret])
            {
                [credDict setObject:[credential secret] forKey:@"secret"];
            }
            if ([credential expired])
            {
                [credDict setObject:@([[credential expired] timeIntervalSince1970]) forKey:@"expired"];
            }
            [resultDict setObject:credDict forKey:@"credential"];
        }
        
        NSString *resultStr = [ShareSDK jsonStringWithObject:resultDict];
        UnitySendMessage([observerStr UTF8String], "_callback", [resultStr UTF8String]);
    }
    
#if defined (__cplusplus)
}
#endif

@implementation ShareSDKUnity3DBridge

@end
