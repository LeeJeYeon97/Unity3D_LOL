using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{

    // Cursor Tex
    Texture2D attackIcon;
    Texture2D handIcon;

    private Ray ray;
    private RaycastHit hit;
    private int mask = (1 << (int)Define.Layer.RED_MINION | 1 << (int)Define.Layer.GROUND
        | (int)Define.Layer.RED_TURRET | (int)Define.Layer.PLAYER);

    void Start()
    {

        handIcon = Resources.Load<Texture2D>("Textures/UI/cursors/hand1");
        attackIcon = Resources.Load<Texture2D>("Textures/UI/cursors/hoverenemy");
    }

    void Update()
    {
        UpdateMouseCursor();
    }

    void UpdateMouseCursor()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            if (hit.transform.gameObject.layer == (int)Define.Layer.GROUND)
            {
                Cursor.SetCursor(handIcon, new Vector2(handIcon.width / 6, 0), CursorMode.Auto);
            }
            else // Enemy
            {
                Cursor.SetCursor(attackIcon, new Vector2(attackIcon.width / 6, 0), CursorMode.Auto);
                //Cursor.SetCursor(attackIcon, new Vector2(0, 0), CursorMode.Auto);
            }
        }
    }
}
