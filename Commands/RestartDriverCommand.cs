using LoupixDeck.PluginSdk;

namespace LoupixDeck.Plugin.Wacom.Commands;

/// <summary>
/// <c>Wacom.RestartDriver</c>: restarts the Wacom driver. Unlike the function keys it needs no
/// tablet, since it is meant for when the driver no longer reports one.
/// </summary>
internal sealed class RestartDriverCommand(WacomPlugin plugin) : IPluginCommand
{
    public CommandDescriptor Descriptor { get; } = FunctionCommand.Make("RestartDriver", "Restart Driver", "Restart", "\U000F0709", // mdi-restart
        "Restart the Wacom driver, for when the pen or tablet stops responding. The pen drops out for a second or two.");

    public ButtonTargets SupportedTargets => ButtonTargets.All;

    /// <summary>The restart waits for the old driver to exit, so it runs off the caller's thread.</summary>
    public Task Execute(CommandContext ctx) => Task.Run(() =>
    {
        try
        {
            if (!plugin.Driver.Restart()) FunctionCommand.Hint(ctx, "Restart failed");
        }
        catch (Exception ex)
        {
            ctx.Host.Logger.Error("Wacom RestartDriver failed", ex);
        }
    });
}
