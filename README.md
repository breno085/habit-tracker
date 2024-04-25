# Habit Logger

Console based CRUD application to track daily habits. Developed with C# and SQLite.

OBS: After doing the coding tracker app i learned that in this app i hard coded the conection to the database in the main class, and that is not a good practise, it is unsafe, the database conection is sensitive and this leave your code vulnerable. So you need to store the conection somewhere else. Also when we call the CreateCommand() method, we are creating an instance of the sql command object, and that's also an unmanaged resource, and we need to dispose of it. So you need to wrap it also in a using statement.
I left this first crud project without fixing these bad practises because i think it made easier to understand the coding tracker app, and to have an reference to what not to do in the future in case i'll have to deal with ADO.NET.

## Requirements

