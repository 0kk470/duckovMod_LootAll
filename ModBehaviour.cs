using Duckov.UI;
using ItemStatsSystem;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace LootAll
{

    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private Button pickAllButton;

        private MethodInfo pickAllBtnMethod;

        void Awake()
        {
            LootView.onOpen += OnLootViewOpened;
            LootView.onClose += OnLootViewClosed;
        }
        void OnDestroy()
        {
            LootView.onOpen -= OnLootViewOpened;
            LootView.onClose -= OnLootViewClosed;
        }

        void OnLootViewOpened(ManagedUIElement lootView)
        {
            InitPickAllButton();
            RegisterInventoryEvents();
            RefreshButton();
        }

        void RegisterInventoryEvents()
        {
            var inventory = LootView.Instance?.TargetInventory;
            if (inventory == null)
                return;

            //UnityEngine.Debug.Log("Registering inventory events for LootAll mod.");
            inventory.onContentChanged -= OnInventoryChanged;
            inventory.onContentChanged += OnInventoryChanged;
        }

        void UnRegisterInventoryEvents()
        {
            var inventory = LootView.Instance?.TargetInventory;
            if (inventory == null)
                return;

            //UnityEngine.Debug.Log("UnRegistering inventory events for LootAll mod.");
            inventory.onContentChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(Inventory inventory, int arg2)
        {
            RefreshButton();
        }

        private void OnLootViewClosed(ManagedUIElement lootView)
        {
            UnRegisterInventoryEvents();
        }

        private void InitPickAllButton()
        {
            if (pickAllButton != null)
                return;

            var fieldInfo = typeof(LootView).GetField("pickAllButton", BindingFlags.NonPublic | BindingFlags.Instance);
            var originalBtn = fieldInfo.GetValue(LootView.Instance) as Button;
            var newBtn = GameObject.Instantiate(originalBtn.gameObject, originalBtn.transform.parent);
            if(newBtn != null)
                pickAllButton = newBtn.GetComponent<Button>();

            pickAllButton.onClick.RemoveAllListeners();
            pickAllButton.onClick.AddListener(PickAllClick);
            pickAllBtnMethod = typeof(LootView).GetMethod("OnPickAllButtonClicked", BindingFlags.NonPublic | BindingFlags.Instance);
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

        private void PickAllClick()
        {
            pickAllBtnMethod?.Invoke(LootView.Instance, null);
        }
    }
}