using System;
using Dalamud.Configuration;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace PixelPerfect;

public sealed partial class PixelPerfect : IDalamudPlugin
{
    public string Name => "判定点指示器";

    private const string CommandName = "/pp";

    private readonly IDalamudPluginInterface pluginInterface;
    private readonly ICommandManager commandManager;
    private readonly IObjectTable objectTable;
    private readonly IGameGui gameGui;
    private readonly ICondition condition;
    private readonly WindowSystem windowSystem = new("PixelPerfect");
    private readonly ConfigWindow configWindow;
    private readonly Config configuration;

    public PixelPerfect(
        IDalamudPluginInterface pluginInterface,
        ICommandManager commandManager,
        IGameGui gameGui,
        ICondition condition,
        IObjectTable objectTable)
    {
        this.pluginInterface = pluginInterface;
        this.commandManager = commandManager;
        this.gameGui = gameGui;
        this.condition = condition;
        this.objectTable = objectTable;

        this.configuration = pluginInterface.GetPluginConfig() as Config ?? new Config();
        this.configWindow = new ConfigWindow(this.configuration, this.SaveConfig);
        this.windowSystem.AddWindow(this.configWindow);

        pluginInterface.UiBuilder.OpenMainUi += this.OpenConfig;
        pluginInterface.UiBuilder.OpenConfigUi += this.OpenConfig;
        pluginInterface.UiBuilder.Draw += this.DrawIndicator;
        pluginInterface.UiBuilder.Draw += this.windowSystem.Draw;

        commandManager.AddHandler(CommandName, new CommandInfo(this.Command)
        {
            HelpMessage = "打开判定点设置。",
        });
    }

    public void Dispose()
    {
        this.pluginInterface.UiBuilder.Draw -= this.windowSystem.Draw;
        this.pluginInterface.UiBuilder.Draw -= this.DrawIndicator;
        this.pluginInterface.UiBuilder.OpenConfigUi -= this.OpenConfig;
        this.pluginInterface.UiBuilder.OpenMainUi -= this.OpenConfig;
        this.commandManager.RemoveHandler(CommandName);
        this.windowSystem.RemoveAllWindows();
    }

    private void OpenConfig()
    {
        this.configWindow.IsOpen = true;
    }

    private void Command(string command, string arguments)
    {
        this.configWindow.Toggle();
    }

    private void SaveConfig()
    {
        this.pluginInterface.SavePluginConfig(this.configuration);
    }
}

public enum IndicatorType
{
    Dot,
    Ring,
}

[Serializable]
public class Config : IPluginConfiguration
{
    public int Version { get; set; } = 8;

    public IndicatorType Type { get; set; } = IndicatorType.Dot;

    public System.Numerics.Vector4 Colour { get; set; } = new(1f, 0f, 0f, 1f);

    public float Scale { get; set; } = 4f;

    public bool OnlyInCombat { get; set; }

    public bool OnlyInInstance { get; set; }
}
