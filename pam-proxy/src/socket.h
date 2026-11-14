#pragma once

/*
 * Open a UNIX domain socket at the given path and start listening.
 *
 * @see stop_server
 * @param socket_path path to the file where the socket should be
 * @param server_fd pointer to the location where the socket descriptor should be stored
 * @param handle function to invoke on an incoming connection
 * @returns -1 on error or 0 on success.
 */
int start_server(const char* socket_path, int* server_fd, void (*handle)(int client_fd));

/*
 * Closes an opened UNIX domain socket.
 *
 * @see start_server
 * @param server_fd the socket file descriptor to close
 * @param socket_path the socket path to unlink or NULL
 */
void stop_server(int server_fd, const char* socket_path);
