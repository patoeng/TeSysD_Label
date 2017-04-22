Public Class frmVisionSetting

    Function checkPassAdministrator() As Boolean
        If txtInsertPassword.Text = frmMain.readDB("password", "tblUsers", "user_id", "Administrator") Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If checkPassAdministrator() = True Then
            If chkForceVisionResult.Checked = True Then
                'If frmMain.monautomate.mwLectureAutomate(frmMain.mw_force_vision_result) = 1 Then
                frmMain.writePLC(frmMain.mw_force_vision_result, 1)
            Else
                frmMain.writePLC(frmMain.mw_force_vision_result, 0)
                ';                chkForceVisionResult.Checked = False
            End If

            If chkSequence.Checked = False Then
                frmMain.writePLC(frmMain.mw_bypass_vision_check, 1)

            Else
                frmMain.writePLC(frmMain.mw_bypass_vision_check, 0)
            End If

        Else
            MsgBox("Password Salah!", MsgBoxStyle.Critical)
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub frmBypassVisionPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If frmMain.monautomate.mwLectureAutomate(frmMain.mw_force_vision_result) = 1 Then
            chkForceVisionResult.Checked = True
        Else
            chkForceVisionResult.Checked = False
        End If

        If frmMain.monautomate.mwLectureAutomate(frmMain.mw_bypass_vision_check) = 1 Then
            chkSequence.Checked = False
        Else
            chkSequence.Checked = True
        End If

    End Sub

End Class