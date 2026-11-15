namespace LibPAM.Types;

/// <summary>
/// A delegate than handles the callback for a conversation, i.e. responding to messages.
/// </summary>
public delegate Status ConversationDelegate(Message[] Messages, ref string[] Responses);
