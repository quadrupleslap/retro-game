Public Class PlayerEntity
    Implements Entity, Collider

    Public Shared MAX_VELOCITY As Double = 12
    Public Shared INC_VELOCITY As Double = 1.2
    Public Shared FRICTION As Double = 1.2
    Public Shared THROTTLE As ULong = 50
    Public Shared DEATH_TICKS As ULong = 20

    Public LeftKey As Key = Key.Left
    Public RightKey As Key = Key.Right
    Public UpKey As Key = Key.Up
    Public DownKey As Key = Key.Down
    Public DeadTicksLeft As ULong = DEATH_TICKS

    Private _PrimaryColor As Color
    Public Property PrimaryColor
        Get
            Return _PrimaryColor
        End Get
        Set(value)
            _PrimaryColor = value
            GradientStart.Color = value
            Cube.Stroke = New SolidColorBrush(value)
            Cube.Stroke = New SolidColorBrush(value)
        End Set
    End Property

    Private _SecondaryColor As Color
    Public Property SecondaryColor
        Get
            Return _SecondaryColor
        End Get
        Set(value)
            _SecondaryColor = value
            GradientStop.Color = value
        End Set
    End Property
    Private ActualAngle As Double = 0
    Private LastShot As ULong = 0
    Private VelX As Double = 0 'Left to Right
    Private VelY As Double = 0 'Up to Down

    Public X As Double = 0
    Public Y As Double = 0
    Public Health As Double = 1
    Public PlayerName As String = "Sp00ky"
    Public Dead As Double = False
    Public Angle As Double = 0

    Sub New(maze As Maze)
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, PLAYER_LAYER)
        PrimaryColor = Colors.Cyan
        SecondaryColor = Colors.Black
        maze.RegisterCollider(Me)
    End Sub

    Public Sub Damage(dmg As Double)
        If Not Dead Then
            Cube.BeginStoryboard(Resources("Damage"))
            Health = Math.Max(0, Health - dmg)

            If FloatEquals(Health, 0) Then
                Dead = True
                PlaySound(My.Resources.DeathEffect)
                Cube.Fill = Brushes.Black
            Else
                PlaySound(My.Resources.PlayerHit)
            End If
        End If
    End Sub

    Friend Sub Heal(healed As Double)
        If Not Dead Then
            Cube.BeginStoryboard(Resources("Heal"))
            PlaySound(My.Resources.Heal)
            Health = Math.Min(1.0, Health + healed)
        End If
    End Sub

    Public Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        Dim up = maze.KeyIsDown(UpKey)
        Dim down = maze.KeyIsDown(DownKey)
        Dim left = maze.KeyIsDown(LeftKey)
        Dim right = maze.KeyIsDown(RightKey)

        If Dead Then
            If DeadTicksLeft <= 0 Then
                For Each entity In maze.Entities
                    If TypeOf entity Is GameOver Then
                        Dim other = DirectCast(entity, GameOver)
                        other.Died(PlayerName)
                        Exit For
                    End If
                Next
                Return Entity.State.Done
            Else
                DeadTicksLeft -= 1
                Return Entity.State.Active
            End If
        End If

        If up Or down Or left Or right Then
            Angle = (450 - Math.Atan2(-VelY, VelX) * 180 / Math.PI) Mod 360
        Else
            Angle = (360 + Math.Round(Angle / 90) * 90) Mod 360
        End If

        If left Then VelX = Math.Max(-MAX_VELOCITY, VelX - INC_VELOCITY)
        If right Then VelX = Math.Min(MAX_VELOCITY, VelX + INC_VELOCITY)
        X += VelX
        If maze.Collides(Me) IsNot Nothing Then
            X -= VelX
            VelX = -VelX
        End If
        VelX /= FRICTION

        If up Then VelY = Math.Max(-MAX_VELOCITY, VelY - INC_VELOCITY)
        If down Then VelY = Math.Min(MAX_VELOCITY, VelY + INC_VELOCITY)
        Y += VelY
        If maze.Collides(Me) IsNot Nothing Then
            Y -= VelY
            VelY = -VelY
        End If
        VelY /= FRICTION

        Dim d1 = (720 - ActualAngle + Angle) Mod 360, d2 = (720 - Angle + ActualAngle) Mod 360
        If d1 <= d2 Then
            ActualAngle = (ActualAngle + d1 / 10) Mod 360
        Else
            ActualAngle = (360 + ActualAngle - d2 / 10) Mod 360
        End If

        Cube.RenderTransform = New TranslateTransform(X, Y)
        Fill.Transform = New RotateTransform(ActualAngle, 12, 12)

        If (maze.CurrentTick - LastShot) > THROTTLE Then
            LastShot = maze.CurrentTick
            maze.AddEntity(New SimpleProjectile(Me, X, Y, ActualAngle))
        End If

        Return Entity.State.Active
    End Function

    Public Function CollisionRect() As Rect Implements Collider.CollisionRect
        Return New Rect(X - 12, Y - 12, 24, 24)
    End Function
End Class
