#include <stddef.h>
#include <security/pam_appl.h>
#include <security/pam_constants.h>
#include <security/pam_types.h>

#include "authenticate.h"

int authenticate(const char* service, const char* user, const char* password) {
    if (service == NULL || user == NULL || password == NULL) {
        return -1;
    }

    struct pam_conv conv = { };
    pam_handle_t* handle;

    int last_status = pam_start(service, NULL, &conv, &handle);

    if (last_status == PAM_SUCCESS) {
        last_status = pam_set_item(handle, PAM_USER, user);
    }

    if (last_status == PAM_SUCCESS) {
        last_status = pam_set_item(handle, PAM_AUTHTOK, password);
    }

    if (last_status == PAM_SUCCESS) {
        last_status = pam_authenticate(handle, PAM_SILENT);
    }

    if (handle != NULL) {
        pam_end(handle, last_status);
    }

    return last_status == PAM_SUCCESS ? 0 : -1;
}
