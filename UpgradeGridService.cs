using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class UpgradeGridService
{
    private static readonly FieldInfo EquipSlotsField =
        typeof(GearDetailsWindow).GetField("equipSlots", BindingFlags.NonPublic | BindingFlags.Instance);

    public static ModuleEquipSlots ResolveEquipSlots(GearDetailsWindow window)
    {
        if (window == null)
            return null;

        return EquipSlotsField?.GetValue(window) as ModuleEquipSlots;
    }

    public static EquippedUpgrades GetCurrentEquippedUpgrades(GearDetailsWindow window, ModuleEquipSlots equipSlots)
    {
        var gearId = window.UpgradablePrefab.Info.ID;
        var list = new List<EquippedUpgrade>();
        var hexMap = equipSlots.HexMap;
        var upgradePositions = new Dictionary<UpgradeInstance, List<(int, int)>>();

        for (var i = 0; i < hexMap.Height; i++)
        for (var j = 0; j < hexMap.Width; j++)
        {
            var node = hexMap[j, i];
            if (node.enabled && node.upgrade != null)
            {
                if (!upgradePositions.ContainsKey(node.upgrade))
                    upgradePositions[node.upgrade] = new List<(int, int)>();
                upgradePositions[node.upgrade].Add((j, i));
            }
        }

        foreach (var kvp in upgradePositions)
        {
            var upgrade = kvp.Key;
            var positions = kvp.Value;
            var rotation = upgrade.GetRotation(window.UpgradablePrefab);
            var id = upgrade.Upgrade.ID.ID;
            int minX = int.MaxValue, minY = int.MaxValue;
            foreach (var pos in positions)
                if (pos.Item1 < minX || (pos.Item1 == minX && pos.Item2 < minY))
                {
                    minX = pos.Item1;
                    minY = pos.Item2;
                }

            list.Add(new EquippedUpgrade(minX, minY, rotation, id));
        }

        return new EquippedUpgrades(gearId, list);
    }

    public static void ApplyUpgradesToGrid(GearDetailsWindow window, ModuleEquipSlots equipSlots,
        List<EquippedUpgrade> upgrades)
    {
        var hexMap = equipSlots.HexMap;
        for (var i = 0; i < hexMap.Height; i++)
        for (var j = 0; j < hexMap.Width; j++)
        {
            var node = hexMap[j, i];
            if (node.upgrade != null)
                equipSlots.Unequip(window.UpgradablePrefab, node.upgrade);
        }

        var groupedUpgrades = upgrades.GroupBy(up => up.ID).ToDictionary(g => g.Key, g => g.ToList());
        var availableInstances = GetAvailableUpgradeInstances(window);

        foreach (var kvp in groupedUpgrades)
        {
            var id = kvp.Key;
            var upgradeList = kvp.Value;
            if (!availableInstances.ContainsKey(id))
                continue;
            var instances = availableInstances[id];
            var usedInstances = new List<UpgradeInstance>();
            foreach (var up in upgradeList)
                if (instances.Count > 0)
                {
                    var instance = instances[0];
                    instances.RemoveAt(0);
                    EquipUpgrade(window, equipSlots, up, instance, !usedInstances.Contains(instance));
                    usedInstances.Add(instance);
                }
                else
                {
                    break;
                }
        }
    }

    public static Dictionary<int, List<UpgradeInstance>> GetAvailableUpgradeInstances(GearDetailsWindow window)
    {
        var dict = new Dictionary<int, List<UpgradeInstance>>();
        var upgradable = window.UpgradablePrefab;

        var enumerator = new PlayerData.UpgradeEnumerator(upgradable);
        while (enumerator.MoveNext())
        {
            var id = enumerator.Upgrade.Upgrade.ID.ID;
            if (!dict.ContainsKey(id)) dict[id] = new List<UpgradeInstance>();
            dict[id].Add(enumerator.Upgrade);
        }

        enumerator = new PlayerData.UpgradeEnumerator(Global.Instance);
        while (enumerator.MoveNext())
        {
            var id = enumerator.Upgrade.Upgrade.ID.ID;
            if (!dict.ContainsKey(id)) dict[id] = new List<UpgradeInstance>();
            dict[id].Add(enumerator.Upgrade);
        }

        return dict;
    }

    public static void EquipUpgrade(GearDetailsWindow window, ModuleEquipSlots equipSlots, EquippedUpgrade up,
        UpgradeInstance upgrade, bool checkOffset = true)
    {
        var hexMap = upgrade.GetPattern().GetModifiedMap(up.Rotation);
        int minK = int.MaxValue, minL = int.MaxValue;
        for (var l = 0; l < hexMap.Height; l++)
        for (var k = 0; k < hexMap.Width; k++)
            if (hexMap[k, l].enabled)
                if (k < minK || (k == minK && l < minL))
                {
                    minK = k;
                    minL = l;
                }

        if (minK == int.MaxValue) minK = 0;
        if (minL == int.MaxValue) minL = 0;
        var offsetX = up.X - minK;
        var offsetY = up.Y - minL;
        int gxMin = int.MaxValue, gxMax = int.MinValue;
        int gyMin = int.MaxValue, gyMax = int.MinValue;
        for (var l = 0; l < hexMap.Height; l++)
        for (var k = 0; k < hexMap.Width; k++)
            if (hexMap[k, l].enabled)
            {
                var gx = offsetX + k;
                var gy = offsetY + l;
                if (gx < gxMin) gxMin = gx;
                if (gx > gxMax) gxMax = gx;
                if (gy < gyMin) gyMin = gy;
                if (gy > gyMax) gyMax = gy;
            }

        var adjustX = 0;
        var adjustY = 0;
        if (gxMin < 0) adjustX = -gxMin;
        if (gxMax >= equipSlots.Width) adjustX -= Mathf.Max(0, gxMax - equipSlots.Width + 1);
        if (gyMin < 0) adjustY = -gyMin;
        if (gyMax >= equipSlots.Height) adjustY -= Mathf.Max(0, gyMax - equipSlots.Height + 1);
        offsetX += adjustX;
        offsetY += adjustY;
        equipSlots.Unequip(window.UpgradablePrefab, upgrade);
        var flag = equipSlots.Unequip(window.UpgradablePrefab, upgrade);
        var result = equipSlots.EquipModule(window.UpgradablePrefab, upgrade, offsetX, offsetY, up.Rotation);
        if (!(result || flag))
        {
            equipSlots.Unequip(window.UpgradablePrefab, upgrade);
            flag = equipSlots.Unequip(window.UpgradablePrefab, upgrade);
            result = equipSlots.EquipModule(window.UpgradablePrefab, upgrade, offsetX, offsetY - 1, up.Rotation);
            if (result || flag)
                offsetY -= 1;
        }

        if (checkOffset && result)
        {
            var actualMin = GetActualUpgradePosition(equipSlots, upgrade);
            if (actualMin.HasValue)
            {
                var actualX = actualMin.Value.Item1;
                var actualY = actualMin.Value.Item2;
                var deltaX = up.X - actualX;
                var deltaY = up.Y - actualY;
                if (deltaX != 0 || deltaY != 0)
                {
                    equipSlots.Unequip(window.UpgradablePrefab, upgrade);
                    equipSlots.EquipModule(window.UpgradablePrefab, upgrade, offsetX + deltaX, offsetY + deltaY,
                        up.Rotation);
                }
            }
        }
    }

    public static (int, int)? GetActualUpgradePosition(ModuleEquipSlots equipSlots, UpgradeInstance upgrade)
    {
        var hexMap = equipSlots.HexMap;
        int? minX = null, minY = null;
        for (var i = 0; i < hexMap.Height; i++)
        for (var j = 0; j < hexMap.Width; j++)
        {
            var node = hexMap[j, i];
            if (node.enabled && node.upgrade == upgrade)
                if (minX == null || j < minX || (j == minX && i < minY))
                {
                    minX = j;
                    minY = i;
                }
        }

        if (minX.HasValue) return (minX.Value, minY.Value);
        return null;
    }
}