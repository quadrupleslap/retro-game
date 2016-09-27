Public Interface Maze
    ReadOnly Property Width As Double
    ReadOnly Property Height As Double

    Function Entities() As IList ' Read-Only
    Sub AddEntity(Entity)

    Function KeyIsDown(key As Key) As Boolean
    Function CurrentTick() As ULong
    Sub Tick()

    Function Collides(c As Collider) As Collider
    Sub RegisterCollider(c As Collider)
End Interface
