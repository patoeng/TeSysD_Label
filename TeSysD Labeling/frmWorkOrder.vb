Public Class frmWorkOrder

    Private Sub frmWorkOrder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlMain.Controls.Clear()

        pnlMain.Controls.Add(frmMain.pnlViewWorkOrder)
    End Sub


    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        frmMain.viewListWorkOrder(txtFindWO.Text)
    End Sub
    Sub deleteWO()
        Dim resultDelete As DialogResult = MessageBox.Show("Are you sure want to delete this WO, " & frmMain.dgvWorkOrder.SelectedRows.Item(0).Cells(0).Value & "?", "Delete WO", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If resultDelete = DialogResult.Yes Then
            frmMain.queryDB("DELETE FROM tblWorkOrder where prd_order = '" & frmMain.dgvWorkOrder.SelectedRows.Item(0).Cells(0).Value & "' ")
            frmMain.viewListWorkOrder("")
        End If
    End Sub

    Private Sub btnDeleteWO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteWO.Click
        deleteWO()
    End Sub

    Private Sub txtFindWO_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFindWO.KeyPress
        If e.KeyChar = vbCr Then
            frmMain.viewListWorkOrder(txtFindWO.Text)
        End If
    End Sub
End Class