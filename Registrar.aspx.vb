Imports System.Data.SqlClient

Public Class Registrar
    Inherits System.Web.UI.Page
    Dim helper As New DatabaseHelper()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub LimpiarCampos()
        txtNombre.Text = ""
        txtApellido.Text = ""
        txtEmail.Text = ""
        txtPassword.Text = ""
        txtTelefono.Text = ""
    End Sub
    Protected Function Registro(cliente As Cliente) As Boolean
        Try
            Dim query As String = "INSERT INTO Clientes (Nombre, Apellidos, Telefono, Email, Password) " &
                                  "VALUES (@Nombre, @Apellidos, @Telefono, @Email, @Password);"

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Nombre", cliente.Nombre),
                New SqlParameter("@Apellidos", cliente.Apellidos),
                New SqlParameter("@Telefono", cliente.Telefono),
                New SqlParameter("@Email", cliente.Email),
                New SqlParameter("@Password", cliente.Clave)
            }

            Return helper.ExecuteNonQuery(query, parametros)
        Catch ex As Exception
            Return False
        End Try
    End Function
    Protected Sub btnRegistrar_Click1(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtNombre.Text) OrElse
    String.IsNullOrWhiteSpace(txtApellido.Text) OrElse
    String.IsNullOrWhiteSpace(txtEmail.Text) OrElse
    String.IsNullOrWhiteSpace(txtPassword.Text) OrElse
    String.IsNullOrWhiteSpace(txtTelefono.Text) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Campos", "Swal.fire('Por favor, complete todos los campos.');", True)
            Return
        End If
        Dim nuevoCliente As New Cliente With {
            .Nombre = txtNombre.Text.Trim(),
            .Apellidos = txtApellido.Text.Trim(),
            .Email = txtEmail.Text.Trim(),
            .Clave = txtPassword.Text.Trim(),
            .Telefono = txtTelefono.Text.Trim()
        }
        If Registro(nuevoCliente) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Exito", "Swal.fire('Registro exitoso. Ahora puede iniciar sesión.'); window.location='login.aspx';", True)
            LimpiarCampos()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Error", "Swal.fire('Error al registrar');", True)
            LimpiarCampos()
        End If
    End Sub
End Class