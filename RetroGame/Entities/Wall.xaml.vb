Public Class Wall
    Implements Entity, Collider

    Private Box As Rect
    Sub New(maze As Maze, bb As Rect)
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, WALL_LAYER)
        Box = bb
        RenderTransform = New TranslateTransform(bb.X, bb.Y)
        Width = bb.Width
        Height = bb.Height
        Box.X -= Box.Width * 0.5
        Box.Y -= Box.Height * 0.5
        maze.RegisterCollider(Me)
    End Sub

    Public Function CollisionRect() As Rect Implements Collider.CollisionRect
        Return Box
    End Function

    Public Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        ' Nothing.
        Return Entity.State.Active
    End Function
End Class
