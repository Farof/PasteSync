# pkg-config --cflags --libs x11

CC=gcc
CFLAGS=-W -Wall -pedantic
LDFLAGS=
EXEC=pastesync
SRC=$(wildcard *.c)
OBJ=$(SRC:.c=.o)

all: $(EXEC)

pastesyncd: $(OBJ)
	@$(CC) -o $@ $^ $(CFLAGS)

pastesync: $(OBJ)
	@$(CC) -o $@ $^ $(CFLAGS)

%.o: %.c
	@$(CC) -o $@ -c $< $(CFLAGS)

.PHONY: clean mrproper

clean:
	@rm -rf *.o

mrproper: clean
	@rm -rf $(EXEC)
