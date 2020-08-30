# Rover Control System
 
This project was made by NASA for the control of Rovers sent to Mars.

# User guide

  - Create Grid
  - Enter Rover's position
  - Enter Navigation command
  - Show results 
  
# Directions

![GitHub Logo](https://www.geographyrealm.com/wp-content/uploads/2014/07/cardinal-points.png)

The directions in the picture are represented in the code below.
```sh
 Direction[] directions = new Direction[]
{
    Direction.North,Direction.West,Direction.South,Direction.East
};
```
In the definition in the code, it is handled in the counterclockwise direction.
 
# Example 

Input Values
Grid : 10 10
for Rover-1 position :  5 3 N , 
moves : RMLMR

Output Value(s)
result for Rover-1 : 6 4 E  

### Todos

 - Write Tests
 - Define intermediate directions (northwest etc.)

 
