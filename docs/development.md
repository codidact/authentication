# Development Instructions

## Configuration

There are a few files that can be used to configure your IDE or to change the
configuration of the authentication server.

Defaults for these files are located in `/docs/defaults` and can be used by
copying them into the same directory but removing the prefix, for example:

~~~none
cp ./docs/defaults/src/WebApp/appsettings.json ./src/WebApp/appsettings.json
~~~

If you want to install all files automatically, you can use:

~~~none
rsync -a ./docs/defaults/ ./
~~~

On Windows you can do this in the file explorer, just select all files in
`/docs/defaults` and drag them into the root folder.

## Running the Tests

You can run the tests by running `dotnet test` in the root directory.

## Running the Server

You can run the authentication server using `dotnet run` in the `src/WebApp`
directory, or by clicking `Debug > Start Debugging` if you use Visual Studio Code
and used the default configuration.
