# Codidact.Authentication

The purpose of this repository is to create an authentication server that will
be used by the [codidact core project][1].

All personally identifiable information (PII) will be managed by this server in
a different database that is completely seperated from everything that core does.
This information can then be retrieved by the core application on demand.

## Build Requirements

### ASP.NET Core 3.1

On Windows you can install the '.NET Core SDK' from [here][2], **do not** install the
'.NET Framework' that is something different.

On Linux you should look if your package manager of choice has a package for you. On
Arch Linux you have to install both the `dotnet-sdk` and `aspnet-runtime` packages.

Otherwise, you can download and install the SDK manually from [here][3]. You probably
want the 'x64' version under 'Binaries' but this depends on your hardware. Make sure
to set the `DOTNET_ROOT` variable and to add the `.dotnet/tools` directory to `PATH`,
refer to the install instructions that appear after the download for more details.

### Visual Studio Code (Recommended for Linux)

You can download Visual Studio Code from [here][4].

*Note. Packages from your package manager likely will not work. It seems that the C#
debugger is not open source and not included therefor. This information needs to be
confirmed though.*

*Note. More detailed instruction will be added in the future, for now you can take this
information from [codidact/core#31][8]. Look at the [`linux-instructions.md`][6] file.*

### Visual Studio (Recommended for Windows)

You can download Visual Studio from [here][5], the 'Community' edition is sufficent.

*Note. More detailed instructions will be added in the future, for now you can take this
information form [codidact/core#31][8]. Look at the [`windows-instructions.md`][7] file.*

### Nginx

You can change the configuration to work without a web-server but it is recommended to use
one. By default, the authentication listens on port 8080 and core on 5000. The following
configuration can be used:

~~~none
error_log /var/log/nginx/error.log warn;

events {
}

http {
    access_log /var/log/nginx/access.log;

    ssl_certificate     certs/localhost.crt.pem;
    ssl_certificate_key certs/localhost.key.pem;

    # Redirect HTTP requests to HTTPS.
    server {
        listen 80;

        return 301 https://$host$request_uri;
    }

    # Terminate encryption and pass requests to 'authentication'.
    server {
        listen 443 ssl;
        server_name sso.localhost;

        location / {
            proxy_pass http://127.0.0.1:8080;
        }
    }

    # Terminate encryption and pass requests to 'core'.
    server {
        listen 443 ssl;
        server_name localhost;

        location / {
            proxy_pass http://127.0.0.1:5000;
        }
    }
}
~~~

*Note. For production, this configuration would have to be changed to use `ssl_preload`
to pass requests to 'authentication' without terminating encryption.*

The certificates can be generated with:

~~~none
mkdir certs
openssl req -x509 -newkey rsa -keyout certs/localhost.key.pem -out certs/localhost.crt.pem -nodes -subj '/CN=localhost/'
~~~

*Note. This will create a self signed certificate, if you visit https://localhost, you will get a warning.*

  [1]: https://github.com/codidact/core
  [2]: https://dotnet.microsoft.com/download
  [3]: https://dotnet.microsoft.com/download/dotnet-core/3.1
  [4]: https://code.visualstudio.com/
  [5]: https://visualstudio.microsoft.com/downloads/
  [6]: https://github.com/codidact/core/blob/4b262d0076ac7018c730eeb0eb6394d367e3ce6e/docs/linux-instructions.md
  [7]: https://github.com/codidact/core/blob/4b262d0076ac7018c730eeb0eb6394d367e3ce6e/docs/windows-instructions.md
  [8]: https://github.com/codidact/core/pull/31
