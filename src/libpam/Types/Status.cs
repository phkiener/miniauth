namespace LibPAM.Types;

/// <summary>
/// The status that is reported after a PAM operation.
/// </summary>
public enum Status
{
    /// <summary>
    /// Successful function return
    /// </summary>
    /// <remarks>PAM_SUCCESS</remarks>
    Success = 0,

    /// <summary>
    /// dlopen() failure when dynamically loading a service module
    /// </summary>
    /// <remarks>PAM_OPEN_ERR</remarks>
    OpenError = 1,

    /// <summary>
    /// Symbol not found
    /// </summary>
    /// <remarks>PAM_SYMBOL_ERR</remarks>
    SymbolError = 2,

    /// <summary>
    /// Error in service module
    /// </summary>
    /// <remarks>PAM_SERVICE_ERR</remarks>
    ServiceError = 3,

    /// <summary>
    /// System error
    /// </summary>
    /// <remarks>PAM_SYSTEM_ERR</remarks>
    SystemError = 4,

    /// <summary>
    /// Memory buffer error
    /// </summary>
    /// <remarks>PAM_BUF_ERR</remarks>
    MemoryBufferError = 5,

    /// <summary>
    /// Permission denied
    /// </summary>
    /// <remarks>PAM_PERM_DENIED</remarks>
    PermissionDenied = 6,

    /// <summary>
    /// Authentication failure
    /// </summary>
    /// <remarks>PAM_AUTH_ERR</remarks>
    AuthenticationError = 7,

    /// <summary>
    /// Can not access authentication data
    /// </summary>
    /// <remarks>PAM_CRED_INSUFFICIENT</remarks>
    CredentialInsufficient = 8,

    /// <summary>
    /// Underlying authentication service can not retrieve authentication information
    /// </summary>
    /// <remarks>PAM_AUTHINFO_UNAVAIL</remarks>
    AuthenticationInformationUnavailable = 9,

    /// <summary>
    /// User not known to the underlying authentication module
    /// </summary>
    /// <remarks>PAM_USER_UNKNOWN</remarks>
    UnknownUser = 10,

    /// <summary>
    /// An authentication service has maintained a retry count which has been reached;
    /// no further retries should be attempted
    /// </summary>
    /// <remarks>PAM_MAXTRIES</remarks>
    MaximumRetriesReached = 11,

    /// <summary>
    /// New authentication token required; this is normally returned if the machine
    /// security policies require that the password should be changed because the
    /// password is NULL or it has aged
    /// </summary>
    /// <remarks>PAM_NEW_AUTHTOK_REQD</remarks>
    NewAuthenticationTokenRequired = 12,

    /// <summary>
    /// User account has expired
    /// </summary>
    /// <remarks>PAM_ACCT_EXPIRED</remarks>
    UserAccountExpired = 13,

    /// <summary>
    /// Can not make/remove an entry for the specified session
    /// </summary>
    /// <remarks>PAM_SESSION_ERR</remarks>
    SessionError = 14,

    /// <summary>
    /// Underlying authentication service can not retrieve user credentials
    /// </summary>
    /// <remarks>PAM_CRED_UNAVAIL</remarks>
    CredentialsUnavailable = 15,

    /// <summary>
    /// User credentials expired
    /// </summary>
    /// <remarks>PAM_CRED_EXPIRED</remarks>
    CredentialsExpired = 16,

    /// <summary>
    /// Failure setting user credentials
    /// </summary>
    /// <remarks>PAM_CRED_ERR</remarks>
    CredentialError = 17,

    /// <summary>
    /// No module specific data is present
    /// </summary>
    /// <remarks>PAM_NO_MODULE_DATA</remarks>
    NoModuleData = 18,

    /// <summary>
    /// Conversation error
    /// </summary>
    /// <remarks>PAM_CONV_ERR</remarks>
    ConversationError = 19,

    /// <summary>
    /// Authentication token manipulation error
    /// </summary>
    /// <remarks>PAM_AUTHTOK_ERR</remarks>
    AuthenticationTokenError = 20,

    /// <summary>
    /// Authentication information cannot be recovered
    /// </summary>
    /// <remarks>PAM_AUTHTOK_RECOVERY_ERR</remarks>
    AuthenticationTokenRecoveryError = 21,

    /// <summary>
    /// Authentication token lock busy
    /// </summary>
    /// <remarks>PAM_AUTHTOK_LOCK_BUSY</remarks>
    AuthenticationTokenLockBusy = 22,

    /// <summary>
    /// Authentication token aging disabled
    /// </summary>
    /// <remarks>PAM_AUTHTOK_DISABLE_AGING</remarks>
    AuthenticationTokenAgingDisabled = 23,

    /// <summary>
    /// Preliminary check by password service
    /// </summary>
    /// <remarks>PAM_TRY_AGAIN</remarks>
    TryAgain = 24,

    /// <summary>
    /// Ignore underlying account module regardless of whether the control flag is required, optional, or sufficient
    /// </summary>
    /// <remarks>PAM_IGNORE</remarks>
    Ignore = 25,

    /// <summary>
    /// Critical error (module fail now request)
    /// </summary>
    /// <remarks>PAM_ABORT</remarks>
    Abort = 26,

    /// <summary>
    /// User's authentication token has expired
    /// </summary>
    /// <remarks>PAM_AUTHTOK_EXPIRED</remarks>
    AuthenticationTokenExpired = 27,

    /// <summary>
    /// Module is not known
    /// </summary>
    /// <remarks>PAM_MODULE_UNKNOWN</remarks>
    UnknownModule = 28,

    /// <summary>
    /// Bad item passed to pam_*_item()
    /// </summary>
    /// <remarks>PAM_BAD_ITEM</remarks>
    BadItem = 29,

    /// <summary>
    /// Conversation function is event driven and data is not available yet
    /// </summary>
    /// <remarks>PAM_CONV_AGAIN</remarks>
    ConverseAgain = 30,

    /// <summary>
    /// Please call this function again to complete authentication stack; before
    /// calling again, verify that conversation is completed
    /// </summary>
    /// <remarks>PAM_INCOMPLETE</remarks>
    Incomplete = 31,
}
