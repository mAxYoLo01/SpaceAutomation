using UnityEngine;

public class TabManager : MonoBehaviour
{
    public Tab currentTab;

    public static TabManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of TabManager found!");
            return;
        }
        instance = this;
    }

    public void SwitchToTab(int tab)
    {
        switch ((Tab)tab)
        {
            case Tab.MAIN:
                BuildManager.instance.OpenMainView();
                break;
            case Tab.BUILD:
                BuildManager.instance.OpenBuildView();
                break;
            case Tab.EDIT:
                BuildManager.instance.OpenEditView();
                break;
            default:
                Debug.LogError("Error: Unknown Tab!");
                break;
        }
        currentTab = (Tab)tab;
    }
}

public enum Tab
{
    MAIN,
    BUILD,
    EDIT
}
