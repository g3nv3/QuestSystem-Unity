using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuestView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private QuestPresenter _presenter;

    [Header("Info panel")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [Header("List panel")]
    [SerializeField] private GameObject _listPanel;
    [SerializeField] private Transform _questParent;
    [SerializeField] private GameObject _questPrefab;
    private List<QuestCell> cells = new List<QuestCell>();
    private List<GameObject> cellsObj = new List<GameObject>();
    private bool listVisible = false;

    [Header("Selected Quest")]
    [SerializeField] private GameObject _selectedPanel;
    [SerializeField] private GameObject _selectBtn;
    [SerializeField] private QuestSelectedPanel _questSelected;
    [SerializeField] private TextMeshProUGUI _btnSelectText;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _highlightedColor;
    [SerializeField] private Color _baseColor;
    private Image highlitedImg;
    private bool hasSelected;
    private bool highlighted;
    private QuestData highlitedData;

    private void OnEnable()
    {
        StaticInput.GetInstance().UI.Enable();
        StaticInput.GetInstance().UI.VisiblePanel.performed += SetVisibleList;

        QuestBus.GetInstance().OnHighlighted += Highlight;
        QuestBus.GetInstance().OnInterrupt += Interrupt;
    }

    private void Start()
    {
        _listPanel.SetActive(listVisible);
        _selectedPanel.SetActive(hasSelected);
    }

    private void OnDisable()
    {
        StaticInput.GetInstance().UI.VisiblePanel.performed -= SetVisibleList;
        QuestBus.GetInstance().OnHighlighted -= Highlight;
        QuestBus.GetInstance().OnInterrupt -= Interrupt;
    }

    public void Load(List<QuestData> data)
    {
        foreach(QuestData item in data)
        {
            var obj = CreateQuestCell(item);
            if (!item.selected) continue;

            hasSelected = true;
            highlitedData = item;
            highlitedImg = obj.GetComponent<Image>();
            highlitedImg.color = _selectedColor;
            _selectedPanel.SetActive(true);

            QuestBus.GetInstance().OnSelect?.Invoke(highlitedData);
        }
    }

    public void StartQuest(QuestData data)
    {
        ShowPanel(data, "Получен квест");
        CreateQuestCell(data);
    }

    public void FinishQuest(QuestData data)
    {
        ShowPanel(data, "Квест завершен");
        if (data.selected)
            _selectedPanel.SetActive(false);

        RemoveQuest(data);
    }

    private void Interrupt(QuestData data)
    {
        RemoveQuest(data);
        _presenter.Interrupt(data);
    }

    private void RemoveQuest(QuestData data)
    {
        int cell_ind = cells.FindIndex(q => ReferenceEquals(q.Data, data));
        Destroy(cellsObj[cell_ind]);

        cellsObj.RemoveAt(cell_ind);
        cells.RemoveAt(cell_ind);
    }
    private void ShowPanel(QuestData data, string name)
    {
        if (!data.animation_start && data.progress < data.goal)
            return;
        if (!data.animation_finish && data.progress >= data.goal)
            return;
        _nameText.text = $"{name}: {data.quest_name}";
        _descriptionText.text = $"{data.quest_description}\n{DescriptionText(data, data.progress < data.goal)}";
        _animator.SetTrigger("Show");
    }
    private string DescriptionText(QuestData data, bool is_start)
    {
        if (is_start)
            return $"Цель: {data.goal}";
        return $"Получено: {data.gold_reward} золота";
    }

    private void Highlight(QuestData data, Image image)
    {
        UnhighlAll();

        highlitedImg = image;
        highlitedData = data;
        highlighted = true;
        highlitedData.highlighted = true;

        _selectBtn.SetActive(highlighted);
        _btnSelectText.text = data.selected ? "Unselect" : "Select";
        highlitedImg.color = highlitedData.selected ? _selectedColor : _highlightedColor;
    }
    private void UnhighlAll()
    {
        _presenter.UnhighlAll();
        _selectBtn.SetActive(false);

        foreach (var cell in cellsObj)
        {
            var img = cell.GetComponent<Image>();
            img.color = img.color == _selectedColor ? _selectedColor : _baseColor;
        }

        highlitedData = null;
        highlighted = false;
    }

    public void select()
    {
        UnselectAll();

        hasSelected = !highlitedData.selected;
        highlighted = true;

        _presenter.select(highlitedData);
        _selectedPanel.SetActive(hasSelected);
        highlitedImg.color = hasSelected ? _selectedColor : _highlightedColor;
        _btnSelectText.text = hasSelected ? "Unselect" : "Select";

        QuestBus.GetInstance().OnSelect?.Invoke(highlitedData);
    }

    private void UnselectAll()
    {
        _presenter.UnselAll(highlitedData);
        foreach (var cell in cellsObj)
        {
            var img = cell.GetComponent<Image>();
            img.color = _baseColor;
        }
    }
    private void SetVisibleList(InputAction.CallbackContext a)
    {
        UnhighlAll();
        listVisible = !listVisible;
        _listPanel.SetActive(listVisible);
        if (listVisible)
            QuestBus.GetInstance().OnUpdateData?.Invoke();
    }
    private GameObject CreateQuestCell(QuestData data)
    {
        GameObject cell_obj = Instantiate(_questPrefab, _questParent.position, Quaternion.identity);
        cell_obj.transform.SetParent(_questParent);
        cell_obj.gameObject.transform.localScale = Vector3.one;
        cellsObj.Add(cell_obj);

        var cell = cell_obj.GetComponent<QuestCell>();
        cell.Init(data);
        cells.Add(cell);
        return cell_obj;
    }
}
