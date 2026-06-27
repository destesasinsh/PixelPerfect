using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Game.ClientState.Conditions;

namespace PixelPerfect;

public sealed partial class PixelPerfect
{
    private void DrawIndicator()
    {
        if (this.objectTable.LocalPlayer == null)
        {
            return;
        }

        if (this.ShouldHideIndicator())
        {
            return;
        }

        if (this.configuration.OnlyInCombat && !this.condition[ConditionFlag.InCombat])
        {
            return;
        }

        if (this.configuration.OnlyInInstance && !this.condition[ConditionFlag.BoundByDuty])
        {
            return;
        }

        if (!this.gameGui.WorldToScreen(this.objectTable.LocalPlayer.Position, out var position))
        {
            return;
        }

        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        ImGuiHelpers.ForceNextWindowMainViewport();
        ImGuiHelpers.SetNextWindowPosRelativeMainViewport(Vector2.Zero);
        ImGui.Begin("PixelPerfectCanvas", ImGuiWindowFlags.NoInputs
            | ImGuiWindowFlags.NoNav
            | ImGuiWindowFlags.NoTitleBar
            | ImGuiWindowFlags.NoScrollbar
            | ImGuiWindowFlags.NoBackground
            | ImGuiWindowFlags.NoFocusOnAppearing);
        ImGui.SetWindowSize(ImGui.GetIO().DisplaySize);

        var colour = ImGui.GetColorU32(this.configuration.Colour);
        if (this.configuration.Type == IndicatorType.Ring)
        {
            ImGui.GetWindowDrawList().AddCircle(position, this.configuration.Scale, colour, 64, 2f);
        }
        else
        {
            ImGui.GetWindowDrawList().AddCircleFilled(position, this.configuration.Scale, colour, 64);
        }

        ImGui.End();
        ImGui.PopStyleVar();
    }

    private bool ShouldHideIndicator()
    {
        return this.condition[ConditionFlag.Occupied38]
            || this.condition[ConditionFlag.OccupiedInEvent]
            || this.condition[ConditionFlag.OccupiedInQuestEvent]
            || this.condition[ConditionFlag.OccupiedInCutSceneEvent]
            || this.condition[ConditionFlag.BetweenAreas]
            || this.condition[ConditionFlag.BetweenAreas51]
            || this.condition[ConditionFlag.WatchingCutscene]
            || this.condition[ConditionFlag.WatchingCutscene78]
            || this.condition[ConditionFlag.BeingMoved];
    }
}
