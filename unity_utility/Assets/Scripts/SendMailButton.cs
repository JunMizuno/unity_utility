using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMailButton : ButtonBase
{
    private const string MAIL_ADDRESS = "create.gamescene@gmail.com";
    private const string NEW_LINE = "\n";
    private const string CAUTION_STATEMENT = "------以下はそのまま送信ください。------" + NEW_LINE;

    private const string ORDER_OPEN_MAILER = "mailto:";

    protected override void OnClick()
    {
        base.OnClick();

        // @todo.mizuno ここに処理
        OpenMailer();
    }

    /// <summary>
    /// @todo. この機能は後ほど別スクリプトに移す予定
    /// </summary>
    private void OpenMailer()
    {
        // @todo.mizuno 本文添付の情報を設定
        string subjects = Application.productName;
        string deviceName = SystemInfo.deviceModel;

#if UNITY_IOS && !UNITY_EDITOR
        deviceName = UnityEngine.iOS.Device.generation.ToString();        
#endif

        string body = NEW_LINE + NEW_LINE + CAUTION_STATEMENT + NEW_LINE;
        body += "Device:" + deviceName + NEW_LINE;
        body += "OS:" + SystemInfo.operatingSystem + NEW_LINE;
        body += "Version:" + Application.version + NEW_LINE;
        body += "Language:" + Application.systemLanguage.ToString() + NEW_LINE;

        // エスケープ処理
        body = System.Uri.EscapeDataString(body);
        subjects = System.Uri.EscapeDataString(subjects);

        // URLスキームのような振る舞い
        Application.OpenURL(ORDER_OPEN_MAILER + MAIL_ADDRESS + "?subject=" + subjects + "&body=" + body);
    }
}
