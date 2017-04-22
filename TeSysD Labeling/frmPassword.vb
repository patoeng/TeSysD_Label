Imports MySql.Data.MySqlClient

Public Class frmPassword
    Sub responsiveForm()
        grbChangePassword.Left = (frmMain.pnlBody.Width / 2) - (grbChangePassword.Width / 2)
        grbChangePassword.Top = (frmMain.pnlBody.Height / 2) - (grbChangePassword.Height / 2)
    End Sub

    'Function checkLogin()
    '    Call connectServer()
    '    Dim strSQL = "SELECT * FROM tblUser WHERE user_id = '" & frmMain.getUserName & "' AND password = '" & frmMain.getPassword & "'"
    '    com = New MySqlCommand(strSQL, conn)
    '    Dim reader As MySqlDataReader = com.ExecuteReader

    '    reader.Read()

    '    If reader.HasRows Then
    '        Return True
    '    Else
    '        Return False
    '    End If

    'End Function

    Private Sub pnlMain_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlMain.Paint
        'cmbUser.Focus()
        'cmbUser.Text = ""
        'txtOldPassword.Text = ""
        'txtNewPassword.Text = ""
        'txtConfirmPassword.Text = ""
    End Sub

    Sub updatePassword()
        Try
            frmMain.queryDB("UPDATE tblUsers SET password = '" & txtNewPassword.Text & "' WHERE user_id = '" & cmbUser.Text & "'")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Function viwPassExisting(ByVal valUser As String) As String
        Return frmMain.readDB("password", "tblUsers", "user_id", valUser)
    End Function

    Sub changePassword()
        If cmbUser.Text <> "" Then
            If txtOldPassword.Text = viwPassExisting(cmbUser.Text) Then
                If txtNewPassword.Text = txtConfirmPassword.Text Then
                    updatePassword()
                    MsgBox("Ubah password berhasil", MsgBoxStyle.Information)
                    frmMain.doSetDisplay("Work Order")
                Else
                    MsgBox("Konfirmasi password salah, silahkan ketik ulang", MsgBoxStyle.Critical)
                End If
            Else
                MsgBox("Password salah!", MsgBoxStyle.Critical)
            End If
        End If
    End Sub

    Private Sub btnChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangePassword.Click
        changePassword()
    End Sub

    Private Sub cmbUser_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbUser.KeyPress
        e.Handled = True
    End Sub

    Private Sub pnlMain_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlMain.Enter
        txtOldPassword.Text = ""
        txtNewPassword.Text = ""
        txtConfirmPassword.Text = ""
        cmbUser.Text = ""
        cmbUser.Focus()
        cmbUser.Select()
    End Sub

    Private Sub frmPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class