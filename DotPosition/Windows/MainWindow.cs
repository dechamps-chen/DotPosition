using System;
using System.Numerics;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace DotPosition.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin;
    private bool _showColorPicker;

    public MainWindow(Plugin plugin) : base($"Dot Position###DotPositionWindow", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 230),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        Plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        float checkboxWidth = ImGui.CalcTextSize("Enabled").X + ImGui.GetStyle().FramePadding.X * 2 + 25;
        float statusWidth = ImGui.CalcTextSize("Active").X + 10;
        var isEnabled = Plugin.Configuration.IsPluginEnabled;
        if (ImGui.Checkbox("Enabled", ref isEnabled))
        {
            Plugin.Configuration.IsPluginEnabled = isEnabled;
            Plugin.PluginInterface.SavePluginConfig(Plugin.Configuration);
        }

        ImGui.SameLine(ImGui.GetContentRegionAvail().X - statusWidth);
        if (Plugin.Configuration.IsPluginEnabled)
        {
            ImGui.TextColored(ImGuiColors.ParsedGreen, "Active");
        }
        else
        {
            ImGui.TextColored(ImGuiColors.DalamudRed, "Inactive");
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        ImGui.Text("Dot Color Settings:");
        ImGui.SameLine();
        if (ImGui.ColorButton("##DotColorPreview", Plugin.Configuration.DotColor,
            ImGuiColorEditFlags.NoAlpha))
        {
            ImGui.OpenPopup("##DotColorPicker");
        }


        if (ImGui.BeginPopup("##DotColorPicker"))
        {
            Vector4 color = Plugin.Configuration.DotColor;
            if (ImGui.ColorPicker4("##Picker", ref color,
                ImGuiColorEditFlags.NoSidePreview | ImGuiColorEditFlags.NoSmallPreview))
            {
                Plugin.Configuration.DotColor = color;
                Plugin.PluginInterface.SavePluginConfig(Plugin.Configuration);
            }

            if (ImGui.Button("Close", new Vector2(ImGui.GetContentRegionAvail().X, 24)))
            {
                ImGui.CloseCurrentPopup();
            }

            ImGui.EndPopup();
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.Text("Click to open color picker");
            ImGui.Text($"Current: R:{Plugin.Configuration.DotColor.X:0.00} G:{Plugin.Configuration.DotColor.Y:0.00} B:{Plugin.Configuration.DotColor.Z:0.00}");
            ImGui.EndTooltip();
        }

        ImGui.SameLine();
        ImGui.Text("<-- Click here to open color picker");

        if (_showColorPicker && ImGui.BeginPopup("##DotColorPicker"))
        {
            Vector4 color = Plugin.Configuration.DotColor;
            if (ImGui.ColorPicker4("##Picker", ref color,
                ImGuiColorEditFlags.NoSidePreview | ImGuiColorEditFlags.NoSmallPreview))
            {
                Plugin.Configuration.DotColor = color;
                Plugin.PluginInterface.SavePluginConfig(Plugin.Configuration);
            }

            if (ImGui.Button("Close", new Vector2(ImGui.GetContentRegionAvail().X, 24)))
            {
                _showColorPicker = false;
                ImGui.CloseCurrentPopup();
            }

            if (!ImGui.IsPopupOpen("##DotColorPicker"))
            {
                _showColorPicker = false;
            }

            ImGui.EndPopup();
        }

        ImGui.Spacing();
        ImGui.Text("Color Presets:");
        ImGui.SameLine();

        if (ImGui.ColorButton("##WhitePreset", new Vector4(1f, 1f, 1f, 1f),
            ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip))
        {
            SetColor(1, 1, 1);
        }
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
        {
            ImGui.BeginTooltip();
            ImGui.TextColored(new Vector4(1f, 1f, 1f, 1f), "White");
            ImGui.EndTooltip();
        }

        ImGui.SameLine();

        if (ImGui.ColorButton("##RedPreset", new Vector4(1f, 0f, 0f, 1f),
            ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip))
        {
            SetColor(1, 0, 0);
        }
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
        {
            ImGui.BeginTooltip();
            ImGui.TextColored(new Vector4(1f, 0f, 0f, 1f), "Red");
            ImGui.EndTooltip();
        }

        ImGui.SameLine();

        if (ImGui.ColorButton("##GreenPreset", new Vector4(0f, 1f, 0f, 1f),
            ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip))
        {
            SetColor(0, 1, 0);
        }
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
        {
            ImGui.BeginTooltip();
            ImGui.TextColored(new Vector4(0f, 1f, 0f, 1f), "Green");
            ImGui.EndTooltip();
        }

        ImGui.SameLine();

        if (ImGui.ColorButton("##BluePreset", new Vector4(0f, 0f, 1f, 1f),
            ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip))
        {
            SetColor(0, 0, 1);
        }
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
        {
            ImGui.BeginTooltip();
            ImGui.TextColored(new Vector4(0f, 0f, 1f, 1f), "Blue");
            ImGui.EndTooltip();
        }

        ImGui.SameLine();

        if (ImGui.ColorButton("##CyanPreset", new Vector4(0f, 1f, 1f, 1f),
            ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip))
        {
            SetColor(0, 1, 1);
        }
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
        {
            ImGui.BeginTooltip();
            ImGui.TextColored(new Vector4(0f, 1f, 1f, 1f), "Cyan");
            ImGui.EndTooltip();
        }

        ImGui.SameLine();

        if (ImGui.ColorButton("##MagentaPreset", new Vector4(1f, 0f, 1f, 1f),
            ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip))
        {
            SetColor(1, 0, 1);
        }
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
        {
            ImGui.BeginTooltip();
            ImGui.TextColored(new Vector4(1f, 0f, 1f, 1f), "Magenta");
            ImGui.EndTooltip();
        }

        ImGui.SameLine();

        if (ImGui.ColorButton("##YellowPreset", new Vector4(1f, 1f, 0f, 1f),
            ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip))
        {
            SetColor(1, 1, 0);
        }
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
        {
            ImGui.BeginTooltip();
            ImGui.TextColored(new Vector4(1f, 1f, 0f, 1f), "Yellow");
            ImGui.EndTooltip();
        }

        ImGui.SameLine();

        if (ImGui.ColorButton("##BlackPreset", new Vector4(0f, 0f, 0f, 1f),
            ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip))
        {
            SetColor(0, 0, 0);
        }
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
        {
            ImGui.BeginTooltip();
            ImGui.TextColored(new Vector4(0f, 0f, 0f, 1f), "Black");
            ImGui.EndTooltip();
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        float radius = Plugin.Configuration.DotRadius;
        ImGui.Text("Dot Radius:");
        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X * 0.7f);
        if (ImGui.SliderFloat("##Radius", ref radius, 1f, 10f, "%.1f"))
        {
            Plugin.Configuration.DotRadius = radius;
            Plugin.PluginInterface.SavePluginConfig(Plugin.Configuration);
        }

        ImGui.SameLine();
        ImGui.Text("Preview:");
        ImGui.SameLine();
        ImGui.GetWindowDrawList().AddCircleFilled(
            ImGui.GetCursorScreenPos() + new Vector2(10, 10),
            radius,
            ImGui.GetColorU32(Plugin.Configuration.DotColor)
        );
    }

    void SetColor(float r, float g, float b)
    {
        Plugin.Configuration.DotColor = new(r, g, b, 1);
        Plugin.PluginInterface.SavePluginConfig(Plugin.Configuration);
    }
}
