Rubiks cube
============

C# Asp.net MVC4 test

1. a class that represents the cube - it stores a reference to a face which is the "first" front.
2. a class that represents each face.
3. each face has links to its neighbors and back.
4. when we rotate rows or columns, we assume we are looking at a face and copy the row/column to its adjacent neighbor, which copies its own corresponding row/column to its own neighbor and so on, until we return to the initiator face and stop.
5. when we rotate the cube we change our 'front' reference in the cube class to the correct neighbor of the old front.

If I increase the size to 1million x 1million the initialization would take far too long. I am not sure how to reduce the time to write each square.

Try it here: http://rubix.azurewebsites.net/
