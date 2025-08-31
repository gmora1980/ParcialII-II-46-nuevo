Imports System.Data.SqlClient

Public Class login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("ClienteId") Is Nothing Then
            Response.Redirect("Clientes.aspx")
        End If
    End Sub
    Protected Function verificarcliente(Clienteingresado As Cliente) As Cliente
        Try
            Dim db As New DatabaseHelper()
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@Email", Clienteingresado.Email),
                New SqlParameter("@Password", Clienteingresado.Clave)
            }
            Dim consulta As String = "SELECT * FROM Clientes WHERE Email = @Email AND Password = @Password"
            Dim dt As DataTable = db.ExecuteQuery(consulta, parameters)
            If dt.Rows.Count = 0 Then

            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)

    End Sub
End Class