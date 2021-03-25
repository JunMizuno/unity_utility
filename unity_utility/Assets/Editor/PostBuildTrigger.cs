using System.Collections;
using System.Collections.Generic;
using System.IO;                    // File
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;        // PostProcessBuild
using System.Reflection;            // BindingFlags

//#if UNITY_IOS || UNITY_IPHONE
using UnityEditor.iOS.Xcode;
//#endif

public static class PostBuildTrigger
{
    enum Position { Begin, End, Pause, Return };

    static string newLine = "\n";
    static string googleFileName = "GoogleService-Info.plist";
    static string entitlementsArrayKey = "com.apple.developer.applesignin";
    static string defaultAccessLevel = "Default";
    static string authenticationServicesFramework = "AuthenticationServices.framework";
    static BindingFlags nonPublicInstanceBinding = BindingFlags.NonPublic | BindingFlags.Instance;

    // @memo.mizuno この属性の引数は処理する順番を指定する。
    [PostProcessBuild(100)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            BasicProcessForIOS(target, path);
        }
        else if (target == BuildTarget.Android)
        {
            BasicProcessForAndroid(target, path);
        }
    }

    public static void BasicProcessForIOS(BuildTarget target, string path)
    {
        string projPath = PBXProject.GetPBXProjectPath(path);
        PBXProject proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));

        // @memo.mizuno Unity2019からPBXProject.GetUnityTargetName()の代わりに使用するように警告があった。
        // @memo.mizuno もしこれでビルド時にリンカーエラーが出る場合、GetUnityFrameworkTargetGuid()を使用すること。
        // @memo.mizuno サイバーエージェントさんのブログでは、構成切り替えを上手にされているので参考のこと。
        // @memo.mizuno https://creator.game.cyberagent.co.jp/?p=7382
        string projectTarget = proj.GetUnityMainTargetGuid();

        // Add dependencies
        proj.AddFrameworkToProject(projectTarget, "AssetsLibrary.framework", false);
        proj.AddFrameworkToProject(projectTarget, "CoreText.framework", false);
        proj.AddFrameworkToProject(projectTarget, "MobileCoreServices.framework", false);
        proj.AddFrameworkToProject(projectTarget, "QuickLook.framework", false);
        proj.AddFrameworkToProject(projectTarget, "Security.framework", false);
        proj.AddFrameworkToProject(projectTarget, "Photos.framework", false);
        proj.AddFrameworkToProject(projectTarget, "AdSupport.framework", false);
        proj.AddFrameworkToProject(projectTarget, "CoreData.framework", false);
        proj.AddFrameworkToProject(projectTarget, "MapKit.framework", false);
        proj.AddFrameworkToProject(projectTarget, "MessageUI.framework", false);
        proj.AddFrameworkToProject(projectTarget, "libz.dylib", false);
        proj.AddFrameworkToProject(projectTarget, "UserNotifications.framework", false);        // FCM

        proj.AddFrameworkToProject(projectTarget, "CoreTelephony.framework", true);
        proj.AddFrameworkToProject(projectTarget, "PassKit.framework", true);
        proj.AddFrameworkToProject(projectTarget, "Social.framework", true);
        proj.AddFrameworkToProject(projectTarget, "StoreKit.framework", true);
        proj.AddFrameworkToProject(projectTarget, "Twitter.framework", true);
        proj.AddFrameworkToProject(projectTarget, authenticationServicesFramework, true);

        proj.AddFileToBuild(projectTarget, proj.AddFile("usr/lib/libz.1.dylib", "Frameworks/libz.1.dylib", PBXSourceTree.Sdk));
        proj.AddFileToBuild(projectTarget, proj.AddFile("usr/lib/libxml2.2.dylib", "Frameworks/libxml2.2.dylib", PBXSourceTree.Sdk));
        proj.AddFileToBuild(projectTarget, proj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", PBXSourceTree.Sdk));
        proj.AddFileToBuild(projectTarget, proj.AddFile("usr/lib/libc++.1.dylib", "Frameworks/libc++.1.dylib", PBXSourceTree.Sdk));

        proj.SetBuildProperty(projectTarget, "ENABLE_BITCODE", "false");
        proj.AddBuildProperty(projectTarget, "OTHER_LDFLAGS", "-lz");

        proj.AddCapability(projectTarget, PBXCapabilityType.PushNotifications);
        proj.AddCapability(projectTarget, PBXCapabilityType.InAppPurchase);

        string plistPath = path + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));

        // iOS13.0.0以降では要らなくなっているから？？
        string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
        if (plist.root.values.ContainsKey(exitsOnSuspendKey))
        {
            plist.root.values.Remove(exitsOnSuspendKey);
        }

        File.WriteAllText(projPath, proj.WriteToString());

        plist.root.values.Remove("NSAppTransportSecurity");
        var appTransportSecurity = plist.root.CreateDict("NSAppTransportSecurity");
        appTransportSecurity?.SetBoolean("NSAllowsArbitraryLoadsInWebContent", true);

        plist.root.SetBoolean("UIRequiresFullScreen", true);
        plist.root.SetString("NSCameraUsageDescription", "${PRODUCT_NAME} used camera");
        plist.root.SetString("NSPhotoLibraryUsageDescription", "${PRODUCT_NAME} used photos");
        plist.root.SetString("NSCalendarsUsageDescription", "Some ad content may access calendar");
        plist.root.SetString("", "");

        // Set Google Mobile Ads
        plist.root.SetString("", "");







        // @memo.mizuno 20201214 Tapjoy Push Notification の実装対応
        InsertTapjoyAuthCodeIntoControllerClass(path);
    }

    /// <summary>
    /// 分割ファイルをビルドする
    /// </summary>
    /// <param name="target"></param>
    /// <param name="path"></param>
    public static void BasicProcessForAndroid(BuildTarget target, string path)
    {



    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="imports"></param>
    /// <param name="methodSignatures"></param>
    /// <param name="valuesToAppend"></param>
    /// <param name="positionsInMethod"></param>
    private static void InsertCodeIntoClass(string filePath, string[] imports, string[] methodSignatures, string[] valuesToAppend, Position[] positionsInMethod)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        string fileContent = File.ReadAllText(filePath);
        List<int> ignoredIndices = new List<int>();

        for (int n = 0; n < valuesToAppend.Length; ++n)
        {
            string val = valuesToAppend[n];

            if (fileContent.Contains(val))
            {
                ignoredIndices.Add(n);
            }
        }

        string[] fileLines = File.ReadAllLines(filePath);
        List<string> newContents = new List<string>();
        bool found = false;
        int foundIndex = -1;
        int braceCount = 0;

        if (imports != null)
        {
            for (int i = 0; i < imports.Length; i++)
            {
                newContents.Add(imports[i]);
            }
        }

        foreach (string line in fileLines)
        {
            if (imports != null)
            {
                string targetSTR = line.Trim();
                bool isContinue = false;
                for (int i = 0; i < imports.Length; i++)
                {
                    if (targetSTR.Contains(imports[i].Trim()))
                    {
                        isContinue = true;
                        break;
                    }
                }

                if (isContinue)
                {
                    continue;
                }
            }

            newContents.Add(line + newLine);
            for (int n = 0; n < methodSignatures.Length; n++)
            {
                if ((line.Trim().Equals(methodSignatures[n])) && !ignoredIndices.Contains(n))
                {
                    foundIndex = n;
                    found = true;
                    braceCount = 0;
                }
            }

            if (found)
            {
                if (line.Trim().Equals("{"))
                {
                    braceCount += 1;
                }

                if ((positionsInMethod[foundIndex] == Position.Begin) && line.Trim().Equals("{"))
                {
                    newContents.Add(valuesToAppend[foundIndex] + newLine);
                    found = false;
                }
                else if ((positionsInMethod[foundIndex] == Position.End) && line.Trim().Equals("}"))
                {
                    if (1 < braceCount)
                    {
                        braceCount -= 1;
                    }
                    else
                    {
                        newContents = newContents.GetRange(0, newContents.Count - 1);
                        newContents.Add(valuesToAppend[foundIndex] + newLine + "}" + newLine);
                        found = false;
                    }
                }
                else if ((positionsInMethod[foundIndex] == Position.Pause) && line.Trim().Equals("UnitySetPlayerFocus(1);"))
                {
                    newContents.Add(valuesToAppend[foundIndex] + newLine);
                    found = false;
                }
                else if ((positionsInMethod[foundIndex] == Position.Return) && (line.Trim().Equals("return;") || line.Trim().Equals("return YES;") || line.Trim().Equals("return NO;")))
                {
                    newContents = newContents.GetRange(0, newContents.Count - 1);
                    newContents.Add(valuesToAppend[foundIndex] + newLine + line + newLine);
                    found = false;
                }
            }
        }

        string output = string.Join("", newContents.ToArray());
        File.WriteAllText(filePath, output);
    }

    /// <summary>
    /// For Tapjoy Push Notification at iOS Platform
    /// </summary>
    /// <param name="projectPath"></param>
    public static void InsertTapjoyAuthCodeIntoControllerClass(string projectPath)
    {
        var filePath = projectPath + "/Classes/UnityAppController.mm";
        string[] imports = { "#import <Tapjoy/Tapjoy.h>" + newLine, "#import <UserNotifications/UserNotifications.h>" + newLine};
        var methodSignatures = new string[]
        {
            "- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo",
            "- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken",
            "- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions",
        };
        var valuesToAppend = new string[]
        {
            newLine + " [Tapjoy setReceiveRemoteNotification:userInfo];" + newLine,
            newLine + " [Tapjoy setDeviceToken:deviceToken];" + newLine,
            newLine + " if (@available(iOS 12.0, *)) {" +
            newLine + "UNUserNotificationCenter* center = [UNUserNotificationCenter currentNotificationCenter];" +
            newLine +
            newLine + "[center requestAuthorizationWithOptions:(UNAuthorizationOptionAlert + UNAuthorizationOptionSound + UNAuthorizationOptionBadge + UNAuthorizationOptionProvisional)" +
            newLine + "completionHandler:^(BOOL granted, NSError * _Nullable error) {" +
            newLine + "[application registerForRemoteNotifications];" +
            newLine + "}];" +
            newLine + " // Registering for remote notifications" +
            newLine + " } else if ([application respondsToSelector:@selector(isRegisteredForRemoteNotifications)]) {" +
            newLine + "     // iOS 8 Notifications" +
            newLine + "     [applications registerUserNotificationSettings:[UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeSound | UIUserNotificationAlert | UIUserNotificationTypeBadge) catrgories:nil]];" +
            newLine + "     [applications registerForRemoteNotifications];" +
            newLine + " } else {" +
            newLine + "     // iOS < 8 Notifications" +
            newLine + "     [applications registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeSound)];" +
            newLine + " }" + newLine,
        };
        var positionsInMethod = new Position[]
        {
            Position.End,
            Position.End,
            Position.Return,
        };

        InsertCodeIntoClass(filePath, imports, methodSignatures, valuesToAppend, positionsInMethod);
    }
}
