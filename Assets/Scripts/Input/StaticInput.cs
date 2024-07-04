using UnityEngine;

public class StaticInput : MonoBehaviour
{
    private static PlayerInput instance;

    public static PlayerInput get_instance()
    {
        if(instance == null)
        {
            instance = new PlayerInput();
        }
        return instance;
    }
}
