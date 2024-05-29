# Habit Logger

Console based CRUD application to track daily habits. Developed with C# and SQLite.

**OBS:** After doing the coding tracker app i learned that in this app i hard coded the conection to the database in the main class, and that is not a good practise, it is unsafe, the database conection is sensitive and this leave your code vulnerable. So you need to store the conection somewhere else. Also when we call the CreateCommand() method, we are creating an instance of the sql command object, and that's also an unmanaged resource, and we need to dispose of it. So you need to wrap it also in a using statement.
**In version 1.0 i fixed these mistakes.**

## Requirements

