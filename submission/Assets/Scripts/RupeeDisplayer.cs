using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RupeeDisplayer : MonoBehaviour
{
    public Inventory inventory;
    public HasHealth playerHealth;
    TextMeshProUGUI text_component;
    // Start is called before the first frame update
    void Start()
    {
        text_component = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory != null && text_component != null)
        {
            string displayText = "Rupees: " + inventory.GetRupees().ToString() + " Keys: " + inventory.GetKeys().ToString() + "\nHP: " + inventory.GetHearts().ToString();
            text_component.text = displayText;
        }
        
    }
}
