﻿using ImGuiNET;
using OpenKh.Tools.LayoutEditor.Interfaces;

namespace OpenKh.Tools.LayoutEditor
{
    public class AppLayoutEditor : IApp
    {
        public object SelectedLayoutEntry { get; private set; }

        public bool Run()
        {
            ImGui.Text("This is the layout editor!");

            return true;
        }
    }
}
