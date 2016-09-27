Public Class Meteor
    Implements Entity, Collider

    Private Shared POWER As Double = 0.5
    Private Shared TIME As ULong = 100
    Private Shared WARN_TIME As ULong = 50

    Private X, Y As Double
    Private K As Double
    Private TargetX, TargetY As Double
    Private DeathTicksLeft As UInteger = 6
    Private MyPlayer As PlayerEntity
    Private StartTick As ULong

    Public Sub New(maze As Maze, x As Double, y As Double, delay As ULong)
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, METEOR_LAYER)
        K = maze.Height
        TargetX = x
        TargetY = y
        StartTick = maze.CurrentTick + delay
        SetLocation(StartTick)
    End Sub

    Private Sub SetLocation(currentTick As ULong)
        Dim t As Double = currentTick - StartTick,
            d = TIME,
            b = -0.5 * K - 50,
            c = TargetY - b
        X = TargetX
        Y = -c * (Math.Sqrt(1 - t / d * t / d) - 1) + b
        RenderTransform = New TranslateTransform(X, Y)
    End Sub

    Public Function CollisionRect() As Rect Implements Collider.CollisionRect
        Return New Rect(X - 33, Y - 33, 66, 66)
    End Function

    Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        If maze.CurrentTick < StartTick Then
            Return Entity.State.Active
        End If

        If DeathTicksLeft < 6 Then
            If DeathTicksLeft = 0 Then
                Return Entity.State.Done
            Else
                If DeathTicksLeft = 5 Then
                    Square.Fill = Brushes.Black
                End If
                DeathTicksLeft -= 1
                Return Entity.State.Active
            End If
        End If

        If maze.CurrentTick = StartTick + TIME - WARN_TIME Then
            PlaySound(My.Resources.MeteorFalling)
            maze.AddEntity(New MeteorWarning(TargetX, TargetY))
        End If

        SetLocation(maze.CurrentTick)

        If maze.CurrentTick - StartTick >= TIME Then
            DeathTicksLeft -= 1 'Start the scratch.
            PlaySound(My.Resources.MeteorHit)
            Dim collider = maze.Collides(Me)
            If collider IsNot Nothing And collider IsNot MyPlayer Then
                If TypeOf collider Is PlayerEntity Then
                    DirectCast(collider, PlayerEntity).Damage(POWER)
                End If
            End If
        End If

        Return Entity.State.Active
    End Function
End Class
