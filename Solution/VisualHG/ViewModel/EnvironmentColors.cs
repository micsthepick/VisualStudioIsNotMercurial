﻿using System;
using System.Reflection;
using System.Windows;

namespace VisualHg.ViewModel
{
    public static class EnvironmentColors
    {
        private static readonly Type environmentColors =
            VisualStudioShell11.GetType("Microsoft.VisualStudio.PlatformUI.EnvironmentColors");

        public static readonly object HeaderBorderColorKey =
            GetKey("InactiveBorderColorKey", SystemColors.ControlColorKey);

        public static readonly object HeaderColorKey =
            GetKey("ToolWindowBackgroundColorKey", SystemColors.ControlLightColorKey);

        public static readonly object HeaderMouseDownColorKey =
            GetKey("CommandBarMouseDownBackgroundBeginColorKey", SystemColors.HighlightColorKey);

        public static readonly object HeaderMouseDownTextColorKey =
            GetKey("CommandBarTextMouseDownColorKey", SystemColors.HighlightTextColorKey);

        public static readonly object HeaderMouseOverColorKey = GetKey("CommandBarMouseOverBackgroundBeginColorKey",
            SystemColors.ControlLightLightColorKey);

        public static readonly object HeaderTextColorKey =
            GetKey("ToolWindowTextColorKey", SystemColors.WindowTextColorKey);

        public static readonly object HighlightColorKey =
            GetKey("SystemHighlightColorKey", SystemColors.HighlightColorKey);

        public static readonly object HighlightTextColorKey =
            GetKey("SystemHighlightTextColorKey", SystemColors.HighlightTextColorKey);

        public static readonly object InactiveSelectionHighlightColorKey =
            GetKey("InactiveBorderColorKey", SystemColors.HighlightColorKey);

        public static readonly object InactiveSelectionHighlightTextColorKey =
            GetKey("ToolWindowTextColorKey", SystemColors.HighlightTextColorKey);

        public static readonly object MenuBorderColorKey =
            GetKey("DropDownPopupBorderColorKey", SystemColors.ActiveBorderColorKey);

        public static readonly object MenuColorKey =
            GetKey("DropDownPopupBackgroundBeginColorKey", SystemColors.MenuColorKey);

        public static readonly object MenuHighlightColorKey =
            GetKey("CommandBarMenuItemMouseOverColorKey", SystemColors.HighlightColorKey);

        public static readonly object MenuHighlightTextColorKey =
            GetKey("CommandBarMenuItemMouseOverTextColorKey", SystemColors.HighlightTextColorKey);

        public static readonly object WindowColorKey =
            GetKey("ToolWindowBackgroundColorKey", SystemColors.WindowColorKey);

        public static readonly object WindowTextColorKey =
            GetKey("ToolWindowTextColorKey", SystemColors.WindowTextColorKey);


        private static object GetKey(string name, ResourceKey resourceKey)
        {
            if (environmentColors == null)
            {
                return resourceKey;
            }

            return GetValue(environmentColors, name);
        }

        private static object GetValue(Type type, string name)
        {
            return type.GetProperty(name).GetValue(null, BindingFlags.Static, null, null, null);
        }
    }
}