#include <stddef.h>
#include <stdio.h>
#include <string.h>
#include <sys/socket.h>
#include <sys/un.h>
#include <unistd.h>

#include "socket.h"

int start_server(const char* socket_path, int* server_fd, void (*handle)(int client_fd)) {
    if (socket_path == NULL || server_fd == NULL || handle == NULL) {
        return -1;
    }

    struct sockaddr_un addr = { .sun_family = AF_UNIX };
    socklen_t addr_len = sizeof(addr);

    if (strlen(socket_path) > sizeof(addr.sun_path)) {
        fprintf(stderr, "Error: Socket path too long\n");
        stop_server(*server_fd, socket_path);

        return -1;
    }

    strcpy(addr.sun_path, socket_path);

    *server_fd = socket(AF_UNIX, SOCK_STREAM, 0);
    if (*server_fd == -1) {
        perror("socket");
        return -1;
    }

    unlink(socket_path);
    if (bind(*server_fd, (struct sockaddr*)&addr, addr_len) == -1) {
        perror("bind");
        stop_server(*server_fd, socket_path);

        return -1;
    }

    if (listen(*server_fd, 10) == -1) {
        perror("listen");
        stop_server(*server_fd, socket_path);

        return -1;
    }

    while (1) {
        int client_fd = accept(*server_fd, (struct sockaddr*)&addr, &addr_len);
        if (client_fd == -1) {
            perror("accept");
            stop_server(*server_fd, socket_path);

            return -1;
        }

        handle(client_fd);
        close(client_fd);
    }

    stop_server(*server_fd, socket_path);
    return 0;
}

void stop_server(int server_fd, const char* socket_path) {
    if (server_fd >= 0) {
        close(server_fd);
    }

    if (socket_path != NULL) {
        unlink(socket_path);
    }
}
