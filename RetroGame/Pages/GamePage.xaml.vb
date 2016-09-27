Imports System.Windows.Threading

Class GamePage
    Private Field As Maze
    Private Timer As DispatcherTimer
    Private Paused As Boolean = False
    Private BoundWindow As Window

    Private Sub TogglePaused() Handles PlayPause.Click
        If Paused Then
            ' Play
            Paused = False
            PlayPause.Template = Resources("PauseButton")
            PauseBar.BeginStoryboard(Me.Resources("PauseBar.FadeOut"))
        Else
            ' Pause
            Paused = True
            PlayPause.Template = Resources("PlayButton")
            PauseBar.BeginStoryboard(Me.Resources("PauseBar.FadeIn"))
        End If
    End Sub

    Private Sub Home() Handles HomeButton.Click
        NavigationService.Navigate(New Uri("/Pages/TitlePage.xaml", UriKind.RelativeOrAbsolute))
    End Sub

    Private Sub Restart() Handles RestartButton.Click
        NavigationService.Refresh()
    End Sub

    Private Sub Ready(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PlayTrack(Track.Game)

        Field = Level
        Field.AddEntity(New Countdown)
        BoundWindow = Window.GetWindow(Me)
        Keyboard.AddKeyUpHandler(BoundWindow, AddressOf KeyPressed)
        Timer = New DispatcherTimer(TimeSpan.FromMilliseconds(10), DispatcherPriority.Normal,
                                    Sub()
                                        If Not Paused Then Field.Tick()
                                    End Sub, Dispatcher)
    End Sub

    Sub KeyPressed(sender As Object, e As KeyEventArgs)
        If e.Key = Key.P Then TogglePaused()
    End Sub

    Private Sub Goodbye(sender As Object, e As RoutedEventArgs) Handles Me.Unloaded
        Keyboard.RemoveKeyUpHandler(BoundWindow, AddressOf KeyPressed)
        Timer.Stop()
    End Sub
End Class
