using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResetJson : MonoBehaviour
{
    [SerializeField] private QuestPresenter presenter;
    [SerializeField] private bool is_reset = false;
    private string key = "quest_data";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (is_reset)
            {
                presenter.can_save = false;
                string path = Path.Combine(Application.persistentDataPath, key);
                File.WriteAllText(path, "");
            }
            SceneManager.LoadScene(0);
        }
    }
}
