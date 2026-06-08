# Game Platform API

A backend platform service built with ASP.NET Core.

This project is designed to simulate a game platform backend that provides player statistics, rankings, marketplace features, and suspicious activity detection.

The goal of this project is to gain hands-on experience with modern backend development practices, including RESTful APIs, authentication, database design, containerization, and cloud deployment.

---

## Features

### Authentication
- User registration
- User login
- JWT-based authentication

### Player Statistics
- Player profile
- Match history
- Kill/Death ratio
- Win/Loss statistics

### Ranking System
- Global rankings
- Seasonal rankings
- Ranking calculations

### Marketplace
- Item listing
- Item purchasing
- Transaction history

### Suspicious Activity Detection
- Detection of abnormal player statistics
- Rule-based player monitoring

---

## Tech Stack

### Backend
- ASP.NET Core Web API
- C#

### Database
- Microsoft SQL Server
- Entity Framework Core

### Authentication
- JWT Authentication

### DevOps
- Docker
- Docker Compose
- GitHub Actions (Planned)

### Cloud
- AWS EC2 (Planned)

---

## Architecture

```text
Client
    ↓
ASP.NET Core API
    ↓
Application Layer
    ↓
Domain Layer
    ↓
Infrastructure Layer
    ↓
SQL Server
```

---

## Project Structure

```text
src

├── GamePlatform.Api
├── GamePlatform.Application
├── GamePlatform.Domain
└── GamePlatform.Infrastructure
```

---

## Development Roadmap

### Phase 1
- [ ] Project setup
- [ ] Database design
- [ ] User authentication

### Phase 2
- [ ] Player statistics API
- [ ] Match history API
- [ ] Ranking API

### Phase 3
- [ ] Marketplace API
- [ ] Suspicious activity detection

### Phase 4
- [ ] Docker support
- [ ] AWS deployment
- [ ] CI/CD pipeline

---

## Learning Goals

- ASP.NET Core
- RESTful API Design
- Entity Framework Core
- JWT Authentication
- Docker
- AWS
- CI/CD
- Clean Architecture
