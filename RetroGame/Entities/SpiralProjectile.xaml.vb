Public Class SpiralProjectile
    Implements Entity, Collider

    Private Shared POWER As Double = 0.25

    Private X, Y, Angle As Double
    Private OffX, OffY As Double
    Private DeathTicksLeft As UInteger = 3
    Private MyPlayer As PlayerEntity
    Private StartTick As ULong

    Public Sub New(maze As Maze, player As PlayerEntity, x As Double, y As Double, angle As Double)
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, STAR_PROJECTILE_LAYER)
        StartTick = maze.CurrentTick
        MyPlayer = player
        OffX = player.X
        OffY = player.Y
        Me.Angle = (90 - angle) * Math.PI / 180
        SetLocation(StartTick)
        Square.Stroke = New SolidColorBrush(player.PrimaryColor)
        Spinning.Angle = Application.RNG.NextDouble * 360
    End Sub

    Private Sub SetLocation(currentTick As ULong)
        Dim t As Double = currentTick - StartTick
        X = OffX + 36 * Math.Sqrt(t) * Math.Cos(Angle)
        Y = OffY + 36 * Math.Sqrt(t) * -Math.Sin(Angle)
        RenderTransform = New TranslateTransform(X, Y)
    End Sub

    Public Function CollisionRect() As Rect Implements Collider.CollisionRect
        Return New Rect(X - 10, Y - 10, 20, 20)
    End Function

    Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        If DeathTicksLeft < 3 Then
            If DeathTicksLeft = 0 Then
                Return Entity.State.Done
            Else
                If DeathTicksLeft = 2 Then
                    Square.Fill = Brushes.Black
                ElseIf DeathTicksLeft = 1 Then
                    Square.Stroke = Brushes.White
                End If
                DeathTicksLeft -= 1
                Return Entity.State.Active
            End If
        End If

        SetLocation(maze.CurrentTick)
        Dim collider = maze.Collides(Me)
        If collider IsNot Nothing And collider IsNot MyPlayer Then
            If TypeOf collider Is PlayerEntity Then
                DirectCast(collider, PlayerEntity).Damage(POWER)
            End If
            DeathTicksLeft -= 1 'Start the scratch.
        End If

        Spinning.Angle += 2
        Return Entity.State.Active
    End Function
End Class
