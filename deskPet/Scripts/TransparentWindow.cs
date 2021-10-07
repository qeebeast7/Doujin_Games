using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Application = UnityEngine.Application;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Windows.Forms;

public class TransparentWindow : MonoBehaviour
{
    private static TransparentWindow instance;
    public static TransparentWindow Instance
    {
        get
        {
            if (instance == null) instance = new TransparentWindow();
            return instance;
        }
    }

    [SerializeField]
    public Material m_Material;

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left; //最左坐标
        public int Top; //最上坐标
        public int Right; //最右坐标
        public int Bottom; //最下坐标
    }
    // Define function signatures to import from Windows APIs

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

    [DllImport("user32.dll")]
    private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);
    [DllImport("user32.dll")]
    private static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

    /// <summary>   
    /// 窗口置顶
    /// </summary>   
    /// <returns></returns>
    [DllImport("user32.dll")]
    private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
    /// <summary>   
    /// 得到当前活动的窗口   
    /// </summary>   
    /// <returns></returns>   
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    const uint LWA_COLORKEY = 0x1;
    // Definitions of window styles
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;

    private const uint WS_EX_LAYERED = 0x80000;
    private const int WS_EX_TRANSPARENT = 0x20;

    private const int GWL_EXSTYLE = (-20);
    private const int LWA_ALPHA = 0x2;
    IntPtr hwnd;

    public bool isPen=false;
    void Start()
    {
        Application.runInBackground = true;
        var margins = new MARGINS() { cxLeftWidth = -1 };

        hwnd = GetActiveWindow();

        DwmExtendFrameIntoClientArea(hwnd, ref margins);

        WindowTop();
        SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
    }
    private void LateUpdate()
    {
        //if (isPen) chuantoulong();
        //else chuantounot();
    }
    /// <summary> 
    /// 设置窗体置顶
    /// </summary> 
    private void WindowTop()
    {
        SetWindowPos(GetForegroundWindow(), -1, 0, 0, 0, 0, 1 | 2);
        uint intExTemp = GetWindowLong(hwnd, GWL_EXSTYLE);
    }

    /// <summary>
    /// 鼠标穿透
    /// </summary>
    public void chuantoulong()
    {
        var s = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, (uint)(s | WS_EX_TRANSPARENT));
    }

    public void chuantounot()
    {
        var s = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, (uint)(s & ~WS_EX_TRANSPARENT));
    }
    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }
    const int WS_BORDER = 0;

    public Vector2Int GetMousePosW2U()
    {
        RECT rect = new RECT();
        GetWindowRect(hwnd, ref rect);
        Vector2Int leftBottom = new Vector2Int(rect.Left, rect.Bottom);
        var mousePos = new Vector2Int(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
        var screenHeight = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        leftBottom.y = screenHeight - leftBottom.y;
        mousePos.y = screenHeight - mousePos.y;
        return mousePos;
    }

    float GetMouseMove(Vector2 last, Vector2 current)
    {
        return Mathf.Abs(current.x - last.x) + Mathf.Abs(current.y - last.y);
    }
}