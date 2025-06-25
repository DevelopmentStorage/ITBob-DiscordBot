namespace ITBob_DiscordBot.Configuration;

public class Messages
{
    public ButtonLabels ButtonLabels { get; } = new();

    public string ReactionRoleRoleNameToShort { get; } =
        "Bitte gib den Namen der Rolle an, die du erstellen möchtest.";
}

public class ButtonLabels
{
    public string ConfirmReactionRoles { get; } = "Bestätige die Erstellung";
}