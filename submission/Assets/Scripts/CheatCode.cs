using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    bool cheat_code = false;
    public Inventory inv;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cheat_code = !cheat_code;

            if (cheat_code)
            {
                inv.CheatResources();
                inv.godmode = true;
                Debug.Log("Cheated!");
            } else
            {
                inv.ResetResources();
                inv.godmode = false;
            }
        }
    }
}
