RaceDay API Endpoint Plan


1. Purpose
The RaceDay API will provide access to information about events, categories, routes, event categories, enrolments, results, and users. The API will allow the RaceDay system to create, retrieve, update, and manage data stored in the database.

 2. API Base URL
`/api`

 3. Event Endpoints

| Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/events | Retrieve all events | Organiser, Participant | None | 200 OK + array of events |
| GET | /api/events/{id} | Retrieve a specific event | Organiser, Participant | None | 200 OK + event object, or 404 Not Found |
| POST | /api/events | Create a new event | Organiser | `{ "name": "string", "date": "datetime", "location": "string" }` | 201 Created + created event object |
| PUT | /api/events/{id} | Update an existing event | Organiser | `{ "name": "string", "date": "datetime", "location": "string" }` | 200 OK + updated event, or 404 Not Found |
| DELETE | /api/events/{id} | Delete an event | Organiser | None | 204 No Content, or 404 Not Found |

 4. Category Endpoints

| Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/categories | Retrieve all categories | Organiser, Participant | None | 200 OK + array of categories |
| GET | /api/categories/{id} | Retrieve a specific category | Organiser, Participant | None | 200 OK + category object, or 404 Not Found |
| POST | /api/categories | Create a new category | Organiser | `{ "name": "string", "description": "string" }` | 201 Created + created category |
| PUT | /api/categories/{id} | Update a category | Organiser | `{ "name": "string", "description": "string" }` | 200 OK + updated category, or 404 Not Found |
| DELETE | /api/categories/{id} | Delete a category | Organiser | None | 204 No Content, or 404 Not Found |

5. Event Category Endpoints

| Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/eventcategories | Retrieve all event categories | Organiser, Participant | None | 200 OK + array of event categories |
| GET | /api/eventcategories/{id} | Retrieve a specific event category | Organiser, Participant | None | 200 OK + event category object, or 404 Not Found |
| POST | /api/eventcategories | Create a new event category | Organiser | `{ "eventId": "int", "categoryId": "int" }` | 201 Created + created event category |
| PUT | /api/eventcategories/{id} | Update an event category | Organiser | `{ "eventId": "int", "categoryId": "int" }` | 200 OK + updated event category, or 404 Not Found |
| DELETE | /api/eventcategories/{id} | Delete an event category | Organiser | None | 204 No Content, or 404 Not Found |

 6. Route Endpoints

| Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/routes | Retrieve all race routes | Organiser, Participant | None | 200 OK + array of routes |
| GET | /api/routes/{id} | Retrieve a specific route | Organiser, Participant | None | 200 OK + route object, or 404 Not Found |
| POST | /api/routes | Create a new route | Organiser | `{ "name": "string", "distanceKm": "decimal", "description": "string" }` | 201 Created + created route |
| PUT | /api/routes/{id} | Update a route | Organiser | `{ "name": "string", "distanceKm": "decimal", "description": "string" }` | 200 OK + updated route, or 404 Not Found |
| DELETE | /api/routes/{id} | Delete a route | Organiser | None | 204 No Content, or 404 Not Found |

7. Enrolment Endpoints

| Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/enrolments | Retrieve all enrolments | Organiser | None | 200 OK + array of enrolments |
| GET | /api/enrolments/{id} | Retrieve a specific enrolment | Organiser, Participant (own record) | None | 200 OK + enrolment object, or 404 Not Found |
| POST | /api/enrolments | Register a user for an event category | Participant | `{ "userId": "int", "eventCategoryId": "int" }` | 201 Created + enrolment object |
| PUT | /api/enrolments/{id} | Update an enrolment | Organiser, Participant (own record) | `{ "eventCategoryId": "int" }` | 200 OK + updated enrolment, or 404 Not Found |
| DELETE | /api/enrolments/{id} | Cancel an enrolment | Organiser, Participant (own record) | None | 204 No Content, or 404 Not Found |

 8. Result Endpoints

| Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/results | Retrieve all race results | Organiser, Participant | None | 200 OK + array of results |
| GET | /api/results/{id} | Retrieve a specific result | Organiser, Participant | None | 200 OK + result object, or 404 Not Found |
| POST | /api/results | Record a new race result | Organiser | `{ "enrolmentId": "int", "finishTime": "time", "position": "int" }` | 201 Created + created result |
| PUT | /api/results/{id} | Update a race result | Organiser | `{ "finishTime": "time", "position": "int" }` | 200 OK + updated result, or 404 Not Found |
| DELETE | /api/results/{id} | Delete a race result | Organiser | None | 204 No Content, or 404 Not Found |

 9. User Endpoints

| Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/users | Retrieve all users | Organiser | None | 200 OK + array of users |
| GET | /api/users/{id} | Retrieve a specific user | Organiser, Participant (own record) | None | 200 OK + user object, or 404 Not Found |
| POST | /api/users | Create a new user | Public (registration) | `{ "name": "string", "email": "string", "password": "string", "role": "Organiser/Participant" }` | 201 Created + created user (excluding password) |
| PUT | /api/users/{id} | Update user information | Organiser, Participant (own record) | `{ "name": "string", "email": "string" }` | 200 OK + updated user, or 404 Not Found |
| DELETE | /api/users/{id} | Delete a user | Organiser | None | 204 No Content, or 404 Not Found |


 11. Security

User information should be protected from unauthorised access. Endpoints that create, update, or delete information should require appropriate authentication and authorisation, based on the roles defined above. Sensitive user information (e.g. passwords) should not be returned by the API.

