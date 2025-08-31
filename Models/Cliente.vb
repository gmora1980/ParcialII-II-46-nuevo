Public Class Cliente
    Inherits Persona
    Public Property ClienteId As Integer

    Public Sub New()
    End Sub
    Public Function dtToCliente(dt As DataTable) As Cliente
        If dt Is Nothing Or dt.Rows.Count = 0 Then
            Return Nothing
        End If
        Dim row As DataRow = dt.Rows(0)
        Dim c As New Cliente() With {
            .ClienteId = Convert.ToInt32(row("ClienteId")),
            .Nombre = Convert.ToString(row("Nombre")),
            .Apellidos = Convert.ToString(row("Apellidos")),
            .Telefono = Convert.ToString(row("Telefono")),
            .Email = Convert.ToString(row("Email")),
            .Clave = Convert.ToString(row("Password"))
        }
        Return c
    End Function
End Class