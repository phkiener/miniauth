namespace LibPAM.Types;

/// <summary>
/// The style in which a message should be presented.
/// </summary>
public enum MessageStyle
{
    /// <summary>
    /// Prompt the user without echoing back the input.
    /// </summary>
    /// <remarks>PAM_PROMPT_ECHO_OFF</remarks>
    PromptWithoutEcho = 1,

    /// <summary>
    /// Prompt the user, echoing back the input.
    /// </summary>
    /// <remarks>PAM_PROMPT_ECHO_ON</remarks>
    Prompt = 2,

    /// <summary>
    /// An error message to display.
    /// </summary>
    /// <remarks>PAM_ERROR_MSG</remarks>
    ErrorMessage = 3,

    /// <summary>
    /// An informational text.
    /// </summary>
    /// <remarks>PAM_TEXT_INFO</remarks>
    Information = 4,

    /// <summary>
    /// Yes/no/maybe conditionals
    /// </summary>
    /// <remarks>PAM_RADIO_TYPE</remarks>
    Radio = 5,

    /// <summary>
    /// This is for server client non-human interaction.. these are NOT part of the X/Open PAM specification.
    /// </summary>
    /// <remarks>PAM_BINARY_PROMPT</remarks>
    BinaryPrompt = 7
}
