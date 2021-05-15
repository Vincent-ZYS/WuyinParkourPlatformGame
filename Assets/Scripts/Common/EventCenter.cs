using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter : MonoBehaviour
{
    public static Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();

    public static void AddListner(EventType eventType, CallBack callBack)
    {
        if(!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }
        Delegate d = m_EventTable[eventType];
        if(d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(String.Format("尝试为事件{0}添加不同类型的委托,当前事件所对应的委托是{1},要添加的委托类型是{2}"
                ,eventType,d.GetType(),callBack.GetType()));
        }
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callBack;
    }
    public static void AddListner<T>(EventType eventType, CallBack<T> callBack)
    {
        if (!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }
        Delegate d = m_EventTable[eventType];
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(String.Format("尝试为事件{0}添加不同类型的委托,当前事件所对应的委托是{1},要添加的委托类型是{2}"
                , eventType, d.GetType(), callBack.GetType()));
        }
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callBack;
    }

    public static void RemoveListener(EventType eventType, CallBack callBack)
    {
        if(m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：对应事件{0}没有对应委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委,当前委托类型为{1},移除委托类型为{2}", 
                    eventType, eventType.GetType(), callBack.GetType()));
            }
        }else
        {
            throw new Exception(string.Format("移除监听错：尝试移除事件不存在！"));
        }
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callBack;
        if(m_EventTable[eventType]==null)
        {
            m_EventTable.Remove(eventType);
        }
    }
    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：对应事件{0}没有对应委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委,当前委托类型为{1},移除委托类型为{2}",
                    eventType, eventType.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错：尝试移除事件不存在！"));
        }
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
        if (m_EventTable[eventType] == null)
        {
            m_EventTable.Remove(eventType);
        }
    }

    public static void BroadCast(EventType eventType)
    {
        Delegate d;
        if(m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack callBack = d as CallBack;
            if(callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件委托{0}对应委托具有不同类型", eventType));
            }
        }
    }

    public static void BroadCast<T>(EventType eventType, T message)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                callBack(message);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件委托{0}对应委托具有不同类型", eventType));
            }
        }
    }
}
