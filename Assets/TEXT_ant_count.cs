using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TEXT_ant_count : MonoBehaviour
{
    
    public TMP_Text antCount;
    public Transform hill;
    public GameObject ant;
    private int count = 0;


    //script is gonna instantiate ants as well as count them, even though I titled the file as count
    public void OnButtonClick()
    {
        // Instantiate at position (0, 0) and zero rotation.
        Instantiate(ant, hill.position, Quaternion.identity);
        count++;
        antCount.text = "Ant Count: " + count;
    }
}
