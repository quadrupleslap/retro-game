Public Class KeyInput
    Public Event ValueChanged(sender As KeyInput, v As Key)

    Private _Listening As Boolean = False
    Private Property Listening As Boolean
        Get
            Return _Listening
        End Get
        Set(value As Boolean)
            _Listening = value
            If Not value Then
                Keyboard.ClearFocus()
            End If
        End Set
    End Property

    Private _SelectedKey As Key = Key.None
    Public Property SelectedKey As Key
        Get
            Return _SelectedKey
        End Get
        Set(value As Key)
            _SelectedKey = value
            TextContent.Text = value.ToString
            RaiseEvent ValueChanged(Me, value)
        End Set
    End Property

    Private Sub Unfocused(sender As Object, e As KeyboardEventArgs) Handles Me.LostKeyboardFocus
        Listening = False
    End Sub

    Private Sub Clicked(sender As Object, e As RoutedEventArgs) Handles Me.Click, Me.GotKeyboardFocus
        Listening = True
    End Sub

    Private Sub Pressed(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If Not Listening Then Return
        SelectedKey = e.Key
        Listening = False
        e.Handled = True
    End Sub
End Class
