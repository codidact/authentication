# Codidact.Authentication

The purpose of this repository is to create an authentication server that will
be used by the [codidact/core][1] repository.

All personally identifiable information (PII) will be managed by this server in
a different database that is completely seperated from everything that Core does.
This information can then be retrieved by the core application on demand.

Instructions for setting up a development environment can be found in
[docs/development.md][1].

  [1]: docs/development.md
