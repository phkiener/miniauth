#include <stdio.h>
#include <stdlib.h>
#include "src/socket.h"

const char* fallback_socket_dir = "/etc/miniauth";

int main(int argc, char** argv) {
    const char* socket_dir = getenv("MINIAUTH_DIR");
    if (socket_dir == NULL) {
        socket_dir = fallback_socket_dir;
    }

    const int socket = open_miniauth_socket();

    printf("opening socket in %s, id is %i\n", socket_dir, socket);
    return 0;
}
