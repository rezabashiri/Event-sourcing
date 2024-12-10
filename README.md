<div id="top"></div>

<div align="center">


  <h3 align="center">Modular and event sourcing bank project</h3>

  <p align="center">
In transactional systems like banks the most reliable approach to implement the 
the software is event sourcing.

  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->

## About The Project

This is a simple modular event-sourcing solution :

- Modular to increase readability, testability and scaling up without any side
  effect to existing use-cases
- Event sourcing design patter to model a transactional environment and maintain
  safety by log all events
- CQRS gifted us the flexibility of optimizing commands and queries

By adopting this architecture, we will:

- Attain a coherent and multi-modular code structure.
- Embrace an Onion architecture within each module.
- Enjoy support for multiple database types.
- Practice the SOLID principles in a programmatic context.
- Conduct both unit and integration tests.
- Consider the events as prof of concepts
- Log the history of system in all states
- Easy to make other use-cases in different team inside each module without any
  conflicts

There is another module in project (EmployeeManagement), just to indicate how
different modules can reside beside each other independently.

<p align="right">(<a href="#top">back to top</a>)</p>

### Built With

- [.net8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [AutoMapper](https://github.com/AutoMapper/AutoMapper)
- [MediatR](https://github.com/jbogard/MediatR)

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- GETTING STARTED -->

## Getting Started

This is an example of how you may give instructions on setting up your project
locally. To get a local copy up and running follow these simple example steps.

### Prerequisites

This is an example of how to list things you need to use the software and how to
install them.

- dotnet
- or docker

### Installation

_Below is how you can run your project via docker compose._

Navigate to src/server

```sh
docker compose up
```

or by dotnet Navigate to src/server

```sh
dotnet build
```

Run, but you need to adjust connection strings in advance

```sh
   dotnet run --project API/Bootstrapper.csproj
```

[Head to http://localhost:5001/swagger/index.html](http://localhost:5001/swagger/index.html)

**Or use visual studio code, visual studio or rider to up and run project**

<p align="right">(<a href="#top">back to top</a>)</p>

To be able to work with system, It's supposed, it could have multiple users and
each user can have 0 or more accounts.

So add at least one user, from user URIs, then use account URIs add any amount
of accounts for users as you wished. Each deposit or withdraw are recognize as
proof of concepts and are being logged in database under
"Application"."EventLogs" table.

Deposit till 10000 in each transaction is allowed and each account can not have
less than 100 at a once, whiles more than 90% of balance can not withdraw in a
single transaction.

<!-- CONTACT -->

## Contact

The modular concept is built on top of my open source modular project that you
can find it my github profile.

Reza Bashiri - [https://rezabashiri.com/](https://rezabashiri.com/) -
rzbashiri@gmail.com - [Linkedin](https://www.linkedin.com/in/reza-bashiri/)

<p align="right">(<a href="#top">back to top</a>)</p>
