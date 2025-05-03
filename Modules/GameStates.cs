using AmongUs.GameOptions;

namespace UnlockDleks.Modules;

public static class GameStates
{
    public static bool IsNormalGame => GameOptionsManager.Instance.CurrentGameOptions.GameMode is GameModes.Normal or GameModes.NormalFools;
    public static bool IsHideNSeek => GameOptionsManager.Instance.CurrentGameOptions.GameMode is GameModes.HideNSeek or GameModes.SeekFools;
    public static bool IsMeeting => AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Joined && MeetingHud.Instance;
    public static bool DleksIsActive => (MapNames)GameOptionsManager.Instance.CurrentGameOptions.MapId == MapNames.Dleks;
}