RaceDay - ASP.NET Core MVC Web Application (Part 3)

 Project Overview
RaceDay is a comprehensive event management platform for sporting events. This repository contains the ASP.NET Core MVC web application (Part 3), which serves as the frontend user interface for both "Organisers" and "Participants". 

The application consumes the RESTful API developed in Part 2 and integrates Azure Blob Storage for cloud-based media management. The entire application is containerised using Docker and automated via GitHub Actions CI/CD workflows.



 Architecture & Key Features

- Decoupled Architecture:** The MVC frontend contains zero direct database access or replicated business logic. All data operations are securely delegated to the Part 2 API via `HttpClient`.
- Session-Based Authentication:** Manages role-based access control (Organiser vs. Participant). Unauthenticated users are strictly prevented from accessing protected routes.
- **Organiser Interface:
  - Event Management (Create, Edit, Delete events).
  - Image Uploads for Event Banners (stored via Azure Blob Storage).
  - Category Management (Age/Distance categories).
  - Participant Enrolment Tracking & Race Results Entry (Capture finish times and positions).
- Participant Interface:
  - Event Discovery with dynamic search and filtering options.
  - One-click Event Enrolment.
  - Personal Dashboard showing enrolled events and race status.
  - My Results page tracking past finish times and positions (e.g., 47th out of 312 finishers).
  - Profile Management with avatar/picture upload.
- Azure Blob Storage: Integrated seamlessly through the Part 2 API to securely store and render event images and profile photos.
- Responsive UI: Built with Bootstrap using a modern, energetic sporting visual theme with visual color-coding for status tracking (e.g., Green for Confirmed, Amber for Pending).
