using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResetJson : MonoBehaviour
{
    [SerializeField] private QuestPresenter _presenter;
    [SerializeField] private bool _isReset = false;
    private string key = "quest_data";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isReset)
            {
                _presenter.CanSave = false;
                string path = Path.Combine(Application.persistentDataPath, key);
                File.WriteAllText(path, "");
            }
            SceneManager.LoadScene(0);
        }
    }
}
