<div align="center" style="margin-bottom: 22px;">
  <img src="https://img.shields.io/badge/C%23-7.3-blue?style=for-the-badge&logo=csharp&logoColor=white" alt="C# 7.3"/>
  <img src="https://img.shields.io/badge/MySQL-Darkblue?style=for-the-badge&logo=mysql&logoColor=white" alt="MySQL"/>
  <img src="https://img.shields.io/badge/SSMS-brightgreen?style=for-the-badge&logo=sqlserver&logoColor=white" alt="SSMS"/>
</div>

<div align="center">
  <span style="font-size: 36px; font-weight: bold;">School</span><br>
  <sup style="font-size: 18px;">Electronic school diary</sup>
</div>

---

> <h4>Educational project for MySQL and database practice. Nothing commercial.</h4>
> README also isn't really needed, but writing it is fun!

---

<div align="center">
  <span style="font-size: 16px; font-weight: light;">Student, Teacher, Director</span><br>
  
  Pet project for C#, MySQL, MVC architecture and school management practice.
  Three user roles with different access levels (5%/50%/100%).
</div>

<p align="center">
  <a href="https://docs.microsoft.com/en-us/dotnet/csharp/">
    <img alt="C# 7.3" src="https://img.shields.io/badge/C%237.3-blue?style=social&logo=csharp&logoColor=white">
  </a>
  <a href="https://dev.mysql.com/doc/">
    <img alt="MySQL" src="https://img.shields.io/badge/MySQL-Darkblue?style=social&logo=mysql&logoColor=white">
  </a>
  <a href="https://www.nunit.org/">
    <img alt="NUnit" src="https://img.shields.io/badge/NUnit-green?style=social&logo=testing&logoColor=white">
  </a>
</p>

---

# Features

### Overview

| **Grades**    | **Homework**  | **Directories**  | **Reports**    |
|---------------|---------------|------------------|----------------|
| View/Edit     | Tasks/Deadlines| Students/Staff  | Print tables   |
| **Schedule**  | **Roles**             | **Architecture** | **Statistics** |
| View/Edit     | Student/Teacher/Director | MVC + Tests    | Microsoft Sql |
| **Schedule**  |   **Roles**   | **Architecture** | Statistics |

> **Access rights:** Student ~5%, Teacher ~50%, Director 100%.

### Details

- **Grades management**
  - View/edit (role-based)
  - Performance statistics
- **Homework**
  - Create/view assignments
  - Deadlines
- **Databases**
  - Staff directory
  - Students directory
- **Reporting**
  - Print tables
  - Data export
- **Schedule**
  - View/edit lessons
- **Role system**
  - LoginForm with authentication
  - Access rights control
  - One Form1 with tabs (no windows)

---

## Getting Started

### Visual Studio

1. Open project in Visual Studio
2. Install via NuGet:
3. Configure MySQL connection string in `appsettings`
4. Run `LoginForm`

### SSMS

1. Create DB using scripts in `/Database/`
2. Run migrations (if any)
3. Check table relationships

---

## Built With

| Framework/Library              | Description                  |
|--------------------------------|------------------------------|
| **C# 7.3**                     | Main language               |
| **Windows Forms**              | UI (2 forms, tabs)          |
| **MySQL**                      | Database                    |
| **Microsoft.SqlClient**        | MySQL driver                |
| **NUnit**                      | Unit tests for controllers  |
| **SSMS**                       | Database management         |

---

## System Requirements

| OS            | **CPU**        | **RAM** |
|---------------|----------------|---------|
| **Windows 10**| Serious?       | 512Mb+  |
| Windows 11    | Serious?       | 512Mb+  |

**DB:** MySQL 8.0+

---

## Architecture Highlights
MVC Pattern:
- Models (DB tables)
- Controllers (SQL operations + NUnit tests)
- Views (LoginForm → Form1 with tabs)

Features:

- No windows, minimal buttons
- Everything on tabs and nesting
- 3 roles: Student(5%) → Teacher(50%) → Director(100%)

---

## Contributing

**Solo project**<br>
Contributions welcome! Open issues or PRs.

---

## License

**Free for open-source use with restrictions:**

- **Free modification**
- **Free distribution** of source code
- **No selling** code or software
- **Changes require** author approval
- **Open-source use only**

> Commercial use or modifications without permission — contact author.

<div align="center">
  <span style="font-size: 14px; color: #666;">
    For learning MySQL + C#<br>
    © 2025 Solo Developer
  </span>
</div>

>
