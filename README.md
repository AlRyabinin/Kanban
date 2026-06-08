# KanbanFlow

A modern Kanban board application for task management, built with .NET 8 and React.

## 📸 Screenshots

![Main Page](./docs/MainPage.jpg)
*Главная страница с досками*

![Board View](./docs/Board.jpg)
*Вид доски с задачами*

## 🚀 Quick Start

### With Docker

git clone https://github.com/AlRyabinin/KanbanFlow.git

cd KanbanFlow

```bash```docker-compose up -d --build

##  Features

- 📋 Create and manage multiple boards
- 📊 Drag-and-drop tasks between columns
- 🎨 Customizable column colors
- ✏️ Full CRUD operations for boards, columns, and tasks
- 🔄 Real-time column and task reordering
- 🐳 Docker support for easy deployment

## 🛠️ Tech Stack

### Backend
- **.NET 8** with ASP.NET Core
- **Entity Framework Core** with SQLite
- **MediatR** for CQRS pattern
- **Clean Architecture** (Domain, Application, Infrastructure, API)

### Frontend
- **React 18** with TypeScript
- **TanStack Query** for server state management
- **dnd-kit** for drag-and-drop
- **Tailwind CSS** for styling
- **Lucide React** for icons

### Infrastructure
- **Docker** & **Docker Compose**
- **Nginx** as reverse proxy

## 🏗️ Architecture

The project follows **Clean Architecture** principles
