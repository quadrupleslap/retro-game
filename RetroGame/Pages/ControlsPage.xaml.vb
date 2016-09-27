Class ControlsPage
    Public Conv As New KeyConverter

    Sub Ready() Handles Me.Loaded
        PlayTrack(Track.Menu)

        P1Up.SelectedKey = Conv.ConvertFromInvariantString(My.Settings.P1Up)
        P2Up.SelectedKey = Conv.ConvertFromInvariantString(My.Settings.P2Up)
        P1Down.SelectedKey = Conv.ConvertFromInvariantString(My.Settings.P1Down)
        P2Down.SelectedKey = Conv.ConvertFromInvariantString(My.Settings.P2Down)
        P1Left.SelectedKey = Conv.ConvertFromInvariantString(My.Settings.P1Left)
        P2Left.SelectedKey = Conv.ConvertFromInvariantString(My.Settings.P2Left)
        P1Right.SelectedKey = Conv.ConvertFromInvariantString(My.Settings.P1Right)
        P2Right.SelectedKey = Conv.ConvertFromInvariantString(My.Settings.P2Right)
    End Sub

    Sub Change(sender As KeyInput, value As Key) Handles _
            P1Up.ValueChanged, P2Up.ValueChanged,
            P1Down.ValueChanged, P2Down.ValueChanged,
            P1Left.ValueChanged, P2Left.ValueChanged,
            P1Right.ValueChanged, P2Right.ValueChanged
        My.Settings.Item(sender.Name) = Conv.ConvertToInvariantString(value)
        My.Settings.Save()
    End Sub

    Sub Done()
        NavigationService.GoBack()
    End Sub
End Class
