using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    // Controlled inputs which lock other if one is pressed
    public bool mousedown_1;
    public bool mousedown_2;
    // ---------------
    public bool menu;
    // Raw inputs
    public bool mouse_1;
    public bool mouse_2;
    public bool mousePressed_1;
    // ---------------
    public bool lockMouseInputs;
}
