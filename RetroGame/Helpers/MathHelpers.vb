Public Module MathHelpers
    Public Function FloatEquals(a As Double, b As Double, Optional prec As Double = 0.000001) As Boolean
        Return Math.Abs(a - b) <= prec
    End Function
End Module
