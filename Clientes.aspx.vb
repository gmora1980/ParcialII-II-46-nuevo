Imports System.Data.SqlClient
Imports Microsoft.Ajax.Utilities

Public Class Clientes
    Inherits System.Web.UI.Page

    Dim H As New DatabaseHelper()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("ClienteId") Is Nothing Then
                Response.Redirect("login.aspx")
            End If
            CargarClientes()
            LimpiarCampos()
        End If
    End Sub
    Private Function Validacioncorreo(Email As String) As Boolean
        Dim emailPattern As String = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
        Return System.Text.RegularExpressions.Regex.IsMatch(Email, emailPattern)
    End Function
    Protected Sub CargarClientes()
        Try
            Dim query As String = "SELECT ClienteId, Nombre, Apellidos, Telefono, Email FROM Clientes"
            Dim dt As DataTable = H.ExecuteQuery(query)
            GvClientes.DataSource = dt
            GvClientes.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Error al cargar los clientes: " & ex.Message & "');", True)

        End Try
    End Sub
    Private Sub LimpiarCampos()
        TxtNombre.Text = ""
        TxtApellido.Text = ""
        TxtTelefono.Text = ""
        TxtEmail.Text = ""
        ClienteID.Value = ""
    End Sub
    Protected Sub BtnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If String.IsNullOrWhiteSpace(TxtNombre.Text) Or String.IsNullOrWhiteSpace(TxtApellido.Text) Or String.IsNullOrWhiteSpace(TxtTelefono.Text) Or String.IsNullOrWhiteSpace(TxtEmail.Text) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Por favor, complete todos los campos.');", True)
                Return
            End If
            If Not Validacioncorreo(TxtEmail.Text) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Por favor, ingrese un correo electrónico válido.');", True)
                Return
            End If
            Dim cliente As New Cliente With {
                .Nombre = TxtNombre.Text,
                .Apellidos = TxtApellido.Text,
                .Telefono = TxtTelefono.Text,
                .Email = TxtEmail.Text,
                .Clave = TxtPass.Text
            }
            If String.IsNullOrEmpty(ClienteID.Value) Then
                Dim consulta As String = "INSERT INTO Clientes (Nombre, Apellidos, Telefono, Email, Password) VALUES (@Nombre, @Apellidos, @Telefono, @Email, @Password)"
                Dim parametros As New List(Of SqlParameter) From {
                    New SqlParameter("@Nombre", cliente.Nombre),
                    New SqlParameter("@Apellidos", cliente.Apellidos),
                    New SqlParameter("@Telefono", cliente.Telefono),
                    New SqlParameter("@Email", cliente.Email),
                    New SqlParameter("@Password", cliente.Clave)
                }
                H.ExecuteNonQuery(consulta, parametros)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Cliente agregado exitosamente.');", True)
            Else
                Dim Id As Integer = Convert.ToInt32(ClienteID.Value)
                Dim consulta As String = "UPDATE Clientes SET Nombre = @Nombre, Apellidos = @Apellidos, Telefono = @Telefono, Email = @Email, Password = @Password WHERE ClienteId = @ClienteId"
                Dim parametros As New List(Of SqlParameter) From {
                    New SqlParameter("@Nombre", cliente.Nombre),
                    New SqlParameter("@Apellidos", cliente.Apellidos),
                    New SqlParameter("@Telefono", cliente.Telefono),
                    New SqlParameter("@Email", cliente.Email),
                    New SqlParameter("@Password", cliente.Clave),
                    New SqlParameter("@ClienteId", Id)
                }
                H.ExecuteNonQuery(consulta, parametros)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Cliente actualizado exitosamente.');", True)
            End If
            LimpiarCampos()
            CargarClientes()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Error al guardar el cliente: " & ex.Message & "');", True)

        End Try
    End Sub

    Protected Sub BtnCancelar_Click(sender As Object, e As EventArgs)
        LimpiarCampos()
    End Sub

    Protected Sub GvClientes_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim rows As GridViewRow = GvClientes.SelectedRow
            ClienteID.Value = GvClientes.DataKeys(rows.RowIndex).Value.ToString()
            TxtNombre.Text = rows.Cells(1).Text
            TxtApellido.Text = rows.Cells(2).Text
            TxtTelefono.Text = rows.Cells(3).Text
            TxtEmail.Text = rows.Cells(4).Text
            TxtPass.Text = ""

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Error al seleccionar el cliente: " & ex.Message & "');", True)

        End Try
    End Sub

    Protected Sub GvClientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Try
            Dim Id As Integer = Convert.ToInt32(GvClientes.DataKeys(e.RowIndex).Value)
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@ClienteId", Id)
            }
            Dim consulta As String = "DELETE FROM Clientes WHERE ClienteId = @ClienteId"
            H.ExecuteNonQuery(consulta, parametros)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Cliente eliminado exitosamente.');", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Error al eliminar el cliente: " & ex.Message & "');", True)

        End Try
    End Sub
End Class