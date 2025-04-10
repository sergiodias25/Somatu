using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.CustomAnimation;

//------- Created by  : Hamza Herbou
//------- Email       : hamza95herbou@gmail.com

namespace EasyUI.Tabs
{
    public enum TabsType
    {
        Horizontal,
        Vertical
    }

    public abstract class TabsUI : MonoBehaviour
    {
        [System.Serializable]
        public class TabsUIEvent : UnityEvent<int> { }

        [Header("Tabs customization :")]
        [SerializeField]
        private Color themeColor = Color.gray;

        [SerializeField]
        private float tabSpacing = 2f;

        [Space]
        [Header("OnTabChange event :")]
        public TabsUIEvent OnTabChange;

        private TabButtonUI[] tabBtns;
        private GameObject[] tabContent;

#if UNITY_EDITOR
        private LayoutGroup layoutGroup;
#endif

        private Color tabColorActive,
            tabColorInactive;
        private int current,
            previous;

        private Transform parentBtns,
            parentContent;

        private int tabBtnsNum,
            tabContentNum;

        private void Start()
        {
            GetTabBtns();
        }

        private void OnEnable()
        {
            AnimateContent(tabContent[current]);
        }

        private void GetTabBtns()
        {
            parentBtns = transform.GetChild(0);
            parentContent = transform.GetChild(1);
            tabBtnsNum = parentBtns.childCount;
            tabContentNum = parentContent.childCount;

            if (tabBtnsNum != tabContentNum)
            {
                Debug.LogError(
                    "!!Number of <b>[Buttons]</b> is not the same as <b>[Contents]</b> ("
                        + tabBtnsNum
                        + " buttons & "
                        + tabContentNum
                        + " Contents)"
                );
                return;
            }

            tabBtns = new TabButtonUI[tabBtnsNum];
            tabContent = new GameObject[tabBtnsNum];
            for (int i = 0; i < tabBtnsNum; i++)
            {
                tabBtns[i] = parentBtns.GetChild(i).GetComponent<TabButtonUI>();
                int i_copy = i;
                tabBtns[i].uiButton.onClick.RemoveAllListeners();
                tabBtns[i].uiButton.onClick.AddListener(() => OnTabButtonClicked(i_copy));

                tabContent[i] = parentContent.GetChild(i).gameObject;
            }

            previous = current = 0;

            tabContent[0].SetActive(true); /* 
            AnimateContent(tabContent[0]); */
        }

        public void OnTabButtonClicked(int tabIndex)
        {
            if (current != tabIndex)
            {
                if (OnTabChange != null)
                    OnTabChange.Invoke(tabIndex);

                previous = current;
                current = tabIndex;

                tabContent[previous].SetActive(false);
                tabContent[current].SetActive(true);
                AnimateContent(tabContent[current]);
            }
        }

        private void AnimateContent(GameObject content)
        {
            foreach (var item in content.transform.GetComponentsInChildren<RectTransform>())
            {
                if (item.gameObject == content)
                {
                    continue;
                }
                CustomAnimation.StatsLoad(item);
            }
        }

#if UNITY_EDITOR
        public void Validate(TabsType type)
        {
            parentBtns = transform.GetChild(0);
            parentContent = transform.GetChild(1);
            tabBtnsNum = parentBtns.childCount;
            tabContentNum = parentContent.childCount;

            tabBtns = new TabButtonUI[tabBtnsNum];
            tabContent = new GameObject[tabBtnsNum];

            for (int i = 0; i < tabBtnsNum; i++)
            {
                tabBtns[i] = parentBtns.GetChild(i).GetComponent<TabButtonUI>();
                tabContent[i] = parentContent.GetChild(i).gameObject;
            }

            if (layoutGroup == null)
                layoutGroup = parentBtns.GetComponent<LayoutGroup>();

            if (type == TabsType.Horizontal)
                ((HorizontalLayoutGroup)layoutGroup).spacing = tabSpacing;
            else if (type == TabsType.Vertical)
                ((VerticalLayoutGroup)layoutGroup).spacing = tabSpacing;
        }
#endif
    }
}
