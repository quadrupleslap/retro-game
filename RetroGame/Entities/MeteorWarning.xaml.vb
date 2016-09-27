Public Class MeteorWarning
    Implements Entity
    Private Ticks As Double = 0

    Public Sub New(x As Double, y As Double)
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, WARNING_LAYER)
        RenderTransform = New TranslateTransform(x, y)
    End Sub

    Public Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        If Ticks Mod 5 = 0 Then
            If Visibility = Visibility.Visible Then
                Visibility = Visibility.Collapsed
            Else
                Visibility = Visibility.Visible
            End If
        End If

        If Ticks = 20 Then
            Return Entity.State.Done
        End If

        Ticks += 1
        Return Entity.State.Active
    End Function
End Class
