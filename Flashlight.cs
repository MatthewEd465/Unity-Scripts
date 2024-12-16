using UnityEngine;

public class Flashlight : MonoBehaviour
{
    
    
    Light light;

        void Start()
    {
        light = GetComponent<Light>();
    }

    
    void Update()
    {
       if(Input.GetKeyUp(KeyCode.B)) 
       {
            light.enabled = !light.enabled;
       }
    }
}
