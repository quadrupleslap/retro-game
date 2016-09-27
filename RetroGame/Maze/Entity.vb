Public Interface Entity
    Enum State
        Done
        Active
    End Enum

    Function Tick(maze As Maze) As State
End Interface
