using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuestView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private QuestPresenter presenter;

    [Header("Info panel")]
    [SerializeField] private TextMeshProUGUI name_txt;
    [SerializeField] private TextMeshProUGUI desc_txt;

    [Header("List panel")]
    [SerializeField] private GameObject list_panel;
    [SerializeField] private Transform quest_parent;
    [SerializeField] private GameObject quest_prefab;
    private List<QuestCell> cells = new List<QuestCell>();
    private List<GameObject> cells_obj = new List<GameObject>();
    private bool list_visible = false;

    [Header("Selected Quest")]
    [SerializeField] private GameObject selected_panel;
    [SerializeField] private GameObject select_btn;
    [SerializeField] private QuestSelectedPanel questSelected;
    [SerializeField] private TextMeshProUGUI btn_sel_text;
    [SerializeField] private Color sel_color;
    [SerializeField] private Color highl_color;
    [SerializeField] private Color base_color;
    private Image highl_img;
    private bool has_selected;
    private bool highlighted;
    private QuestData highlighted_data;

    private void OnEnable()
    {
        StaticInput.get_instance().UI.Enable();
        StaticInput.get_instance().UI.VisiblePanel.performed += set_visible_list;
        QuestBus.get_instance().on_highlighted += highlight;
    }

    private void Start()
    {
        list_panel.SetActive(list_visible);
        selected_panel.SetActive(has_selected);
    }

    private void OnDisable()
    {
        StaticInput.get_instance().UI.VisiblePanel.performed -= set_visible_list;
        QuestBus.get_instance().on_highlighted -= highlight;
    }

    public void load(List<QuestData> data)
    {
        foreach(QuestData item in data)
        {
            var obj = create_quest_cell(item);
            if (!item.selected) continue;

            highlighted_data = item;
            has_selected = true;
            highl_img = obj.GetComponent<Image>();
            highl_img.color = sel_color;
            selected_panel.SetActive(true);

            QuestBus.get_instance().on_select?.Invoke(highlighted_data);
        }
    }

    private void highlight(QuestData data, Image image)
    {
        unhighl_all();

        highl_img = image;
        highlighted_data = data;
        highlighted = true;
        highlighted_data.highlighted = true;

        select_btn.SetActive(highlighted);
        btn_sel_text.text = data.selected ? "Unselect" : "Select";
        highl_img.color = highlighted_data.selected ? sel_color : highl_color;
    }

    public void select()
    {
        unsel_all();

        has_selected = !highlighted_data.selected;
        highlighted = true;

        presenter.select(highlighted_data);
        selected_panel.SetActive(has_selected);
        highl_img.color = has_selected ? sel_color : highl_color;
        btn_sel_text.text = has_selected ? "Unselect" : "Select";

        QuestBus.get_instance().on_select?.Invoke(highlighted_data);
    }

    private void unhighl_all()
    {
        presenter.unhighl_all();
        select_btn.SetActive(false);

        foreach (var cell in cells_obj)
        {
            var img = cell.GetComponent<Image>();
            img.color = img.color == sel_color ? sel_color : base_color;
        }

        highlighted_data = null;
        highlighted = false;
    }

    private void unsel_all()
    {
        presenter.unsel_all(highlighted_data);
        foreach (var cell in cells_obj)
        {
            var img = cell.GetComponent<Image>();
            img.color = base_color;
        }
    }

    private void set_visible_list(InputAction.CallbackContext a)
    {
        unhighl_all();
        list_visible = !list_visible;
        list_panel.SetActive(list_visible);
        if (list_visible)
            QuestBus.get_instance().on_update_data?.Invoke();
    }

    private void show_panel(QuestData data, string name)
    {
        if (!data.animation_start && data.progress < data.goal)
            return;
        if (!data.animation_finish && data.progress >= data.goal) 
            return; 
        name_txt.text = $"{name}: {data.quest_name}";
        desc_txt.text = $"{data.quest_description}\n{dec_txt(data, data.progress < data.goal)}";
        animator.SetTrigger("Show");
    }

    private string dec_txt(QuestData data, bool is_start)
    {
        if (is_start)
            return $"Цель: {data.goal}";
        return $"Получено: {data.gold_reward} золота";
    }

    private GameObject create_quest_cell(QuestData data)
    {
        GameObject cell_obj = Instantiate(quest_prefab, quest_parent.position, Quaternion.identity);
        cell_obj.transform.SetParent(quest_parent);
        cell_obj.gameObject.transform.localScale = Vector3.one;
        cells_obj.Add(cell_obj);

        var cell = cell_obj.GetComponent<QuestCell>();
        cell.init(data);
        cells.Add(cell);
        return cell_obj;
    }

    public void start_quest(QuestData data)
    {
        show_panel(data, "Получен квест");
        create_quest_cell(data);
    }

    public void finish_quest(QuestData data)
    {
        show_panel(data, "Квест завершен");
        if(data.selected) selected_panel.SetActive(false);

        int cell_ind = cells.FindIndex(q => ReferenceEquals(q.data, data));
        Destroy(cells_obj[cell_ind]);

        cells_obj.RemoveAt(cell_ind);
        cells.RemoveAt(cell_ind);
    }
}
