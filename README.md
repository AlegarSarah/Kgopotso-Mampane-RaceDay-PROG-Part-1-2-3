RaceDay API

Project Overview

RaceDay is an API-based system designed to manage race events, participants, categories, enrolments, routes, and race results.

The project uses a relational database and a REST API structure to support the management of race-day information.

Main Features

- Manage race events
- Manage participants and users
- Manage race categories
- Manage event categories
- Manage participant enrolments
- Manage race routes
- Record and manage race results

Database

The RaceDay database contains the following main tables:

1. Category
2. Enrolment
3. Event
4. EventCategory
5. Result
6. Route
7. User

Database documentation is available in the "/docs" folder.

API Documentation

The planned API endpoints are documented in:

"/docs/API.md"

The endpoint plan describes the main operations required by the RaceDay system.

Entity Relationship Diagram

The database structure is documented using an Entity Relationship Diagram (ERD).

The ERD is available in:

"/docs/ERD.md"

Database Script

The SQL database script is available in:

"/docs/Database.sql"

It contains the database tables, relationships, and sample data required for the project.

Continuous Integration

GitHub Actions is used to automatically build the project.

The build workflow verifies that the project can be successfully compiled whenever changes are pushed to the repository.

Build Status

The GitHub Actions build workflow has been successfully configured and tested.

Build status: PASSED

Project Documentation

Project documentation is organised in the "/docs" folder:

- "Database.sql" — database creation script
- "ERD.md" — Entity Relationship Diagram documentation
- "API.md" — API endpoint plan

Technologies

- C#
- .NET
- SQL Server
- REST API
- Git
- GitHub
- GitHub Actions