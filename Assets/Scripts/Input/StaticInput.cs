using UnityEngine;

public class StaticInput : MonoBehaviour
{
    private static PlayerInput instance;

    public static PlayerInput GetInstance()
    {
        if(instance == null)
        {
            instance = new PlayerInput();
        }
        return instance;
    }
}
