#include <errno.h>
#include <fcntl.h>
#include <signal.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <syslog.h>
#include <unistd.h>
#include "libpastesync.h"

static void handler(int);
void daemonize();
void listen_signals();
void run_loop();

void usage(char* argv[]) {
  fprintf(stderr, "Usage: %s [-d]\n\t-d\tlaunch as daemon\n", argv[0]);
}

int main(int argc, char *argv[]) {
  int opt;
  int daemon = 0;
  
  while ((opt = getopt(argc, argv, "d")) != -1) {
    switch (opt) {
      case 'd':
        daemon = 1;
        break;
      default:
        usage(argv);
        exit(EXIT_FAILURE);
    }
  }

  if (daemon == 1) {
    daemonize(argv[0]);
  }

  listen_signals();
  run_loop();

  return EXIT_SUCCESS;
}

void listen_signals() {
  struct sigaction sa, old_sa;

  sa.sa_handler = handler;
  sigemptyset(&sa.sa_mask);
  sa.sa_flags = 0;

  sigaction(SIGINT, NULL, &old_sa);
  if (old_sa.sa_handler != SIGINT)
    sigaction(SIGINT, &sa, NULL);
}

static void handler(int signum) {
  fprintf(stdout, "received signal: %i\n", signum);
  exit(EXIT_SUCCESS);
}

void daemonize(char *command) {
  printf("daemonize\n");

  pid_t pid, sid;

  pid = fork();

  if (pid < 0) {
    fprintf(stderr, "forking failed\n");
    exit(EXIT_FAILURE);
  }

  if (pid > 0) {
    exit(EXIT_SUCCESS);
  }

  umask(0);

  // openlog(command, LOG_NOWAIT | LOG_PID, LOG_USER);
  // syslog(LOG_NOTICE, "daemon started");

  sid = setsid();
  if (sid < 0) {
    exit(EXIT_FAILURE);
  }

  if (chdir("/") < 0) {
    exit(EXIT_FAILURE);
  }

  close(STDIN_FILENO);
  close(STDOUT_FILENO);
  close(STDERR_FILENO);
}

void run_loop() {
  int clipboard_changed;
  char *latest_clipboard = NULL;
  char *new_clipboard = NULL;

  while (1) {
    get_pipe_output("./pastesync -n", &new_clipboard);

    if ((latest_clipboard == NULL && new_clipboard != NULL) || strcmp(latest_clipboard, new_clipboard) != 0)
      clipboard_changed = 1;

    if (clipboard_changed) {
      printf("Clipboard data changed: %s\n", new_clipboard);
      latest_clipboard = new_clipboard;
    }

    clipboard_changed = 0;
    sleep(3);
  }
}
