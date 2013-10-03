#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#define BUFFER_SIZE 256
#define MAX_CLIPBOARD_SIZE 1024 * 256
#define INPUT_PRE "echo -n \""
#define INPUT_POST "\" | xclip -selection clipboard"
#define CHAR_SIZE sizeof(char)

void println();
int output_clipboard();
int set_clipboard(char*);

void usage(char* argv[]) {
  fprintf(stderr, "Usage:\n\t%s [-o]\t// output clipboard content\n\t%s -i <data>\t// set clipboard content\n", argv[0], argv[0]);
}

int main(int argc, char* argv[]) {
  enum {
    INPUT_MODE,
    OUPUT_MODE
  } mode = OUPUT_MODE;
  int EXIT_CODE = EXIT_SUCCESS;
  int opt;
  char* input;

  while ((opt = getopt(argc, argv, "i:")) != -1) {
    switch (opt) {
      case 'i':
        mode = INPUT_MODE;
        input = optarg;
        break;
      default:
        usage(argv);
        exit(EXIT_FAILURE);
    }
  }

  if (mode == INPUT_MODE) {
    EXIT_CODE = set_clipboard(input);
  } else if (mode == OUPUT_MODE) {
    EXIT_CODE = output_clipboard();
  }

  exit(EXIT_CODE);
}

void println() {
  printf("\n");
}

int output_clipboard() {
  char *data = NULL;
  char *new_data;
  char buffer[BUFFER_SIZE];
  int new_size, i = 0;
  int data_remaining = 0;
  FILE *fd = popen("xclip -o -selection clipboard", "r");

  while (fgets(buffer, BUFFER_SIZE, fd) != 0) {
    i++;
    new_size = BUFFER_SIZE * i;

    if (new_size > MAX_CLIPBOARD_SIZE) {
      new_size = MAX_CLIPBOARD_SIZE;
      data_remaining = 1;
    }
    
    if (data == NULL) {
      new_data = malloc(new_size);
    } else {
      new_data = realloc(data, new_size);
    }

    if (new_data == NULL) {
      printf("abort, abort!\n");
      return EXIT_FAILURE;
    } else {
      data = new_data;
      sprintf(data, "%s%s", data, buffer);
    }

    if (data_remaining == 1) break;
  }
  pclose(fd);

  fprintf(stdout, "%s\n", data);
  /*printf("flushed: %s\n", data_remaining ? "FALSE" : "TRUE");*/

  free(data);

  return EXIT_SUCCESS;
}

int set_clipboard(char* input) {
  int len = strlen(INPUT_PRE) + strlen(input) + strlen(INPUT_PRE);
  char *command = malloc(len * CHAR_SIZE);

  sprintf(command, "%s%s%s", INPUT_PRE, input, INPUT_POST);
  /*printf("%s\n", command);*/
  system(command);

  return EXIT_SUCCESS;
}
