Public Class GameOver
    Implements Entity

    Public Shared DESCRIPTIONS As String() = {
        " Sucks", " Fails", " is a Loser", ", KYS", " is Drunk", " Needs Sleep",
        " Got Rekt", " has No M8s", " is Jelly", " Needs Some Pants", " is a Twelvie", " Disappoints",
        " Disrespected Harambe"
    }

    Public Jitter As Double = 30
    Public Message As String = Nothing
    Public TicksToShow As UInteger = 100

    Public Sub New()
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, GAME_OVER_LAYER)
    End Sub

    Public Sub Died(noun As String)
        If Message = Nothing Then
            PlayTrack(Track.Victory)
            Message = noun + DESCRIPTIONS(Application.RNG.Next(0, DESCRIPTIONS.Length))
            TicksToShow -= 1
        End If
    End Sub

    Public Shared Function ColorFromHSV(hue As Double, saturation As Double, value As Double) As Color
        Dim hi = Convert.ToInt32(Math.Floor(hue / 60)) Mod 6
        Dim f = hue / 60 - Math.Floor(hue / 60)
        value = value * 255

        Dim v = Convert.ToInt32(value)
        Dim p = Convert.ToInt32(value * (1 - saturation))
        Dim q = Convert.ToInt32(value * (1 - f * saturation))
        Dim t = Convert.ToInt32(value * (1 - (1 - f) * saturation))

        If hi = 0 Then
            Return Color.FromArgb(255, v, t, p)
        ElseIf (hi = 1) Then
            Return Color.FromArgb(255, q, v, p)
        ElseIf (hi = 2) Then
            Return Color.FromArgb(255, p, v, t)
        ElseIf (hi = 3) Then
            Return Color.FromArgb(255, p, q, v)
        ElseIf (hi = 4) Then
            Return Color.FromArgb(255, t, p, v)
        Else
            Return Color.FromArgb(255, v, p, q)
        End If
    End Function

    Public Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        If TicksToShow = 0 Then
            TextLabel.Foreground = New SolidColorBrush(ColorFromHSV(Application.RNG.NextDouble * 360, Application.RNG.NextDouble, 1))

            RenderTransform = New TranslateTransform(
            Application.RNG.NextDouble() * 2 * Jitter - Jitter,
            Application.RNG.NextDouble() * 2 * Jitter - Jitter)
        ElseIf TicksToShow < 100 Then ' Triggered!
            TicksToShow -= 1
            If TicksToShow = 0 Then
                TextLabel.Content = Message
            End If
        End If
        Return Entity.State.Active
    End Function
End Class
