Public Class login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("ClienteId") Is Nothing Then
            Response.Redirect("default.aspx")
        End If)
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)

    End Sub
End Class