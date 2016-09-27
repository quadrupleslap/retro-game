Public Class Countdown
    Implements Entity

    Private CurrentTick As ULong = 0
    Public Jitter As Double = 0

    Private keyConv As New KeyConverter

    Public Sub New()
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, COUNTDOWN_LAYER)
    End Sub

    Private Function GetKey(id As String) As Key
        Return keyConv.ConvertFromInvariantString(My.Settings.Item(id))
    End Function

    Private Sub Done(maze As Maze)
        Dim P1 = New PlayerEntity(maze) With {
            .UpKey = GetKey("P1Up"),
            .DownKey = GetKey("P1Down"),
            .LeftKey = GetKey("P1Left"),
            .RightKey = GetKey("P1Right"),
            .PrimaryColor = Colors.Cyan,
            .SecondaryColor = Colors.Black,
            .PlayerName = "Blue",
            .X = -100, .Y = 0, .Angle = 90
        }, P2 = New PlayerEntity(maze) With {
            .UpKey = GetKey("P2Up"),
            .DownKey = GetKey("P2Down"),
            .LeftKey = GetKey("P2Left"),
            .RightKey = GetKey("P2Right"),
            .PrimaryColor = Colors.Red,
            .SecondaryColor = Colors.Black,
            .PlayerName = "Red",
            .X = 100, .Y = 0, .Angle = 270
        }

        maze.Entities().Add(P1)
        maze.Entities().Add(P2)
        maze.Entities.Add(New HealthBar(maze, P1, HealthBar.Location.TopLeft))
        maze.Entities.Add(New HealthBar(maze, P2, HealthBar.Location.TopRight))
        maze.Entities.Add(New GameOver)
        maze.SpawnRandomPowerup()
        maze.SpawnRandomPowerup()
        maze.SpawnRandomPowerup()
    End Sub

    Public Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        If CurrentTick = 0 Then
            CountdownLabel.Content = "3"
        ElseIf CurrentTick = 90 Then
            CountdownLabel.Content = "2"
            Jitter += 1
        ElseIf CurrentTick = 180 Then
            CountdownLabel.Content = "1"
            Jitter += 1
        ElseIf CurrentTick = 270 Then
            CountdownLabel.Content = "Go!"
            Jitter += 2
        ElseIf CurrentTick >= 360 Then
            Done(maze)
            Return Entity.State.Done
        End If

        CurrentTick += 1
        RenderTransform = New TranslateTransform(
            Application.RNG.NextDouble() * 2 * Jitter - Jitter,
            Application.RNG.NextDouble() * 2 * Jitter - Jitter)
        Return Entity.State.Active
    End Function
End Class
