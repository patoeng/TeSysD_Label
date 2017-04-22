Imports MySql.Data.MySqlClient

Public Class frmLogin

    Sub responsiveForm()
        grbAuthentification.Left = (frmMain.pnlBody.Width / 2) - (grbAuthentification.Width / 2)
        grbAuthentification.Top = (frmMain.pnlBody.Height / 2) - (grbAuthentification.Height / 2)
        pctLogo.Left = grbAuthentification.Left - 15
        lblLogo.Left = grbAuthentification.Left + 25
        pctLogo.Top = grbAuthentification.Top - 40
        lblLogo.Top = pctLogo.Top
    End Sub

    Private Sub cmbUser_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbUser.TextChanged
        If cmbUser.Text = "Operator" Then
            txtPassword.Enabled = False
        Else
            txtPassword.Enabled = True
        End If
    End Sub

    Private Sub cmbUser_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbUser.KeyPress
        If cmbUser.Text <> "" Then
            If e.KeyChar = vbCr Then
                txtPassword.Focus()
            End If
        End If
        e.Handled = True

    End Sub

    Function checkLogin(ByVal valUser As String, ByVal valPass As String) As Boolean
        Try
            Call connectServer()
            Dim strSQL = "SELECT * FROM tblUsers WHERE user_id = '" & valUser & "' AND password = '" & valPass & "'"
            com = New MySqlCommand(strSQL, conn)
            Dim reader As MySqlDataReader = com.ExecuteReader

            reader.Read()
            If reader.HasRows Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox("checkLog : " & ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function

    Sub doLogin()
        If cmbUser.Text = "Operator" Then
            frmMain.checkLogin("Operator", "")
            frmMain.doSetDisplay("Work Order")
        Else
            If checkLogin(cmbUser.Text, txtPassword.Text) = True Then
                frmMain.checkLogin(cmbUser.Text, txtPassword.Text)
                frmMain.doSetDisplay("Work Order")
            Else
                MsgBox("Incorrect password!", MsgBoxStyle.Critical)
            End If

        End If

    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        doLogin()
    End Sub

    Private Sub pnlMain_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlMain.Paint
        'pctLogo.Image = Image.FromFile(Application.StartupPath & "/images/favicon.ico")
        'frmMain.getUserName = ""
        'frmMain.getPassword = ""
        'cmbUser.Text = ""
        'cmbUser.Focus()
        'txtPassword.Text = ""
        'cmbUser.Select()
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = vbCr Then
            If txtPassword.Text <> "" Then
                doLogin()
            End If
        End If
    End Sub

    Private Sub pnlMain_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlMain.Enter
        pctLogo.Image = Image.FromFile(Application.StartupPath & "/Images/favicon.ico")
        frmMain.getUserName = ""
        frmMain.getPassword = ""
        cmbUser.Text = ""
        txtPassword.Text = ""
        cmbUser.Focus()
        cmbUser.Select()
    End Sub

End Class