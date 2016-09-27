Imports NAudio.Wave

Public Module GlobalTrack
    Public Enum Track
        None
        Game
        Menu
        Victory
    End Enum

    Private CurrentTrack As Track = Track.None
    Private Player As New MediaPlayer
    Private PlayingHead As Boolean = False

    Private Device As New WaveOutEvent
    Private Format As WaveFormat
    Private Mixer As SampleProviders.MixingSampleProvider

    Sub New()
        AddHandler Player.MediaEnded, Sub()
                                          Player.Position = TimeSpan.Zero
                                          If PlayingHead Then
                                              PlayingHead = False
                                              LoadBodyTrack()
                                          End If
                                          Player.Play()
                                      End Sub

        Format = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2)
        Device = New WaveOutEvent()
        Mixer = New SampleProviders.MixingSampleProvider(Format)
        Mixer.ReadFully = True

        Device.Init(Mixer)
        Device.Play()
    End Sub

    Public Sub PlaySound(snd As IO.Stream)
        Dim x = New WaveFileReader(snd)
        Mixer.AddMixerInput(x.ToSampleProvider)
    End Sub

    Private Sub LoadBodyTrack()
        Select Case CurrentTrack
            Case Track.Victory
                Player.Open(New Uri("./Resources/VictoryTrack.Body.mp3", UriKind.Relative))
            Case Else
                Throw New InvalidOperationException()
        End Select
    End Sub

    Public Sub PlayTrack(t As Track)
        If t <> CurrentTrack Then
            ' Reset the player.
            PlayingHead = False
            Player.Position = TimeSpan.Zero
            Player.Stop()

            ' Start the new track.
            Select Case t
                Case Track.None
                    ' Nothing.
                Case Track.Game
                    Player.Open(New Uri("./Resources/GameTrack.mp3", UriKind.Relative))
                    Player.Play()
                Case Track.Menu
                    Player.Open(New Uri("./Resources/MenuTrack.mp3", UriKind.Relative))
                    Player.Play()
                Case Track.Victory
                    PlayingHead = True
                    Player.Open(New Uri("./Resources/VictoryTrack.Head.mp3", UriKind.Relative))
                    Player.Play()
            End Select
            CurrentTrack = t
        End If
    End Sub
End Module
