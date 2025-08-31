Public Class Persona
    Public Property Nombre As String
    Public Property Apellidos As String
    Public Property Telefono As String
    Public Property Email As String
    Public Property Clave As String
    Public Sub New()
    End Sub
    Public Function dtToPersona(dt As DataTable) As Persona
        If dt Is Nothing Or dt.Rows.Count = 0 Then
            Return Nothing
        End If
        Dim row As DataRow = dt.Rows(0)
        Dim p As New Persona() With {
            .Nombre = Convert.ToString(row("Nombre")),
            .Apellidos = Convert.ToString(row("Apellidos")),
            .Telefono = Convert.ToString(row("Telefono")),
            .Email = Convert.ToString(row("Email")),
            .Clave = Convert.ToString(row("Password"))
        }
        Return p
    End Function
End Class
