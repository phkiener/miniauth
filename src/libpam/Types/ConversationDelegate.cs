namespace LibPAM.Types;

public delegate Status ConversationDelegate(IReadOnlyList<Message> Messages, out string[] Responses);
