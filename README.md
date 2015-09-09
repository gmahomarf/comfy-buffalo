Unicron
==========

A SPA starter using Angular, Nancy and NHibernate.


- CQRS-ready with synchronous command dispatcher.
- Domain Events already installed with BlingBag
- SpecFlow and Selenium set up and ready for Features and Acceptance Criteria (scenarios)
- Database deployer re-builds database from domain entities

Active demo: [http://unicron-starter.azurewebsites.net/](http://unicron-starter.azurewebsites.net/)

## Requirements

- Install Node
    - on Windows install [chocolatey](https://chocolatey.org/)
    - Read here for some [tips on Windows](http://jpapa.me/winnode)
    - open command prompt as administrator
        - type `choco install nodejs`
        - type `choco install nodejs.install`
- Install Python 2.7
    `choco install python -version 2.7.2`
- Open terminal
- Type `npm install -g node-inspector bower gulp typescript tsd phantomjs tslint karma karma-cli`

## Quick Start
Prior to start, clone this repo and navigate to `src/Unicron.Web/Client` and run the following commands:

```bash
$ npm install
```

![Unicron](https://static.squarespace.com/static/51b3dc8ee4b051b96ceb10de/51ce6099e4b0d911b4489b79/51ce618ee4b0d911b44980fe/1355765265947/1000w/unicron121212145556.jpeg)
