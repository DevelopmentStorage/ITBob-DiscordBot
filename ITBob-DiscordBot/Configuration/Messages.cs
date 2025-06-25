using ITBob_DiscordBot.Services;

namespace ITBob_DiscordBot.Configuration;

public class Messages
{
    public VerifyMessages VerifyMessages { get; set; } = new();
    public ReactionRoleMessages ReactionRoleMessages { get; set; } = new();
}

public class VerifyMessages
{
    public string VerifyStartMessage { get; set; } =
        """
        Willkommen auf dem Disocrd-Server von Irgendwas mit IT! 👋
        > Lese diese Message sorgfältig durch, um den Verifizierungsprozess zu verstehen. (https://discord.com/channels/1078249394161324053/1385340956403961886/1385351297036849242)
        > Kicke auf den Button unten, um den Verifizierungsprozess zu starten.
        > Warte auf die Verifizierung durch einen Admin, bevor du den Server vollständig nutzen kannst.
        """;

    public string VerifyModalTitle { get; set; } = "Verifizierung starten";
    public string VerifyModalNameOption { get; set; } = "Name";
    public string VerifyModalClassOption { get; set; } = "Klasse";

    public string VerifySuccessMessage { get; set; } =
        "Du wurdest verifiziert! Ein Admin wird dich in Kürze in die entsprechenden Rollen einordnen.";

    public string VerifyAdminRequestMessage { get; set; } =
        """
        Eine neue Verifizierungsanfrage wurde gestellt. Bitte überprüfe die Details und genehmige oder lehne sie ab.
        > **Name:** {0}
        > **Klasse:** {1}
        > **Ersteller:** <@{2}>
        """;


    public string VerifyFailedMessage { get; set; } =
        "Die Verifizierung ist fehlgeschlagen. Bitte versuche es erneut.";
}

public class ReactionRoleMessages
{
    public string AdminReactionRoleStartProcess { get; set; } =
        """
                Eine neue Reaction Role wurde erstellt. Bitte überprüfe die Details und genehmige oder lehne sie ab.
                > **Rollenname:** {0}
                > **Ersteller:** <@{1}>
                > **Game Name:** {2}
                > **Message URL:** {3}
        """;

    public string ForumThreadCreated { get; set; } =
        """
        ## :video_game: Willkommen im Game-Forum für {0}!
        > Bitte beachte, dass du hier nur Beiträge zu {0} erstellen solltest.
        > Wenn du selber eine role erstellen willst gehe zu <#{1}> und erstelle dort eine neue Reaction Role.
        > **Bitte gehe mit allen höflich um beachte unsere Regeln in <#1385340956403961886>!**
        """;

    public string IsNotAllowedToSendMessagesInGameThread { get; set; } =
        """
        -# ❗ Du bist nicht berechtigt, Nachrichten in diesem Game-Forum zu senden.
        -# (Bitte kontaktiere einen Admin, wenn du denkst, dass dies ein Fehler ist.)
        """;

    public string ReactionRoleSuccessfullyCreated { get; set; } =
        "Die Reaction Role wurde erfolgreich erstellt! Ein Admin wird diese nun überprüfen und genehmigen.";

    public ErrorData Errors { get; set; } = new();

    public class ErrorData
    {
        public string ReactionRoleRoleNameToShort { get; set; } =
            "Bitte gib den Namen der Rolle an, die du erstellen möchtest.";

        public string ReactionRoleCreatedFailed { get; set; } =
            "Die Reaction Role konnte nicht erstellt werden. Bitte versuche es später erneut.";

        public string ForumThreadCreatedFailed { get; set; } =
            "Die Erstellung des Forum-Threads ist fehlgeschlagen. Bitte versuche es später erneut.";

        public string AdminReactionRoleDenyFailed { get; set; } =
            "Die Anfrage für die Reaction Role konnte nicht abgelehnt werden. Bitte versuche es später erneut.";

        public string AdminReactionRoleDeny { get; set; } =
            "Die Anfrage für die Reaction Role wurde abgelehnt.";
    }
}