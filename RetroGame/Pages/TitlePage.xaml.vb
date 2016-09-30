Class TitlePage
    Sub GoToDeveloperPage()
        Process.Start("https://github.com/quadrupleslap/")
        Windows.Application.Current.Shutdown()
    End Sub

    Private Sub Quit()
        Windows.Application.Current.Shutdown()
    End Sub

    Private Sub TitlePage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PlayTrack(Track.Menu)
    End Sub
End Class
