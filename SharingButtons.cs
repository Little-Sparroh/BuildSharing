using Sparroh.UI;
using UnityEngine;

public class SharingButtons : MonoBehaviour
{
    private static bool _barRegistered;
    private static SharingButtons _active;

    public GearDetailsWindow window;
    private ModuleEquipSlots equipSlots;

    private void Update()
    {
        GearActionBar.Tick();

        if (window == null && Menu.Instance != null && Menu.Instance.IsOpen && Menu.Instance.WindowSystem != null)
            window = Menu.Instance.WindowSystem.GetTop() as GearDetailsWindow;

        if (GearActionBar.IsGearMenuOpen())
        {
            _active = this;
            RegisterBar();
        }
    }

    private void OnEnable()
    {
        _active = this;
        RegisterBar();
    }

    private void OnDisable()
    {
        if (_active == this)
            _active = null;
    }

    private static string Clipboard_GetText()
    {
        return GUIUtility.systemCopyBuffer;
    }

    private static void Clipboard_SetText(string text)
    {
        GUIUtility.systemCopyBuffer = text;
    }

    private void RegisterBar()
    {
        if (_barRegistered)
            return;

        GearActionBar.Register("copy_grid", "Copy Grid", GearActionBar.OrderCopyGrid, StaticCopyGrid,
            UIButtonStyle.Primary);
        GearActionBar.Register("paste_code", "Paste Code", GearActionBar.OrderPasteCode, StaticPasteCode);
        GearActionBar.SetSlotVisible("copy_grid", true);
        GearActionBar.SetSlotVisible("paste_code", true);
        _barRegistered = true;
    }

    private static void StaticCopyGrid()
    {
        var host = _active ?? FindObjectOfType<SharingButtons>();
        host?.CopyGridToClipboard();
    }

    private static void StaticPasteCode()
    {
        var host = _active ?? FindObjectOfType<SharingButtons>();
        host?.PasteCodeFromClipboard();
    }

    private void CopyGridToClipboard()
    {
        ResolveWindow();
        if (window == null || window.UpgradablePrefab == null) return;
        EnsureEquipSlots();
        if (equipSlots == null) return;

        var upgrades = UpgradeGridService.GetCurrentEquippedUpgrades(window, equipSlots);
        Clipboard_SetText(upgrades.ToString());
    }

    private void PasteCodeFromClipboard()
    {
        ResolveWindow();
        if (window == null || window.UpgradablePrefab == null) return;
        EnsureEquipSlots();
        if (equipSlots == null) return;

        var code = Clipboard_GetText();
        if (string.IsNullOrEmpty(code)) return;
        var equippedUpgrades = new EquippedUpgrades(code);
        UpgradeGridService.ApplyUpgradesToGrid(window, equipSlots, equippedUpgrades.Upgrades);
    }

    private void ResolveWindow()
    {
        if (window != null) return;
        if (Menu.Instance != null && Menu.Instance.IsOpen && Menu.Instance.WindowSystem != null)
            window = Menu.Instance.WindowSystem.GetTop() as GearDetailsWindow;
    }

    private void EnsureEquipSlots()
    {
        if (window == null) return;
        equipSlots = UpgradeGridService.ResolveEquipSlots(window);
    }
}