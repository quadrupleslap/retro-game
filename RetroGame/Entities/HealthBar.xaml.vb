Public Class HealthBar
    Implements Entity, Collider

    Private Health As Double = 1
    Private Player As PlayerEntity
    Private Box As Rect
    Private DeadTicksLeft As Double = 5

    Public Enum Location
        TopRight
        TopLeft
    End Enum

    Public Sub New(maze As Maze, player As PlayerEntity, location As Location)
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, HEALTH_BAR_LAYER)
        Health = player.Health
        Me.Player = player
        Bar.BorderBrush = New SolidColorBrush(player.PrimaryColor)
        Fill.Fill = New SolidColorBrush(player.PrimaryColor)

        If location = Location.TopLeft Then
            Dim x = -0.5 * maze.Width + 0.5 * Bar.Width + 5
            Dim y = -0.5 * maze.Height + 0.5 * Bar.Height + 5
            RenderTransform = New TranslateTransform(x, y)
            Box = New Rect(x - 0.5 * Width, y - 0.5 * Height, Width, Height)
            Bar.Child.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left)
        ElseIf location = Location.TopRight Then
            Dim x = 0.5 * maze.Width - 0.5 * Bar.Width - 5
            Dim y = -0.5 * maze.Height + 0.5 * Bar.Height + 5
            RenderTransform = New TranslateTransform(x, y)
            Box = New Rect(x - 0.5 * Width, y - 0.5 * Height, Width, Height)
            Bar.Child.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Right)
        End If

        maze.RegisterCollider(Me)
    End Sub

    Public Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        If Player.Health <> Health Then
            Health = Player.Health
            AnimateDamagedBar()
        ElseIf Player.Dead Then
            If DeadTicksLeft = 0 Then
                Return Entity.State.Done
            Else
                Bar.BorderBrush = Brushes.White
                DeadTicksLeft -= 1
            End If
        End If

        Return Entity.State.Active
    End Function

    Sub AnimateDamagedBar()
        Bar.Child.BeginAnimation(WidthProperty, New Animation.DoubleAnimation(
            Health * Bar.Width,
            New Duration(TimeSpan.FromSeconds(0.25))
        ) With {
            .EasingFunction = New Animation.BackEase With {
                .EasingMode = Animation.EasingMode.EaseOut
            }
        })
    End Sub

    Public Function CollisionRect() As Rect Implements Collider.CollisionRect
        Return Box
    End Function
End Class
