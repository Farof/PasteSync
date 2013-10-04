#include "libpastesync.h"

int get_pipe_output(char *command, char **out) {
  char *data = NULL;
  char *new_data;
  char buffer[BUFFER_SIZE];
  int new_size, i = 0;
  int data_remaining = 0;
  FILE *fd;

  fd = popen(command, "r");

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
      fprintf(stderr, "abort, abort!\n");
      return EXIT_FAILURE;
    } else {
      data = new_data;
      sprintf(data, "%s%s", data, buffer);
    }

    if (data_remaining == 1) break;
  }

  pclose(fd);

  *out = malloc(new_size);
  strcpy(*out, data);
  free(data);
  /*fprintf(stdout, "%s\n", data);*/
  /*printf("flushed: %s\n", data_remaining ? "FALSE" : "TRUE");*/

  return EXIT_SUCCESS;
}