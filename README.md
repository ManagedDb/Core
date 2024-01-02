# ManagedDb.Core

## Description

ManagedDb is a free and open-source project written in C# that automates the flow when a team uses a repository as a database. 

Managed DB is a service that provides a database engine for teams who use Git as a data source. For example, a team may store CSV files in a Git repo and use Git to track and audit their data. However, this method has some drawbacks:

* There is no automatic validation of the data types and consistency when adding or updating data.
* There is no easy way to share the data with others. They would have to share the repo and file link or build an application, among other options.
* There is no simple way to get the "delta" data based on the latest changes. They would have to parse the Git data.

Managed DB solves these issues by writing applications and integrating them into release pipelines. Whenever a user changes their data, our pipelines will detect it, process the data, create the appropriate database, and also verify the data consistency.

## Getting started

### Dependencies

* **Language**: The project is written in C#.
* **Framework**: .NET 7.
* **CI/CD**: All activities will be executed on GitHub Actions.


### Installation

Comming soon...

## Contributing

We welcome contributions to ManagedDb. If you are interested in contributing, please read our contributing guide to learn how to get started.

You can also check out our open issues and pull requests to see what we are working on and how you can help.

We appreciate your feedback and support. Thank you for being part of the ManagedDb community!


I hope this helps! Let me know if you have any other questions or if there’s anything else I can do for you.

## License

Coming soon...

## Contact

Coming soon...