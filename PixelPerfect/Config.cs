using System;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using System.Numerics;

namespace PixelPerfect;

public sealed class ConfigWindow : Window
{
    private readonly Config configuration;
    private readonly Action saveConfig;
    private readonly string[] indicatorTypes = { "圆点", "圆环" };

    public ConfigWindow(Config configuration, Action saveConfig)
        : base("判定点设置")
    {
        this.configuration = configuration;
        this.saveConfig = saveConfig;

        this.Size = new Vector2(380, 250);
        this.SizeCondition = ImGuiCond.FirstUseEver;
    }

    public override void Draw()
    {
        var type = (int)this.configuration.Type;
        if (ImGui.Combo("样式", ref type, this.indicatorTypes, this.indicatorTypes.Length))
        {
            this.configuration.Type = (IndicatorType)type;
            this.saveConfig();
        }

        var colour = this.configuration.Colour;
        if (ImGui.ColorEdit4("颜色", ref colour, ImGuiColorEditFlags.NoInputs))
        {
            this.configuration.Colour = colour;
            this.saveConfig();
        }

        ImGui.Text("快捷颜色");
        if (ImGui.Button("红色"))
        {
            this.SetColour(new Vector4(1f, 0f, 0f, 1f));
        }

        ImGui.SameLine();
        if (ImGui.Button("黄色"))
        {
            this.SetColour(new Vector4(1f, 1f, 0f, 1f));
        }

        ImGui.SameLine();
        if (ImGui.Button("绿色"))
        {
            this.SetColour(new Vector4(0f, 1f, 0f, 1f));
        }

        ImGui.SameLine();
        if (ImGui.Button("青色"))
        {
            this.SetColour(new Vector4(0f, 1f, 1f, 1f));
        }

        ImGui.SameLine();
        if (ImGui.Button("蓝色"))
        {
            this.SetColour(new Vector4(0f, 0.45f, 1f, 1f));
        }

        ImGui.SameLine();
        if (ImGui.Button("白色"))
        {
            this.SetColour(new Vector4(1f, 1f, 1f, 1f));
        }

        var scale = this.configuration.Scale;
        if (ImGui.SliderFloat("缩放", ref scale, 2f, 80f, "%.0f"))
        {
            this.configuration.Scale = Math.Clamp(scale, 2f, 80f);
            this.saveConfig();
        }

        var onlyInCombat = this.configuration.OnlyInCombat;
        if (ImGui.Checkbox("仅在进入战斗后显示", ref onlyInCombat))
        {
            this.configuration.OnlyInCombat = onlyInCombat;
            this.saveConfig();
        }

        var onlyInInstance = this.configuration.OnlyInInstance;
        if (ImGui.Checkbox("仅在副本中显示，例如迷宫、讨伐、raid 等", ref onlyInInstance))
        {
            this.configuration.OnlyInInstance = onlyInInstance;
            this.saveConfig();
        }
    }

    private void SetColour(Vector4 colour)
    {
        this.configuration.Colour = colour;
        this.saveConfig();
    }
}
