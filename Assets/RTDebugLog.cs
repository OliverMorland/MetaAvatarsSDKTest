using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTDebugLog : MonoBehaviour
{
    private void Update()
    {
        if (RTDebug.m_Messages != null)
        {
            for (int i = 0; i < RTDebug.m_Messages.Count; i++)
            {
                switch (RTDebug.m_Messages[i].msgType)
                {
                    case RTDebug.MessageType.Log:
                        Debug.Log("RT: " + RTDebug.m_Messages[i].time.ToString("R") + ": " + RTDebug.m_Messages[i].msg);
                        break;
                    case RTDebug.MessageType.Warn:
                        Debug.LogWarning("RT: " + RTDebug.m_Messages[i].time.ToString("R") + ": " + RTDebug.m_Messages[i].msg);
                        break;
                    case RTDebug.MessageType.Error:
                        Debug.LogError("RT: " + RTDebug.m_Messages[i].time.ToString("R") + ": " + RTDebug.m_Messages[i].msg);
                        break;
                    default:
                        break;
                }
            }
            RTDebug.m_Messages.Clear();
        }
    }
}

public static class RTDebug
{
    public enum MessageType { Log, Warn, Error };

    public class RTDbgMessage
    {
        public float time;
        public string msg;
        public MessageType msgType;
    }

    public static GameObject RTDebugLogInstance = null;

    public static List<RTDbgMessage> m_Messages;

    static void updateMessageList(MessageType msgType, string msg)
    {
        if (RTDebugLogInstance == null)
        {
            RTDebugLogInstance = new GameObject("RTDebugLogInstance");
            RTDebugLogInstance.AddComponent<RTDebugLog>();
        }

        float t = Time.realtimeSinceStartup;
        RTDbgMessage rtDbgMsg = new RTDbgMessage();
        rtDbgMsg.time = t;
        rtDbgMsg.msg = msg + "\n" + Environment.StackTrace;
        rtDbgMsg.msgType = msgType;

        if (m_Messages == null)
            m_Messages = new List<RTDbgMessage>();
        m_Messages.Add(rtDbgMsg);
        m_Messages.Sort((a, b) => (a.time.CompareTo(b.time)));
    }
    public static void Log(string msg)
    {
        updateMessageList(MessageType.Log, msg);
    }
    public static void LogWarning(string msg)
    {
        updateMessageList(MessageType.Warn, msg);
    }
    public static void LogError(string msg)
    {
        updateMessageList(MessageType.Error, msg);
    }
}
