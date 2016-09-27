Public Class SimpleProjectile
    Implements Entity, Collider

    Private Shared VEL As Double = 10
    Private Shared POWER As Double = 0.1
    Private Shared FRIENDLY_FIRE As Boolean = False

    Private X, Y, Dx, Dy As Double
    Private Bounces As UInteger = 3
    Private LeftPlayer As Boolean = False
    Private DeathTicksLeft As UInteger = 2
    Private MyPlayer As PlayerEntity

    Public Sub New(player As PlayerEntity, x As Double, y As Double, angle As Double)
        InitializeComponent()
        SetValue(Panel.ZIndexProperty, SIMPLE_PROJECTILE_LAYER)
        Dim rad = (90 - angle) * -Math.PI / 180
        Me.X = x
        Me.Y = y
        Dx = VEL * Math.Cos(rad)
        Dy = VEL * Math.Sin(rad)
        RenderTransform = New TranslateTransform(x, y)
        MyPlayer = player
        Square.Stroke = New SolidColorBrush(player.PrimaryColor)
        Spinning.Angle = Application.RNG.NextDouble * 360
    End Sub

    Public Function CollisionRect() As Rect Implements Collider.CollisionRect
        Return New Rect(X - 10, Y - 10, 20, 20)
    End Function

    Function Tick(maze As Maze) As Entity.State Implements Entity.Tick
        If Bounces = 0 Then
            If DeathTicksLeft = 0 Then
                Return Entity.State.Done
            Else
                If DeathTicksLeft = 2 Then
                    Square.Fill = Brushes.Black
                ElseIf DeathTicksLeft = 1 Then
                    Square.Stroke = Brushes.White
                End If
                DeathTicksLeft -= 1
                Return Entity.State.Active
            End If
        End If

        X += Dx
        If ShouldReverse(maze) Then
            X -= Dx
            Dx = -Dx
        End If

        Y += Dy
        If ShouldReverse(maze) Then
            Y -= Dy
            Dy = -Dy
        End If

        Spinning.Angle += 10
        RenderTransform = New TranslateTransform(X, Y)

        Return Entity.State.Active
    End Function

    Function ShouldReverse(maze As Maze) As Boolean
        Dim collider = maze.Collides(Me)
        If collider Is Nothing Then
            LeftPlayer = True
            Return False
        ElseIf TypeOf collider Is PlayerEntity Then
            If collider Is MyPlayer And Not (LeftPlayer And FRIENDLY_FIRE) Then
                Return False
            End If

            DirectCast(collider, PlayerEntity).Damage(POWER)
            Bounces = 0
            Return True
        ElseIf Bounces > 0 Then
            Bounces -= 1
            Return True
        End If

        Return True
    End Function
End Class
