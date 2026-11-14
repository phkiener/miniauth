#pragma once

/*
 * Authenticates a user with PAM, using the given service.
 *
 * @param service PAM module to use
 * @param user the user to authenticate
 * @param password the password to check
 * @return 0 on success, -1 on error
 */
int authenticate(const char* service, const char* user, const char* password);
