namespace LibPAM.Types;

/// <summary>
/// The types of items that can be set/retrieved.
/// </summary>
public enum ItemType
{
    /// <summary>
    /// The service name.
    /// </summary>
    /// <remarks>PAM_SERVICE</remarks>
    Service = 1,

    /// <summary>
    /// The user name.
    /// </summary>
    /// <remarks>PAM_USER</remarks>
    User = 2,

    /// <summary>
    /// The tty name.
    /// </summary>
    /// <remarks>PAM_TTY</remarks>
    TTY = 3,

    /// <summary>
    /// The remote host name.
    /// </summary>
    /// <remarks>PAM_RHOST</remarks>
    RemoteHost = 4,

    /// <summary>
    /// The pam_conv structure.
    /// </summary>
    /// <remarks>PAM_CONV</remarks>
    Conversation = 5,

    /// <summary>
    /// The authentication token (password).
    /// </summary>
    /// <remarks>PAM_AUTHTOK</remarks>
    AuthenticationToken = 6,

    /// <summary>
    /// The old authentication token.
    /// </summary>
    /// <remarks>PAM_OLDAUTHTOK</remarks>
    PreviousAuthenticationToken = 7,

    /// <summary>
    /// The remote user name.
    /// </summary>
    /// <remarks>PAM_RUSER</remarks>
    RemoteUser = 8,

    /// <summary>
    /// the prompt for getting a username.
    /// </summary>
    /// <remarks>PAM_USER_PROMPT</remarks>
    UserPrompt = 9,

    /// <summary>
    /// App supplied function to override failure delays.
    /// </summary>
    /// <remarks>PAM_FAIL_DELAY</remarks>
    FailureDelay = 10,

    /// <summary>
    /// X display name.
    /// </summary>
    /// <remarks>PAM_XDISPLAY</remarks>
    XDisplayName = 11,

    /// <summary>
    /// X server authentication data.
    /// </summary>
    /// <remarks>PAM_XAUTHDATA</remarks>
    XAuthenticationData = 12,

    /// <summary>
    /// The type for pam_get_authtok.
    /// </summary>
    /// <remarks>PAM_AUTHTOK_TYPE</remarks>
    AuthenticationTokenType = 13,
}
