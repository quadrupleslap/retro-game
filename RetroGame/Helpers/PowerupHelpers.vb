Imports System.Runtime.CompilerServices

Public Module PowerupHelpers
    <Extension()>
    Sub SpawnRandomPowerup(maze As Maze)
        Dim n As Double = Application.RNG.NextDouble
        Dim x As Entity
        Select Case n
            Case < 0.1
                x = New MeteorStorm(maze)
            Case < 0.5
                x = New HealthUp(maze)
            Case Else
                x = New RadialShot(maze)
        End Select
        maze.AddEntity(x)
    End Sub
End Module
