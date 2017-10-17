using UnityEngine;
using System.Collections;

public class HideMouse : MonoBehaviour {

	public static bool showCursor = true;
    public bool show_cursor;

	// Use this for initialization
	void Start () {
		Cursor.visible = HideMouse.showCursor;
	}
    private void Update()
    {
        if (showCursor != show_cursor)
        {
            ToggleCursor();
            showCursor = show_cursor;
        }   
    }

 

	public static void ToggleCursor() {
		HideMouse.showCursor = !HideMouse.showCursor;
		Cursor.visible = HideMouse.showCursor;
	}

	public static void ShowCursor() {
		HideMouse.showCursor = true;
		Cursor.visible = HideMouse.showCursor;
	}
	public static void HideCursor() {
		HideMouse.showCursor = false;
		Cursor.visible = HideMouse.showCursor;
	}
}
