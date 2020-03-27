#!/usr/bin/env python3

"""
This script is meant to be run after checkout. The idea is that the default
configuration is checked into the repository but it is possible to make local
configuration changes without making the worktree dirty.

If local changes were made, running this script will override them with the
defaults.
"""

import sys
import os
import shutil


def show_usage():
    print("""\
Usage: defaults.py [OPTION]...

Copy the default configuration files into the worktree. Existing files will be
overwritten.

      --help        display this help and exit
      --version     output version information and exit

""", end="")


def show_version():
    # Refer to gitattributes(5).
    fileid = "$Id$"[5:-2]

    print(f"""\
defaults.py {fileid}

""", end="")


for arg in sys.argv[1:]:
    if arg == "--help":
        show_usage()
        sys.exit()

    if arg == "--version":
        show_version()
        sys.exit()

    sys.exit(f"error: invalid argument '{arg}'")

worktree_directory = os.path.dirname(os.path.dirname(__file__))
defaults_directory = os.path.join(worktree_directory, "defaults")

shutil.copytree(defaults_directory, worktree_directory,
                dirs_exist_ok=True, symlinks=True)
