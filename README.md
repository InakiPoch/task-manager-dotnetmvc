# TASK MANAGER WEBSITE

Simple task manager website done with ASP .NET 8.0, building a new MVC Project.

* Final Project of C#'s university course.

* DataBase done locally with SQLite 3.


## Project Considerations

* A user can only create boards for themselves, regardless of their access level.

* A user can update all fields of their own tasks, but only the status of tasks assigned to them that do not belong to them

* An administrator has read, edit, and delete permissions for Users, Boards, and Tasks. For the tasks assigned to them, the administrator can only modify the status.

## Initialization

Need to have SQLiteStudio installed to import database within the repo.

Website uses a login page. There are two types of users:

### Administrator

* Username: usuario1

* Password: admin1

### Casual

* Username: usuario4

* Password: op2
