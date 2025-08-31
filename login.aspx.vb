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
            If dt.Rows.Count > 0 Then
                Dim completo As Cliente = Clienteingresado.dtToCliente(dt)
                Session("ClienteId") = completo.ClienteId
                Session("Nombre") = completo.Nombre
                Session("Apellidos") = completo.Apellidos
                Session("Telefono") = completo.Telefono
                Session("Email") = completo.Email
                Return completo
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Dim ingresado As New Cliente() With {
            .Email = txtEmail.Text.Trim(),
            .Clave = txtPassword.Text.Trim()
        }
        If String.IsNullOrWhiteSpace(ingresado.Email) Or String.IsNullOrWhiteSpace(ingresado.Clave) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Campos", "Swal.fire('Por favor, complete todos los campos.');", True)
            Exit Sub
        End If
        Dim cliente As Cliente = verificarcliente(ingresado)
        If cliente IsNot Nothing Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AccesoExitoso",
                "Swal.fire('Acceso Exitoso').then(() => { window.location.href = 'Clientes.aspx'; });", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Error", "Swal.fire('Error al ingresar los datos');", True)
        End If
    End Sub
End Class