Imports RetroGame

Public Class EmptyMaze
    Implements Maze, Collider

    Private Tick As ULong = 0
    Private ToAdd As New List(Of Entity)
    Private Colliders As New List(Of Collider)

    Private ReadOnly Property Maze_Width As Double Implements Maze.Width
        Get
            Return ActualWidth
        End Get
    End Property

    Private ReadOnly Property Maze_Height As Double Implements Maze.Height
        Get
            Return ActualHeight
        End Get
    End Property

    Public Function CurrentTick() As ULong Implements Maze.CurrentTick
        Return Tick
    End Function

    Public Function Collides(c As Collider) As Collider Implements Maze.Collides
        Dim r = c.CollisionRect

        For Each x In Colliders
            If x IsNot c And x.CollisionRect.IntersectsWith(r) Then
                Return x
            End If
        Next

        Return Nothing
    End Function

    Public Function KeyIsDown(key As Key) As Boolean Implements Maze.KeyIsDown
        Return Keyboard.IsKeyDown(key)
    End Function

    Private Function Maze_Entities() As IList Implements Maze.Entities
        Return Entities.Children
    End Function

    Private Sub Maze_Tick() Implements Maze.Tick
        Tick += 1

        Dim i = 0
        While i < Entities.Children.Count
            If DirectCast(Entities.Children(i), Entity).Tick(Me) = Entity.State.Done Then
                Dim j = 0
                While j < Colliders.Count
                    If Colliders(j) Is Entities.Children(i) Then
                        Colliders.RemoveAt(i)
                    Else
                        j += 1
                    End If
                End While
                Entities.Children.RemoveAt(i)
            Else
                i += 1
            End If
        End While

        For Each child In ToAdd
            Entities.Children.Add(child)
        Next
        ToAdd.Clear()
    End Sub

    Public Sub AddEntity(Entity As Object) Implements Maze.AddEntity
        ToAdd.Add(Entity)
    End Sub

    Public Sub RegisterCollider(c As Collider) Implements Maze.RegisterCollider
        Colliders.Add(c)
    End Sub

    Public Function CollisionRect() As Rect Implements Collider.CollisionRect
        Return New Rect(-0.5 * ActualWidth, -0.5 * ActualHeight - 10, ActualWidth, 10)
    End Function

    Private Sub Ready(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim thickness As Double = 100
        AddEntity(New Wall(Me, New Rect(0, -0.5 * (ActualHeight + thickness), ActualWidth, thickness))) 'Top
        AddEntity(New Wall(Me, New Rect(0, +0.5 * (ActualHeight + thickness), ActualWidth, thickness))) 'Bottom
        AddEntity(New Wall(Me, New Rect(-0.5 * (ActualWidth + thickness), 0, thickness, ActualHeight))) 'Left
        AddEntity(New Wall(Me, New Rect(+0.5 * (ActualWidth + thickness), 0, thickness, ActualHeight))) 'Right
    End Sub
End Class
