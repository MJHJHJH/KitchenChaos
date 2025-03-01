using System;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public abstract class UIFormLogicExtension : UIFormLogic
{
    private Canvas m_CachedCanvas = null;
    private CanvasGroup m_CanvasGroup = null;
    private List<Canvas> m_CachedCanvasContainer = new List<Canvas>();

    //UI的深度处理 - 用于层级管理
    public int OriginalDepth
    {
        get;
        private set;
    }
    public int Depth
    {
        get
        {
            return m_CachedCanvas.sortingOrder;
        }
    }

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
        m_CachedCanvas.overrideSorting = true;
        OriginalDepth = m_CachedCanvas.sortingOrder;
        m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
        RectTransform transform = GetComponent<RectTransform>();
        transform.anchorMin = Vector2.zero;
        transform.anchorMax = Vector2.one;
        transform.anchoredPosition = Vector2.zero;
        transform.sizeDelta = Vector2.zero;
        gameObject.GetOrAddComponent<GraphicRaycaster>();
    }

    protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
    {
        int oldDepth = Depth;
        base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        int deltaDepth = UIConst.DepthFactor * uiGroupDepth +
            UIConst.DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
        GetComponentsInChildren(true, m_CachedCanvasContainer);
        for (int i = 0; i < m_CachedCanvasContainer.Count; i++)
        {
            m_CachedCanvasContainer[i].sortingOrder += deltaDepth;
        }

        m_CachedCanvasContainer.Clear();
    }

    //管理事件的监听与销毁
    private EventSubscriber eventSubscriber;
    protected void Subscribe(int id, EventHandler<GameEventArgs> handler)
    {
        if (eventSubscriber == null)
            eventSubscriber = EventSubscriber.Create(this);

        eventSubscriber.Subscribe(id, handler);
    }

    protected void UnSubscribe(int id, EventHandler<GameEventArgs> handler)
    {
        if (eventSubscriber != null)
            eventSubscriber.UnSubscribe(id, handler);
    }

    protected void UnSubscribeAll()
    {
        if (eventSubscriber != null)
            eventSubscriber.UnSubscribeAll();
    }

    protected void Close()
    {
        GameEntry.UI.CloseUIForm(UIForm);
    }
}