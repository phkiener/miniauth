namespace LibPAM.Types;

/// <summary>
/// Flags that can be passed to certain operations.
/// </summary>
[Flags]
public enum Flags
{
    /// <summary>
    /// Authentication service should not generate any messages.
    /// </summary>
    /// <remarks>PAM_SILENT</remarks>
    Silent = 0x8000,

    /// <summary>
    /// The authentication service should return PAM_AUTH_ERROR if the user has a
    /// null authentication token.
    /// </summary>
    /// <remarks>PAM_DISALLOW_NULL_AUTHTOK</remarks>
    DisallowNullAuthenticationToken = 0x0001,

    /// <summary>
    /// Set user credentials for an authentication service.
    /// </summary>
    /// <remarks>PAM_ESTABLISH_CRED</remarks>
    EstablishCredentials = 0x0002,

    /// <summary>
    /// Delete user credentials associated with an authentication service.
    /// </summary>
    /// <remarks>PAM_DELETE_CRED</remarks>
    DeleteCredentials = 0x0004,

    /// <summary>
    /// Reinitialize user credentials.
    /// </summary>
    /// <remarks>PAM_REINITIALIZE_CRED</remarks>
    ReinitializeCredentials = 0x0008,

    /// <summary>
    /// Extend lifetime of user credentials.
    /// </summary>
    /// <remarks>PAM_REFRESH_CRED</remarks>
    RefreshCredentials = 0x0010,

    /// <summary>
    /// The password service should only update those passwords that have aged;
    /// if this flag is not passed, the password service should update all passwords.
    /// </summary>
    /// <remarks>PAM_CHANGE_EXPIRED_AUTHTOK</remarks>
    ChangeExpiredAuthenticationToken = 0x0020,
}
