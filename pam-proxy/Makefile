CC=gcc
CFLAGS=-Wall
LDFLAGS=-lpam -lc

pam-proxy: main.o socket.o | out-dirs
	$(CC) $(CLFAGS) $(LDFLAGS) -o bin/pam-proxy obj/main.o obj/socket.o

main.o: main.c src/socket.h | out-dirs
	$(CC) $(CFLAGS) main.c -c -o obj/main.o

socket.o: src/socket.c src/socket.h | out-dirs
	$(CC) $(CFLAGS) src/socket.c -c -o obj/socket.o

run: pam-proxy
	./bin/pam-proxy

out-dirs:
	mkdir -p obj/
	mkdir -p bin/

clean:
	rm -rf obj/
	rm -rf bin/
