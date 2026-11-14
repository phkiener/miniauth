pam-proxy: obj/main.o obj/authenticate.o obj/socket.o obj/groups.o | bin-dir
	gcc -Wall -lpam -o bin/pam-proxy obj/main.o obj/authenticate.o obj/socket.o obj/groups.o

obj/main.o: main.c src/authenticate.h | obj-dir
	gcc -Wall -c -o obj/main.o main.c

obj/authenticate.o: src/authenticate.c src/authenticate.h | obj-dir
	gcc -Wall -c -o obj/authenticate.o src/authenticate.c

obj/socket.o: src/socket.c src/socket.h | obj-dir
	gcc -Wall -c -o obj/socket.o src/socket.c

obj/groups.o: src/groups.c src/groups.h | obj-dir
	gcc -Wall -c -o obj/groups.o src/groups.c

bin-dir:
	mkdir -p bin/

obj-dir:
	mkdir -p obj/

clean:
	rm -rf obj/
	rm -rf bin/
