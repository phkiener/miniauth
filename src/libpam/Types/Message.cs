namespace LibPAM.Types;

/// <summary>
/// A message sent to the conversation by a module.
/// </summary>
/// <param name="Style">How the message should be displayed.</param>
/// <param name="Value">The message itself.</param>
public sealed record Message(MessageStyle Style, string Value);
