# Habit Logger

Console based CRUD application to track daily habits. Developed with C# and SQLite.

All the requirements and challenges were done.

**OBS:** After doing the coding tracker app i learned that in this app i hard coded the conection to the database in the main class, and that is not a good practise, it is unsafe, the database conection is sensitive and this leave your code vulnerable. So you need to store the conection somewhere else. Also when we call the CreateCommand() method, we are creating an instance of the sql command object, and that's also an unmanaged resource, and we need to dispose of it. So you need to wrap it also in a using statement.
**In version 1.0 i fixed these mistakes.**

## Requirements

This is an application where you’ll register one habit.
 - This habit can't be tracked by time (ex. hours of sleep), only by quantity - (ex. number of water glasses a day)
 - The application should store and retrieve data from a real database
 - When the application starts, it should create a sqlite database, if one isn’t present.
 - It should also create a table in the database, where the habit will be logged.
 - The app should show the user a menu of options.
 - The users should be able to insert, delete, update and view their logged habit.
 - You should handle all possible errors so that the application never crashes.
 - The application should only be terminated when the user inserts 0.
 - You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
 - Your project needs to contain a Read Me file where you'll explain how your app works.

 ## What will you learn?

  - Test your SQL commands on DB Browser before using them in your program.
  - You can keep all of the code in one single class if you wish.
  - Use a switch statement for the user input menus.
  - Don't forget the user input's validation: Check for incorrect dates. What happens if a menu option is chosen that's not available? What happens if the users input a string instead of a number?

 ## Challenges

 - Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
 - Seed Data into the database automatically when the database gets created  for the first time, generating a few habits and inserting a hundred records  with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
 - Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.