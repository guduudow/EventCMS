# PassionProject

This project is a CMS (Content Management System) created with ASP.NET. The project is an event management system where users and events can be created, read, updated, and deleted. The project involves a many-to-many relationship, as each user can sign up for multiple events and an event can have multiple users. It utilizes Microsoft's SQL Server Database to create local databases.

For this project specifically, there are three tables. The first one is a user table (called attendee). It lists all the potential signees in the system. The columns it contains are First Name, Last Name, Email, and Phone Number. The second table is the events table (called Receptions), it has all the events in the system as well. The columns it contains are The Name, Location, Date, Description, Start Time, End Time, and Price of the event. The last table is a bridging table which links the two aforementioned tables together by their primary ID.

Each of the tables has API controllers that contact the SQL database to load the queries performed (CRUD). Views have also been made showcasing the data on an actual web page. In order to bring the many-to-many relationships to the web page, the project utilizes ViewModels to show all connected data to a specific data in the table the user is looking at. For example, if one were to be looking at a user, they would see all information tied to that user, such as the basic information such as first name, last name, email, and phone number, but they would also see all the events that the user has signed up for.

Each link is clickable, so clicking on one of the events mentioned will send you to the event page where you will again be able to see all the general information about the event. With ViewModels, you will see all the users that have signed up for it. From this screen, you can add users to the event and remove them. The additions will show up on the user's profile, while the removals will not.


## Features

- CRUD operations for users and events
- Many-to-many relationship between users and events
- Utilizes Microsoft SQL Server Database for local databases
- Three tables: 
  - **Attendee (User) Table**: Contains information about potential signees, including first name, last name, email, and phone number.
  - **Receptions (Events) Table**: Lists all events in the system, with details such as name, location, date, description, start time, end time, and price.
  - **Bridging Table**: Links the user and event tables together by their primary ID.
- API controllers for CRUD operations on the database
- Views to showcase data on web pages
- ViewModels to display connected data for a specific entity (user or event)

## How to Use

### Cloning the Repository

To clone the repository, run the following command:

```bash
git clone https://github.com/guduudow/HTTP5226.git
```
### Setting up the Project

1. Navigate to the project directory:
```bash
cd PassionProject
```
2. Ensure you have the necessary tools and frameworks to run ASP.NET project.
3. Apply migrations to create the database schema
4. Run the project

### Adding Migrations
To add migrations, use the following commands:
```bash
migrations add [name_of_migration]
database update
```

## Copyright
&copy; 2024 Ederes Gure. All rights reserved.
