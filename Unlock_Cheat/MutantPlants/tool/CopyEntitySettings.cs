﻿using UnityEngine;
using STRINGS;
using System;

namespace Unlock_Cheat.MutantPlants
{
    class MutantCopyBuildingSettings : KMonoBehaviour
    {
        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.RefreshUserMenu, (object data) => OnRefreshUserMenu(data));
        }

        private void OnRefreshUserMenu(object data)
        {
            UserMenu userMenu = Game.Instance.userMenu;
            GameObject gameObject = base.gameObject;
            string iconName = "action_mirror";
            string text = UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.NAME;
            var on_click = new System.Action(ActivateCopyTool);
            Enum.TryParse(nameof(Action.BuildingUtility1), out Action shortcutKey);
            string tooltipText = UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.TOOLTIP;
            userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, shortcutKey, null, null, null, tooltipText, true), 1f);
        }

        private void ActivateCopyTool()
        {
            MutantCopySettingsTool.instance.SetSourceObject(base.gameObject);
            MutantCopySettingsTool.instance.Activate();
        }
    }
}
