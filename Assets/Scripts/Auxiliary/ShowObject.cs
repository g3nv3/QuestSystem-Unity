using UnityEngine;

public class ShowObject : MonoBehaviour
{
    public void show(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void hide(GameObject obj)
    {
        obj.SetActive(false);
    }
}
