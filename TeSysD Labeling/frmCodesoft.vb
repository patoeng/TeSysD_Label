Public Class frmCodesoft

    Private Sub frmCodesoft_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            txtHorizontalOffset.Value = frmMain.readDataBase("SELECT * FROM tblSetCodeSoft WHERE template = '" & Replace(frmMain.varTemplate, " - new is", "") & "'", "horizontal") / 100
            txtVerticalOffset.Value = frmMain.readDataBase("SELECT * FROM tblSetCodeSoft WHERE template = '" & Replace(frmMain.varTemplate, " - new is", "") & "'", "vertical") / 100 'gDocGroup.VertPrintOffset
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnSetCodesoft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetCodesoft.Click
        'On Error Resume Next
        Try

            frmMain.queryDB("UPDATE tblSetCodeSoft SET horizontal = '" & txtHorizontalOffset.Value * 100 & "', vertical = '" & txtVerticalOffset.Value * 100 & "', last_update = '" & frmMain.dateNowMysql & "' WHERE template = '" & Replace(frmMain.varTemplate, " - new is", "") & "'")

            gDocGroup.HorzPrintOffset = txtHorizontalOffset.Value * 100 '0:          'txtHorizontalOffset.Value
            gDocGroup.VertPrintOffset = txtVerticalOffset.Value * 100 'offsetCSVertical '0 'txtVerticalOffset.Value
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Me.Close()
    End Sub
End Class