using Duckov.UI;
using UnityEngine.UI;

namespace LootAll
{

    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private Button pickAllButton;

        void Awake()
        {
            LootView.onOpen += OnLootViewOpened;
        }
        void OnDestroy()
        {
            LootView.onOpen -= OnLootViewOpened;
        }

        void OnLootViewOpened(ManagedUIElement lootView)
        {
            FindPickAllButton();
            Invoke("RefreshButton", 0.05f);
        }

        private void FindPickAllButton()
        {
            if (pickAllButton != null)
                return;
            var fieldInfo = typeof(LootView).GetField("pickAllButton",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            pickAllButton = fieldInfo.GetValue(LootView.Instance) as Button;
        }

        private void RefreshButton()
        {
            if (pickAllButton != null)
            {
                var inventory = LootView.Instance.TargetInventory;
                pickAllButton.gameObject.SetActive(inventory != null);
                bool interactable = inventory?.GetItemCount() > 0;
                pickAllButton.interactable = interactable;
            }
        }
    }
}