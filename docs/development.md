# Development Instructions

## Configuration

There are a number of files that can be edited to configure your IDE or to
change the configuration of the authentication server.

Defaults for these files are located in `/defaults` and can be copied into the
worktree by running `python ./scripts/defaults.py`.

On Windows you can do this in the file explorer, just select all files in
`/docs/defaults` and drag them into the root folder.

## Running the Tests

You can run the tests by running `dotnet test` in the root directory.

## Running the Server

You can run the authentication server using `dotnet run` in the `src/WebApp`
directory, or by clicking `Debug > Start Debugging` if you use Visual Studio Code
and used the default configuration.
