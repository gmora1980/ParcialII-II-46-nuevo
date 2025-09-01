Imports System.Data.SqlClient

Public Class login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("ClienteId") Is Nothing Then
            Response.Redirect("Clientes.aspx")
        End If
    End Sub
    Protected Function Verify(client As Cliente) As Cliente
        Try
            Dim help As New DatabaseHelper()

            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@Email", client.Email),
                New SqlParameter("@Password", client.Clave)
            }

            Dim query As String = "SELECT ClienteId, Nombre, Apellidos, Telefono, Email, Password FROM Clientes WHERE Email = @Email AND Password = @Password"

            Dim dt As DataTable = help.ExecuteQuery(query, parameters)

            If dt.Rows.Count > 0 Then
                ' Usar el método de instancia DtToClientes para mapear datos
                Dim clienteCompleto As Cliente = client.dtToCliente(dt)
                ' Guardar datos en sesión
                Session("ClienteId") = clienteCompleto.ClienteID
                Session("Nombre") = clienteCompleto.Nombre
                Session("Apellidos") = clienteCompleto.Apellidos
                Session("Telefono") = clienteCompleto.Telefono
                Session("Email") = clienteCompleto.Email
                Return clienteCompleto
            Else
                Return Nothing
            End If

        Catch ex As Exception
            ' Aquí puedes loguear el error si quieres
            Return Nothing
        End Try
    End Function

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        lblError.Visible = False

        Dim client As New Cliente With {
            .Email = txtEmail.Text.Trim(),
            .Clave = txtPassword.Text
        }

        If String.IsNullOrWhiteSpace(client.Email) OrElse String.IsNullOrWhiteSpace(client.Clave) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CamposVacios",
                                                "Swal.fire('Por favor, ingrese email y contraseña.');", True)
            Exit Sub
        End If

        Dim clienteCompleto As Cliente = Verify(client)

        If clienteCompleto IsNot Nothing Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AccesoExitoso",
                "Swal.fire('Acceso Exitoso').then(() => { window.location.href = 'Clientes.aspx'; });", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorLogin",
                "Swal.fire('Usuario o contraseña incorrectos.');", True)
        End If
    End Sub
End Class