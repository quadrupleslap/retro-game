Class MainWindow
    Private Sub Ready() Handles Me.Initialized
        Navigate(New Uri("/Pages/TitlePage.xaml", UriKind.RelativeOrAbsolute))
    End Sub
End Class
