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

if sys.version_info < (3, 7, 3):
    sys.exit("error: This script requires Python 3.7.3 or later.")


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

# Workaround for 'shutil.copytree' until Python 3.8 becomes availiabe on most
# systems. Debian Stable is taken as an indicator for this:
# https://packages.debian.org/buster/python3
for srcdir, dirnames, filenames in os.walk(defaults_directory):
    dstdir = os.path.join(worktree_directory,
                          os.path.relpath(srcdir, defaults_directory))

    os.makedirs(dstdir, exist_ok=True)

    for filename in filenames:
        shutil.copy2(os.path.join(srcdir, filename),
                     os.path.join(dstdir, filename),
                     follow_symlinks=False)
