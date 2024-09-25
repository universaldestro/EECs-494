using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int rupee_count = 0;
    int keys_count = 0;
    int bombs_count = 0;
    int hearts_count = 0;

    // for cheat code
    int maxRupees = 99;
    int maxKeys = 5;
    int maxBombs = 10;
    public int maxHearts = 6;
    public bool godmode = false;

    private void Start()
    {
        hearts_count = maxHearts;
    }
    public void AddRupees(int num_rupees)
    {
        rupee_count += num_rupees;
    }

    public int GetRupees()
    {
        return rupee_count;
    }

    public void AddKeys(int num_keys)
    {
        keys_count += num_keys;
    }

    public int GetKeys()
    {
        return keys_count;
    }

    public void AddBombs(int num_bombs)
    {
        bombs_count += num_bombs;
    }

    public int GetBombs()
    {
        return bombs_count;
    }

    public void AddHearts(int num_hearts)
    {
        Debug.Log("Added: ");
        Debug.Log(num_hearts);
        if (hearts_count < maxHearts || num_hearts <0) { 
            hearts_count += num_hearts;
        }
        
    }

    public int GetHearts()
    {
        return hearts_count;
    }

    public void CheatResources()
    {
        rupee_count = maxRupees;
        keys_count = maxKeys;
        bombs_count = maxBombs;
        hearts_count = maxHearts;
        godmode = true;
    }

    public void ResetResources()
    {
        hearts_count = maxHearts;
        keys_count = 0;
    }
}
