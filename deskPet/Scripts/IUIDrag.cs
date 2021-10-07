using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Screen = UnityEngine.Screen;

public class IUIDrag : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler {

    protected EventSystem es;
    protected GraphicRaycaster gra;
    List<RaycastResult> list;

    protected RectTransform rt;
    protected float minWidth;
    protected float maxWidth;
    protected float minHeight;
    protected float maxHeight;

    Vector3 offset;

    public virtual void Start()
    {
        list = new List<RaycastResult>();
        es = FindObjectOfType<EventSystem>();
        gra = FindObjectOfType<GraphicRaycaster>();
        rt = GetComponent<RectTransform>();
        SetRange();
    }
    public virtual void Update()
    {
        list = GraphicRaycaster(TransparentWindow.Instance.GetMousePosW2U());
        if (list.Count > 0)
        {
            TransparentWindow.Instance.chuantounot();
        }
        else
        {
            TransparentWindow.Instance.chuantoulong();
        }
    }
    private List<RaycastResult> GraphicRaycaster(Vector2 pos)
    {
        var mPointerEventData = new PointerEventData(es);
        mPointerEventData.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        gra.Raycast(mPointerEventData, results);
        return results;
    }
    void SetRange()
    {
        //minWidth = rt.rect.width * rt.pivot.x*transform.localScale.x;
        //maxWidth = Screen.width - rt.rect.width * (1 - rt.pivot.x) * transform.localScale.x;
        minWidth = rt.rect.width * rt.pivot.x;
        maxWidth = Screen.width - rt.rect.width * (1 - rt.pivot.x);
        minHeight = rt.rect.width * rt.pivot.y * transform.localScale.y;
        maxHeight = Screen.height - rt.rect.height * (1 - rt.pivot.y) * transform.localScale.y;
    }
    Vector3 LimitDragRange(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x,minWidth,maxWidth);
        pos.y= Mathf.Clamp(pos.y,minHeight,maxHeight);
        pos = new Vector3(pos.x,pos.y,0);
        return pos;
    }
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out worldPos);
        offset = rt.position - worldPos;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out mousePos);
        //SetRange();
        rt.position = LimitDragRange(mousePos + offset);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {

    }
}
