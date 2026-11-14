#include <signal.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/_endian.h>
#include <sys/_types/_ssize_t.h>
#include <sys/socket.h>
#include <unistd.h>

#include "src/authenticate.h"
#include "src/groups.h"
#include "src/socket.h"

const char* pam_module = "login";
const char* socket_file = "/etc/miniauth/pam.socket";

int socket_fd;

void handle_signal(int signal) {
    stop_server(socket_fd, socket_file);
    exit(0);
}

void handle_connection(int client_fd) {
    char header[4];

    ssize_t header_bytes = read(client_fd, header, sizeof(header));
    if (header_bytes != 4) {
        return;
    }

    if (header[0] != 'M' || header[1] != 'A') {
        return;
    }

    uint16_t message_size;
    memcpy(&message_size, header + 2, sizeof(uint16_t));

    uint16_t content_size = ntohs(message_size);

    char* buffer = malloc(content_size);
    ssize_t message_read = read(client_fd, buffer, content_size);
    if (message_read < content_size) {
        goto finish;
    }

    const char* user = strtok(buffer, ":");
    if (user == NULL) {
        goto finish;
    }

    const char* password = strtok(NULL, ":");
    if (password == NULL) {
        goto finish;
    }

    int result = authenticate(pam_module, user, password);
    if (result == -1) {
        char response_header[4] = { 'A', 'F', 0, 0 };
        send(client_fd, response_header, sizeof(response_header), 0);
    } else {
        char response_header[2] = { 'A', 'S' };
        send(client_fd, response_header, sizeof(response_header), 0);

        char** groups = NULL;
        int ngroups;
        get_groups(user, groups, &ngroups);

        for (int i = 0; i < ngroups; ++i) {
            char* group = groups[i];
            const size_t length = strlen(group);

            if (length > UINT16_MAX) {
                free(group);
                continue;
            }

            const uint16_t network_length = htons((uint16_t)length);
            send(client_fd, &network_length, 2, 0);
            send(client_fd, group, length, 0);

            free(group);
        }

        free(groups);

        char end[2] = { 0, 0 };
        send(client_fd, end, sizeof(end), 0);
    }

    finish:
    free(buffer);
    return;
}

int main(int argc, char** argv) {
    signal(SIGINT, handle_signal);

    int result = start_server(socket_file, &socket_fd, handle_connection);
    if (result == -1) {
        exit(255);
    }

    return 0;
}
