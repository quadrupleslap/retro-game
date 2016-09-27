﻿Public Class HealthUp
    Implements Entity, Collider

    Private VelX As Double = 0 'Left to Right
    Private VelY As Double = 0 'Up to Down
    Private X As Double = 0
    Private Y As Double = 0
    Private State As Entity.State = Entity.State.Active
    Private Done As Boolean = False
    Private Angle As Double = 0

    Public Sub New(maze As Maze)
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, POWERUP_LAYER)

        Dim xRange = maze.Width - Width - 5, yRange = maze.Height - Height - 5
        Do
            X = (Application.RNG.NextDouble - 0.5) * xRange
            Y = (Application.RNG.NextDouble - 0.5) * yRange
        Loop Until maze.Collides(Me) Is Nothing
        Translation.X = X
        Translation.Y = Y

        VelX = Application.RNG.NextDouble * 0.2 - 0.1
        VelY = Application.RNG.NextDouble * 0.2 - 0.1

        Rotation.CenterX = 0.5 * Width
        Rotation.CenterY = 0.5 * Height
        Scaling.CenterX = 0.5 * Width
        Scaling.CenterY = 0.5 * Height

        Dim duration = New Duration(TimeSpan.FromMilliseconds(500))
        Scaling.BeginAnimation(ScaleTransform.ScaleXProperty, New Animation.DoubleAnimation(0, 1, duration) With {
            .EasingFunction = New Animation.BackEase() With {.EasingMode = Animation.EasingMode.EaseOut}
        })
        Scaling.BeginAnimation(ScaleTransform.ScaleYProperty, New Animation.DoubleAnimation(0, 1, duration) With {
            .EasingFunction = New Animation.BackEase() With {.EasingMode = Animation.EasingMode.EaseOut}
        })
    End Sub

    Public Function CollisionRect() As Rect Implements Collider.CollisionRect
        Return New Rect(X - 0.5 * Width, Y - 0.5 * Height, Width, Height)
    End Function

    Public Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        If Done Then Return State

        Dim colliding As Collider, player As PlayerEntity = Nothing

        Y += VelY
        colliding = maze.Collides(Me)
        If maze.Collides(Me) IsNot Nothing Then
            If TypeOf colliding Is PlayerEntity Then player = colliding
            Y -= VelY
            VelY = -VelY
        End If

        X += VelX
        colliding = maze.Collides(Me)
        If maze.Collides(Me) IsNot Nothing Then
            If TypeOf colliding Is PlayerEntity Then player = colliding
            X -= VelX
            VelX = -VelX
        End If

        Rotation.Angle = Angle
        Translation.X = X
        Translation.Y = Y

        If player IsNot Nothing Then
            player.Heal(0.25)

            Dim duration = New Duration(TimeSpan.FromMilliseconds(100))
            Scaling.BeginAnimation(ScaleTransform.ScaleXProperty, New Animation.DoubleAnimation(0, duration))
            Scaling.BeginAnimation(ScaleTransform.ScaleYProperty, New Animation.DoubleAnimation(0, duration))
            Translation.BeginAnimation(TranslateTransform.YProperty, New Animation.DoubleAnimation(Y - 50, duration))
            Task.Delay(duration.TimeSpan.TotalMilliseconds).ContinueWith(
                Sub()
                    Dispatcher.Invoke(Sub()
                                          State = Entity.State.Done
                                          maze.SpawnRandomPowerup()
                                      End Sub)
                End Sub)

            Done = True
        End If

        Angle += 1
        Return State
    End Function
End Class
