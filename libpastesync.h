#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define BUFFER_SIZE 256
#define MAX_CLIPBOARD_SIZE 1024 * 256
#define CHAR_SIZE sizeof(char)

int get_pipe_output(char*, char**);