#include <unistd.h>
#include "libpastesync.h"

#define INPUT_PRE "echo -n \""
#define INPUT_POST "\" | xclip -selection clipboard"

void println();
int get_pipe_output(char*, char**);
int output_clipboard(int);
int set_clipboard(char*);

void usage(char* argv[]) {
  fprintf(stdout, "Usage:\t%s [-onh] [-i <data>]\n", argv[0]);
  fprintf(stdout, "\t-o\t\toutput clipboard content\n");
  fprintf(stdout, "\t-n\t\tno newline after output\n");
  fprintf(stdout, "\t-i <data>\tset clipboard content\n");
  fprintf(stdout, "\t-h\t\tdisplay this help\n");
}

int main(int argc, char *argv[]) {
  enum {
    INPUT_MODE,
    OUPUT_MODE
  } mode = OUPUT_MODE;
  int EXIT_CODE = EXIT_SUCCESS;
  int opt;
  int nflag = 0;
  char* input;

  while ((opt = getopt(argc, argv, "ni:")) != -1) {
    switch (opt) {
      case 'i':
        mode = INPUT_MODE;
        input = optarg;
        break;
      case 'n':
        nflag = 1;
        break;
      case 'h':
        usage(argv);
        exit(EXIT_SUCCESS);
        break;
      default:
        usage(argv);
        exit(EXIT_FAILURE);
    }
  }

  if (mode == INPUT_MODE) {
    EXIT_CODE = set_clipboard(input);
  } else if (mode == OUPUT_MODE) {
    EXIT_CODE = output_clipboard(nflag);
  }

  exit(EXIT_CODE);
}

void println() {
  printf("\n");
}

int output_clipboard(int nflag) {
  char *data = NULL;
  int ret;

  ret = get_pipe_output("xclip -o -selection clipboard", &data);

  if (ret == EXIT_FAILURE) {
    fprintf(stderr, "failed reading clipboard\n");
    return EXIT_FAILURE;
  }

  if (nflag) {
    fprintf(stdout, "%s", data);
  } else {
    fprintf(stdout, "%s\n", data);
  }

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
